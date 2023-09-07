//using Leopotam.EcsLite;
//using UnityEngine;

//namespace Nashet.ECS
//{
//	sealed class TaxCollectionSystem : IEcsRunSystem, IEcsInitSystem
//	{
//		private EcsFilter walletFilter;
//		private EcsPool<WalletComponent> walletPool;
//		private EcsPool<ProducerComponent> producers;

//		void IEcsInitSystem.Init(IEcsSystems systems)
//		{
//			walletFilter = systems.GetWorld().Filter<ProducerComponent>().End();//.Exc<Country>()
//			walletPool = systems.GetWorld().GetPool<WalletComponent>();
//			producers = systems.GetWorld().GetPool<ProducerComponent>();
//		}

//		public void Run(IEcsSystems systems)
//		{
//			foreach (var entity in walletFilter)
//			{
//				ref var walletComponent = ref walletPool.Get(entity);


//				var producer = producers.Get(entity);

//				if (producer.country.Unpack(systems.GetWorld(), out int unpackedEntity))
//				{
//					ref var countryWallet = ref walletPool.Get(unpackedEntity);
//					var tax = (int)(walletComponent.money * 0.1f);
//					walletComponent.money -= tax;
//					countryWallet.money += tax;
//				}
//				else
//				{
//					Debug.LogError("Unpack failed");
//				}
//			}
//		}
//	}
//}