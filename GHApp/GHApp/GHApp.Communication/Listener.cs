using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Practices.Unity;

namespace GHApp.Communication
{
	/// <summary>
	/// Observes client messages, notifies on server stream
	/// </summary>
	/// <typeparam name="TQuery"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
	public class Listener<TQuery, TResponse> : IListener<TQuery, TResponse>
		where TQuery : class, IMessage
		where TResponse : class, IMessage
	{
		private readonly ICommunicationChannel _queryChannel;
		private readonly ICommunicationChannel _responseChannel;
		private readonly CompositeDisposable _disposable = new CompositeDisposable();

		public Listener([Dependency(ChannelNames.Client)]ICommunicationChannel queryChannel, [Dependency(ChannelNames.Server)]ICommunicationChannel responseChannel)
		{
			_queryChannel = queryChannel;
			_responseChannel = responseChannel;
		}

		public void Listen(Func<TQuery, TResponse> processFunc)
		{
			var disposable = _queryChannel.MessageStream
				.Where(m => m != null && m.GetType() == typeof(TQuery))
				.Cast<TQuery>()
				.Subscribe(m => _responseChannel.SendMessage(processFunc(m)));
			_disposable.Add(disposable);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}