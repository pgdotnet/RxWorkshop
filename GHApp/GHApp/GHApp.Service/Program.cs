using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using GHApp.Communication;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using GHApp.Contracts.Notifications;
using GHApp.Contracts.Queries;
using GHApp.Contracts.Responses;

namespace GHApp.Service
{
    public class Program
    {
        private static IGitHubUserSearchService _userService;
        private static IGithubBrowser _githubBrowser;

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
            _githubBrowser = new GithubBrowser(new HttpClientFactory());

            var userListener = new Listener<UserQuery, UserResponse>(clientChannel, serverChannel);
            userListener.ListenObservable(GetUser);

            var repoListener = new Listener<RepoQuery, RepoResponse>(clientChannel, serverChannel);
            repoListener.ListenObservable(GetRepo);

            var publisher = new Publisher<RepoNotification>(serverChannel);
            _githubBrowser.NewCommitsFeed
                .Select(c => new RepoNotification { Commit = c })
                .Subscribe(publisher);

            var demos = new Demos();
            demos.WatchRepoDemo();
        }

        private static IObservable<UserResponse> GetUser(UserQuery query)
        {
            Console.WriteLine("Got UserQuery({0}), Id = {1}", query.Name, query.Id);
            return _githubBrowser.GetUser("nikodemrafalski")
                .Select(u => new UserResponse(query.Id, new List<User> { u }));
            //return _userService.FindUser(query.Name)
            //    .Select(u => new UserResponse(query.Id, u.ToList()));
        }

        private static IObservable<RepoResponse> GetRepo(RepoQuery query)
        {
            Console.WriteLine("Got RepoQuery({0}), Id = {1}", query.User.Name, query.Id);
            return _githubBrowser.GetRepos(query.User)
                .Select(r => new RepoResponse(query.Id, r.ToList()));
        }
    }
}