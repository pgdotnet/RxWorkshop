using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Serialization.Formatters.Binary;

namespace GHApp.Communication
{
	public class UdpCommunicationChannel : ICommunicationChannel
	{
		private readonly IUdpClientServer _udpClientServer;
		private readonly IChannelConfig _channelConfig;
		private readonly CompositeDisposable _disposable = new CompositeDisposable();
		private readonly object _lock = new object();

		private Subject<object> _messageSubject;

		public UdpCommunicationChannel(IUdpClientServer udpClientServer, IChannelConfig channelConfig)
		{
			_udpClientServer = udpClientServer;
			_channelConfig = channelConfig;
		}

		public IObservable<object> MessageStream
		{
			get
			{
				lock (_lock)
				{
					if (_messageSubject == null)
					{
						_messageSubject = new Subject<object>();
						_disposable.Add(_udpClientServer.Listen(_channelConfig.Port).Select(Deserialize).Subscribe(_messageSubject));
					}
				}
				return _messageSubject.AsObservable();
			}
		}

		public IObservable<Unit> SendMessage(object message)
		{
			if (message == null)
				throw new ArgumentNullException("message");
			if (!message.GetType().IsSerializable)
				throw new ArgumentException("Given type must be serializable!", "T");

			return _udpClientServer.Send(_channelConfig.Address, _channelConfig.Port, Serialize(message));
		}

		private byte[] Serialize(object data)
		{
			if (data == null)
				return new byte[0];

			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, data);
				return stream.ToArray();
			}
		}

		private object Deserialize(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;

			using (MemoryStream stream = new MemoryStream(bytes))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(stream);
			}
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}