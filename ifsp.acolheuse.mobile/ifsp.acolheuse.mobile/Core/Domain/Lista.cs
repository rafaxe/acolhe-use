﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ifsp.acolheuse.mobile.Core.Domain
{
    public class Lista : BindableBase
    {
        private string id;
        private string nome;
        private bool adicionado;
        private bool isListaEspera;
        private bool isAtendimento;
        private bool isAlta;
        private bool isInterconsulta;

        public string Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Nome
        {
            get { return nome; }
            set { nome = value; OnPropertyChanged(); }
        }
        public bool Adicionado
        {
            get { return adicionado; }
            set { adicionado = value; OnPropertyChanged(); }
        }
        public bool IsListaEspera
        {
            get { return isListaEspera; }
            set { isListaEspera = value; OnPropertyChanged(); }
        }
        public bool IsAtendimento
        {
            get { return isAtendimento; }
            set { isAtendimento = value; OnPropertyChanged(); }
        }
        public bool IsAlta
        {
            get { return isAlta; }
            set { isAlta = value; OnPropertyChanged(); }
        }
        public bool IsInterconsulta
        {
            get { return isInterconsulta; }
            set { isInterconsulta = value; OnPropertyChanged(); }
        }
    }
}