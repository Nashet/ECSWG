using Leopotam.EcsLite;

namespace Nashet.ECS
{
	public struct CountryComponent
	{

	}

	public struct ProducerComponent
	{
		public EcsPackedEntity country;
	}

	public struct BattleComponent
	{
		public EcsPackedEntity attacker;
		public EcsPackedEntity victim;
	}
}