using FluentResults;
using Microsoft.AspNetCore.Mvc;
using organiza_med_tcc.Extensions;
using organiza_med_tcc.Models;

namespace organiza_med_tcc.Controllers.Compartilhado
{
    public abstract class WebControllerBase : Controller
    {
        protected IActionResult MensagemRegistroNaoEncontrado(int idRegistro)
        {
            TempData.SerializarMensagemViewModel(new MensagemViewModel
                {
                    Titulo = "Erro",
                    Mensagem = $"Nao foi possivel encontrar o registro ID [{idRegistro}]!"
                }
            );

            return RedirectToAction("Index", "Home");
        }

        protected void ApresentarMensagemDeErro(Result resultado)
        {
            ViewBag.Mensagem(new MensagemViewModel
                {
                    Titulo = "Erro",
                    Mensagem = resultado.Errors[0].Message
                }
            );
        }

        protected void ApresentarMensagemDeSucesso(string mensagem)
        {
            TempData.SerializarMensagemViewModel(new MensagemViewModel
            {
                Titulo = "Sucesso",
                Mensagem = mensagem
            }
            );
        }
    }
}
