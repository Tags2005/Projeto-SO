Classe InstrucaoSimulada:

﻿using Projeto_SO.Config_PCB.Enum;

namespace Projeto_SO.Config_PCB
{
    public class InstrucaoSimulada
    {
        public TipoInstrucao Tipo { get; set; }
        public int Valor { get; set; }          // Duração (CPU), Endereço Lógico (MEMORIA), Quantidade (ES/ARQ)
        public int? IdDispositivo { get; set; } // Necessário se for instrução de E/S
        public bool NaoBloqueante { get; set; }  // Suporte a E/S não-bloqueante

        public string? AcaoArquivo { get; set; } // "ABRIR", "LER", "ESCREVER", "FECHAR"
        public string? CaminhoArquivo { get; set; } // Ex: "/docs/teste.txt"
        public int? DescriptorFd { get; set; }   // Descriptor de Arquivo (fd) para Ler/Escrever/Fechar
    }
}