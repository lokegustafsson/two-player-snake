using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace TwoPlayerSnake.Networking
{
    sealed class UdpWrapper<T> : IDisposable
    {
        private UdpClient _client;
        private IPEndPoint _remote;
        private bool _exclusiveConnection;
        private List<T> _received;
        private DataContractSerializer _serializer;

        internal bool Pending { get { return _received.Count != 0; } }

        internal UdpWrapper(UdpClient client, IPEndPoint remote, bool exclusiveConnection)
        {
            _client = client;
            _remote = remote;
            _exclusiveConnection = exclusiveConnection;

            if (_client.Client.Connected)
            {
                throw new ArgumentException("The passed UdpClient must not be connected!");
            }
            if (exclusiveConnection)
            {
                _client.Connect(remote);
            }

            _received = new List<T>();
            _serializer = new DataContractSerializer(typeof(T));

            _client.BeginReceive(OnReceive, null);
        }
        public void Dispose() => _client.Dispose();

        internal void Send(T payload)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, payload);
                byte[] dgram = ms.ToArray();
                if (_exclusiveConnection)
                {
                    _client.Send(dgram, dgram.Length);
                }
                else
                {
                    _client.Send(dgram, dgram.Length, _remote);
                }
            }
        }

        internal List<T> GetReceived()
        {
            List<T> received;
            lock (_received)
            {
                received = _received;
                _received = new List<T>();
            }
            return received;
        }

        private void OnReceive(IAsyncResult result)
        {
            Program.Log(this).Debug("{this} received", this.GetType().Name);
            
            byte[] data;
            try
            {
                IPEndPoint origin = new IPEndPoint(IPAddress.Any, 0);
                data = _client.EndReceive(result, ref origin);
            }
            catch (ObjectDisposedException e)
            {
                Program.Log(this).Warning(e, "{this} recieved after disposing", this.GetType().Name);
                return;
            }

            _client.BeginReceive(OnReceive, null);

            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    T payload = (T)_serializer.ReadObject(ms);
                    lock (_received) { _received.Add(payload); }
                }
            }
            catch (SerializationException e)
            {
                // Simply ignore invalid received objects
                Program.Log(this).Warning(e, "Received non-{T}", typeof(T));
            }
        }
    }
}