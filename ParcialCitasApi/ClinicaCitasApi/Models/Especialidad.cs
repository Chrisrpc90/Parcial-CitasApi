namespace ClinicaCitasApi.Models
{
    // ✅ Nueva entidad solicitada: Especialidad como Web Service
    public class Especialidad
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }

        public Especialidad(int id, string nombre, string? descripcion = null)
        {
            Id = id;
            SetNombre(nombre);
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        }

        public void Update(string nombre, string? descripcion = null)
        {
            SetNombre(nombre);
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        }

        private void SetNombre(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Nombre de especialidad es requerido.");
            if (value.Trim().Length < 3) throw new ArgumentException("Nombre de especialidad mínimo 3 caracteres.");
            if (value.Trim().Length > 60) throw new ArgumentException("Nombre de especialidad máximo 60 caracteres.");
            Nombre = value.Trim();
        }
    }
}
