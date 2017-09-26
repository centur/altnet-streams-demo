using System.Collections.Generic;
using Orleans.Providers.Streams.SimpleMessageStream;
using Orleans.Runtime.Configuration;

namespace Utils
{
	public static class FluentConfig
	{
		public static string AltNetStream = "alt.net.stream";
		
		public static ClientConfiguration SimpleMessageStreamProvider(this ClientConfiguration config, string streamName,
			string pubSubType = "ImplicitOnly")
		{
			var streamConfiguration = new Dictionary<string, string> {{"PubSubType", pubSubType}};
			config.RegisterStreamProvider<SimpleMessageStreamProvider>(streamName, streamConfiguration);
			return config;
		}


		public static ClusterConfiguration SimpleMessageStreamProvider(this ClusterConfiguration config, string streamName,
			string pubSubType = "ImplicitOnly")
		{
			var streamConfiguration = new Dictionary<string, string> {{"PubSubType", pubSubType}};
			config.Globals.RegisterStreamProvider<SimpleMessageStreamProvider>(streamName, streamConfiguration);
			return config;
		}
	}
}