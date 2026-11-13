using System.Collections.Generic;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_PCB.Enum;

namespace Projeto_SO
{
    // Bloco de Controle do Processo PCB - serve como contêiner do programa simulado,
    // mantendo suas informações de estado, recursos de memória e suas threads.
    public class PCB
    {
        public int IdProcesso { get; private set; }
        public EstadoProcesso Estado { get; set; }
        public TabelaPaginas TabelaPaginas { get; private set; }
        //Instante de tempo (clock) em que o processo foi criado
        public int InstanteCriacao { get; private set; }
        //Lista de threads (TCBs) pertencentes a este processo
        public List<TCB> Threads { get; private set; }
        public Dictionary<int, object> TabelaArquivosAbertos { get; private set; }

        //Construtor
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
