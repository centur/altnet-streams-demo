using GrainInterfaces.Model;
using Orleans;
using Orleans.Streams;

namespace GrainInterfaces.Listeners
{
	public interface IChatMessageListener : IAsyncObserver<ChatMsg>, IGrainWithGuidKey
	{
	}
}