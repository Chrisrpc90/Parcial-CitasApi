using System.ComponentModel.DataAnnotations;

namespace ClinicaCitasApi.Dtos.Medicos
{
    public class UpdateMedicoRequest
    {
        [Required, StringLength(60, MinimumLength = 2)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(60, MinimumLength = 2)]
        public string Apellidos { get; set; } = string.Empty;

        // Ahora se actualiza por Id (entidad separada)
        [Range(1, int.MaxValue, ErrorMessage = "EspecialidadId debe ser mayor a 0.")]
        public int EspecialidadId { get; set; }
    }
}
