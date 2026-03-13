using ClinicaCitasApi.Models;

namespace ClinicaCitasApi.Storage
{
    public class InMemoryDatabase
    {
        // Map para IDs + performance
        public Dictionary<int, Paciente> Pacientes { get; } = new();
        public Dictionary<int, Medico> Medicos { get; } = new();
        public Dictionary<int, Cita> Citas { get; } = new();

        // entidad: Especialidad
        public Dictionary<int, Especialidad> Especialidades { get; } = new();

        private int _pacienteId = 0;
        private int _medicoId = 0;
        private int _citaId = 0;
        private int _especialidadId = 0;

        public int NextPacienteId() => ++_pacienteId;
        public int NextMedicoId() => ++_medicoId;
        public int NextCitaId() => ++_citaId;
        public int NextEspecialidadId() => ++_especialidadId;

        public InMemoryDatabase()
        {
            // Seed Pacientes
            var p1 = new Paciente(NextPacienteId(), "Jesus", "Lujan Carrion", "45896312", "920115789", "Lagarto123@gmail.com");
            var p2 = new Paciente(NextPacienteId(), "Ana", "Lopez", "87654321", "920115782", "AnaL521@gmail.com");
            Pacientes[p1.Id] = p1;
            Pacientes[p2.Id] = p2;

            // Especialidades por defecto (SIEMPRE existen al iniciar PARA PRUEBAS)
            var e1 = new Especialidad(NextEspecialidadId(), "Cardiología");
            var e2 = new Especialidad(NextEspecialidadId(), "Pediatría");
            Especialidades[e1.Id] = e1;
            Especialidades[e2.Id] = e2;

            //médicos ahora apuntan por EspecialidadId
            var m1 = new Medico(NextMedicoId(), "Carlos", "Perez", e1.Id);
            var m2 = new Medico(NextMedicoId(), "Maria", "Gomez", e2.Id);
            Medicos[m1.Id] = m1;
            Medicos[m2.Id] = m2;
        }
    }
}
