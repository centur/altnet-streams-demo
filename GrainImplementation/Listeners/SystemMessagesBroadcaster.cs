using System;
using System.Threading.Tasks;
using GrainInterfaces.Listeners;
using GrainInterfaces.Model;
using Orleans;
using Orleans.Streams;
using Utils;

namespace GrainImplementation.Listeners
{
	[ImplicitStreamSubscription(nameof(ChatMsg))]
	public class SystemMessagesBroadcaster : BaseListener<ChatMsg>, ISystemMessagesBroadcaster
	{
		public SystemMessagesBroadcaster()
		{
			StreamNamespaces = new[] {nameof(ChatMsg)};
		}

		public override Task OnNextAsync(ChatMsg item, StreamSequenceToken token = null)
		{
			if (item.Author == "System")
			{
				PrettyConsole.Line($"==== System message: '{item.Text}'", ConsoleColor.Green);
			}
			return Task.CompletedTask;
		}
	}
}