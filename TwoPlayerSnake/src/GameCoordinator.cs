using Avalonia.Input;
using System;
using System.Diagnostics;
using System.Linq;
using TwoPlayerSnake.ViewModels;
using TwoPlayerSnake.Networking;
using TwoPlayerSnake.Game;

namespace TwoPlayerSnake
{
    sealed class GameCoordinator : Coordinator
    {
        private readonly GameViewModel _gameViewModel;
        private readonly InputManager _input;

        private Match _match;
        private UdpWrapper<GamePacket> _connection;

        internal GameCoordinator(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            _input = new InputManager();

            _match = new Match();
            _match.MatchFinishedEvent += OnMatchFinishedEvent;
        }
        internal void StartGame(UdpWrapper<GamePacket> connection)
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
            _connection = connection;
            _match = new Match();
            _match.MatchFinishedEvent += OnMatchFinishedEvent;
        }

        protected override void Update()
        {
            Stopwatch st = new Stopwatch();
            st.Start();

            Direction remoteMove;
            if (_connection != null)
            {
                Program.Log(this).Debug("Online");
                _connection.Send(new GamePacket(_input.Direction));
                // Placeholder, local and remote are currently out of sync
                remoteMove = _connection.GetReceived().Last().Direction;
            }
            else
            {
                Program.Log(this).Debug("Offline");
                remoteMove = Direction.Right;
            }

            _match.ApplyTurn(_input.Direction, remoteMove);
            _gameViewModel.Cells = _match.GetCells();

            st.Stop();
            Program.Log(this).Debug(String.Format("Update took {0} ms", st.ElapsedMilliseconds));
        }

        internal void OnKeyDown(KeyEventArgs args)
        {
            _input.OnKeyDown(args);
        }

        private void OnMatchFinishedEvent(MatchResult result)
        {
            Program.Log(this).Information("Match finished: {0}", result);
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
            _match = new Match();
            _match.MatchFinishedEvent += OnMatchFinishedEvent;
        }
    }
}