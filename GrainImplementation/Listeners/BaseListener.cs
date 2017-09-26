using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;
using Utils;

namespace GrainImplementation.Listeners
{
	public class BaseListener<TMessage> : Grain, IAsyncObserver<TMessage>, IGrainWithGuidKey
	{
		private readonly List<IAsyncStream<TMessage>> _streams = new List<IAsyncStream<TMessage>>();
		private readonly List<StreamSubscriptionHandle<TMessage>> _handles = new List<StreamSubscriptionHandle<TMessage>>();

		protected string[] StreamNamespaces { get; set; } = {""};
		protected string StreamProvider { get; set; } = FluentConfig.AltNetStream;

		public override async Task OnActivateAsync()
		{
			var streamProvider = GetStreamProvider(StreamProvider);

			foreach (var listenerNamespace in StreamNamespaces)
			{
				var stream = streamProvider.GetStream<TMessage>(this.GetPrimaryKey(), listenerNamespace);
				_streams.Add(stream);
				_handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));
			}


			await base.OnActivateAsync();
		}

		public virtual Task OnNextAsync(TMessage item, StreamSequenceToken token = null)
		{
			return Task.CompletedTask;
		}

		public virtual Task OnCompletedAsync()
		{
			return Task.CompletedTask;
		}

		public virtual Task OnErrorAsync(Exception ex)
		{
			return Task.CompletedTask;
		}
	}
}