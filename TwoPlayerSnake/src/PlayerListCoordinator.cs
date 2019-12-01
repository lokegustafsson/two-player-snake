using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TwoPlayerSnake.ViewModels;
using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake
{
    sealed class PlayerListCoordinator : Coordinator
    {
        private readonly PlayerListViewModel _playerView;
        private readonly PlayerDataViewModel _playerDataBox;
        private readonly UdpWrapper<Player> _multicaster;
        private readonly InvitationManager _invitationManager;

        internal PlayerListCoordinator(PlayerListViewModel listViewModel, PlayerDataViewModel dataViewModel, Action<UdpWrapper<GamePacket>> onJoinGameEvent)
        {

            _playerView = listViewModel;
            _playerDataBox = dataViewModel;
            {
                UdpClient multicastClient = new UdpClient(Config.MulticastEndPoint.Port);
                multicastClient.JoinMulticastGroup(Config.MulticastEndPoint.Address);
                multicastClient.MulticastLoopback = true;
                _multicaster = new UdpWrapper<Player>(multicastClient, Config.MulticastEndPoint, false);
            }
            _invitationManager = new InvitationManager();
            _invitationManager.JoinGameEvent += onJoinGameEvent;

            _playerDataBox.SetPublicEndPoint(_invitationManager.ListenerEndPoint);
        }

        protected override void Update()
        {
            List<Player> players = _multicaster.GetReceived();
            Program.Log(this).Debug("Multicaster recieved {num} players", players.Count);
            _invitationManager.SyncConnections(players.Select(x => x.PublicEndPoint).ToHashSet());

            _playerView.Set(players, _invitationManager.GetContext());

            _multicaster.Send(_playerDataBox.Actor);
        }
    }
}
