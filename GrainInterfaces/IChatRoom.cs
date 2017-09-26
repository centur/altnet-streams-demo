using System.Threading.Tasks;
using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces
{
	public interface IChatRoom : IGrainWithStringKey
	{
		Task<bool> Join(string nickname);
		Task<bool> Leave(string nickname);
		Task<bool> Message(ChatMsg msg);
		Task<ChatMsg[]> ReadHistory(int numberOfMessages);
	}
}