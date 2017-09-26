using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces;
using GrainInterfaces.Model;
using Orleans;
using Utils;

namespace GrainImplementation
{
	public class ChatRoom : Grain, IChatRoom
	{

		private readonly List<ChatMsg> _messages = new List<ChatMsg>(100);
		private readonly List<string> _onlineMembers = new List<string>(10);

		public override Task OnActivateAsync()
		{
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


			_onlineMembers.Add("Boromir");
			_onlineMembers.Add("Gandalf");
			_onlineMembers.Add("Gollum");

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

		public async Task<bool> Join(string nickname)
		{
			if (_onlineMembers.Contains(nickname)) return false;
			_onlineMembers.Add(nickname);

			PrettyConsole.Line($"==== System message: '{ nickname} joins the chat '{this.GetPrimaryKeyString()}' ...'", ConsoleColor.Green);
			

			return true;
		}

		public async Task<bool> Leave(string nickname)
		{
			if (!_onlineMembers.Contains(nickname)) return false;

			_onlineMembers.Remove(nickname);


			PrettyConsole.Line($"==== System message: '{ nickname} leaves the chat...'", ConsoleColor.Green);

			return true;
		}

		public async Task<bool> Message(ChatMsg msg)
		{
			_messages.Add(msg);


			var textHasMention = msg.Text.Contains("@");
			if (textHasMention)
			{
				PrettyConsole.Line($"==== MENTION DETECTED: '{msg.Author}' mentions someone. Sending a notification email.", ConsoleColor.Green);
			}

			return true;
		}

	}
}