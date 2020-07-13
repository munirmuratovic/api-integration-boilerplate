using ApiIntegrationBoilerplate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApiIntegrationBoilerplate.Services
{
    public interface IIntegrationService
    {
        Status GetHealth();
        Task<Configuration> SetConfigAsync(ConfigJson configJson);
        Task<XmlResponse> GetXmlResponseAsync(XmlRequest xmlRequestObject);
        Task<List<User>> GetAllUsersAsync();
        Task<HttpStatusCode> PostHitAsync(Hit hit);
    }

    public class IntegrationService : IIntegrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISessionManager _sessionManager;
        private readonly ITokenManager _tokenManager;
        private Configuration _configuration;

        public IntegrationService(IHttpClientFactory httpClientFactory, ISessionManager sessionManager, ITokenManager tokenManager, Configuration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _tokenManager = tokenManager;
            _configuration = configuration;
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Status GetHealth()
        {
            throw new NotImplementedException();
        }

        public async Task<XmlResponse> GetXmlResponseAsync(XmlRequest xmlRequestObject)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), _configuration.ConfigJson.ApiUrl);

            xmlRequestObject.SessionId = _configuration.SessionId;

            // XML Serialization
            using (StringWriter stringwriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlRequest));
                serializer.Serialize(stringwriter, xmlRequestObject);
                string serializedXml = stringwriter.ToString();
                request.Content = new StringContent(serializedXml);
            }

            // Getting response
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml");
            string response = await client.SendAsync(request).Result.Content.ReadAsStringAsync();

            // XML Deserialization
            XmlResponse result;
            using (TextReader reader = new StringReader(response))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(XmlResponse));
                result = (XmlResponse)deserializer.Deserialize(reader);
            }

            return result;
        }

        public Task<HttpStatusCode> PostHitAsync(Hit hit)
        {
            throw new NotImplementedException();
        }

        public Task<Configuration> SetConfigAsync(ConfigJson configJson)
        {
            throw new NotImplementedException();
        }
    }
}
