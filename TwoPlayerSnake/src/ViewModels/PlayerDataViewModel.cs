using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics.ViewModels;
using System.Net;
using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake.ViewModels
{
    sealed class PlayerDataViewModel : ViewModel
    {
        private Player _actor;
        internal Player Actor
        {
            get => _actor;
            set
            {
                _actor = value;
                Notify(nameof(Actor));
            }
        }

        public PlayerDataViewModel()
        {
            Actor = new Player()
            {
                PublicEndPoint = null,
                Name = "MyName",
                Opponent = null
            };
        }
        internal void SetPublicEndPoint(IPEndPoint publicEndPoint)
        {
            Actor.PublicEndPoint = publicEndPoint;
            Notify(nameof(Actor));
        }
    }
}