using FileConvert.Entities.Returns;
using Microsoft.AspNetCore.Http;

namespace FileConvert.Domain.IServices;

public interface IConverterService
{
    Task<RetornoGenerico<byte[]>> ConverterFileToPdf(IFormFile file);
}
