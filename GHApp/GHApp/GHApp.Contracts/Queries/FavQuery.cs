using System;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Queries
{
    [Serializable]
    public class FavQuery : MessageBase
    {
        public Repo Repo { get; private set; }

        public FavQuery(Repo repo)
            : base(null)
        {
            Repo = repo;
        }
    }
}