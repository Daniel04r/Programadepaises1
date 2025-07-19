using Programadepaises1.ViewModels;

namespace Programadepaises1.Views;

public partial class FavoritosPage : ContentPage
{
    public FavoritosPage(FavoritosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is FavoritosViewModel viewModel)
        {
            await viewModel.CargarFavoritosCommand.ExecuteAsync(null);
        }
    }
}
