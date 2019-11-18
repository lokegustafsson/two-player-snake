using System;
using System.Net;

namespace TwoPlayerSnake
{
    static class Config
    {
        internal const int GridSize = 20;

        internal static readonly TimeSpan GameStepTime = TimeSpan.FromMilliseconds(150);
        internal static readonly TimeSpan CommunicationStepTime = TimeSpan.FromMilliseconds(50);
        internal static readonly TimeSpan InputDelay = TimeSpan.FromMilliseconds(50);

        internal static readonly IPEndPoint MulticastEndPoint = new IPEndPoint(IPAddress.Parse("224.0.1.1"), 34525);
    }
}