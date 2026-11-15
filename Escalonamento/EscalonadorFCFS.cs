using Projeto_SO.Config_PCB;
using Projeto_SO.Escalonamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.Escalonamento
{
    // First-Come, First-Served (ordem de chegada)
    public class EscalonadorFCFS : EscalonadorBase
    {
        private readonly Queue<TCB> fila = new Queue<TCB>();

        public override string Nome => "FCFS";

        public override int Quantidade => fila.Count;

        public override void AdicionarThread(TCB thread)
        {
            fila.Enqueue(thread);
        }

        public override TCB? ProximaThread()
        {
            return fila.Count > 0 ? fila.Dequeue() : null;
        }
    }
}