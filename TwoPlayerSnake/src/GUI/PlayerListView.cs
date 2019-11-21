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
        private Dictionary<IPEndPoint, InviteStatus> _context;

        public PlayerListView()
        {
            AvaloniaXamlLoader.Load(this);
            Items = new List<Tuple<Player, InviteStatus>>();
        }
        internal void SetData(List<Player> playerData)
        {
            Items = playerData.Select(x =>
            {
                InviteStatus status;
                if (_context.TryGetValue(x.PublicEndPoint, out status))
                {
                    return Tuple.Create(x, status);
                }
                return null;
            }).Where(x => x != null).ToList();
        }
        internal void SetContext(Dictionary<IPEndPoint, InviteStatus> context)
        {
            _context = context;
            Items.RemoveAll(x => !_context.ContainsKey(x.Item1.PublicEndPoint));
            for (int i = 0; i < Items.Count; i++)
            {
                var (player, status) = Items[i];
                Items[i] = Tuple.Create(player, _context[player.PublicEndPoint]);
            }
        }

        internal event Action<Player> MakeInvitationEvent;
        internal event Action<Player> WithdrawInvitationEvent;
    }
}