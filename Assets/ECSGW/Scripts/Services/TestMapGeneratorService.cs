//using Nashet.Configs;
//using System.Collections.Generic;

//namespace Nashet.Services
//{
//	public class TestMapGeneratorService : MapGeneratorService
//	{
//		public TestMapGeneratorService(IConfigService configService) : base(configService)
//		{
//		}

//		public override Map Generate()
//		{
//			var list = new List<List<GameCell>>(config.xSize);

//			for (int i = 0; i < config.xSize; i++)
//			{
//				var collumn = new List<GameCell>(config.ySize);
//				list.Add(collumn);
//				for (int j = 0; j < config.ySize; j++)
//				{
//					collumn.Add(new GameCell(cellTypes["Red"]));
//				}
//			}

//			for (int x = 1; x < config.xSize; x++)
//			{
//				for (int y = 1; y < config.ySize; y++)
//				{
//					list[x][y].SetCellType(cellTypes["Green"]);
//				}
//			}

//			config = configService.GetConfig<MapGenerationConfig>();
//			var map = new Map(list, configService.GetConfig<GameplayConfig>().ExplodeLineTreshold, config.AllowedCellTypes.ConvertAll(x => GetData(x)));

//			return map;
//		}
//	}
//}