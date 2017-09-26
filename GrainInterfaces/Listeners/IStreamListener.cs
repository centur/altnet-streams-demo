using GrainInterfaces.Model;
using Orleans.Streams;

namespace GrainInterfaces.Listeners
{
	public interface IChatMessageListener : IAsyncObserver<ChatMsg>
	{
	}
}