using Nashet.Configs;
using Nashet.ECS; // can be replaced with intermediate class
using Nashet.Utils;
using System;
using System.Collections.Generic;

namespace Nashet.Services
{
	public class MapLoaderService : IMapLoaderService
	{
		protected MapGenerationConfig config;
		protected Dictionary<string, string> cellTypes = new Dictionary<string, string>();
		protected IConfigService configService;

		public MapLoaderService(IConfigService configService)
		{
			this.configService = configService;
			config = configService.GetConfig<MapGenerationConfig>();

			foreach (var item in config.AllowedCellTypes)
			{
				cellTypes[item.Id] = item.Id;
			}
		}

		public virtual MapComponent LoadMap()
		{
			var map = new List<List<GameCell>>(config.xSize);

			for (int x = 0; x < config.xSize; x++)

			{
				var collumn = new List<GameCell>(config.ySize);
				map.Add(collumn);
				for (int y = 0; y < config.ySize; y++)
				{
					collumn.Add(new GameCell(GetRandomCellType()));
				}
			}
			var mapComponent = new MapComponent(map, configService.GetConfig<GameplayConfig>().ExplodeLineTreshold, config.AllowedCellTypes.ConvertAll(x => GetData(x)));

			return mapComponent;
		}

		public string GetRandomCellType()
		{
			var r = new Random();
			var random = r.Next(config.AllowedCellTypes.Count);
			var randomElement = config.AllowedCellTypes[random];
			return GetData(randomElement);
		}

		protected string GetData(CellTypeConfig config)
		{
			return cellTypes[config.Id];
		}
	}

	public delegate void OnMapGenerated(MapComponent map);
	public interface IMapLoaderService : IService
	{
		MapComponent LoadMap();
		string GetRandomCellType();
	}
}