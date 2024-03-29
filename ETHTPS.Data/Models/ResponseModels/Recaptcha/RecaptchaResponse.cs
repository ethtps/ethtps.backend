﻿using System;

using Newtonsoft.Json;

namespace ETHTPS.Data.Core.Models.ResponseModels.Recaptcha
{
    public sealed class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; } // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)

        [JsonProperty("hostname")]
        public string HostName { get; set; }    // the hostname of the site where the reCAPTCHA was solved

        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
