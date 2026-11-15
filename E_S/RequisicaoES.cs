using System;
using System.Collections.Generic;
using System.Text;
using Projeto_SO.Config_PCB;

namespace Projeto_SO.E_S
{
    //Pedido de E/S feito por uma thread a um dispositivo
    public class RequisicaoES
    {
        public TCB Thread { get; private set; }

        // Quantidade de blocos ou caracteres a transferir
        public int Quantidade { get; private set; }

        // Clock do pedido feito
        public int InstanteSolicitacao { get; private set; }

        public RequisicaoES(TCB thread, int quantidade, int instanteSolicitacao)
        {
            Thread = thread;
            Quantidade = quantidade < 1 ? 1 : quantidade;
            InstanteSolicitacao = instanteSolicitacao;
        }
    }
}

