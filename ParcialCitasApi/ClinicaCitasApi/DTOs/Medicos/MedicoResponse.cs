namespace ClinicaCitasApi.Dtos.Medicos
{
    public class MedicoResponse
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;

        public int EspecialidadId { get; set; }
        public string EspecialidadNombre { get; set; } = string.Empty;
    }
}
