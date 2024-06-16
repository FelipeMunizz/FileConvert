using FileConvert.Domain.IServices;
using FileConvert.Entities.Returns;
using Microsoft.AspNetCore.Mvc;

namespace FileConvert.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private readonly IConverterService _service;

        public ConverterController(IConverterService service)
        {
            _service = service;
        }

        [HttpPost("ConverterToPdf")]
        public async Task<ActionResult<RetornoGenerico<object>>> ConverterToPdf(IFormFile file)
        {
            RetornoGenerico<byte[]> retornoService = await _service.ConverterFileToPdf(file);

            if (!retornoService.Success)
                return BadRequest(retornoService);

            RetornoGenerico<object> retorno = new RetornoGenerico<object>
            {
                Success = true,
                Message = retornoService.Message,
                Data = File(retornoService.Data, "application/pdf", Path.GetFileNameWithoutExtension(file.FileName) + ".pdf")
            };

            return Ok(retorno);
        }
    }
}
