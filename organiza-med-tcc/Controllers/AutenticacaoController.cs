using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using organiza_med_tcc.Controllers.Compartilhado;
using organiza_med_tcc.Models;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Autenticacao;

namespace organiza_med_tcc.Controllers
{
    public class AutenticacaoController : WebControllerBase
{
    private readonly AutenticacaoServico servicoAutenticacao;

    public AutenticacaoController(AutenticacaoServico servicoAutenticacao)
    {
        this.servicoAutenticacao = servicoAutenticacao;
    }

    public IActionResult Registrar()
    {
        return View(new RegistrarViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(RegistrarViewModel registrarVm)
    {
        if (!ModelState.IsValid)
            return View(registrarVm);

        var usuario = new Usuario()
        {
            UserName = registrarVm.Usuario,
            Email = registrarVm.Email
        };

        var senha = registrarVm.Senha!;

        var resultado = await servicoAutenticacao
            .Registrar(usuario, senha, TipoUsuarioEnum.Clinica);

        if (resultado.IsSuccess)
            return RedirectToAction("Index", "Home");

        foreach (var erro in resultado.Errors)
            ModelState.AddModelError(string.Empty, erro.Message);

        return View(registrarVm);
    }

    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVm, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(loginVm);

        var resultado = await servicoAutenticacao.Login(loginVm.Usuario!, loginVm.Senha!);

        if (resultado.IsSuccess)
            return LocalRedirect(returnUrl ?? "/");

        var msgErro = resultado.Errors.First().Message;

        ModelState.AddModelError(string.Empty, msgErro);

        return View(loginVm);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await servicoAutenticacao.Logout();

        return RedirectToAction(nameof(Login));
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }
}
}