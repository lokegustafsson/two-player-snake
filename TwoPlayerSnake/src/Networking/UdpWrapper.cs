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
        private List<T> _received;
        private DataContractSerializer _serializer;

        internal bool Pending { get { return _received.Count != 0; } }

        internal UdpWrapper(UdpClient client)
        {
            _client = client;
            _serializer = new DataContractSerializer(typeof(T));

            _received = new List<T>();
            _client.BeginReceive(OnReceive, null);
        }
        public void Dispose() => _client.Dispose();

        internal void Send(T payload)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, payload);
                byte[] dgram = ms.ToArray();
                _client.Send(dgram, dgram.Length);
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
            IPEndPoint origin = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = _client.EndReceive(result, ref origin);
            Debug.Assert(origin == _client.Client.RemoteEndPoint);

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
                Program.Log(this).Error(e, "Received non-{T}", typeof(T));
            }
            _client.BeginReceive(OnReceive, null);
        }
    }
}