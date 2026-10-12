using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoGerenciadorTarefas
{
    internal class Tarefa
    {
        private static int proximoId = 1;

        public int Id { get; private set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Prioridade { get; set; }
        public string Status { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataConclusao { get; private set; }

        public Tarefa(string titulo, string descricao, int prioridade)
        {
            Id = proximoId++;
            Titulo = titulo;
            Descricao = descricao;
            Prioridade = prioridade;
            Status = "Aberta";
            DataCriacao = DateTime.Now;
            DataConclusao = null;
        }

        public void concluir()
        {
            if (Status == "Aberta")
            {
                Status = "Fechada";
                DataConclusao = DateTime.Now;
            }
        }

        public void cancelar()
        {
            if (Status == "Aberta")
            {
                Status = "Cancelada";
                DataConclusao = null;
            }
        }

        public void reabrir()
        {
            if (Status == "Fechada" || Status == "Cancelada")
            {
                Status = "Aberta";
                DataConclusao = null;
            }
        }

        public override string ToString()
        {
            string prioridade = Prioridade switch
            {
                1 => "Alta",
                2 => "Média",
                3 => "Baixa",
                _ => "Inválida"
            };

            return $"ID: {Id} | Título: {Titulo} | " +
                   $"Prioridade: {prioridade} | Status: {Status}";
        }

    }
}
