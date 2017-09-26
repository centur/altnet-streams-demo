using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace GrainInterfaces.Listener
{
	public interface IStreamListener<TMessage> : IGrainWithGuidKey
	{
		Task ReceiveCommand(TMessage item, StreamSequenceToken token = null);
		Task OnCommandCompleted();
		Task OnCommandError(Exception ex);
	}
}
