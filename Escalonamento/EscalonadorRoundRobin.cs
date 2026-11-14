using Projeto_SO.Escalonamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.Escalonamento
{
    // Round Robin: FCFS com preempção por quantum
    public class EscalonadorRoundRobin : EscalonadorFCFS
    {
        private readonly int quantum;

        public override string Nome => "Round Robin";

        public override int Quantum => quantum;

        public EscalonadorRoundRobin(int quantum)
        {
            if (quantum <= 0)
            {
                throw new ArgumentException("O quantum deve ser maior que zero.", nameof(quantum));
            }

            this.quantum = quantum;
        }
    }
}