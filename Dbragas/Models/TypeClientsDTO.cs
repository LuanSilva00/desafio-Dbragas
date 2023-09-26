using System.ComponentModel.DataAnnotations;

public class TypeClientCreateDto
{
    [Required]
    public string Name { get; set; }
}