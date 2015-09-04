using System;

namespace GHApp.Service
{
    public class Demos
    {
        private readonly GithubBrowser _githubBrowser;

        public Demos()
        {
            _githubBrowser = new GithubBrowser(new HttpClientFactory());
        }

        public void Demo1()
        {
            _githubBrowser
                .GetUser("nikodemrafalski")
                .Subscribe(v => { }, error => { }, () => { });
        }
    }
}