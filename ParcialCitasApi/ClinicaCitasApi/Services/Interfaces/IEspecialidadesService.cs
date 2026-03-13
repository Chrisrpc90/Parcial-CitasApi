using ClinicaCitasApi.Dtos.Especialidades;

namespace ClinicaCitasApi.Services.Interfaces
{
    public interface IEspecialidadesService
    {
        IEnumerable<EspecialidadResponse> GetAll();
        EspecialidadResponse GetById(int id);
        EspecialidadResponse Create(CreateEspecialidadRequest request);
        EspecialidadResponse Update(int id, UpdateEspecialidadRequest request);
        void Delete(int id);
    }
}
