namespace Nashet.ECS
{
	public struct DamageComponent
	{
		public int damage;
		public int distance;

		public void Set(int damage, int distance)
		{
			this.damage = damage;
			this.distance = distance;
		}
	}
}