using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using GHApp.Contracts.Dto;
using Newtonsoft.Json;

namespace GHApp.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GetUser("shanselman")
               .SelectMany(GetRepos)
               .SelectMany(r => r)
               .Subscribe(repo => { }, e => { });

            GetUser("shanselman")
               .SelectMany(GetRepos)
               .SelectMany(r => r)
               .SelectMany(GetCommits)
               .SelectMany(c => c)
               .Subscribe(commit => { }, e => { });

            Console.ReadLine();
        }

        private static IObservable<User> GetUser(string username)
        {
            Uri url = new Uri($"https://api.github.com/users/{username}");

            return Observable
                .Using(HttpClientFactory.CreateHttpClient, c => Observable.FromAsync(() => c.GetStringAsync(url)))
                .Select(JsonConvert.DeserializeObject<User>);
        }

        private static IObservable<IEnumerable<Repo>> GetRepos(User user)
        {
            Uri url = user.ReposUrl;

            return Observable
                .Using(HttpClientFactory.CreateHttpClient, c => Observable.FromAsync(() => c.GetStringAsync(url)))
                .Select(JsonConvert.DeserializeObject<IEnumerable<Repo>>);
        }

        private static IObservable<IEnumerable<Commit>> GetCommits(Repo repo)
        {
            Uri url = new Uri(repo.CommitsUrl.ToString().Replace("{/sha}", string.Empty));

            return Observable
                .Using(HttpClientFactory.CreateHttpClient, c => Observable.FromAsync(() => c.GetStringAsync(url)))
                .Select(JsonConvert.DeserializeObject<IEnumerable<Commit>>);
        }
    }

    public static class HttpClientFactory
    {
        public static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("nikodemrafalski:blabla");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Add("User-Agent", "Rx-Demo-App");
            return client;
        }
    }
}
