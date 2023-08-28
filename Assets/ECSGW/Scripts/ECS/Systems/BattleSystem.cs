using Leopotam.EcsLite;
using Nashet.Utils;
using UnityEngine;

namespace Nashet.ECS
{
	public delegate void OnUnitDied(Vector2Int position);

	sealed class BattleSystem : IEcsRunSystem, IEcsInitSystem
	{
		public event OnUnitDied OnUnitDied;

		private EcsWorld world;
		private EcsFilter battleFilter;
		private EcsPool<BattleComponent> battles;
		private EcsPool<HealthComponent> healths;
		private EcsPool<DamageComponent> damages;
		private EcsPool<PositionComponent> positions;

		public void Init(IEcsSystems systems)
		{
			world = systems.GetWorld();
			battleFilter = world.Filter<BattleComponent>().End();
			battles = world.GetPool<BattleComponent>();
			healths = world.GetPool<HealthComponent>();
			damages = world.GetPool<DamageComponent>();
			positions = world.GetPool<PositionComponent>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in battleFilter)
			{
				ref var health = ref healths.Get(entity);
				var battle = battles.Get(entity);
				var damage = battle.Attacker.UnpackComponent(world, damages);

				health.current -= damage.damage;
				if (health.current < 0)
				{
					health.current = 0;
					// im dead, man					

					var position = positions.Get(entity);
					OnUnitDied?.Invoke(position.pos);
					world.DelEntity(entity);
				}
				else
				{
					battles.Del(entity);
				}
			}
		}
	}
}