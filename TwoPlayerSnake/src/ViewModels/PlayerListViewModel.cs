using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake.ViewModels
{
    sealed class PlayerListViewModel : ViewModel
    {
        private List<Tuple<Player, InviteStatus>> _items;

        public List<Tuple<Player, InviteStatus>> Items
        {
            get => _items;
            private set
            {
                _items = value;
                Notify(nameof(Items));
            }
        }

        internal void Set(List<Player> availablePlayers, Dictionary<IPEndPoint, InviteStatus> context)
        {
            Items = availablePlayers.Select(x => Tuple.Create(x, context[x.PublicEndPoint])).ToList();
        }
    }
}