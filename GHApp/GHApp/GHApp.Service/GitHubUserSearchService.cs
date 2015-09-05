using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GHApp.Contracts;
using GHApp.Contracts.Dto;

namespace GHApp.Service
{
    public class GitHubUserSearchService : IGitHubUserSearchService
    {
        private readonly IHttpClientFactory _factory;

        public GitHubUserSearchService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IObservable<IEnumerable<User>> FindUser(string userNamePart)
        {
            string queryString = "https://api.github.com/search/users?q={0}&page={1}&per_page=10";

            var subject = new ReplaySubject<IEnumerable<User>>();
            return subject.AsObservable();
        }
    }
}