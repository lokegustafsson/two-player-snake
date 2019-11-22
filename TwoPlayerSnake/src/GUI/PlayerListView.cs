using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake.GUI
{
    public class PlayerListView : UserControl
    {
        private List<Tuple<Player, InviteStatus>> Items;

        public PlayerListView()
        {
            AvaloniaXamlLoader.Load(this);
        }
        internal void Set(List<Player> availablePlayers, Dictionary<IPEndPoint, InviteStatus> context)
        {
            Items = availablePlayers.Select(x => Tuple.Create(x, context[x.PublicEndPoint])).ToList();
        }

        internal event Action<Player> MakeInvitationEvent;
        internal event Action<Player> WithdrawInvitationEvent;
        internal event Action<Player> RejectInvitationEvent;
        internal event Action<Player> AcceptInvitationEvent;
    }
}