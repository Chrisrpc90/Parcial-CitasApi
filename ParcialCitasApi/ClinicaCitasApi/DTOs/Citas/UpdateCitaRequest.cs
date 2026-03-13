using System.ComponentModel.DataAnnotations;

namespace ClinicaCitasApi.Dtos.Citas
{
    public class UpdateCitaRequest
    {
        [Required]
        public DateTime FechaHora { get; set; }

        [Required, StringLength(120, MinimumLength = 3)]
        public string Motivo { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)]
        public int PacienteId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int MedicoId { get; set; }
    }
}