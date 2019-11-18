using System;
using System.Runtime.Serialization;

namespace TwoPlayerSnake.Networking
{
    [DataContract]
    sealed class GamePacket
    {
        [DataMember]
        internal readonly Direction Direction;

        [DataMember]
        internal readonly DateTime CreationTime;

        internal GamePacket(Direction direction)
        {
            Direction = direction;
            CreationTime = DateTime.UtcNow;
        }
    }
}