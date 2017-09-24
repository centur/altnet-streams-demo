using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;
using Utils;

namespace OrleansClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var clientInstance = InitializeClient(args).GetAwaiter().GetResult();

			PrettyConsole.Line("==== CLIENT: Initialized ====");
			PrettyConsole.Line("CLIENT: Write commands:", ConsoleColor.Green);

			Console.ReadLine();
		}

		static async Task<IClusterClient> InitializeClient(string[] args)
		{
			int initializeCounter = 0;

			var config = ClientConfiguration.LocalhostSilo();
			var builder = new ClientBuilder().UseConfiguration(config);

			var initSucceed = true;
			while (initSucceed)
			{

				try
				{
					var client = builder.Build();
					await client.Connect();
					return client;
				}
				catch (Exception exc)
				{
					PrettyConsole.Line(exc.Message, ConsoleColor.Cyan);
					initSucceed = false;
				}

				if (initializeCounter++ > 10)
				{
					return null;
				}

				PrettyConsole.Line("Client Init Failed. Sleeping 5s...", ConsoleColor.Red);
				Thread.Sleep(TimeSpan.FromSeconds(5));
			}

			return null;
		}
	}
}