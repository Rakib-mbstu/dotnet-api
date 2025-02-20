using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comments
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5,ErrorMessage ="Title must be greater than 5")]
        [MaxLength(15,ErrorMessage = "Tile must be less than 15")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5,ErrorMessage ="Title must be greater than 5")]
        [MaxLength(50,ErrorMessage = "Tile must be less than 15")]
        public string Content { get; set; } = string.Empty;
    }
}