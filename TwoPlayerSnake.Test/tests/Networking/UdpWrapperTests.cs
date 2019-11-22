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
            string msg = "testingCanLoopback";

            using (var wrapper = new UdpWrapper<string>(client, localEP, true))
            {
                wrapper.Send(msg);
                Thread.Sleep(10);
                Assert.True(wrapper.Pending);
                var recieved = wrapper.GetReceived();
                Assert.False(wrapper.Pending);
                Assert.Single(recieved);
                Assert.Equal(recieved.Single(), msg);
            }
        }

        [Fact]
        public void CanMulticastLoopback()
        {
            IPEndPoint multicastEP = Config.MulticastEndPoint;
            string msg = "testingCanMulticastLoopBack";

            UdpClient client = new UdpClient(multicastEP.Port);
            client.JoinMulticastGroup(multicastEP.Address);
            client.MulticastLoopback = true;

            using (var wrapper = new UdpWrapper<string>(client, multicastEP, false))
            {
                wrapper.Send(msg);
                Thread.Sleep(10);
                Assert.True(wrapper.Pending);
                var recieved = wrapper.GetReceived();
                Assert.False(wrapper.Pending);
                Assert.Single(recieved);
                Assert.Equal(recieved.Single(), msg);
            }
        }
    }
}