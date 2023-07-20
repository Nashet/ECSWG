using Nashet.Configs;
using Nashet.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nashet.Services
{
	public class ConfigService : IConfigService
	{
		private readonly Dictionary<Type, object> allConfigs = new Dictionary<Type, object>();

		public ConfigService()
		{
			foreach (var item in ConfigHolder.Instance.AllConfigs)
			{
				AddConfig(item);
			}
		}

		public virtual T GetConfig<T>() where T : class
		{
			return (T)allConfigs[typeof(T)];
		}

		public void AddConfig(ScriptableObject config)
		{
			allConfigs[config.GetType()] = config;
		}
	}

	public interface IConfigService : IService
	{
		T GetConfig<T>() where T : class;// todo check LL USES
	}
}