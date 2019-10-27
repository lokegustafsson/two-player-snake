using Avalonia;
using Avalonia.Input;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using TwoPlayerSnake.Game;
using TwoPlayerSnake.GUI;

namespace TwoPlayerSnake
{
    sealed class Coordinator
    {
        private Match _match;
        private GameView _gameView;
        private InputManager _input;

        internal event Action<Exception> PropogateExceptionEvent;

        internal Coordinator(AppWindow appWindow)
        {
            _match = new Match();
            _match.MatchFinishedEvent += OnMatchFinishedEvent;

            _gameView = appWindow.GetGameView();

            _input = new InputManager();
            appWindow.KeyDown += _input.OnKeyDown;
        }

        internal void Update()
        {
            Stopwatch st = new Stopwatch(); st.Start();

            // Placeholder direction until networking is implemented
            _match.ApplyTurn(_input.Direction, Direction.Right);
            _gameView.Update(_match.GetCells());

            st.Stop();
            Log.ForContext("Area", "Coordinator").Debug(String.Format("Update took {0} ms", st.ElapsedMilliseconds));
        }

        private void OnMatchFinishedEvent(MatchResult result)
        {
            Log.ForContext("Area", "Coordinator").Information("Match finished: {0}", result);
            throw new NotImplementedException();
        }
    }
}