using System;
using GHApp.Communication;

namespace GHApp.Contracts.Queries
{
	[Serializable]
	public class UserQuery : MessageBase
	{
		public string Name { get; private set; }

		public UserQuery(string name)
			: base(null)
		{
			Name = name;
		}
	}
}