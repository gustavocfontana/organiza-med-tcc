using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using organiza_med_tcc.Controllers.Compartilhado;
using organiza_med_tcc.Models;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;

namespace organiza_med_tcc.Controllers
{
    public class AtividadesController : WebControllerBase
    {
        private readonly AtividadesServico servico;
        private readonly MedicosServico servicoMedicos;
        private readonly IMapper mapeador;

        public AtividadesController(AtividadesServico servico, MedicosServico servicoMedicos, IMapper mapeador)
        {
            this.servico = servico;
            this.servicoMedicos = servicoMedicos;
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

            var atividades = resultado.Value;

            var listarAtividadesVm =
                mapeador.Map<IEnumerable<ListarAtividadesViewModel>>(atividades);

            return View(listarAtividadesVm);
        }

        public IActionResult Adicionar()
        {
            var model = new InserirAtividadesViewModel
            {
                Medicos = servicoMedicos.ObterTodos().Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Adicionar(InserirAtividadesViewModel inserirVm)
        {
            if (!ModelState.IsValid)
            {
                inserirVm.Medicos = servicoMedicos.ObterTodos().Value;
                return View(inserirVm);
            }

            // Obter os médicos completos baseado nos IDs
            var medicos = new List<Medico>();
            foreach (var medicoId in inserirVm.MedicoId)
            {
                var medicoResult = servicoMedicos.ObterPorId(medicoId);
                if (medicoResult.IsSuccess)
                {
                    medicos.Add(medicoResult.Value);
                }
            }

            var atividade = mapeador.Map<Atividade>(inserirVm);
            atividade.MedicosEnvolvidos = medicos;

            var resultado = servico.Adicionar(atividade);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());
                inserirVm.Medicos = servicoMedicos.ObterTodos().Value;
                return View(inserirVm);
            }

            servicoMedicos.AtualizarRanking();
            ApresentarMensagemDeSucesso($"O registro ID [{atividade.Id}] foi inserido com sucesso!");
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

            var atividade = resultado.Value;

            var editarVm = mapeador.Map<EditarAtividadesViewModel>(atividade);

            return View(editarVm);
        }

        [HttpPost]
        public IActionResult Atualizar(EditarAtividadesViewModel editarVM)
        {
            Console.WriteLine("Método Editar foi chamado.");
            Console.WriteLine($"Dados recebidos: {JsonConvert.SerializeObject(editarVM)}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState não é válido.");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Erro de validação: {error.ErrorMessage}");
                    }
                }

                editarVM.Medicos = servicoMedicos.ObterTodos().Value;
                return View(editarVM);
            }

            var atividade = mapeador.Map<Atividade>(editarVM);
            Console.WriteLine($"Dados da atividade mapeada: {JsonConvert.SerializeObject(atividade)}");

            var resultado = servico.Atualizar(atividade);
            Console.WriteLine($"Resultado da edicao da atividade: {resultado.IsSuccess}");

            if (resultado.IsFailed)
            {
                Console.WriteLine("Falha ao editar atividade.");
                ApresentarMensagemDeErro(resultado.ToResult());
                editarVM.Medicos = servicoMedicos.ObterTodos().Value;
                return View(editarVM);
            }

            ApresentarMensagemDeSucesso($"O registro ID [{atividade.Id}] foi inserido com sucesso!");
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

            var atividade = resultado.Value;

            var detalhesVm = mapeador.Map<DetalhesAtividadesViewModel>(atividade);

            return View(detalhesVm);
        }

        [HttpPost]
        public IActionResult Excluir(DetalhesAtividadesViewModel detalhesVm)
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

            var atividade = resultado.Value;

            var detalhesVm = mapeador.Map<DetalhesAtividadesViewModel>(atividade);

            return View(detalhesVm);
        }
    }
}
