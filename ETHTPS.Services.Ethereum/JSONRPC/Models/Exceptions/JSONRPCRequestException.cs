using System;

namespace ETHTPS.Services.Ethereum.JSONRPC.Models.Exceptions
{
    public sealed class JSONRPCRequestException : Exception
    {
        public JSONRPCRequestException() : base()
        {

        }

        public JSONRPCRequestException(string message) : base(message)
        {

        }
    }
}
