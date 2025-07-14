namespace DGPCE.Sigemad.Application.Contracts.Caching;

public interface ISIGEMemoryCache
{
    /// <summary>
    /// Establece un valor en caché usando la duración por defecto desde configuración.
    /// </summary>
    Task SetCacheAsync<T>(string key, T value, CancellationToken token = default) where T : class;

    /// <summary>
    /// Establece un valor en caché con una duración personalizada.
    /// </summary>
    Task SetCacheAsync<T>(string key, T value, int minutesToExpire, CancellationToken token = default) where T : class;

    /// <summary>
    /// Obtiene un valor desde la caché.
    /// </summary>
    Task<T?> GetCacheAsync<T>(string key, CancellationToken token = default) where T : class;

    /// <summary>
    /// Si la clave no existe, ejecuta el delegado para obtener el valor, lo guarda en caché y lo retorna. Usa la duración por defecto.
    /// </summary>
    Task<T?> SetCacheIfEmptyAsync<T>(string key, Func<Task<T>> setter, CancellationToken token = default) where T : class;

    /// <summary>
    /// Si la clave no existe, ejecuta el delegado para obtener el valor, lo guarda en caché con duración personalizada y lo retorna.
    /// </summary>
    Task<T?> SetCacheIfEmptyAsync<T>(string key, Func<Task<T>> setter, int minutesToExpire, CancellationToken token = default) where T : class;

    /// <summary>
    /// Elimina una clave de la caché.
    /// </summary>
    Task RemoveCache(string key, CancellationToken token = default);
}