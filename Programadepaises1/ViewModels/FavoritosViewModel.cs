using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Programadepaises1.Models;
using Programadepaises1.Services;

namespace Programadepaises1.ViewModels;

public partial class FavoritosViewModel : ObservableObject
{
    private readonly IDbService _dbService;

    [ObservableProperty]
    private ObservableCollection<PaisModel> paisesFavoritos = new();

    [ObservableProperty]
    private bool estaCargando;

    [ObservableProperty]
    private bool hayFavoritos;

    public FavoritosViewModel(IDbService dbService)
    {
        _dbService = dbService;
    }

    [RelayCommand]
    private async Task CargarFavoritosAsync()
    {
        if (EstaCargando) return;

        try
        {
            EstaCargando = true;

            var favoritos = await _dbService.ObtenerFavoritosAsync();
            var paisesModelo = favoritos.Select(f => f.ToPaisModel()).ToList();

            PaisesFavoritos = new ObservableCollection<PaisModel>(paisesModelo);
            HayFavoritos = PaisesFavoritos.Any();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"No se pudieron cargar los favoritos: {ex.Message}",
                "OK");
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private async Task EliminarFavoritoAsync(PaisModel pais)
    {
        try
        {
            var confirmacion = await Application.Current!.MainPage!.DisplayAlert(
                "Confirmar eliminación",
                $"¿Estás seguro de que quieres eliminar {pais.NombreComun} de favoritos?",
                "Sí", "No");

            if (confirmacion)
            {
                var eliminado = await _dbService.EliminarFavoritoAsync(pais.NombreComun);
                if (eliminado)
                {
                    PaisesFavoritos.Remove(pais);
                    HayFavoritos = PaisesFavoritos.Any();

                    await Application.Current!.MainPage!.DisplayAlert(
                        "Favorito eliminado",
                        $"{pais.NombreComun} se eliminó de favoritos",
                        "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"Error al eliminar favorito: {ex.Message}",
                "OK");
        }
    }

    public async Task ActualizarFavoritosAsync()
    {
        await CargarFavoritosAsync();
    }
}
