using Leopotam.EcsLite;
using System.Threading;
using UnityEngine;

namespace Nashet.ECS
{
	public delegate void OnUnitMoved(Vector2Int from, Vector2Int toPosition, int unitID);
	sealed class AIMoveSystem : IEcsRunSystem, IEcsInitSystem
	{
		public event OnUnitMoved UnitMoved;

		private EcsWorld world;
		private EcsFilter AIUnitsFilter;
		private EcsFilter blocksMovementFilter;
		private EcsPool<PositionComponent> positions;
		private EcsPool<MovementSpeed> speeds;

		public void Init(IEcsSystems systems)
		{
			world = systems.GetWorld();
			positions = world.GetPool<PositionComponent>();
			speeds = world.GetPool<MovementSpeed>();
			AIUnitsFilter = world.Filter<MovementSpeed>().Exc<PlayerComponent>().End();
			blocksMovementFilter = world.Filter<BlocksMovementComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in AIUnitsFilter)
			{
				ref var position = ref positions.Get(entity);
				var speed = speeds.Get(entity);

				var newPosition = position.pos;


				for (int i = 0; i < speed.speed; i++)
				{
					newPosition.x -= 1;
					if (newPosition.x < 0)
					{
						newPosition.x = 0;
					}

					if (newPosition == position.pos) // check if there is anybody
					{
						continue;
					}
					if (!IsEmpty(newPosition))
					{
						continue;
					}


					var oldPosition = position.pos;
					position.pos = newPosition;
					UnitMoved?.Invoke(oldPosition, newPosition, entity);
					//Debug.LogError("AI moved from " + oldPosition + " to " + newPosition);
				}
			}
		}

		/// <summary>
		/// I can use some array of availabe cell instead of this:
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private bool IsEmpty(Vector2Int pos)
		{
			foreach (var entity in blocksMovementFilter)
			{
				var position = positions.Get(entity);
				if (position.pos == pos)
					return false;
			}
			return true;
		}
	}
}