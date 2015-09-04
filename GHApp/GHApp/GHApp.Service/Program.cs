using GHApp.Communication;
using GHApp.Contracts.Dto;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.Service
{
	internal class Program
	{
		public static void Main()
		{
			var cs = new UdpClientServer();
			var serverChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = "127.0.0.1", Port = 12345 });
			var clientChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = "127.0.0.1", Port = 12346 });
			var listener = new Listener<UserQuery, UserResponse>(clientChannel, serverChannel);
			listener.Listen(x => new UserResponse(x.Id, new User { Bio = "100%" }));

			var demos = new Demos();
			demos.WatchRepoDemo();
		}
	}
}
