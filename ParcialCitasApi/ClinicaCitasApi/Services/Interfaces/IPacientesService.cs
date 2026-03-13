using ClinicaCitasApi.Dtos.Pacientes;

namespace ClinicaCitasApi.Services.Interfaces
{
    public interface IPacientesService
    {
        IEnumerable<PacienteResponse> GetAll();
        PacienteResponse GetById(int id);
        PacienteResponse Create(CreatePacienteRequest request);
        PacienteResponse Update(int id, UpdatePacienteRequest request);
        void Delete(int id);
    }
}