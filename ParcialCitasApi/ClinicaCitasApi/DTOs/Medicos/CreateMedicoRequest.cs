using System.ComponentModel.DataAnnotations;

public class CreateMedicoRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
    [MaxLength(60, ErrorMessage = "El nombre no puede exceder 60 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres.")]
    [MaxLength(60, ErrorMessage = "El apellido no puede exceder 60 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    //  se envía el Id de la especialidad (Separacion de entidades)
    [Range(1, int.MaxValue, ErrorMessage = "EspecialidadId debe ser mayor a 0.")]
    public int EspecialidadId { get; set; }
}
