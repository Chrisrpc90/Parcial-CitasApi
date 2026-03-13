using ClinicaCitasApi.Dtos.Pacientes;
using ClinicaCitasApi.Models;
using ClinicaCitasApi.Storage;

namespace ClinicaCitasApi.Services
{
    public class PacientesService : Services.Interfaces.IPacientesService
    {
        private readonly InMemoryDatabase _db;

        public PacientesService(InMemoryDatabase db) => _db = db;

        public IEnumerable<PacienteResponse> GetAll()
            => _db.Pacientes.Values.Select(ToResponse).OrderBy(x => x.Id);

        public PacienteResponse GetById(int id)
        {
            if (!_db.Pacientes.TryGetValue(id, out var p))
                throw new KeyNotFoundException($"Paciente {id} no existe.");
            return ToResponse(p);
        }

        public PacienteResponse Create(CreatePacienteRequest request)
        {
            // DNI único (regla de negocio simple)
            if (_db.Pacientes.Values.Any(x => x.Dni == request.Dni.Trim()))
                throw new ArgumentException("Ya existe un paciente con ese DNI.");

            var paciente = new Paciente(_db.NextPacienteId(), request.Nombres, request.Apellidos, request.Dni, request.Telefono, request.Email);
            _db.Pacientes[paciente.Id] = paciente;
            return ToResponse(paciente);
        }

        public PacienteResponse Update(int id, UpdatePacienteRequest request)
        {
            if (!_db.Pacientes.TryGetValue(id, out var paciente))
                throw new KeyNotFoundException($"Paciente {id} no existe.");

            paciente.Update(request.Nombres, request.Apellidos, request.Telefono, request.Email);
            return ToResponse(paciente);
        }

        public void Delete(int id)
        {
            if (!_db.Pacientes.Remove(id))
                throw new KeyNotFoundException($"Paciente {id} no existe.");
        }

        private static PacienteResponse ToResponse(Paciente p) => new()
        {
            Id = p.Id,
            Nombres = p.Nombres,
            Apellidos = p.Apellidos,
            Dni = p.Dni,
            Telefono = p.Telefono,
            Email = p.Email
        };
    }
}