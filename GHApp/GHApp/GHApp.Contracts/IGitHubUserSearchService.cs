using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts
{
    public interface IGitHubUserSearchService
    {
        IObservable<IEnumerable<User>> FindUser(string userNamePart);
    }
}
