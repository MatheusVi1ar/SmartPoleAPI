﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPoleAPI.Model
{
    public class UsuarioModel
    {
        private string nome;
        public string Nome
        {
            get
            {
                return nome;
            }
            set
            {
                nome = value;
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        private string senha;
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }
    }
}
