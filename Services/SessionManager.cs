using ApiIntegrationBoilerplate.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApiIntegrationBoilerplate.Services
{
    public interface ISessionManager
    {
        Task<string> GetSessionAsync();
        Task SetSessionAsync();
    }

    public class SessionManager : ISessionManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private Configuration _configuration;

        public SessionManager(IHttpClientFactory httpClientFactory, Configuration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> GetSessionAsync()
        {
            if (_configuration.SessionId == null)
            {
                await SetSessionAsync();
            }
            return _configuration.SessionId;
        }

        /// <summary>Sets the session.</summary>
        public async Task SetSessionAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), _configuration.ConfigJson.ApiUrl);

            // Creates new object from JSON data
            XmlRequest loginRequestObj = new XmlRequest();
            loginRequestObj.Command = "Login";

            // XML Serialization
            using (StringWriter stringwriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlRequest));
                serializer.Serialize(stringwriter, loginRequestObj);
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
                var deserializer = new XmlSerializer(typeof(XmlResponse));
                result = (XmlResponse)deserializer.Deserialize(reader);
            }

            if (result.Response == "SUCCESS")
            {
                _configuration.SessionId = result.SessionId;
            }
            else
            {
                _configuration.SessionId = null;
            }
        }
    }
}
