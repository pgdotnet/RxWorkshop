using System;

namespace GHApp.Communication
{
	[Serializable]
	public abstract class MessageBase : IMessage
	{
		public Guid Id { get; private set; }

		protected MessageBase(Guid? id = null)
		{
			Id = id ?? Guid.NewGuid();
		}
	}
}