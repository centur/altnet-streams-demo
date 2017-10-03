using System;
using System.Reflection;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.CodeGeneration;
using Utils;

namespace GrainImplementation
{
#pragma warning disable 618
	public class TickerGrain : Grain, ITicker, IGrainInvokeInterceptor
#pragma warning restore 618
	{
		private int counter;

		public override Task OnActivateAsync()
		{
			counter = 0;
			PrettyConsole.Line("ActivateAsync Called", ConsoleColor.Green);
			return base.OnActivateAsync();
		}


		public override Task OnDeactivateAsync()
		{
			PrettyConsole.Line("==== Deactivate Called ===", ConsoleColor.Cyan);

			return base.OnDeactivateAsync();
		}

		public Task<bool> Tick(int exceptionMod)
		{
			counter++;

			PrettyConsole.Line($"Tick: {counter}");

			if (counter >= 0 && counter % exceptionMod == 0)
			{
				PrettyConsole.Line("EXCEPTION !!!!", ConsoleColor.Red);
				throw new Exception("Bang !!!");
			}
			return Task.FromResult(true);
		}

		public async Task<object> Invoke(MethodInfo method, InvokeMethodRequest request, IGrainMethodInvoker invoker)
		{
			try
			{
				return await invoker.Invoke(this, request);
			}
			catch (Exception e)
			{
				PrettyConsole.Line("Exception in interceptor");
				DeactivateOnIdle();
				throw;
			}
		}
	}
}