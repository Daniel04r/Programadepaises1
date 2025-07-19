
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Programadepaises1.Models;
using Programadepaises1.Services;
using System.Collections.ObjectModel;

namespace Programadepaises1.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IPaisService _paisService;
    private readonly IDbService _dbService;

    [ObservableProperty]
    private ObservableCollection<PaisModel> paises = new();

    [ObservableProperty]
    private bool estaCargando;

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    private List<PaisModel> _todosLosPaises = new();

    public MainViewModel(IPaisService paisService, IDbService dbService)
    {
        _paisService = paisService;
        _dbService = dbService;
    }

    [RelayCommand]
    private async Task CargarPaisesAsync()
    {
        if (EstaCargando) return;

        try
        {
            EstaCargando = true;

            var paisesApi = await _paisService.ObtenerPaisesAsync();

            // Verificar cuáles son favoritos
            foreach (var pais in paisesApi)
            {
                pais.EsFavorito = await _dbService.EsFavoritoAsync(pais.NombreComun);
            }

            _todosLosPaises = paisesApi;
            Paises = new ObservableCollection<PaisModel>(paisesApi);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"No se pudieron cargar los países: {ex.Message}",
                "OK");
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task ToggleFavoritoAsync(PaisModel pais)
    {
        try
        {
            if (pais.EsFavorito)
            {
                // Quitar de favoritos
                var eliminado = await _dbService.EliminarFavoritoAsync(pais.NombreComun);
                if (eliminado)
                {
                    pais.EsFavorito = false;
                    await Application.Current!.MainPage!.DisplayAlert(
                        "Favorito eliminado",
                        $"{pais.NombreComun} se eliminó de favoritos",
                        "OK");
                }
            }
            else
            {
                // Agregar a favoritos
                var agregado = await _dbService.AgregarFavoritoAsync(pais);
                if (agregado)
                {
                    pais.EsFavorito = true;
                    await Application.Current!.MainPage!.DisplayAlert(
                        "Favorito agregado",
                        $"{pais.NombreComun} se agregó a favoritos",
                        "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"Error al actualizar favorito: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private void BuscarPaises()
    {
        if (string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            Paises = new ObservableCollection<PaisModel>(_todosLosPaises);
        }
        else
        {
            var paisesFiltrados = _todosLosPaises.Where(p =>
                p.NombreComun.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                p.Region.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                p.CapitalTexto.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            Paises = new ObservableCollection<PaisModel>(paisesFiltrados);
        }
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        BuscarPaises();
    }
}
