using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OrganizaMed.Dominio.Autenticacao
{
    public class Usuario : IdentityUser<int>
    {
        public int ? PessoaId { get; set; }
        public Usuario ? Pessoa { get; set; }

        public Usuario()
        {
            EmailConfirmed = true;
        }
    }
}