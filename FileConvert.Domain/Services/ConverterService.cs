using FileConvert.Domain.Interfaces;
using FileConvert.Domain.IServices;
using FileConvert.Entities.Returns;
using Microsoft.AspNetCore.Http;

namespace FileConvert.Domain.Services
{
    public class ConverterService : IConverterService
    {
        private readonly IConverter _repository;

        public ConverterService(IConverter repository)
        {
            _repository = repository;
        }

        public async Task<RetornoGenerico<byte[]>> ConverterFileToPdf(IFormFile file)
        {

            if (file == null || file.Length == 0)
                return new RetornoGenerico<byte[]>
                {
                    Success = false,
                    Message = "Arquivo não anexado",
                    Data = null
                };

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (fileExtension == ".mp3" || fileExtension == ".mp4")
                return new RetornoGenerico<byte[]>
                {
                    Success = false,
                    Message = "Arquivos multimidias não são suportados."
                };

            using(MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                byte[] pdfBytes = await _repository.ConverterFile(fileBytes, fileExtension);

                if (pdfBytes == null)
                    return new RetornoGenerico<byte[]>
                    {
                        Success = false,
                        Message = "Erro ao converter arquivo."
                    };

                return new RetornoGenerico<byte[]>
                {
                    Success = true,
                    Message = "Arquivo convertido com sucesso",
                    Data = pdfBytes
                };
            }            
        }
    }
}
