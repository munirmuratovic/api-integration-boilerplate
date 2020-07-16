using ApiIntegrationBoilerplate.Models;
using System;
using System.Net.Http;

namespace ApiIntegrationBoilerplate.Services
{
    public interface ITokenManager
    {
        string GetToken();
        void SetBasicToken();
    }

    public class TokenManager : ITokenManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private Configuration _config;

        public TokenManager(IHttpClientFactory httpClientFactory, Configuration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public string GetToken()
        {
            if (string.IsNullOrEmpty(_config.Token))
            {
                SetBasicToken();
            }

            return _config.Token;
        }

        public void SetBasicToken()
        {
            string newToken = Convert.ToBase64String(
                System.Text.ASCIIEncoding.ASCII.GetBytes(
                    $"{_config.ConfigJson.Username}:{_config.ConfigJson.Password}"));

            _config.Token = newToken;
        }
    }
}
