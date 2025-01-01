using System.ComponentModel.DataAnnotations;

namespace userAPI.DTOs
{
    public class UserCreationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
