namespace ApiIntegrationBoilerplate.Models
{
    public class Status
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public Status(string message)
        {
            Name = "API Integration 1.0";
            Message = message;
        }
    }
}
