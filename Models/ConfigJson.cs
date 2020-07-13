using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiIntegrationBoilerplate.Models
{
    public class ConfigJson
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ApiUrl { get; set; }

        [Required]
        public Actions Actions { get; set; }

        public ConfigJson()
        {
            Actions = new Actions();
        }
    }

    public class Actions
    {
        public IList<string> Entry { get; set; }
        public IList<string> Exit { get; set; }
    }
}