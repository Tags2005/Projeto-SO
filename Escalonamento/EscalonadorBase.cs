using Projeto_SO.Config_PCB;
using Projeto_SO.Escalonamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.Escalonamento
{
    // Classe base abstrata das políticas de escalonamento
    public abstract class EscalonadorBase : IEscalonador
    {
        public abstract string Nome { get; }

        public virtual int Quantum => 0;

        public abstract int Quantidade { get; }

        public bool TemThreadsProntas => Quantidade > 0;

        public abstract void AdicionarThread(TCB thread);

        public abstract TCB? ProximaThread();
    }
}