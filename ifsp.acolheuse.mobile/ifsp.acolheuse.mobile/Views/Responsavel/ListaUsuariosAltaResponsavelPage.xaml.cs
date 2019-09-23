﻿using ifsp.acolheuse.mobile.Core.Domain;
using ifsp.acolheuse.mobile.Services;
using ifsp.acolheuse.mobile.ViewModels.Responsavel;
using Xamarin.Forms;

namespace ifsp.acolheuse.mobile.Views.Responsavel
{
    public partial class ListaUsuariosAltaResponsavelPage : ContentPage
    {
        private readonly ListaUsuariosAltaResponsavelPageViewModel _viewModel;
        public ListaUsuariosAltaResponsavelPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as ListaUsuariosAltaResponsavelPageViewModel;
        }


        protected override void OnAppearing()
        {

        }

        private async void LvPaciente_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (e.ItemData != null)
            {
                var item = (Paciente)e.ItemData;
                if (await MessageService.Instance.ShowAsyncYesNo("Deseja promover este usuário a lista de espera?"))
                {
                    _viewModel.PromoverListaEspera(item);
                }
            }
        }

        SearchBar searchBar = null;
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (lvPaciente.DataSource != null)
            {
                this.lvPaciente.DataSource.Filter = FilterListView;
                this.lvPaciente.DataSource.RefreshFilter();
            }
        }

        private bool FilterListView(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var pessoa = obj as Paciente;
            if (pessoa.NomeCompleto.ToLower().Contains(searchBar.Text.ToLower()) || pessoa.NomeCompleto.ToLower().Contains(searchBar.Text.ToLower()))
                return true;
            else
                return false;
        }
    }
}