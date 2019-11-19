using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xunit;

using TwoPlayerSnake.Networking;

namespace TwoPlayerSnake.Test.Networking
{
    public class UdpWrapperTests
    {
        [Fact]
        public void CanLoopback()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Loopback, 34525);

            UdpClient client = new UdpClient(localEP);
            client.Connect(localEP);
            var wrapper = new UdpWrapper<string>(client);

            wrapper.Send("testing");
            Thread.Sleep(10);
            Assert.True(wrapper.Pending);
            var recieved = wrapper.GetReceived();
            Assert.Single(recieved);
            Assert.Equal(recieved.Single(), "testing");
        }
    }
}