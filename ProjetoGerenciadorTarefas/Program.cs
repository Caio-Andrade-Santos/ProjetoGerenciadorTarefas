
using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoGerenciadorTarefas;

class Program
{
    static Projetos projetos = new Projetos();

    static void Main()
    {
        int opcao;

        do
        {
            Console.WriteLine("\n========== GERENCIADOR DE PROJETOS ==========");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar projeto");
            Console.WriteLine("2. Pesquisar projeto");
            Console.WriteLine("3. Remover projeto");
            Console.WriteLine("4. Adicionar tarefa em projeto");
            Console.WriteLine("5. Concluir tarefa");
            Console.WriteLine("6. Cancelar tarefa");
            Console.WriteLine("7. Reabrir tarefa");
            Console.WriteLine("8. Listar tarefas de um projeto");
            Console.WriteLine("9. Filtrar tarefas por status ou prioridade em um projeto");
            Console.WriteLine("10. Filtrar tarefas em todos os projetos");
            Console.WriteLine("11. Resumo geral");
            Console.Write("Escolha uma opção: ");

            opcao = LerInteiro();

            switch (opcao)
            {
                case 0:
                    Console.WriteLine("Encerrando o programa...");
                    break;
                case 1:
                    AdicionarProjeto();
                    break;
                case 2:
                    PesquisarProjeto();
                    break;
                case 3:
                    RemoverProjeto();
                    break;
                case 4:
                    AdicionarTarefa();
                    break;
                case 5:
                    AlterarStatusTarefa("concluir");
                    break;
                case 6:
                    AlterarStatusTarefa("cancelar");
                    break;
                case 7:
                    AlterarStatusTarefa("reabrir");
                    break;
                case 8:
                    ListarTarefasProjeto();
                    break;
                case 9:
                    FiltrarProjeto();
                    break;
                case 10:
                    FiltrarTodosProjetos();
                    break;
                case 11:
                    ResumoGeral();
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

        } while (opcao != 0);
    }

    static int LerInteiro()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int valor))
                return valor;

            Console.Write("Digite um número válido: ");
        }
    }

    static string LerTexto(string mensagem)
    {
        string texto;

        do
        {
            Console.Write(mensagem);
            texto = Console.ReadLine() ?? "";
            texto = texto.Trim();

            if (texto.Length == 0)
                Console.WriteLine("O campo não pode ficar vazio.");

        } while (texto.Length == 0);

        return texto;
    }

    static Projeto SelecionarProjeto()
    {
        Console.Write("Digite o ID do projeto: ");
        int id = LerInteiro();

        Projeto p = projetos.buscarPorId(id);

        if (p == null)
            Console.WriteLine("Projeto não encontrado.");

        return p;
    }

    static Tarefa SelecionarTarefa(Projeto p)
    {
        if (p.Tarefas.Count == 0)
        {
            Console.WriteLine("Esse projeto não possui tarefas.");
            return null;
        }

        Console.WriteLine("\nTarefas do projeto:");
        foreach (Tarefa t in p.Tarefas)
            Console.WriteLine(t);

        Console.Write("Digite o ID da tarefa: ");
        int id = LerInteiro();

        Tarefa tarefa = p.buscarTarefaPorId(id);

        if (tarefa == null)
            Console.WriteLine("Tarefa não encontrada nesse projeto.");

        return tarefa;
    }

    static void AdicionarProjeto()
    {
        string nome = LerTexto("Nome do projeto: ");

        Projeto p = new Projeto(nome);

        if (projetos.adicionar(p))
            Console.WriteLine($"Projeto cadastrado! ID: {p.Id}");
        else
            Console.WriteLine("Não foi possível cadastrar o projeto.");
    }

    static void PesquisarProjeto()
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        Console.WriteLine($"\nProjeto: {p.Nome}");
        Console.WriteLine($"ID: {p.Id}");
        Console.WriteLine($"Tarefas abertas: {p.totalAbertas()}");
        Console.WriteLine($"Tarefas fechadas: {p.totalFechadas()}");
        Console.WriteLine($"Tarefas canceladas: {p.totalCanceladas()}");

        Console.WriteLine("\n--- Tarefas por status ---");

        string[] status = { "Aberta", "Fechada", "Cancelada" };

        foreach (string s in status)
        {
            Console.WriteLine($"\n{s}:");

            List<Tarefa> tarefas = p.tarefasPorStatus(s);

            if (tarefas.Count == 0)
                Console.WriteLine("Nenhuma tarefa.");

            foreach (Tarefa t in tarefas)
                Console.WriteLine(t);
        }
    }

    static void RemoverProjeto()
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        if (projetos.remover(p))
            Console.WriteLine("Projeto removido com sucesso.");
        else
            Console.WriteLine(
                "Não foi possível remover. O projeto possui tarefas.");
    }

    static void AdicionarTarefa()
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        string titulo = LerTexto("Título da tarefa: ");
        string descricao = LerTexto("Descrição: ");

        int prioridade;

        do
        {
            Console.WriteLine("Prioridade: 1-Alta | 2-Média | 3-Baixa");
            Console.Write("Escolha: ");
            prioridade = LerInteiro();

            if (prioridade < 1 || prioridade > 3)
                Console.WriteLine("Prioridade inválida.");

        } while (prioridade < 1 || prioridade > 3);

        Tarefa t = new Tarefa(titulo, descricao, prioridade);
        p.adicionarTarefa(t);

        Console.WriteLine($"Tarefa cadastrada! ID: {t.Id}");
    }

    static void AlterarStatusTarefa(string acao)
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        Tarefa t = SelecionarTarefa(p);

        if (t == null)
            return;

        switch (acao)
        {
            case "concluir":
                if (t.Status != "Aberta")
                {
                    Console.WriteLine(
                        "Somente tarefas abertas podem ser concluídas.");
                    return;
                }

                t.concluir();
                Console.WriteLine("Tarefa concluída.");
                break;

            case "cancelar":
                if (t.Status != "Aberta")
                {
                    Console.WriteLine(
                        "Somente tarefas abertas podem ser canceladas.");
                    return;
                }

                t.cancelar();
                Console.WriteLine("Tarefa cancelada.");
                break;

            case "reabrir":
                if (t.Status == "Aberta")
                {
                    Console.WriteLine("A tarefa já está aberta.");
                    return;
                }

                t.reabrir();
                Console.WriteLine("Tarefa reaberta.");
                break;
        }
    }

    static void ListarTarefasProjeto()
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        Console.WriteLine($"\nTarefas do projeto: {p.Nome}");

        if (p.Tarefas.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa cadastrada.");
            return;
        }

        foreach (Tarefa t in p.Tarefas)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine(t);
            Console.WriteLine($"Descrição: {t.Descricao}");
            Console.WriteLine($"Criada em: {t.DataCriacao:dd/MM/yyyy HH:mm}");

            if (t.DataConclusao.HasValue)
                Console.WriteLine(
                    $"Data de conclusão: {t.DataConclusao.Value:dd/MM/yyyy HH:mm}");
        }
    }

    static void ExibirTarefas(List<Tarefa> tarefas)
    {
        if (tarefas.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa encontrada.");
            return;
        }

        foreach (Tarefa t in tarefas)
        {
            Console.WriteLine(t);
            Console.WriteLine($"Descrição: {t.Descricao}");
        }
    }

    static string SelecionarStatus()
    {
        Console.WriteLine("1. Aberta");
        Console.WriteLine("2. Fechada");
        Console.WriteLine("3. Cancelada");
        Console.Write("Status: ");

        int op = LerInteiro();

        return op switch
        {
            1 => "Aberta",
            2 => "Fechada",
            3 => "Cancelada",
            _ => ""
        };
    }

    static int SelecionarPrioridade()
    {
        Console.WriteLine("1. Alta");
        Console.WriteLine("2. Média");
        Console.WriteLine("3. Baixa");
        Console.Write("Prioridade: ");

        int op = LerInteiro();

        return op >= 1 && op <= 3 ? op : 0;
    }

    static int SelecionarTipoFiltro()
    {
        Console.WriteLine("1. Filtrar por status");
        Console.WriteLine("2. Filtrar por prioridade");
        Console.Write("Escolha: ");

        return LerInteiro();
    }

    static void FiltrarProjeto()
    {
        Projeto p = SelecionarProjeto();

        if (p == null)
            return;

        int tipo = SelecionarTipoFiltro();

        if (tipo == 1)
        {
            string status = SelecionarStatus();

            if (status == "")
            {
                Console.WriteLine("Status inválido.");
                return;
            }

            ExibirTarefas(p.tarefasPorStatus(status));
        }
        else if (tipo == 2)
        {
            int prioridade = SelecionarPrioridade();

            if (prioridade == 0)
            {
                Console.WriteLine("Prioridade inválida.");
                return;
            }

            ExibirTarefas(p.tarefasPorPrioridade(prioridade));
        }
        else
        {
            Console.WriteLine("Tipo de filtro inválido.");
        }
    }

    static void FiltrarTodosProjetos()
    {
        int tipo = SelecionarTipoFiltro();

        string status = "";
        int prioridade = 0;

        if (tipo == 1)
        {
            status = SelecionarStatus();

            if (status == "")
            {
                Console.WriteLine("Status inválido.");
                return;
            }
        }
        else if (tipo == 2)
        {
            prioridade = SelecionarPrioridade();

            if (prioridade == 0)
            {
                Console.WriteLine("Prioridade inválida.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Tipo de filtro inválido.");
            return;
        }

        bool encontrou = false;

        foreach (Projeto p in projetos.listar())
        {
            List<Tarefa> resultado;

            if (tipo == 1)
                resultado = p.tarefasPorStatus(status);
            else
                resultado = p.tarefasPorPrioridade(prioridade);

            if (resultado.Count == 0)
                continue;

            encontrou = true;
            Console.WriteLine($"\n=== Projeto: {p.Nome} (ID {p.Id}) ===");
            ExibirTarefas(resultado);
        }

        if (!encontrou)
            Console.WriteLine("Nenhuma tarefa encontrada com esse filtro.");
    }

    static void ResumoGeral()
    {
        List<Projeto> lista = projetos.listar();

        int totalProjetos = lista.Count;
        int abertas = lista.Sum(p => p.totalAbertas());
        int fechadas = lista.Sum(p => p.totalFechadas());
        int canceladas = lista.Sum(p => p.totalCanceladas());

        int totalTarefas = abertas + fechadas + canceladas;

        // Percentual de tarefas concluídas em relação às tarefas abertas.
        double percentual = abertas > 0
            ? (double)fechadas / abertas * 100
            : 0;

        Console.WriteLine("\n========== RESUMO GERAL ==========");
        Console.WriteLine($"Quantidade de projetos: {totalProjetos}");
        Console.WriteLine($"Total de tarefas: {totalTarefas}");
        Console.WriteLine($"Tarefas abertas: {abertas}");
        Console.WriteLine($"Tarefas fechadas: {fechadas}");
        Console.WriteLine($"Tarefas canceladas: {canceladas}");
        Console.WriteLine(
            $"Fechadas em relação às abertas: {percentual:F2}%");
    }
}
