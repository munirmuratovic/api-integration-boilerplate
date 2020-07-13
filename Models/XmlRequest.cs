using System.Xml.Serialization;

namespace ApiIntegrationBoilerplate.Models
{
    [XmlRoot(ElementName = "XMLREQUEST")]
    public class XmlRequest
    {
        [XmlElement(ElementName = "COMMAND")]
        public string Command { get; set; }

        [XmlAttribute(AttributeName = "sessionid")]
        public string SessionId { get; set; }
    }
}
