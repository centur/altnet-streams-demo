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
		private static string _joinedChannel = "#general";

		static void Main(string[] args)
		{
			var clientInstance = InitializeClient(args).GetAwaiter().GetResult();

			PrettyConsole.Line("==== CLIENT: Initialized ====", ConsoleColor.Cyan);
			PrettyConsole.Line("CLIENT: Write commands:", ConsoleColor.Cyan);

			Menu(clientInstance);

			PrettyConsole.Line("==== CLIENT: Shutting down ====", ConsoleColor.DarkRed);
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

		private static void PrintHints()
		{
			var menuColor = ConsoleColor.Magenta;
			PrettyConsole.Line("Type '/join <channel>' to join specific channel", menuColor);
			PrettyConsole.Line("Type '<message>' to send a message", menuColor);
			PrettyConsole.Line("Type '/exit' to exit client.", menuColor);
		}

		private static void Menu(IClusterClient client)
		{
			string input;
			do
			{
				PrintHints();

				input = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(input)) continue;

				if (input.StartsWith("/join"))
				{
					var channelName = input.Replace("/join ", "");
					JoinChannel(client, channelName);
				}
				else
				{
					SendMessage(client, input, _joinedChannel);
				}
			} while (input != "/exit");
		}

		private static void SendMessage(IClusterClient client, string input, string joinedChannel)
		{
			PrettyConsole.Line($"Sending '{input}' to channel {joinedChannel}");
		}

		private static void JoinChannel(IClusterClient client, string channelName)
		{
			PrettyConsole.Line($"Joining to channel {channelName}");
		}
	}
}