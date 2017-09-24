using System.Threading.Tasks;
using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces
{
	public interface IChatRoom : IGrainWithStringKey
	{
		Task<ChatMsg[]> ReadHistory(int numberOfMessages);
		Task<bool> PostMessage(ChatMsg msg);
	}
}