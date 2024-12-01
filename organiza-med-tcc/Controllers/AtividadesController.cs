using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using organiza_med_tcc.Controllers.Compartilhado;
using organiza_med_tcc.Models;
using OrganizaMed.Aplicacao.Servicos;
using OrganizaMed.Dominio.Atividades;

namespace organiza_med_tcc.Controllers
{
    public class AtividadesController : WebControllerBase
    {
        readonly private IMapper mapeador;
        readonly private AtividadesServico servico;
        readonly private MedicosServico servicoMedicos;

        public AtividadesController(AtividadesServico servico, MedicosServico servicoMedicos, IMapper mapeador)
        {
            this.servico = servico;
            this.servicoMedicos = servicoMedicos;
            this.mapeador = mapeador;
        }

        public IActionResult Listar()
        {
            Result<List<Atividade>> resultado = servico.ObterTodos();
            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction("Index", "Home");
            }

            List<Atividade> ? atividades = resultado.Value;

            IEnumerable<ListarAtividadesViewModel> ? listarAtividadesVm =
                mapeador.Map<IEnumerable<ListarAtividadesViewModel>>(atividades);

            return View(listarAtividadesVm);
        }

        public IActionResult Adicionar()
        {
            InserirAtividadesViewModel model = new InserirAtividadesViewModel
            {
                Medicos = servicoMedicos.ObterTodos().Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Adicionar(InserirAtividadesViewModel inserirVm)
        {
            Console.WriteLine("Método Adicionar foi chamado.");
            Console.WriteLine($"DataInicio: {inserirVm.DataInicio}");
            Console.WriteLine($"DataFim: {inserirVm.DataFim}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState não é válido.");
                foreach (ModelStateEntry modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        Console.WriteLine($"Erro de validação: {error.ErrorMessage}");
                }

                inserirVm.Medicos = servicoMedicos.ObterTodos().Value;
                return View(inserirVm);
            }

            IEnumerable<string> medicosIndisponiveis = servicoMedicos.VerificarDisponibilidade(inserirVm.MedicoId, inserirVm.DataInicio, inserirVm.DataFim);
            Console.WriteLine($"Médicos indisponíveis: {string.Join(", ", medicosIndisponiveis)}");

            if (medicosIndisponiveis.Any())
            {
                Console.WriteLine("Existem médicos indisponíveis.");
                ModelState.AddModelError("", "Os seguintes médicos não estão disponíveis no período selecionado: " + string.Join(", ", medicosIndisponiveis));
                inserirVm.Medicos = servicoMedicos.ObterTodos().Value;
                return View(inserirVm);
            }

            Atividade ? atividade = mapeador.Map<Atividade>(inserirVm);
            Console.WriteLine($"Dados da atividade mapeada: {JsonConvert.SerializeObject(atividade)}");

            Result<Atividade> resultado = servico.Adicionar(atividade);
            Console.WriteLine($"Resultado da adição da atividade: {resultado.IsSuccess}");

            if (resultado.IsFailed)
            {
                Console.WriteLine("Falha ao adicionar atividade.");
                ApresentarMensagemDeErro(resultado.ToResult());
                inserirVm.Medicos = servicoMedicos.ObterTodos().Value;
                return View(inserirVm);
            }

            Result<Atividade> resultadoRk = servico.Adicionar(atividade);

            if (resultadoRk.IsSuccess)
            {
                // Atualiza o ranking após adicionar uma nova atividade
                servicoMedicos.AtualizarRanking();

                ApresentarMensagemDeSucesso($"O registro ID [{atividade.Id}] foi inserido com sucesso!");
                return RedirectToAction(nameof ( Listar ));
            }

            ApresentarMensagemDeSucesso($"O registro ID [{atividade.Id}] foi inserido com sucesso!");
            return RedirectToAction(nameof ( Listar ));
        }

        public IActionResult Atualizar(int id)
        {
            Result<Atividade> resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Atividade ? atividade = resultado.Value;

            EditarAtividadesViewModel ? editarVm = mapeador.Map<EditarAtividadesViewModel>(atividade);

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
                foreach (ModelStateEntry modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        Console.WriteLine($"Erro de validação: {error.ErrorMessage}");
                }

                editarVM.Medicos = servicoMedicos.ObterTodos().Value;
                return View(editarVM);
            }

            Atividade ? atividade = mapeador.Map<Atividade>(editarVM);
            Console.WriteLine($"Dados da atividade mapeada: {JsonConvert.SerializeObject(atividade)}");

            Result<Atividade> resultado = servico.Atualizar(atividade);
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
            Result<Atividade> resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Atividade ? atividade = resultado.Value;

            DetalhesAtividadesViewModel ? detalhesVm = mapeador.Map<DetalhesAtividadesViewModel>(atividade);

            return View(detalhesVm);
        }

        [HttpPost]
        public IActionResult Excluir(DetalhesAtividadesViewModel detalhesVm)
        {
            Result<Atividade> resultado = servico.Remover(detalhesVm.Id);

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
            Result<Atividade> resultado = servico.ObterPorId(id);

            if (resultado.IsFailed)
            {
                ApresentarMensagemDeErro(resultado.ToResult());

                return RedirectToAction(nameof ( Listar ));
            }

            Atividade ? atividade = resultado.Value;

            DetalhesAtividadesViewModel ? detalhesVm = mapeador.Map<DetalhesAtividadesViewModel>(atividade);

            return View(detalhesVm);
        }
    }
}
