namespace ClinicaCitasApi.Models
{
    public class Medico
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; } = string.Empty;
        public string Apellidos { get; private set; } = string.Empty;

        // ✅ Ahora se guarda por Id (Especialidad como entidad separada)
        public int EspecialidadId { get; private set; }

        public Medico(int id, string nombres, string apellidos, int especialidadId)
        {
            Id = id;
            SetNombres(nombres);
            SetApellidos(apellidos);
            SetEspecialidadId(especialidadId);
        }

        public void Update(string nombres, string apellidos, int especialidadId)
        {
            SetNombres(nombres);
            SetApellidos(apellidos);
            SetEspecialidadId(especialidadId);
        }

        private void SetNombres(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Nombres es requerido.");
            Nombres = value.Trim();
        }

        private void SetApellidos(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Apellidos es requerido.");
            Apellidos = value.Trim();
        }

        private void SetEspecialidadId(int value)
        {
            if (value <= 0) throw new ArgumentException("EspecialidadId debe ser mayor a 0.");
            EspecialidadId = value;
        }
    }
}
