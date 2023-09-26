using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dbragas.Models
{
    public class ClientPatchDTO
    {
        [StringLength(120)]
        [Unicode(false)]
        public string? Name { get; set; }

        [StringLength(150)]
        [Unicode(false)]
        public string? Email { get; set; }

        public Guid? TypeClientId { get; set; }

    }
    public class ClientCreateDto
    {
        [Required(ErrorMessage = "O campo Name é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo tipo de cliente é obrigatório")]
        public string TypeClient { get; set; }

       public Guid? TypeClientId {get; set; }   
    }
    public class ExcelImportDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TypeName { get; set; }
    }


}

