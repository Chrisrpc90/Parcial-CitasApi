using ClinicaCitasApi.Dtos.Especialidades;
using ClinicaCitasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaCitasApi.Controllers
{
    [ApiController]
    [Route("api/especialidades")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadesService _service;

        public EspecialidadesController(IEspecialidadesService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] CreateEspecialidadRequest request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateEspecialidadRequest request)
            => Ok(_service.Update(id, request));

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
