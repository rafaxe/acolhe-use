﻿using ifsp.acolheuse.mobile.ViewModels;
using Xamarin.Forms;

namespace ifsp.acolheuse.mobile.Views.Administrador
{
    public partial class CadastroAcaoPage : ContentPage
    {
        private readonly CadastroAcaoPageViewModel _viewModel;

        public CadastroAcaoPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as CadastroAcaoPageViewModel;
        }
        protected override void OnAppearing()
        {
        }
        private void LvEstagiarios_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
        private void LvResponsaveis_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}