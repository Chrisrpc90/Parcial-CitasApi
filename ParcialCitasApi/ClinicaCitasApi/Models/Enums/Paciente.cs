namespace ClinicaCitasApi.Models
{
    public class Paciente
    {
        public int Id { get; private set; }
        public string Nombres { get; private set; } = string.Empty;
        public string Apellidos { get; private set; } = string.Empty;
        public string Dni { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Email { get; private set; }

        public Paciente(int id, string nombres, string apellidos, string dni, string? telefono, string? email)
        {
            Id = id;
            SetNombres(nombres);
            SetApellidos(apellidos);
            SetDni(dni);
            Telefono = telefono;
            Email = email;
        }

        public void Update(string nombres, string apellidos, string? telefono, string? email)
        {
            SetNombres(nombres);
            SetApellidos(apellidos);
            Telefono = telefono;
            Email = email;
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

        private void SetDni(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("DNI es requerido.");
            Dni = value.Trim();
        }
    }
}