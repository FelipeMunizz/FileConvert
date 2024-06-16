using Microsoft.AspNetCore.Http;

namespace FileConvert.Domain.Interfaces;

public interface IConverter
{
    Task<byte[]> ConverterFile(byte[] file, string extension);
}
