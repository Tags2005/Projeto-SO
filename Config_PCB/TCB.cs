using Projeto_SO.Config_PCB.EnumEstadoThread;

namespace Projeto_SO.Config_PCB
{
    // Bloco de Controle de Thread TCB - representa a menor unidade de processamento
    // a ser escalonada e executada na CPU.
    public class TCB
    {
        public int IdThread { get; private set; }
        public int IdProcesso { get; private set; }

        //Ponteiro/referência para o PCB do processo pai   
        public PCB ProcessoBase { get; private set; }
        public EstadoThread Estado { get; set; }

        //Quantidade restante de ciclos de clock/tempo de CPU que a thread precisa para concluir
        public int SurtoCpuRestante { get; set; }

        //Contador de programa(PC) que aponta para a próxima instrução a ser executada
        public int ContadorPrograma { get; private set; }

        //Nível de prioridade da thread para o escalonamento
        public int Prioridade { get; set; }

        //Nível de prioridade da thread para o escalonamento
        public RegistradoresSimulados Registradores { get; private set; }

        //Construtor
        public TCB(int idThread, PCB processoBase)
        {
            IdThread = idThread;
            ProcessoBase = processoBase;
            IdProcesso = processoBase.IdProcesso;
            Estado = EstadoThread.Novo;
            SurtoCpuRestante = surtoCpuRestante;
            ContadorPrograma = 0;
            Prioridade = prioridade;
            Registradores = new RegistradoresSimulados();
            // Adiciona automaticamente esta thread à lista de threads do seu processo pai
            processoBase.Threads.Add(this);
        }

    }
}