using System.ComponentModel.DataAnnotations;

public class CreateCitaRequest
{
    [Required(ErrorMessage = "La fecha y hora es obligatoria.")]
    public DateTime FechaHora { get; set; }

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [MinLength(5, ErrorMessage = "El motivo debe tener al menos 5 caracteres.")]
    [MaxLength(120, ErrorMessage = "El motivo no puede exceder 120 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "PacienteId debe ser mayor a 0.")]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MedicoId debe ser mayor a 0.")]
    public int MedicoId { get; set; }
}