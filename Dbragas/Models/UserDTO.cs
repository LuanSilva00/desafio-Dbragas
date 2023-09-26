using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(120, ErrorMessage = "O campo Nome deve ter no máximo {120} caracteres.")]
    [Unicode(false)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email é obrigatório.")]
    [StringLength(150, ErrorMessage = "O campo Email deve ter no máximo {150} caracteres.")]
    [Unicode(false)]
    [EmailAddress(ErrorMessage = "Email deve ser um endereço de email válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatório.")]
    [StringLength(14, ErrorMessage = "Senha deve ter no máximo {14} caracteres.")]
    [Unicode(false)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Username é obrigatório.")]
    [StringLength(120, ErrorMessage = "Username deve ter no máximo {120} caracteres.")]
    [Unicode(false)]
    public string Username { get; set; }
}
public class PatchUserDTO
{
    [StringLength(120, ErrorMessage = "O campo Nome deve ter no máximo {120} caracteres.")]
    [Unicode(false)]
    public string?Name { get; set; }
    [StringLength(150, ErrorMessage = "O campo Email deve ter no máximo {150} caracteres.")]
    [Unicode(false)]
    public string?Email { get; set; }
    [StringLength(120, ErrorMessage = "Username deve ter no máximo {120} caracteres.")]
    [Unicode(false)]
    public string?Username { get; set; }
}