using Projeto_SO.Config_PCB.EnumEstadoThread;

namespace Projeto_SO.Config_PCB
{
    public class TCB
    {
        public int IdThread { get; private set; }
        public int IdProcesso { get; private set; }

        //Ponteiro para a classe PCB que representa o processo base da thread   
        public PCB ProcessoBase { get; private set; }
        public EstadoThread Estado { get; set; }
        public int SurtoCpuRestante { get; set; }
        public int ContadorPrograma { get; private set; }

        public TCB(int idThread, PCB processoBase)
        {
            IdThread = idThread;
            ProcessoBase = processoBase;
            IdProcesso = processoBase.IdProcesso;
            Estado = EstadoThread.Novo;
        }

    }
}