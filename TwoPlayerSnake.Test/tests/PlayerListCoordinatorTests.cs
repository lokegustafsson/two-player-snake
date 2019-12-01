using System;
using System.Threading;
using TwoPlayerSnake.ViewModels;
using Xunit;

namespace TwoPlayerSnake.Test
{
    public sealed class PlayerListCoordinatorTests
    {
        /// <summary>
        /// We should find ourselves in the player list, so it should never be empty
        /// </summary>
        [Fact]
        public void PlayerListNotEmpty()
        {
            var listVM = new PlayerListViewModel();
            var dataVM = new PlayerDataViewModel();
            var coordinator = new PlayerListCoordinator(listVM, dataVM, (wrapper) => {});

            coordinator.Run(TimeSpan.FromMilliseconds(50));
            Thread.Sleep(100);
            Assert.NotEmpty(listVM.Items);
        }
    }
}