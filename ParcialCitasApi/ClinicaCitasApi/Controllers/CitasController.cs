using ClinicaCitasApi.Dtos.Citas;
using ClinicaCitasApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaCitasApi.Controllers
{
    [ApiController]
    [Route("api/citas")]
    public class CitasController : ControllerBase
    {
        private readonly ICitasService _service;

        public CitasController(ICitasService service) => _service = service;

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] CreateCitaRequest request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateCitaRequest request)
            => Ok(_service.Update(id, request));

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/cancelar")]
        public IActionResult Cancelar(int id) => Ok(_service.Cancelar(id));
    }
}