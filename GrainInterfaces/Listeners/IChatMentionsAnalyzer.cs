using GrainInterfaces.Model;
using Orleans;
using Orleans.Streams;

namespace GrainInterfaces.Listeners
{
	public interface IChatMentionsAnalyzer : IAsyncObserver<ChatMsg>, IGrainWithGuidKey
	{
	}
}