using FileConvert.Domain.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Xceed.Words.NET;

namespace FileConvert.Infra.Repository
{
    public class ConverterRepository : IConverter
    {
        public async Task<byte[]> ConverterFile(byte[] fileBytes, string extension)
        {
            return await Task.Run(() =>
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var document = new Document())
                    {
                        var writer = PdfWriter.GetInstance(document, outputStream);
                        document.Open();

                        switch (extension.ToLower())
                        {
                            case ".txt":
                                AddTextToPdf(document, fileBytes);
                                break;
                            case ".html":
                                AddHtmlToPdf(document, writer, fileBytes);
                                break;
                            case ".docx":
                                AddDocxToPdf(document, fileBytes);
                                break;
                            case ".jpg":
                            case ".jpeg":
                            case ".png":
                                AddImageToPdf(document, fileBytes);
                                break;
                        }

                        document.Close();
                    }
                    return outputStream.ToArray();
                }
            });
        }

        private void AddTextToPdf(Document document, byte[] fileBytes)
        {
            var text = System.Text.Encoding.UTF8.GetString(fileBytes);
            document.Add(new Paragraph(text));
        }

        private void AddHtmlToPdf(Document document, PdfWriter writer, byte[] fileBytes)
        {
            using (var reader = new StringReader(System.Text.Encoding.UTF8.GetString(fileBytes)))
            {
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);
            }
        }

        private void AddImageToPdf(Document document, byte[] fileBytes)
        {
            var image = Image.GetInstance(fileBytes);
            document.Add(image);
        }

        private void AddDocxToPdf(Document document, byte[] fileBytes)
        {
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                var docx = DocX.Load(memoryStream);
                foreach (var paragraph in docx.Paragraphs)
                {
                    document.Add(new Paragraph(paragraph.Text));
                }
            }
        }
    }
}
