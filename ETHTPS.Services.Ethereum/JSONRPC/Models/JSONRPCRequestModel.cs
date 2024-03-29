﻿using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;

namespace ETHTPS.Services.Ethereum.JSONRPC.Models
{
    public sealed class JSONRPCRequestModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("jsonrpc")]
        public string JsonRPC { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("[]")]
        public List<object> Params { get; set; }
    }
}
