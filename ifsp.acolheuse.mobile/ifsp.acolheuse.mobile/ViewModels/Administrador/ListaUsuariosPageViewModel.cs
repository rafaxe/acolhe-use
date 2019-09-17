﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ifsp.acolheuse.mobile.Core.Repositories;
using Prism.Navigation;

namespace ifsp.acolheuse.mobile.ViewModels
{
    public class ListaUsuariosPageViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IAcaoRepository acaoRepository;

        public ListaUsuariosPageViewModel(INavigationService navigationService, IAcaoRepository acaoRepository) :
            base(navigationService)
        {
            this.navigationService = navigationService;
            this.acaoRepository = acaoRepository;
            Title = "My View A";
        }
    }
}