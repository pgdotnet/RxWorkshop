using System;
using System.Configuration;
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
            var serverAddress = ConfigurationManager.AppSettings.Get("ServerAddress");
            var serverPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));

            var clientAddress = ConfigurationManager.AppSettings.Get("ClientAddress");
            var clientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));

            var cs = new UdpClientServer();
            var serverChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = serverAddress, Port = serverPort });
            var clientChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = clientAddress, Port = clientPort });

            var listener = new Listener<UserQuery, UserResponse>(clientChannel, serverChannel);
            listener.Listen(GetUser);

            var demos = new Demos();
            demos.WatchRepoDemo();
        }

        private static UserResponse GetUser(UserQuery query)
        {
            Console.WriteLine("Got UserQuery({0}), Id = {1}", query.Name, query.Id);
            return new UserResponse(query.Id, new User { Bio = "100%" });
        }
    }
}
