using Leopotam.EcsLite;
using Nashet.Services;
using Nashet.Utils;
using UnityEngine;

namespace Nashet.ECS
{
	sealed class InitSystem : IEcsInitSystem
	{
		public void Init(IEcsSystems systems)
		{
			var World = systems.GetWorld();
			var mapPool = World.GetPool<MapComponent>();
			var playerPool = World.GetPool<PlayerComponent>();
			var healths = World.GetPool<HealthComponent>();
			var positions = World.GetPool<PositionComponent>();
			var damages = World.GetPool<DamageComponent>();
			var speeds = World.GetPool<MovementSpeed>();
			var unitTypes = World.GetPool<UnitTypeComponent>();
			var movementBlocks = World.GetPool<BlocksMovementComponent>();
			var owners = World.GetPool<OwnerComponent>();


			var map = World.NewEntity();
			mapPool.Add(map);
			ref var mapComponent = ref mapPool.Get(map);
			mapComponent.LoadFrom(ServiceLocator.Instance.Get<IMapLoaderService>().LoadMap());
			//map.AddnSet(mapPool).LoadFrom(ServiceLocator.Instance.Get<IMapLoaderService>().LoadMap());


			//var playerEntity = World.NewEntity();
			//playerPool.Add(playerEntity);

			AddUnit(0, 0);
			AddUnit(2, 0);
			AddUnit(4, 3);
			AddUnit(6, 3);
			AddUnit(7, 6);
			AddUnit(8, 3);
			AddUnit(0, 5);
			AddUnit(11, 6);

			void AddUnit(int x, int y)
			{
				var unitEntity = World.NewEntity();
				//unitEntity.Add(health).Add(position);
				unitEntity.AddnSet(healths).Set(100, 8);
				unitEntity.AddnSet(positions).Set(new Vector2Int(x, y));
				unitEntity.AddnSet(damages).Set(12, 2);
				unitEntity.AddnSet(speeds).speed = 4;
				unitEntity.AddnSet(unitTypes).unitId = "A";
				unitEntity.Add(movementBlocks);
				unitEntity.AddnSet(owners).ownerId = 1;
			}

			//that is for experiment:
			var unitEntity = World.NewEntity();
			healths.Add(unitEntity);
			ref var health = ref healths.Get(unitEntity);
			health.Set(45, 45);
		}
	}
}

