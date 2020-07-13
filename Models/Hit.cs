using System.ComponentModel.DataAnnotations;

namespace ApiIntegrationBoilerplate.Models
{
    public class Hit
    {
        [Required]
        public string CardIdentity { get; set; }
    }
}
