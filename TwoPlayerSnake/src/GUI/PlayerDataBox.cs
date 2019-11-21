using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Net;

using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake.GUI
{
    public class PlayerDataBox : UserControl
    {
        internal Player Data { get; }

        public PlayerDataBox()
        {
            AvaloniaXamlLoader.Load(this);
            Data = new Player()
            {
                PublicEndPoint = null,
                Name = "MyName",
                Opponent = null
            };
        }
        internal void SetPublicEndPoint(IPEndPoint publicEndPoint)
        {
            Data.PublicEndPoint = publicEndPoint;
        }
    }
}