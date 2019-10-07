﻿using ifsp.acolheuse.mobile.ViewModels;
using ifsp.acolheuse.mobile.Core.Domain;
using Xamarin.Forms;
using ifsp.acolheuse.mobile.ViewModels.Responsavel;

namespace ifsp.acolheuse.mobile.Views.Responsavel
{
    public partial class CadastroServidorPage : ContentPage
    {
        private readonly CadastroServidorPageViewModel _viewModel;
        public CadastroServidorPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as CadastroServidorPageViewModel;
        }

        protected override void OnAppearing()
        {
        }

        private void LvEstagiario_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var estagiario = (Estagiario)e.Item;
                _viewModel.ItemTapped(estagiario);
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.CheckPass();
        }
    }
}
