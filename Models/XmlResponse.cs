using System.Xml.Serialization;

namespace ApiIntegrationBoilerplate.Models
{
    [XmlRoot(ElementName = "XMLRESPONSE")]
    public class XmlResponse
    {
        [XmlElement(ElementName = "RESPONSE")]
        public string Response { get; set; }

        [XmlAttribute(AttributeName = "sessionid")]
        public string SessionId { get; set; }
    }
}
