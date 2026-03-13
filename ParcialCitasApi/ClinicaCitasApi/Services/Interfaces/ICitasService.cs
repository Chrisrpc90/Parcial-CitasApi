using ClinicaCitasApi.Dtos.Citas;

namespace ClinicaCitasApi.Services.Interfaces
{
    public interface ICitasService
    {
        IEnumerable<CitaResponse> GetAll();
        CitaResponse GetById(int id);
        CitaResponse Create(CreateCitaRequest request);
        CitaResponse Update(int id, UpdateCitaRequest request);
        void Delete(int id);
        CitaResponse Cancelar(int id);
    }
}