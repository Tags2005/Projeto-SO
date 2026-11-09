using System.Collections.Generic;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_PCB.Enum;

namespace Projeto_SO
{
    public class PCB
    {
        public int IdProcesso { get; private set; }
        public EstadoProcesso Estado { get; set; }
        public TabelaPaginas TabelaPaginas { get; private set; }
        public int InstanteCriacao { get; private set; }
        public List<TCB> Threads { get; private set; }

        public PCB(int idProcesso, int instanteCriacao)
        {
            IdProcesso = idProcesso;
            InstanteCriacao = instanteCriacao;
            Estado = EstadoProcesso.Novo;
            TabelaPaginas = new TabelaPaginas();
            Threads = new List<TCB>();
        }

    }
}
