using System;

namespace GHApp.Communication
{
    public interface ICommunicationChannel<T>
    {
        IObservable<T> MessageStream { get; }

        IObservable<int> SendMessage(T message);
    }
}