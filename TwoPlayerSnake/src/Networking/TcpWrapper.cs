using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;

namespace TwoPlayerSnake.Networking
{
    sealed class TcpWrapper<T> : IDisposable
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly DataContractSerializer _serializer;
        private readonly CancellationTokenSource _cancelSource;

        internal readonly ConcurrentQueue<T> Received;

        internal TcpWrapper(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _serializer = new DataContractSerializer(typeof(T));
            _cancelSource = new CancellationTokenSource();
            Received = new ConcurrentQueue<T>();

            ThreadPool.QueueUserWorkItem<CancellationToken>(Receive, _cancelSource.Token, false);
        }
        public void Dispose()
        {
            _cancelSource.Cancel();
            _client.Dispose(); // also disposes the NetworkStream
        }

        internal void Send(T payload) => _serializer.WriteObject(_stream, payload);

        internal event Action ReceiveEvent;

        private void Receive(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!_stream.DataAvailable)
                {
                    continue;
                }
                try
                {
                    Received.Enqueue((T)_serializer.ReadObject(_stream));
                    ReceiveEvent();
                }
                catch (SerializationException e)
                {
                    // Simply ignore invalid received objects
                    Program.Log(this).Error(e, "Received non-{T}", typeof(T));
                }
            }
        }
    }
}