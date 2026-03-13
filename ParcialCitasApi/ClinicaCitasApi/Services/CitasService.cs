using ClinicaCitasApi.Dtos.Citas;
using ClinicaCitasApi.Models;
using ClinicaCitasApi.Storage;

namespace ClinicaCitasApi.Services
{
    public class CitasService : Services.Interfaces.ICitasService
    {
        private readonly InMemoryDatabase _db;

        public CitasService(InMemoryDatabase db) => _db = db;

        public IEnumerable<CitaResponse> GetAll()
            => _db.Citas.Values.Select(ToResponse).OrderBy(x => x.Id);

        public CitaResponse GetById(int id)
        {
            if (!_db.Citas.TryGetValue(id, out var cita))
                throw new KeyNotFoundException($"Cita {id} no existe.");
            return ToResponse(cita);
        }

        public CitaResponse Create(CreateCitaRequest request)
        {
            EnsurePacienteMedicoExist(request.PacienteId, request.MedicoId);
            EnsureNoOverlap(request.MedicoId, request.FechaHora, ignoreCitaId: null);

            var cita = new Cita(_db.NextCitaId(), request.FechaHora, request.Motivo, request.PacienteId, request.MedicoId);
            _db.Citas[cita.Id] = cita;
            return ToResponse(cita);
        }

        public CitaResponse Update(int id, UpdateCitaRequest request)
        {
            if (!_db.Citas.TryGetValue(id, out var cita))
                throw new KeyNotFoundException($"Cita {id} no existe.");

            EnsurePacienteMedicoExist(request.PacienteId, request.MedicoId);
            EnsureNoOverlap(request.MedicoId, request.FechaHora, ignoreCitaId: id);

            cita.Update(request.FechaHora, request.Motivo, request.PacienteId, request.MedicoId);
            return ToResponse(cita);
        }

        public void Delete(int id)
        {
            if (!_db.Citas.Remove(id))
                throw new KeyNotFoundException($"Cita {id} no existe.");
        }

        public CitaResponse Cancelar(int id)
        {
            if (!_db.Citas.TryGetValue(id, out var cita))
                throw new KeyNotFoundException($"Cita {id} no existe.");

            cita.Cancelar();
            return ToResponse(cita);
        }

        private void EnsurePacienteMedicoExist(int pacienteId, int medicoId)
        {
            if (!_db.Pacientes.ContainsKey(pacienteId))
                throw new KeyNotFoundException($"Paciente {pacienteId} no existe.");
            if (!_db.Medicos.ContainsKey(medicoId))
                throw new KeyNotFoundException($"Médico {medicoId} no existe.");
        }

        // Regla pro: un médico no puede tener 2 citas a la misma hora exacta
        private void EnsureNoOverlap(int medicoId, DateTime fechaHora, int? ignoreCitaId)
        {
            var conflict = _db.Citas.Values.Any(c =>
                c.MedicoId == medicoId &&
                c.FechaHora == fechaHora &&
                (ignoreCitaId == null || c.Id != ignoreCitaId));

            if (conflict)
                throw new ArgumentException("El médico ya tiene una cita programada en esa fecha/hora.");
        }

        private CitaResponse ToResponse(Cita c)
        {
            var paciente = _db.Pacientes[c.PacienteId];
            var medico = _db.Medicos[c.MedicoId];
            var especialidad = _db.Especialidades[medico.EspecialidadId];

            return new CitaResponse
            {
                Id = c.Id,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                Estado = c.Estado.ToString(),

                // muestra creacion y actualizacion para auditoría 
                FechaCreacion = c.FechaCreacion,
                FechaActualizacion = c.FechaActualizacion,

                PacienteId = paciente.Id,
                PacienteNombre = $"{paciente.Nombres} {paciente.Apellidos}",

                MedicoId = medico.Id,
                MedicoNombre = $"{medico.Nombres} {medico.Apellidos}",
                MedicoEspecialidad = especialidad.Nombre
            };
        }
    }
}
