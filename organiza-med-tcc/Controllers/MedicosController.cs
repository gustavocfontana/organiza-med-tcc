using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using organiza_med_tcc.Controllers.Compartilhado;
using organiza_med_tcc.Models;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;
using OrganizaMed.Dominio.Medicos;

namespace organiza_med_tcc.Controllers
{
    public class MedicosController : WebControllerBase
    {
        private readonly MedicosServico servico;
        private readonly IMapper mapeador;

        public MedicosController(MedicosServico servico, IMapper mapeador)
        {
            this.servico = servico;
            this.mapeador = mapeador;
        }

        public IActionResult Listar()
        {
            var resultado = servico.ObterTodos();

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction("Index", "Home");
            }

            var medicos = resultado.Value;

            var listarMedicosVm =
                mapeador.Map<IEnumerable<ListarMedicosViewModel>>(medicos);

            return View(listarMedicosVm);
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(InserirMedicosViewModel inserirVm)
        {
            if (!ModelState.IsValid)
                return View(inserirVm);

            var medico = mapeador.Map<Medico>(inserirVm);

            var resultado = servico.Adicionar(medico);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            ApresentarMensagemDeSucesso($"O registro ID [{medico.Id}] foi inserido com sucesso!");

            return RedirectToAction(nameof ( Listar ));
        }

        public IActionResult Atualizar(int id)
        {
            var resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            var medico = resultado.Value;

            var editarVm = mapeador.Map<EditarMedicosViewModel>(medico);

            return View(editarVm);
        }

        [HttpPost]
        public IActionResult Atualizar(EditarMedicosViewModel editarVM)
        {
            if (!ModelState.IsValid)
                return View(editarVM);

            var medico = mapeador.Map<Medico>(editarVM);

            var resultado = servico.Atualizar(medico);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            ApresentarMensagemDeErro(resultado.ToResult());

            return RedirectToAction(nameof ( Listar ));
        }

        public IActionResult Excluir(int id)
        {
            var resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            var medico = resultado.Value;

            var detalhesVm = mapeador.Map<DetalhesMedicosViewModel>(medico);

            return View(detalhesVm);
        }

        [HttpPost]
        public IActionResult Excluir(DetalhesMedicosViewModel detalhesVm)
        {
            var resultado = servico.Remover(detalhesVm.Id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            ApresentarMensagemDeErro(resultado.ToResult());

            return RedirectToAction(nameof ( Listar ));
        }

        public IActionResult Detalhes(int id)
        {
            var resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            var medico = resultado.Value;

            var detalhesVm = mapeador.Map<DetalhesMedicosViewModel>(medico);

            return View(detalhesVm);
        }

        public IActionResult TopMedicos()
        {
            servico.AtualizarRanking();
            var topMedicos = servico.ObterTodos().Value
                .OrderByDescending(m => m.HorasTrabalhadas)
                .Take(10)
                .Select(m => new TopMedicosViewModel
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    HorasTrabalhadas = m.HorasTrabalhadas
                }).ToList();

            return View(topMedicos);
        }
    }
}