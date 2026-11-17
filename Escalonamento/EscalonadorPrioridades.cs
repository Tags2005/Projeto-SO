using Projeto_SO.Config_PCB;
using Projeto_SO.Escalonamento;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.Escalonamento
{
    //   MENOR valor numérico de TCB.Prioridade = MAIOR prioridade
    public class EscalonadorPrioridades : EscalonadorBase
    {
        private readonly PriorityQueue<TCB, (int Prioridade, long Ordem)> fila =
            new PriorityQueue<TCB, (int Prioridade, long Ordem)>();

        private long contadorChegada = 0;

        public override string Nome => "Prioridades";

        public override int Quantidade => fila.Count;

        public override void AdicionarThread(TCB thread)
        {
            fila.Enqueue(thread, (thread.Prioridade, contadorChegada++));
        }

        public override TCB? ProximaThread()
        {
            return fila.Count > 0 ? fila.Dequeue() : null;
        }
    }
}