using Leopotam.EcsLite;
using Nashet.Utils;

namespace Nashet.ECS
{
	sealed class BattleSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld world;
		private EcsFilter battleFilter;
		private EcsPool<BattleComponent> battles;
		private EcsPool<HealthComponent> healths;
		private EcsPool<DamageComponent> damages;

		public void Init(IEcsSystems systems)
		{
			world = systems.GetWorld();
			battleFilter = world.Filter<BattleComponent>().End();
			battles = world.GetPool<BattleComponent>();
			healths = world.GetPool<HealthComponent>();
			damages = world.GetPool<DamageComponent>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in battleFilter)
			{
				ref var health = ref healths.Get(entity);
				var battle = battles.Get(entity);
				var damage = battle.Attacker.UnpackComponent(world, damages);

				health.current -= damage.damage;

				battles.Del(entity);
			}
		}
	}
}