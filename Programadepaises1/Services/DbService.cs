using SQLite;
using Programadepaises1.Models;

namespace Programadepaises1.Services;

public interface IDbService
{
    Task<List<PaisFavorito>> ObtenerFavoritosAsync();
    Task<bool> AgregarFavoritoAsync(PaisModel pais);
    Task<bool> EliminarFavoritoAsync(string nombreComun);
    Task<bool> EsFavoritoAsync(string nombreComun);
}

public class DbService : IDbService
{
    private SQLiteAsyncConnection? _database;

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database is not null)
            return _database;

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "paises.db");
        _database = new SQLiteAsyncConnection(databasePath);

        await _database.CreateTableAsync<PaisFavorito>();

        return _database;
    }

    public async Task<List<PaisFavorito>> ObtenerFavoritosAsync()
    {
        try
        {
            var database = await GetDatabaseAsync();
            return await database.Table<PaisFavorito>()
                                 .OrderBy(p => p.NombreComun)
                                 .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al obtener favoritos: {ex.Message}");
            return new List<PaisFavorito>();
        }
    }

    public async Task<bool> AgregarFavoritoAsync(PaisModel pais)
    {
        try
        {
            var database = await GetDatabaseAsync();
            var favorito = PaisFavorito.FromPaisModel(pais);

            await database.InsertAsync(favorito);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al agregar favorito: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarFavoritoAsync(string nombreComun)
    {
        try
        {
            var database = await GetDatabaseAsync();
            var favorito = await database.Table<PaisFavorito>()
                                        .Where(p => p.NombreComun == nombreComun)
                                        .FirstOrDefaultAsync();

            if (favorito != null)
            {
                await database.DeleteAsync(favorito);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al eliminar favorito: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EsFavoritoAsync(string nombreComun)
    {
        try
        {
            var database = await GetDatabaseAsync();
            var favorito = await database.Table<PaisFavorito>()
                                        .Where(p => p.NombreComun == nombreComun)
                                        .FirstOrDefaultAsync();

            return favorito != null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al verificar favorito: {ex.Message}");
            return false;
        }
    }
}
