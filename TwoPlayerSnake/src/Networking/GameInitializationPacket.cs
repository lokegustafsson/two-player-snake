using System.Runtime.Serialization;

namespace TwoPlayerSnake.Networking
{
    [DataContract]
    sealed class GameInitializationPacket
    {
        [DataMember]
        internal InitRequest Request;
        // Soon: add synchronization stuff

        internal GameInitializationPacket(InitRequest request)
        {
            Request = request;
        }
    }
}