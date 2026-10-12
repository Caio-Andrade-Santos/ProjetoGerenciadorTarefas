using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoGerenciadorTarefas
{
    internal class Projeto
    {
        public int Id { get; private set; }
        public string Nome { get; set; }
        public List<Tarefa> Tarefas { get; private set; }

        private static int proximoId = 1;

        public Projeto(string nome)
        {
            Id = proximoId++;
            Nome = nome;
            Tarefas = new List<Tarefa>();
        }

        public void adicionarTarefa(Tarefa t)
        {
            if (t != null && !Tarefas.Contains(t))
            {
                Tarefas.Add(t);
            }
        }

        public bool removerTarefa(Tarefa t)
        {
            return Tarefas.Remove(t);
        }

        public Tarefa buscarTarefa(Tarefa t)
        {
            if (t == null)
                return null;

            return Tarefas.FirstOrDefault(x => x.Id == t.Id);
        }

        public Tarefa buscarTarefaPorId(int id)
        {
            return Tarefas.FirstOrDefault(t => t.Id == id);
        }

        public List<Tarefa> tarefasPorStatus(string s)
        {
            return Tarefas
                .Where(t => t.Status.Equals(
                    s, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Tarefa> tarefasPorPrioridade(int p)
        {
            return Tarefas
                .Where(t => t.Prioridade == p)
                .ToList();
        }

        public int totalAbertas()
        {
            return Tarefas.Count(t => t.Status == "Aberta");
        }

        public int totalFechadas()
        {
            return Tarefas.Count(t => t.Status == "Fechada");
        }

        public int totalCanceladas()
        {
            return Tarefas.Count(t => t.Status == "Cancelada");
        }
    }
}
