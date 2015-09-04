using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using Newtonsoft.Json;

namespace GHApp.Service
{
    public class GithubBrowser : IGithubBrowser
    {
        private readonly IHttpClientFactory _clientFactory;

        public GithubBrowser(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IObservable<User> GetUser(string username)
        {
            var uri = new Uri(string.Format("https://api.github.com/users/{0}", username));
            return WrapHttpGet<User>(uri);
        }

        public IObservable<IEnumerable<Repo>> GetRepos(User user)
        {
            return WrapHttpGet<IEnumerable<Repo>>(user.ReposUrl);
        }

        public IObservable<IEnumerable<Commit>> GetCommits(Repo repo)
        {
            var uri = new Uri(repo.CommitsUrl.ToString().Replace("{/sha}", string.Empty));
            return WrapHttpGet<IEnumerable<Commit>>(uri);
        }

        public IObservable<Unit> StopWatchingRepo(Repo repo)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> StartWatchingRepo(Repo repo)
        {
            throw new NotImplementedException();
        }

        public IObservable<Commit> NewCommitsFeed { get; }

        private IObservable<TResult> WrapHttpGet<TResult>(Uri uri)
        {
            return
                Observable
                    .Using(
                        _clientFactory.CreateHttpClient,
                        client => Observable.FromAsync(() => client.GetStringAsync(uri)))
                    .Select(JsonConvert.DeserializeObject<TResult>);
        }
    }
}