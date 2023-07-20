using Leopotam.EcsLite;
using Nashet.Configs;
using Nashet.ECS;
using Nashet.Services;
using Nashet.Utils;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Nashet.Controllers
{
	public delegate void OnUnitClicked(Vector2Int position, Vector3 worldPosition);
	public delegate void OnWaypointsRefresh(HashSet<Vector2Int> wayPoints);
	public delegate void OnExplosionHappened(Vector2Int where, int amount);
	public delegate void OnUnitAppeared(Vector2Int position, string unitType);
	public delegate void OnUnitMoved(Vector2Int from, Vector2Int toPosition);
	public enum Direction { horizontal, vertical }

	public class MapController : IMapController
	{
		public event OnExplosionHappened ExplosionHappened;
		public event OnUnitAppeared UnitAppeared;
		public event OnUnitClicked UnitClicked;
		public event OnUnitClicked EmptyCellClicked;
		public event OnWaypointsRefresh WaypointsRefresh;
		public event OnUnitMoved UnitMoved;
		public bool IsReady { get; private set; }

		private readonly IConfigService configService;
		private readonly MapComponent map;
		private readonly EcsWorld world;
		private EcsPackedEntity? previouslySelectedUnit;
		private EcsPool<PositionComponent> positions;

		public MapController(IConfigService configService, MapComponent map, EcsWorld world)
		{
			this.map = map;
			this.world = world;

			//map.ExplosionHappened += ExplosionHappenedHandler;
			this.configService = configService;
			positions = world.GetPool<PositionComponent>();
			IsReady = true;
		}

		public void CreateUnits()
		{

			var filter = world.Filter<UnitTypeComponent>().Inc<PositionComponent>().End();
			var positions = world.GetPool<PositionComponent>();
			var types = world.GetPool<UnitTypeComponent>();


			foreach (int entity in filter)
			{
				ref var position = ref positions.Get(entity);
				ref var type = ref types.Get(entity);

				UnitAppeared?.Invoke(position.pos, type.unitId);
			}
		}

		private void ExplosionHappenedHandler(Vector2Int where, int amount)
		{
			ExplosionHappened?.Invoke(where, amount);
		}

		public void SimulateOneStep()
		{
			//map.CheckRowsForCoincidence();
		}

		public Sprite GetSprite(string cellType)
		{
			var config = configService.GetConfig<MapGenerationConfig>();
			var cellTypeConfig = config.AllowedCellTypes.Find(x => x.Id == cellType);
			return cellTypeConfig.sprite;
		}

		public Sprite GetSprite(int x, int y)
		{
			//todo fix taht
			var config = configService.GetConfig<MapGenerationConfig>();
			var cellType = map.GetElement(x, y);
			var cellTypeConfig = config.AllowedCellTypes.Find(x => x.Id == cellType);
			return cellTypeConfig.sprite;
		}

		public Vector2Int GetSize()
		{
			return new Vector2Int(map.xSize, map.ySize);
		}

		public string GetType(int x, int y)
		{
			return map.GetElement(x, y);
		}

		public bool IsPossibleSlide(Vector2Int from, Vector2Int to)
		{
			return false;// map.IsPossibleSlide(from, to);//todo fix
		}

		public void MakeSlide(Vector2Int from, Vector2Int to)
		{
			//map.MakeSlide(from, to);
		}

		public void RefreshView()
		{

		}

		internal void HandleCellClicked(Vector2Int clickedCell, Vector3 worldPosition)
		{
			// I can caсhe it in mapComponent
			//var cell = map.GetElement(position.x, position.y);
			var filter = world.Filter<UnitTypeComponent>().Inc<PositionComponent>().End();

			var types = world.GetPool<UnitTypeComponent>();

			bool clickedOnEmptyCell = true;

			foreach (int entity in filter)
			{
				ref var position = ref positions.Get(entity);
				if (position.pos == clickedCell)
				{
					ref var type = ref types.Get(entity);
					map.CopyTo(position, clickedCell);

					UnitClicked?.Invoke(clickedCell, worldPosition);

					RefreshWaypoints(clickedCell);

					if (previouslySelectedUnit.HasValue)
					{
						previouslySelectedUnit.Value.Unpack(world, out int unpackedEntity);
						if (entity == unpackedEntity)
						{
							WaypointsRefresh?.Invoke(null);
							previouslySelectedUnit = null;
							return;
						}
					}
					previouslySelectedUnit = world.PackEntity(entity);

					clickedOnEmptyCell = false;
					break;
				}
			}

			if (clickedOnEmptyCell)
			{
				ClickedOnEmptyCellHandler(clickedCell, worldPosition);
			}
		}

		private void RefreshWaypoints(Vector2Int clickedCell)
		{
			var list = NearByPoints2(clickedCell);
			list.Remove(clickedCell);
			WaypointsRefresh?.Invoke(list);
		}

		private void ClickedOnEmptyCellHandler(Vector2Int clickedCell, Vector3 worldPosition)
		{
			if (previouslySelectedUnit != null)
			{
				var fromPosition = previouslySelectedUnit.Value.UnpackComponent(world, positions);
				MoveUnit(previouslySelectedUnit.Value, fromPosition.pos, clickedCell);
				previouslySelectedUnit = null;
			}
			else
			{
				EmptyCellClicked?.Invoke(clickedCell, worldPosition);
			}
		}

		private void MoveUnit(EcsPackedEntity entity, Vector2Int from, Vector2Int toPosition)
		{
			entity.Unpack(world, out int unpackedEntity);
			ref var position = ref positions.Get(unpackedEntity);
			position.pos = toPosition;
			UnitMoved?.Invoke(from, toPosition);
		}

		private HashSet<Vector2Int> NearByPoints2(Vector2Int pos)
		{
			var firstRing = NearByPoints(pos);
			var result = new HashSet<Vector2Int>();
			foreach (var item in firstRing)
			{
				var b = NearByPoints(item);
				result.AddRange(b);
			}
			result.AddRange(firstRing);
			return result;
		}
		private HashSet<Vector2Int> NearByPoints(Vector2Int pos)
		{
			return new HashSet<Vector2Int> {
			new Vector2Int(pos.x, pos.y - 1),
			new Vector2Int(pos.x-1, pos.y),
			new Vector2Int(pos.x, pos.y + 1),
			new Vector2Int(pos.x+1, pos.y) };
		}
	}

	public interface IMapController
	{
		bool IsReady { get; }

		event OnExplosionHappened ExplosionHappened;
		event OnUnitAppeared UnitAppeared;
		event OnWaypointsRefresh WaypointsRefresh;
		event OnUnitMoved UnitMoved;

		void SimulateOneStep();

		//todo put in a separate controller
		void RefreshView()
		{ }

		Sprite GetSprite(int x, int y);
		Vector2Int GetSize();
		string GetType(int x, int y);
		Sprite GetSprite(string cellType);
		void CreateUnits();
	}
}