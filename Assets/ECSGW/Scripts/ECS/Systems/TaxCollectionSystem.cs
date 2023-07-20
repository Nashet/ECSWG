using Leopotam.EcsLite;
using UnityEngine;

namespace Nashet.ECS
{
	sealed class TaxCollectionSystem : IEcsRunSystem
	{
		public void Run(IEcsSystems systems)
		{
			var walletFilter = systems.GetWorld().Filter<ProducerComponent>().End();//.Exc<Country>()

			var walletPool = systems.GetWorld().GetPool<WalletComponent>();
			var producers = systems.GetWorld().GetPool<ProducerComponent>();

			foreach (var entity in walletFilter)
			{
				ref var walletComponent = ref walletPool.Get(entity);


				ref var producer = ref producers.Get(entity);

				if (producer.country.Unpack(systems.GetWorld(), out int unpackedEntity))
				{
					ref var countryWallet = ref walletPool.Get(unpackedEntity);
					var tax = (int)(walletComponent.money * 0.1f);
					walletComponent.money -= tax;
					countryWallet.money += tax;
				}
				else
				{
					Debug.LogError("Unpack failed");
				}
			}
		}
	}
}