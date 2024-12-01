using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using organiza_med_tcc.Controllers.Compartilhado;
using organiza_med_tcc.Models;
using OrganizaMed.Aplicacao.Servicos;

namespace organiza_med_tcc.Controllers
{
    public class MedicosController : WebControllerBase
    {
        readonly private IMapper mapeador;
        readonly private MedicosServico servico;

        public MedicosController(MedicosServico servico, IMapper mapeador)
        {
            this.servico = servico;
            this.mapeador = mapeador;
        }

        public IActionResult Listar()
        {
            Result<List<Medico>> ? resultado = servico.ObterTodos();

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction("Index", "Home");
            }

            List<Medico> ? medicos = resultado.Value;

            IEnumerable<ListarMedicosViewModel> ? listarMedicosVm =
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

            Medico ? medico = mapeador.Map<Medico>(inserirVm);

            Result<Medico> ? resultado = servico.Adicionar(medico);

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
            Result<Medico> ? resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Medico ? medico = resultado.Value;

            EditarMedicosViewModel ? editarVm = mapeador.Map<EditarMedicosViewModel>(medico);

            return View(editarVm);
        }

        [HttpPost]
        public IActionResult Atualizar(EditarMedicosViewModel editarVM)
        {
            if (!ModelState.IsValid)
                return View(editarVM);

            Medico ? medico = mapeador.Map<Medico>(editarVM);

            Result<Medico> ? resultado = servico.Atualizar(medico);

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
            Result<Medico> ? resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Medico ? medico = resultado.Value;

            DetalhesMedicosViewModel ? detalhesVm = mapeador.Map<DetalhesMedicosViewModel>(medico);

            return View(detalhesVm);
        }

        [HttpPost]
        public IActionResult Excluir(DetalhesMedicosViewModel detalhesVm)
        {
            Result<Medico> ? resultado = servico.Remover(detalhesVm.Id);

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
            Result<Medico> ? resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Medico ? medico = resultado.Value;

            DetalhesMedicosViewModel ? detalhesVm = mapeador.Map<DetalhesMedicosViewModel>(medico);

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
