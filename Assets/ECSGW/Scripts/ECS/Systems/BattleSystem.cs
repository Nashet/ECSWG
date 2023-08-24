using Leopotam.EcsLite;
using Nashet.Utils;

namespace Nashet.ECS
{
	sealed class BattleSystem : IEcsRunSystem
	{
		public void Run(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			var battleFilter = world.Filter<BattleComponent>().End();
			var battles = world.GetPool<BattleComponent>();
			var healths = world.GetPool<HealthComponent>();
			var damages = world.GetPool<DamageComponent>();

			foreach (var entity in battleFilter)
			{
				ref var health = ref healths.Get(entity);
				ref var battle = ref battles.Get(entity);
				var damage = battle.Attacker.UnpackComponent(world, damages);

				health.current -= damage.damage;


				if (health.current < 0)
				{
					health.current = 0;
					// im dead, man. Put it to health system
				}

				battles.Del(entity);
			}
		}
	}
}