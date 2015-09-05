using System;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Notifications
{
    [Serializable]
    public class RepoNotification : MessageBase
    {
        public Repo Repo { get; set; }
    }
}