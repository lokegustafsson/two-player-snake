using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TwoPlayerSnake.GUI;
using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake
{
    sealed class PlayerListCoordinator : Coordinator
    {
        private readonly PlayerListView _playerView;
        private readonly PlayerDataBox _playerDataBox;
        private readonly UdpWrapper<Player> _multicaster;
        private readonly InvitationManager _invitationManager;

        internal PlayerListCoordinator(AppWindow appWindow, Action<UdpWrapper<GamePacket>> onJoinGameEvent)
        {

            _playerView = appWindow.Locate<PlayerListView>();
            _playerDataBox = appWindow.Locate<PlayerDataBox>();
            {
                UdpClient multicastClient = new UdpClient();
                multicastClient.JoinMulticastGroup(Config.MulticastEndPoint.Address);
                multicastClient.Connect(Config.MulticastEndPoint);
                multicastClient.MulticastLoopback = true;
                _multicaster = new UdpWrapper<Player>(multicastClient);
            }
            _invitationManager = new InvitationManager();
            _invitationManager.JoinGameEvent += onJoinGameEvent;

            _playerDataBox.SetPublicEndPoint(_invitationManager.ListenerEndPoint);
        }

        protected override void Update()
        {
            _multicaster.Send(_playerDataBox.Data);
            _playerView.SetContext(_invitationManager.GetContext());

            List<Player> players = _multicaster.GetReceived();
            _playerView.SetData(players);
            _invitationManager.SyncConnections(players.Select(x => x.PublicEndPoint).ToHashSet());
        }
    }
}