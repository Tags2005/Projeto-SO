using System;
using System.Collections.Generic;
using System.Text;
using Projeto_SO.Config_PCB;

namespace Projeto_SO.Escalonamento
{
    // O núcleo da simulação adiciona threads prontas e pede a próxima a ser executada
    public interface IEscalonador
    {
        // Nome da política 
        string Nome { get; }

        // Quantum em unidades de clock
        int Quantum { get; }

        //Quantidade de threads aguardando na fila
        int Quantidade { get; }

        // Indica se há ao menos uma thread pronta
        bool TemThreadsProntas { get; }

        // insere uma thread na estrutura de prontos conforme a política
        void AdicionarThread(TCB thread);

        // Remove e retorna a próxima thread a receber a CPU
        TCB? ProximaThread();
    }
}