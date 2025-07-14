using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DGPCE.Sigemad.Infrastructure.Services;
public class LocalFileService : IFileService
{
    private readonly string _folderBase;

    public LocalFileService(IConfiguration configuration)
    {
        _folderBase = configuration["Archivos:DirectorioBase"];
    }

    public async Task DeleteAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<Stream> GetFileAsync(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException();
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<Archivo?> MapArchivo(FileDto archivo, Archivo? archivoExistente,string ARCHIVOS_PATH, bool actualizarFichero = false)
    {

        if (actualizarFichero)
        {
            if (archivo != null)
            {
                var fileEntity = new Archivo
                {
                    NombreOriginal = archivo?.FileName ?? string.Empty,
                    NombreUnico = $"{Path.GetFileNameWithoutExtension(archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{archivo?.Extension ?? string.Empty}",
                    Tipo = archivo?.ContentType ?? string.Empty,
                    Extension = archivo?.Extension ?? string.Empty,
                    PesoEnBytes = archivo?.Length ?? 0
                };

                fileEntity.RutaDeAlmacenamiento = await SaveFileAsync(archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
                fileEntity.FechaCreacion = DateTime.Now;
                return fileEntity;
            }
            else {
                return null;
            }
        }

        return archivoExistente;
    }

    public async Task<FileDto?> ProcesarArchivoRequest(IFormFile? archivo)
    {

        if (archivo == null) return null;

        using var memoryStream = new MemoryStream();
        await archivo.CopyToAsync(memoryStream);

        return new FileDto
        {
            Extension = Path.GetExtension(archivo.FileName),
            Length = archivo.Length,
            FileName = archivo.FileName,
            ContentType = archivo.ContentType,
            Content = memoryStream.ToArray()
        };

    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string fileCategory)
    {
        // Generar la ruta de almacenamiento
        string year = DateTime.UtcNow.Year.ToString();
        string month = DateTime.UtcNow.Month.ToString("D2");

        var folderPath = Path.Combine(_folderBase, fileCategory, year, month);
        var filePath = Path.Combine(folderPath, fileName);

        // Crear directorios si no existen
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Guardar el archivo
        using var fileStreamToSave = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamToSave);

        return filePath;
    }

    public async Task<string> SaveFileAsync(byte[] fileBytes, string fileName, string fileCategory)
    {
        // Generar la ruta de almacenamiento
        string year = DateTime.UtcNow.Year.ToString();
        string month = DateTime.UtcNow.Month.ToString("D2");

        var folderPath = Path.Combine(_folderBase, fileCategory, year, month);
        var filePath = Path.Combine(folderPath, fileName);

        // Crear directorios si no existen
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Guardar el archivo
        await File.WriteAllBytesAsync(filePath, fileBytes);

        return filePath;
    }
}
