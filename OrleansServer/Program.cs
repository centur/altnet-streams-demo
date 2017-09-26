using System;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace OrleansServer
{
	class Program
	{
		static void Main(string[] args)
		{

			var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
			siloConfig.Defaults.DefaultTraceLevel = Severity.Error;

			var silo = new SiloHost("Alt.NET Demo Silo", siloConfig);
			silo.InitializeOrleansSilo();
			silo.StartOrleansSilo();

			Console.WriteLine("Press Enter to close.");
			Console.ReadLine();

			// shut the silo down after we are done.
			silo.ShutdownOrleansSilo();
		}
	}
}
