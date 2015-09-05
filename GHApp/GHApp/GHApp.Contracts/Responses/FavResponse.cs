using System;
using System.Collections.Generic;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Responses
{
    [Serializable]
    public class FavResponse : MessageBase
    {
        public FavResponse(Guid id)
            : base(id)
        {
        }
    }
}