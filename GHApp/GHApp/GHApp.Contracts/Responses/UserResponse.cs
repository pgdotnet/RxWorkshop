using System;
using GHApp.Communication;
using GHApp.Contracts.Dto;

namespace GHApp.Contracts.Responses
{
	[Serializable]
	public class UserResponse : MessageBase
	{
		public User User { get; private set; }

		public UserResponse(Guid id, User user)
			: base(id)
		{
			User = user;
		}
	}
}