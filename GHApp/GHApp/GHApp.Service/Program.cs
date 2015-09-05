using System;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using GHApp.Communication;
using GHApp.Contracts;
using GHApp.Contracts.Notifications;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.Service
{
    public class Program
    {
        private static IGitHubUserSearchService _userService;

        public static void Main()
        {
            var serverAddress = ConfigurationManager.AppSettings.Get("ServerAddress");
            var serverPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));

            var clientAddress = ConfigurationManager.AppSettings.Get("ClientAddress");
            var clientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));

            var cs = new UdpClientServer();
            var serverChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = serverAddress, Port = serverPort });
            var clientChannel = new UdpCommunicationChannel(cs, new ChannelConfig { Address = clientAddress, Port = clientPort });

            _userService = new GitHubUserSearchService(new HttpClientFactory());

            var listener = new Listener<UserQuery, UserResponse>(clientChannel, serverChannel);
            listener.ListenObservable(GetUser);

            var publisher = new Publisher<RepoNotification>(serverChannel);
            Observable.Interval(TimeSpan.FromSeconds(2))
                .Select(_ => new RepoNotification())
                .Subscribe(publisher);

            var demos = new Demos();
            demos.WatchRepoDemo();
        }

        private static IObservable<UserResponse> GetUser(UserQuery query)
        {
            Console.WriteLine("Got UserQuery({0}), Id = {1}", query.Name, query.Id);
            return _userService.FindUser(query.Name)
                .Select(u => new UserResponse(query.Id, u.ToList()));
        }
    }
}