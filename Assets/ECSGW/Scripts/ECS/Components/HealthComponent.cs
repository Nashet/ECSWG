namespace Nashet.ECS
{
	public struct HealthComponent
	{
		public int max;
		public int current;

		public void Set(int max, int current)
		{
			this.max = max;
			this.current = current;
		}
	}
}