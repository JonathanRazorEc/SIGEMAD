using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.AspNetCore.Http;

namespace DGPCE.Sigemad.Application.Contracts.Files;
public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string fileCategory);
    Task<string> SaveFileAsync(byte[] fileBytes, string fileName, string fileCategory);
    Task DeleteAsync(string filePath);
    Task<Stream> GetFileAsync(string filePath);

    Task<Archivo?> MapArchivo(FileDto archivo, Archivo? archivoExistente, string ARCHIVOS_PATH, bool actualizarFichero = false);

    Task<FileDto?> ProcesarArchivoRequest(IFormFile? archivo);

}
