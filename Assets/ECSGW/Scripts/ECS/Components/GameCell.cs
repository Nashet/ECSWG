namespace Nashet.ECS
{
	public class GameCell
	{
		public string CellType { get; private set; }

		public GameCell(string cellType)
		{
			this.CellType = cellType;
		}

		public override string ToString()
		{
			return CellType;
		}

		public void SetCellType(string cellType)
		{
			CellType = cellType;
		}
	}
}
