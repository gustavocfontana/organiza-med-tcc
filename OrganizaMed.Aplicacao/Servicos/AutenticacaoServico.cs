using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using OrganizaMed.Dominio.Autenticacao;

namespace OrganizaMed.Aplicacao.Servicos
{
    public class AutenticacaoServico
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly RoleManager<Perfil> roleManager;

        public AutenticacaoServico(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, RoleManager<Perfil> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<Result<Usuario>> Registrar(
        Usuario usuario, string senha, TipoUsuarioEnum tipoUsuario)
    {
        var resultadoCriacaoUsuario = await userManager.CreateAsync(usuario, senha);

        var tipoUsuarioStr = tipoUsuario.ToString();

        var resultadoBuscaTipoUsuario = await roleManager.FindByNameAsync(tipoUsuarioStr);

        if (resultadoBuscaTipoUsuario is null)
        {
            var perfil = new Perfil()
            {
                Name = tipoUsuarioStr,
                NormalizedName = tipoUsuarioStr.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await roleManager.CreateAsync(perfil);
        }

        await userManager.AddToRoleAsync(usuario, tipoUsuarioStr);

        if (resultadoCriacaoUsuario.Succeeded && tipoUsuario == TipoUsuarioEnum.Clinica)
        {
            await signInManager.SignInAsync(usuario, isPersistent: false);
        }
        else if (!resultadoCriacaoUsuario.Succeeded)
        {
            var erros = resultadoCriacaoUsuario.Errors.Select(s => s.Description);

            return Result.Fail(erros);
        }

        return Result.Ok(usuario);
    }

    public async Task<Result> Login(string usuario, string senha)
    {
        var resultadoLogin = await signInManager.PasswordSignInAsync(
            usuario,
            senha,
            false,
            false
        );

        if (!resultadoLogin.Succeeded)
            return Result.Fail("Login ou senha incorretos");

        return Result.Ok();
    }

    public async Task<Result> Logout()
    {
        await signInManager.SignOutAsync();

        return Result.Ok();
    }
    }
}
