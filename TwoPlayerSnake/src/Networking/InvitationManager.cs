using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace TwoPlayerSnake.Networking
{
    /// <remarks>
    /// The <see cref="InvitationManager"/> handles all tasks that should be disabled when in a game.
    /// </remarks>
    sealed class InvitationManager
    {
        private TcpListener _listener;
        private Dictionary<IPEndPoint, Tuple<InviteStatus, TcpWrapper<GameInitializationPacket>>> _connections;

        public bool AcceptingConnections { get; private set; }
        public IPEndPoint ListenerEndPoint { get; }

        internal InvitationManager()
        {
            RestartAcceptingConnections();
            
            // create a listener and let the underlying provider determine the local endpoint
            _listener = new TcpListener(IPAddress.Any, 0);
            ListenerEndPoint = (IPEndPoint)_listener.LocalEndpoint;
            _connections = new Dictionary<IPEndPoint, Tuple<InviteStatus, TcpWrapper<GameInitializationPacket>>>();

            _listener.Start();
            _listener.BeginAcceptTcpClient(AcceptConnection, null);

        }
        internal void RestartAcceptingConnections()
        {
            if (AcceptingConnections)
            {
                throw new ArgumentException("This function can only be called on a non-accepting instance of `InvitationManager`");
            }
            AcceptingConnections = true;
        }
        internal void CloseDeadConnections(HashSet<IPEndPoint> liveConnections)
        {
            foreach (IPEndPoint deadKey in _connections.Keys.Where(x => !liveConnections.Contains(x)))
            {
                _connections[deadKey].Item2?.Dispose();
                _connections.Remove(deadKey);
            }
            foreach (IPEndPoint newKey in liveConnections.Where(x => !_connections.ContainsKey(x)))
            {
                _connections.Add(newKey, NewEmptyConnection());
            }
        }
        internal Dictionary<IPEndPoint, InviteStatus> GetContext()
        {
            var toReturn = new Dictionary<IPEndPoint, InviteStatus>();
            foreach (var pair in _connections)
            {
                toReturn.Add(pair.Key, pair.Value.Item1);
            }
            return toReturn;
        }

        internal void AcceptInvitationFrom(IPEndPoint player)
        {
            SendAndClose(player, InviteStatus.SentByRemote, InitRequest.Accept);
            JoinGameWith(player);
        }
        internal void RejectInvitationFrom(IPEndPoint player) =>
            SendAndClose(player, InviteStatus.SentByRemote, InitRequest.Reject);

        internal void WithdrawInvitationTo(IPEndPoint player) =>
            SendAndClose(player, InviteStatus.SentByUs, InitRequest.Withdraw);

        internal void SendInvitationTo(IPEndPoint player)
        {
            if (_connections[player].Item1 != InviteStatus.None)
            {
                throw new ArgumentException("Passed player must not be invited/have invited us");
            }
            TcpClient client = new TcpClient();
            client.Connect(player);
            var wrapper = new TcpWrapper<GameInitializationPacket>(client);
            wrapper.ReceiveEvent += () =>
            {
                while (wrapper.Received.Any())
                {
                    GameInitializationPacket packet;
                    wrapper.Received.TryDequeue(out packet);
                    switch (packet.Request)
                    {
                        case InitRequest.Accept:
                            JoinGameWith(player);
                            break;
                        case InitRequest.Reject:
                            wrapper.Dispose();
                            _connections[player] = NewEmptyConnection();
                            break;
                    }
                }
            };
            _connections[player] = Tuple.Create(InviteStatus.SentByUs, wrapper);
        }

        internal event Action<UdpWrapper<GamePacket>> JoinGameEvent;

        #region Private methods

        private void AcceptConnection(IAsyncResult result)
        {
            TcpClient client = _listener.EndAcceptTcpClient(result);
            _listener.BeginAcceptTcpClient(AcceptConnection, null);

            IPEndPoint player = (IPEndPoint)client.Client.RemoteEndPoint;
            var wrapper = new TcpWrapper<GameInitializationPacket>(client);

            if (!AcceptingConnections)
            {
                wrapper.Send(new GameInitializationPacket(InitRequest.Reject));
                wrapper.Dispose();
                return;
            }

            // can only accept invitations from remotes with no preexisting invites
            if (_connections[player].Item1 == InviteStatus.None)
            {
                _connections[player] = Tuple.Create(InviteStatus.SentByRemote, wrapper);
                wrapper.ReceiveEvent += () =>
                {
                    wrapper.Dispose();
                    _connections[player] = NewEmptyConnection();
                };
            }
            else
            {
                Program.Log(this).Warning("TcpListener received invalid request");
            }
        }

        /// <summary>
        /// Helper function for withdrawing and rejecting, i.e. when something is sent to a player before closing the connection
        /// </summary>
        private void SendAndClose(IPEndPoint player, InviteStatus expectedStatus, InitRequest request)
        {
            if (expectedStatus == InviteStatus.SentByUs && _connections[player].Item1 != InviteStatus.SentByUs)
            {
                throw new ArgumentException("Passed player must already be invited");
            }
            if (expectedStatus == InviteStatus.SentByRemote && _connections[player].Item1 != InviteStatus.SentByRemote)
            {
                throw new ArgumentException("Passed player must already have sent an invitation");
            }
            TcpWrapper<GameInitializationPacket> wrapper = _connections[player].Item2;
            wrapper.Send(new GameInitializationPacket(request));
            wrapper.Dispose();
            _connections[player] = NewEmptyConnection();
        }

        private void JoinGameWith(IPEndPoint player)
        {
            AcceptingConnections = false;
            // withdraw all and reject all
            foreach (var pair in _connections)
            {
                switch (pair.Value.Item1)
                {
                    case InviteStatus.SentByUs:
                        WithdrawInvitationTo(pair.Key);
                        continue;
                    case InviteStatus.SentByRemote:
                        RejectInvitationFrom(pair.Key);
                        continue;
                }
            }
            // udp setup and return
            UdpClient client = new UdpClient();
            client.Connect(player);
            JoinGameEvent(new UdpWrapper<GamePacket>(client));
        }

        private Tuple<InviteStatus, TcpWrapper<GameInitializationPacket>> NewEmptyConnection() =>
            new Tuple<InviteStatus, TcpWrapper<GameInitializationPacket>>(InviteStatus.None, null);

        #endregion
    }
}