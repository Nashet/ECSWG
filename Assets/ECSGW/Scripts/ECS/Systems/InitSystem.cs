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
			var health = World.GetPool<HealthComponent>();
			var position = World.GetPool<PositionComponent>();
			var damage = World.GetPool<DamageComponent>();
			var speed = World.GetPool<MovementSpeed>();
			var unitType = World.GetPool<UnitTypeComponent>();

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

			var unitEntity = World.NewEntity();
			health.Add(unitEntity);
			ref var zz = ref health.Get(unitEntity);
			zz.Set(45, 45);

			void AddUnit(int x, int y)
			{
				var unitEntity = World.NewEntity();
				//unitEntity.Add(health).Add(position);
				unitEntity.AddnSet(health).Set(100, 100);
				unitEntity.AddnSet(position).Set(new Vector2Int(x, y));
				unitEntity.AddnSet(damage).Set(12, 2);
				unitEntity.AddnSet(speed).speed = 4;
				unitEntity.AddnSet(unitType).unitId = "A";
			}
		}


		//private Vector2Int GetRandom()
		//{

		//}
	}
}