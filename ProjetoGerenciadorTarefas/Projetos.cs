using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoGerenciadorTarefas
{
    internal class Projetos
    {
        private List<Projeto> itens = new List<Projeto>();

        public bool adicionar(Projeto p)
        {
            if (p == null || buscarPorId(p.Id) != null)
                return false;

            itens.Add(p);
            return true;
        }

        public bool remover(Projeto p)
        {
            if (p == null || p.Tarefas.Count > 0)
                return false;

            return itens.Remove(p);
        }

        public Projeto buscar(Projeto p)
        {
            if (p == null)
                return null;

            return buscarPorId(p.Id);
        }

        public Projeto buscarPorId(int id)
        {
            return itens.FirstOrDefault(p => p.Id == id);
        }

        public List<Projeto> listar()
        {
            return new List<Projeto>(itens);
        }
    }
}
