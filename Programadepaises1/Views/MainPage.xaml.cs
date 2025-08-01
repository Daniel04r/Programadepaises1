﻿using Programadepaises1.ViewModels;

namespace Programadepaises1.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainViewModel viewModel)
        {
            await viewModel.CargarPaisesCommand.ExecuteAsync(null);
        }
    }
}
