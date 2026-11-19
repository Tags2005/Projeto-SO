using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.SistemaArquivos
{
    // Representa um bloco físico no disco simulado para a Alocação Encadeada
    public class BlocoDisco
    {
        public int IdBloco { get; private set; }
        public bool Ocupado { get; set; } = false;
        public int ProximoBlocoId { get; set; } = -1; // Ponteiro para o próximo bloco do arquivo (-1 = Fim do Arquivo / EOF)

        public BlocoDisco(int idBloco)
        {
            IdBloco = idBloco;
        }
    }
}