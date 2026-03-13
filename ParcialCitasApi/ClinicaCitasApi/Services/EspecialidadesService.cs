using ClinicaCitasApi.Dtos.Especialidades;
using ClinicaCitasApi.Models;
using ClinicaCitasApi.Storage;

namespace ClinicaCitasApi.Services
{
    public class EspecialidadesService : Services.Interfaces.IEspecialidadesService
    {
        private readonly InMemoryDatabase _db;

        public EspecialidadesService(InMemoryDatabase db) => _db = db;

        public IEnumerable<EspecialidadResponse> GetAll()
            => _db.Especialidades.Values.Select(ToResponse).OrderBy(x => x.Id);

        public EspecialidadResponse GetById(int id)
        {
            if (!_db.Especialidades.TryGetValue(id, out var e))
                throw new KeyNotFoundException($"Especialidad {id} no existe.");
            return ToResponse(e);
        }

                public EspecialidadResponse Create(CreateEspecialidadRequest request)
            {
                var nombre = request.Nombre.Trim();

                //NO permitir nombres repetidos (incluye las 2 seed)
                var exists = _db.Especialidades.Values.Any(x =>
                    x.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                if (exists)
                    throw new InvalidOperationException($"Ya existe la especialidad '{nombre}'.");

                var esp = new Especialidad(_db.NextEspecialidadId(), nombre, request.Descripcion);
                _db.Especialidades[esp.Id] = esp;

                return ToResponse(esp);
            }

        public EspecialidadResponse Update(int id, UpdateEspecialidadRequest request)
        {
            if (!_db.Especialidades.TryGetValue(id, out var esp))
                throw new KeyNotFoundException($"Especialidad {id} no existe.");

            // evita duplicados al actualizar
            var exists = _db.Especialidades.Values.Any(x =>
                x.Id != id && x.Nombre.Equals(request.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));

            if (exists) throw new InvalidOperationException("Ya existe otra especialidad con ese nombre.");

            esp.Update(request.Nombre, request.Descripcion);
            return ToResponse(esp);
        }

        public void Delete(int id)
        {
            if (!_db.Especialidades.Remove(id))
                throw new KeyNotFoundException($"Especialidad {id} no existe.");

            // Regla opcional: si hay médicos usando esa especialidad, podrías bloquear.
            // (No lo forzamos para el parcial, pero es buena práctica).
        }

        private static EspecialidadResponse ToResponse(Especialidad e) => new()
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Descripcion = e.Descripcion
        };
    }
}
