using Leopotam.EcsLite;
using Nashet.Configs;
using Nashet.ECS;
using Nashet.FlagGeneration;
using Nashet.GameplayView; //todo hmm
using Nashet.Services;
using Nashet.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Nashet.Controllers
{
	public delegate void OnUnitClicked(Vector2Int position, Vector3 worldPosition);
	public delegate void OnWaypointsRefresh(HashSet<Vector2Int> wayPoints);
	public delegate void OnExplosionHappened(Vector2Int where, int amount);
	public delegate void OnUnitAppeared(Vector2Int position, string unitType, int unitId);
	public delegate void OnUnitMoved(Vector2Int from, Vector2Int toPosition, int unitId);
	public delegate void OnUnitDied(Vector2Int positi, int unitId);

	public enum Direction { horizontal, vertical }

	public class MapController : MonoBehaviour, IMapController
	{
		public event OnExplosionHappened ExplosionHappened;
		public event OnUnitAppeared UnitAppeared;
		public event OnUnitClicked UnitClicked;
		public event OnUnitClicked EmptyCellClicked;
		public event OnWaypointsRefresh WaypointsRefresh;
		public event OnUnitMoved UnitMoved;
		public event OnUnitDied OnUnitDied;
		public bool IsReady { get; private set; }

		[SerializeField] GameObject unitPrefab;

		private IConfigService configService;
		private IEcsSystems perTurnSystems;

		private MapComponent map;
		private EcsWorld world;
		private EcsPackedEntity? previouslySelectedUnit;
		private EcsPool<PositionComponent> positions;
		private EcsPool<BattleComponent> battles;
		private EcsPool<DamageComponent> damages;

		private readonly Queue<ValueTuple<Vector2Int, Vector2Int, int>> queue = new();
		private ECSRunner ECSRunner;

		private static WaitForSeconds waitForSecondsPointOne;

		private float xOffset;
		private float yOffset;

		// Cache the WaitForSeconds instance on first usage
		private static WaitForSeconds WaitForSecondsPointOne
		{
			get
			{
				if (waitForSecondsPointOne == null)
				{
					waitForSecondsPointOne = new WaitForSeconds(0.1f);
				}
				return waitForSecondsPointOne;
			}
		}

		private IEnumerator Start()
		{
			while (ECSRunner == null || !ECSRunner.IsReady)
			{
				yield return WaitForSecondsPointOne;
			}
			CreateViewForUnits();
		}

		public void Initialize(IConfigService configService, MapComponent map, ECSRunner ECSRunner)
		{
			this.map = map;
			this.ECSRunner = ECSRunner;
			world = ECSRunner.world;
			perTurnSystems = ECSRunner.perTurnUpdateSystems;

			this.configService = configService;
			positions = world.GetPool<PositionComponent>();
			battles = world.GetPool<BattleComponent>();
			damages = world.GetPool<DamageComponent>();

			xOffset = GetSize().x / 2f - 0.5f;
			yOffset = GetSize().y / 2f - 0.5f;//todo fixit			

			foreach (var item in ECSRunner.updateSystems.GetAllSystems())
			{

				if (item is BattleSystem healthSystem)
				{
					healthSystem.OnUnitDied += OnUnitDiedHandler;
				}
			}

			foreach (var item in ECSRunner.perTurnUpdateSystems.GetAllSystems())
			{
				if (item is AIMoveSystem AIMoveSystem)
				{
					AIMoveSystem.UnitMoved += AIUnitMOvedHandler;
				}
			}
			StartCoroutine(CustomUpdate());
			IsReady = true;
		}

		private IEnumerator CustomUpdate()
		{
			while (true)
			{
				if (queue.Count == 0)
				{
					yield return WaitForSecondsPointOne;
					continue;
				}
				var item = queue.Dequeue();
				UnitMoved?.Invoke(item.Item1, item.Item2, item.Item3);
				yield return WaitForSecondsPointOne;
			}
		}

		private void AIUnitMOvedHandler(Vector2Int from, Vector2Int toPosition, int unitID)
		{
			queue.Enqueue(new ValueTuple<Vector2Int, Vector2Int, int>(from, toPosition, unitID));
		}

		private void OnUnitDiedHandler(Vector2Int position, int unitId)
		{
			OnUnitDied?.Invoke(position, unitId);
		}

		private void CreateViewForUnits()
		{
			var filter = world.Filter<UnitTypeComponent>().Inc<PositionComponent>().End();
			var types = world.GetPool<UnitTypeComponent>();
			Sprite flag = CreateFlag();
			Sprite flag2 = CreateFlag();
			var middle = map.xSize / 2;

			foreach (int entity in filter)
			{
				ref var position = ref positions.Get(entity);
				ref var type = ref types.Get(entity);

				var unit = Instantiate(unitPrefab, GetOffsetedPosition(position.pos), Quaternion.identity);

				unit.transform.parent = transform;
				unit.name = $"Unit {type.unitId})";

				var unitView = unit.GetComponent<UnitView>();
				unitView.Subscribe(this, entity, position.pos.x < middle ? flag : flag2);
				UnitAppeared?.Invoke(position.pos, type.unitId, entity);
			}
		}

		private static Sprite CreateFlag()
		{
			var texture = FlagGenerator.Generate(128, 128);
			Sprite sprite2 = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(1f, 1f));
			return sprite2;
		}

		//todo make some util for that?
		public Vector3 GetOffsetedPosition(Vector2Int position)
		{
			return new Vector3((position.x - xOffset), (position.y - yOffset), 0);
		}

		private void ExplosionHappenedHandler(Vector2Int where, int amount)
		{
			ExplosionHappened?.Invoke(where, amount);
		}

		public void SimulateOneStep()
		{
			perTurnSystems.Run();
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

		internal void HandleCellClicked(Vector2Int clickedCell, Vector3 worldPosition)
		{
			// I can caсhe it in mapComponent
			//var cell = map.GetElement(position.x, position.y);
			var filter = world.Filter<UnitTypeComponent>().Inc<PositionComponent>().End();

			var types = world.GetPool<UnitTypeComponent>();

			bool clickedOnEmptyCell = true;

			foreach (int entity in filter)
			{
				ref var clickedPosition = ref positions.Get(entity);
				if (clickedPosition.pos == clickedCell)
				{
					//ref var type = ref types.Get(entity);
					//map.CopyTo(somePosition, clickedCell);

					if (previouslySelectedUnit.HasValue)
					{
						previouslySelectedUnit.Value.Unpack(world, out int unpackedPreviouslySelectedUnit);
						if (entity == unpackedPreviouslySelectedUnit)
						{
							WaypointsRefresh?.Invoke(null);
							previouslySelectedUnit = null;
							return;
						}
						else
						{
							var damage = damages.Get(unpackedPreviouslySelectedUnit);

							var attackersPosition = positions.Get(unpackedPreviouslySelectedUnit);
							var actualDistance = GetDistance(clickedPosition.pos, attackersPosition.pos);
							if (actualDistance <= damage.distance)
							{
								//attacked someone
								ref var battle = ref entity.AddnSet(battles);
								battle.Attacker = previouslySelectedUnit.Value;
								//here run only some systems some how
							}
						}
					}
					else
					{
						UnitClicked?.Invoke(clickedCell, worldPosition);

						RefreshWaypoints(clickedCell);
						previouslySelectedUnit = world.PackEntity(entity);
					}

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
			var waypoints = NearByPoints2(clickedCell);
			waypoints.Remove(clickedCell);
			WaypointsRefresh?.Invoke(waypoints);
		}

		private int GetDistance(Vector2Int from, Vector2Int toPosition)
		{
			return Mathf.Abs(from.x - toPosition.x) + Mathf.Abs(from.y - toPosition.y);
		}

		private bool IsAllowedMoveDistance(Vector2Int from, Vector2Int toPosition)
		{
			return GetDistance(from, toPosition) <= 2;
		}

		private void ClickedOnEmptyCellHandler(Vector2Int clickedCell, Vector3 worldPosition)
		{
			if (previouslySelectedUnit != null)
			{
				var fromPosition = previouslySelectedUnit.Value.UnpackComponent(world, positions);
				if (!IsAllowedMoveDistance(fromPosition.pos, clickedCell))
					return;

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
			UnitMoved?.Invoke(from, toPosition, unpackedEntity);
		}

		private static HashSet<Vector2Int> NearByPoints2(Vector2Int pos)
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

		private static HashSet<Vector2Int> NearByPoints(Vector2Int pos)
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
		event OnUnitDied OnUnitDied;
		event OnUnitClicked EmptyCellClicked;
		event OnUnitClicked UnitClicked;

		void SimulateOneStep();

		Sprite GetSprite(int x, int y);
		Vector2Int GetSize();
		Sprite GetSprite(string cellType);
		Vector3 GetOffsetedPosition(Vector2Int position);
	}
}