using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.Model;
using Orleans;
using Orleans.Streams;
using Utils;

namespace GrainImplementation
{
	public class ChatRoom : Grain, IChatRoom
	{
		private List<ChatMsg> _messages = new List<ChatMsg>(100);

		private IStreamProvider _streamProvider;

		public override Task OnActivateAsync()
		{
			_streamProvider = base.GetStreamProvider(FluentConfig.AltNetStream);

			_messages.Add(new ChatMsg("System", "Gandalf joins the chat...") {Created = DateTimeOffset.Now.AddHours(-10)});
			_messages.Add(new ChatMsg("System", "Boromir joins the chat...") {Created = DateTimeOffset.Now.AddHours(-8)});
			_messages.Add(new ChatMsg("System", "Gollum joins the chat...") {Created = DateTimeOffset.Now.AddHours(-8)});
			_messages.Add(new ChatMsg("System", "Bilbo joins the chat...") {Created = DateTimeOffset.Now.AddMinutes(-10)});

			_messages.Add(new ChatMsg("Bilbo", "Hey guys!") {Created = DateTimeOffset.Now.AddMinutes(-9)});
			_messages.Add(new ChatMsg("Gandalf", "Yo, my little clever friend") {Created = DateTimeOffset.Now.AddMinutes(-8)});
			_messages.Add(
				new ChatMsg("Boromir", "Hello, you owe me a ring, amirite ?") {Created = DateTimeOffset.Now.AddMinutes(-7)});
			_messages.Add(
				new ChatMsg("Gollum", "Master bring us Our Preciousss....") {Created = DateTimeOffset.Now.AddMinutes(-6)});

			_messages.Add(new ChatMsg("System", "Bilbo leaves the chat...") {Created = DateTimeOffset.Now.AddMinutes(-5)});

			return base.OnActivateAsync();
		}

		public Task<ChatMsg[]> ReadHistory(int numberOfMessages)
		{
			var response = _messages
				.OrderByDescending(x => x.Created)
				.Take(numberOfMessages)
				.OrderBy(x => x.Created)
				.ToArray();

			return Task.FromResult(response);
		}

		public async Task<bool> Message(ChatMsg msg)
		{
			_messages.Add(msg);

			
			var stream = _streamProvider.GetStream<ChatMsg>(Guid.NewGuid(), nameof(ChatMsg));
			await stream.OnNextAsync(msg);

			return true;
		}
	}
}