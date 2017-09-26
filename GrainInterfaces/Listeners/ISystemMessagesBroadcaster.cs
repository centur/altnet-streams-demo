using GrainInterfaces.Model;
using Orleans;
using Orleans.Streams;

namespace GrainInterfaces.Listeners
{
	public interface ISystemMessagesBroadcaster : IAsyncObserver<ChatMsg>, IGrainWithGuidKey
	{
	}
}