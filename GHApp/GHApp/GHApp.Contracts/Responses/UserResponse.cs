using System;
using System.Collections.Generic;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Responses
{
    [Serializable]
    public class UserResponse : MessageBase
    {
        public List<User> Users { get; private set; }

        public UserResponse(Guid id, List<User> users)
            : base(id)
        {
            Users = users;
        }
    }
}