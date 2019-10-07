﻿using Plugin.CloudFirestore.Attributes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ifsp.acolheuse.mobile.Core.Domain
{
    public class User : BindableBase
    {
        private string email;
        private string password;
        private string tipo;
        private string id;
        private string acessToken;

        public string Email
        {
            get
            {
                if (!String.IsNullOrEmpty(email))
                {
                    email = email.Trim();
                    email = email.ToLower();
                }

                return email;
            }
            set { email = value; RaisePropertyChanged(); }
        }

        [Ignored]
        public string Password
        {
            get
            {
                if (!String.IsNullOrEmpty(password))
                    password = password.Trim();

                return password;
            }
            set { password = value; RaisePropertyChanged(); }
        }
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; RaisePropertyChanged(); }
        }
        [Id]
        public string Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }

        public string AcessToken
        {
            get { return acessToken; }
            set { acessToken = value; RaisePropertyChanged(); }
        }

    }
}
