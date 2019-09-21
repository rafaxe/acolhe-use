﻿using ifsp.acolheuse.mobile.Core.Repositories;
using Prism.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ifsp.acolheuse.mobile.Core.Domain;
using ifsp.acolheuse.mobile.Services;
using System.Threading.Tasks;
using Prism.Services;

namespace ifsp.acolheuse.mobile.ViewModels
{
    public class CadastroAcaoPageViewModel : ViewModelBase
    {
        #region commands
        public DelegateCommand _editarResponsaveisCommand { get; set; }
        public DelegateCommand _editarEstagiariosCommand { get; set; }
        public DelegateCommand _salvarAcaoCommand { get; set; }
        public DelegateCommand _configurarDiaCommand { get; set; }

        public DelegateCommand EditarResponsaveisCommand => _editarResponsaveisCommand ?? (_editarResponsaveisCommand = new DelegateCommand(EditarListaResponsaveisAsync));
        public DelegateCommand EditarEstagiariosCommand => _editarEstagiariosCommand ?? (_editarEstagiariosCommand = new DelegateCommand(EditarListaEstagiariosAsync));
        public DelegateCommand SalvarAcaoCommand => _salvarAcaoCommand ?? (_salvarAcaoCommand = new DelegateCommand(SalvarAcaoAsync));
        public DelegateCommand ConfigurarDiaCommand => _configurarDiaCommand ?? (_configurarDiaCommand = new DelegateCommand(ConfigurarDiaAsync));
        #endregion

        #region properties
        private Acao acao;
        private IEnumerable<Linha> linhasCollection;
        private string dia;
        private int tamanhoLvResponsaveis;
        private int tamanhoLvEstagiarios;
        private Linha linha;

        public int TamanhoLvResponsaveis
        {
            get { return tamanhoLvResponsaveis; }
            set { tamanhoLvResponsaveis = value; RaisePropertyChanged(); }
        }
        public int TamanhoLvEstagiarios
        {
            get { return tamanhoLvEstagiarios; }
            set { tamanhoLvEstagiarios = value; RaisePropertyChanged(); }
        }
        public Acao Acao
        {
            get { return acao; }
            set { acao = value; RaisePropertyChanged(); }
        }
        public IEnumerable<Linha> LinhasCollection
        {
            get { return linhasCollection; }
            set { linhasCollection = value; RaisePropertyChanged(); }
        }
        public string Dia
        {
            get { return dia; }
            set { dia = value; RaisePropertyChanged(); }
        }
        public List<string> DiasCollection
        {
            get { return new List<string> { "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira" }; }
        }
        public Linha Linha
        {
            get { return linha; }
            set { linha = value; RaisePropertyChanged(); }
        }
        #endregion

        private INavigationService navigationService;
        private IAcaoRepository acaoRepository;
        private ILinhaRepository linhaRepository;
        private IPageDialogService dialogService;

        public CadastroAcaoPageViewModel(INavigationService navigationService, IAcaoRepository acaoRepository, ILinhaRepository linhaRepository, IPageDialogService dialogService) :
           base(navigationService)
        {
            this.navigationService = navigationService;
            this.acaoRepository = acaoRepository;
            this.linhaRepository = linhaRepository;
            this.dialogService = dialogService;
            Acao = new Acao();
            Linha = new Linha();
            Title = "Cadastro de Ação";
        }

        public async void EditarListaResponsaveisAsync()
        {
            var navParameters = new NavigationParameters();
            navParameters.Add("acao", acao);
            await navigationService.NavigateAsync("EdicaoListaResponsaveisPage", navParameters);
        }
        public async void EditarListaEstagiariosAsync()
        {
            var navParameters = new NavigationParameters();
            navParameters.Add("acao", acao);
            await navigationService.NavigateAsync("EdicaoListaEstagiariosPage", navParameters);
        }
        public async void ConfigurarDiaAsync()
        {

            if (String.IsNullOrEmpty(Acao.Id))
            {
                var action = await dialogService.DisplayActionSheetAsync("Para configurar é necessário salvar a ação.", "Cancelar", null, "Salvar");

                if (action == "Cancelar")
                    return;

                Acao.GuidAcao = Guid.NewGuid().ToString();
                await acaoRepository.AddAsync(Acao);
                Acao = await acaoRepository.GetByGuidAsync(Acao.GuidAcao.ToString());
            }

            var navParameters = new NavigationParameters();
            navParameters.Add("acao", Acao);
            navParameters.Add("dia", Dia);
            await navigationService.NavigateAsync("HorarioAcaoPage", navParameters);
        }

        public async void SalvarAcaoAsync()
        {
            if(String.IsNullOrEmpty(Acao.GuidAcao))
                Acao.GuidAcao = Guid.NewGuid().ToString();

            this.Acao.IdLinha = Linha.Id;
            await acaoRepository.AddOrUpdateAsync(Acao, Acao.Id);
            await navigationService.GoBackAsync();
        }

        public async void CarregarLinhaAcao(string IdAcao)
        {
            try
            {
                Acao = await acaoRepository.GetAsync(IdAcao);
                Linha = await linhaRepository.GetAsync(Acao.IdLinha);
                LinhasCollection = await linhaRepository.GetAllAsync();
                Dia = DiasCollection[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void CarregarLinha()
        {
            LinhasCollection = await linhaRepository.GetAllAsync();
            Dia = DiasCollection[0];
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters["linha"] != null)
            {
                Linha = parameters["linha"] as Linha;
            }
            if (parameters["acao"] != null)
            {
                Acao = parameters["acao"] as Acao;
                CarregarLinhaAcao(Acao.Id);
            }
            else
            {
                CarregarLinha();
            }
        }
    }
}
