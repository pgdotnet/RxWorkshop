using System;
using GHApp.Communication;

namespace GHApp.Contracts.Queries
{
    [Serializable]
    public class RepoQuery : MessageBase
    {
        public string Username { get; private set; }

        public RepoQuery(string username)
            : base(null)
        {
            Username = username;
        }
    }
}