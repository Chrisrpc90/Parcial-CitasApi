using System.ComponentModel.DataAnnotations;

namespace ClinicaCitasApi.Dtos.Pacientes
{
    public class UpdatePacienteRequest
    {
        [Required, StringLength(60, MinimumLength = 2)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(60, MinimumLength = 2)]
        public string Apellidos { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress, StringLength(120)]
        public string? Email { get; set; }
    }
}