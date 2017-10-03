using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
	public interface ITicker: IGrainWithGuidKey
	{
		Task<bool> Tick(int exceptionMod);
	}
}