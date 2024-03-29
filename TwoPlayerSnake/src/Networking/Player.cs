using System;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;

namespace TwoPlayerSnake.Networking
{
    [DataContract]
    sealed class Player
    {
        /*
        The properties determine the external interface used in the application, while the private fields remain serializable
        */

        [DataMember]
        internal string Name;
        [DataMember]
        private string _publicEndPoint;
        [DataMember]
        private string _opponent;

        internal IPEndPoint PublicEndPoint
        {
            get { return Decode(_publicEndPoint); }
            set { _publicEndPoint = Encode(value); }
        }

        internal IPEndPoint Opponent
        {
            get { return Decode(_opponent); }
            set { _opponent = Encode(value); }
        }

        #region Private
        private const char seperator = ':';

        private string Encode(IPEndPoint endPoint)
        {
            return (endPoint == null) ? null : (endPoint.Address.ToString() + seperator + endPoint.Port.ToString());
        }

        private IPEndPoint Decode(string str)
        {
            string[] values = str.Split(seperator);
            return new IPEndPoint(IPAddress.Parse(values.First()), Int32.Parse(values.Last()));
        }
        #endregion
    }
}