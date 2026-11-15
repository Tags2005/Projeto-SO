using Projeto_SO.SistemaArquivos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.SistemaArquivos
{
    // Arrquivo aberto por um processo: aponta para o FCB e guarda a posição atual
    public class DescritorArquivo
    {
        public FCB Arquivo { get; private set; }

        public int Posicao { get; set; }

        public DescritorArquivo(FCB arquivo)
        {
            Arquivo = arquivo;
            Posicao = 0;
        }
    }
}