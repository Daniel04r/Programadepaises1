using System.Text.Json;
using Programadepaises1.Models;

namespace Programadepaises1.Services;

public interface IPaisService
{
    Task<List<PaisModel>> ObtenerPaisesAsync();
}

public class PaisService : IPaisService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://restcountries.com/v3.1/all?fields=name,flags,capital,region,population";

    public PaisService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PaisModel>> ObtenerPaisesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(BaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var paises = JsonSerializer.Deserialize<List<PaisModel>>(json);

                return paises?.OrderBy(p => p.NombreComun).ToList() ?? new List<PaisModel>();
            }

            return new List<PaisModel>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al obtener países: {ex.Message}");
            return new List<PaisModel>();
        }
    }
}
