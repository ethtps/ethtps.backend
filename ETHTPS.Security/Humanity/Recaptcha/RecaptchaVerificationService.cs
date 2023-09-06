using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Data.Core.Models.ResponseModels.Recaptcha;

using Newtonsoft.Json;

namespace ETHTPS.API.Security.Core.Humanity.Recaptcha
{
    public sealed class RecaptchaVerificationService : IRecaptchaVerificationService
    {
        private readonly string? _privateKey;
        private readonly HttpClient _httpClient = new();
        private readonly DBConfigurationProviderWithCache _dBConfigurationProvider;

        public RecaptchaVerificationService(DBConfigurationProviderWithCache configurationProvider)
        {
            _privateKey = configurationProvider.GetFirstConfigurationString("RecaptchaSecretKey");
            _dBConfigurationProvider = configurationProvider;
        }

        public bool IsHumanityCheckRequired { get => _dBConfigurationProvider.GetFirstConfigurationString("RequireRecaptcha") == "true"; }

        public async Task<bool> CheckHumanityAsync(string recaptchaToken)
        {
            if (!IsHumanityCheckRequired)
                return true;

            var dictionary = new Dictionary<string, string>()
                    {
                        { "secret", _privateKey ?? "undefined" },
                        { "response", recaptchaToken }

                    };
            var postContent = new FormUrlEncodedContent(dictionary);
            var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent);
            var stringContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(stringContent))
            {
                return false;
            }
            var googleReCaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(stringContent);
            return googleReCaptchaResponse?.Success ?? false;
        }

    }
}
