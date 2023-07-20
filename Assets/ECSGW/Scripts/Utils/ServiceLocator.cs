using System.Collections.Generic;

namespace Nashet.Utils
{
	public class ServiceLocator : Singleton<ServiceLocator>
	{
		private readonly List<IService> services = new List<IService>();
		public bool initialized { get; private set; }
		// private readonly ILogService logService = default;//[Inject] 

		public ServiceLocator()
		{
			if (initialized)
				return;
			//logService.Log("ServiceManager Is ready");
		}

		public T Get<T>() where T : class, IService
		{
			//if (Instance == null)

			foreach (var t in services)
			{
				if (t is T service)
				{
					return service;
				}
			}

			//MLogger.LogWarningFormat("No service registered of type {0}",typeof(T));
			return null;
		}

		public IService Add(IService s)
		{
			services.Add(s);
			return s;
		}

		public void MarkReady()
		{
			initialized = true;
		}
	}
}
