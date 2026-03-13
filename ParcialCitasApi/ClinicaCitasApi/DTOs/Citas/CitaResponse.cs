namespace ClinicaCitasApi.Dtos.Citas
{
    public class CitaResponse
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        // datos relacionados para facilitar el consumo (evita hacer 3 llamadas para mostrar info básica)
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;

        public int MedicoId { get; set; }
        public string MedicoNombre { get; set; } = string.Empty;
        public string MedicoEspecialidad { get; set; } = string.Empty;
    }
}
