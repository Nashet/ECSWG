using Leopotam.EcsLite;

namespace Nashet.ECS
{
	sealed class HealthSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld world;
		private EcsFilter healthFilter;
		private EcsPool<HealthComponent> healths;

		public void Init(IEcsSystems systems)
		{
			world = systems.GetWorld();
			healths = world.GetPool<HealthComponent>();
			healthFilter = world.Filter<HealthComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			//foreach (var entity in healthFilter)
			//{
			//	ref var health = ref healths.Get(entity);


			//}
		}
	}
}