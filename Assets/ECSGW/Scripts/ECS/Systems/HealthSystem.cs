using Leopotam.EcsLite;
using UnityEngine;

namespace Nashet.ECS
{
	public delegate void OnUnitDied(Vector2Int position);
	sealed class HealthSystem : IEcsRunSystem, IEcsInitSystem
	{
		public event OnUnitDied OnUnitDied;

		private EcsWorld world;
		private EcsFilter healthFilter;
		private EcsPool<HealthComponent> healths;
		private EcsPool<PositionComponent> positions;

		public void Init(IEcsSystems systems)
		{
			world = systems.GetWorld();
			healths = world.GetPool<HealthComponent>();
			positions = world.GetPool<PositionComponent>();
			healthFilter = world.Filter<HealthComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in healthFilter)
			{
				ref var health = ref healths.Get(entity);

				if (health.current < 0)
				{
					health.current = 0;
					// im dead, man					
					
					var position = positions.Get(entity);
					OnUnitDied?.Invoke(position.pos);
					world.DelEntity(entity);
				}
			}
		}
	}
}