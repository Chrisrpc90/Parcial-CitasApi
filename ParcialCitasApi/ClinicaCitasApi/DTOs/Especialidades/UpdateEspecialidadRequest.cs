using System.ComponentModel.DataAnnotations;

namespace ClinicaCitasApi.Dtos.Especialidades
{
    public class UpdateEspecialidadRequest
    {
        [Required, StringLength(60, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }
    }
}
