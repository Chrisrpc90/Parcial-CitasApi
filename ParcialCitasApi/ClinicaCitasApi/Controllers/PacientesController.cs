using ClinicaCitasApi.Dtos.Pacientes;
using ClinicaCitasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaCitasApi.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacientesService _service;

        public PacientesController(IPacientesService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] CreatePacienteRequest request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdatePacienteRequest request)
            => Ok(_service.Update(id, request));

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}