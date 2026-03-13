using ClinicaCitasApi.Dtos.Medicos;

namespace ClinicaCitasApi.Services.Interfaces
{
    public interface IMedicosService
    {
        IEnumerable<MedicoResponse> GetAll();
        MedicoResponse GetById(int id);
        MedicoResponse Create(CreateMedicoRequest request);
        MedicoResponse Update(int id, UpdateMedicoRequest request);
        void Delete(int id);
    }
}