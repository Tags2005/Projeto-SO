using System;
using Projeto_SO;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_GerencMem;

namespace Projeto_SO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** TESTE DE SIMULAÇÃO COM MÚLTIPLAS INSTRUÇÕES ****\n");

            // 1. Inicializa o Gerenciador de Memória
            IPoliticaSubstituicaoPagina fifo = new PoliticaSubstituicaoFIFO();
            GerenciadorMemoria gerencMemoria = new GerenciadorMemoria(
                totalMolduras: 2,
                tamanhoPagina: 1024,
                tempoAtendimento: 5,
                politica: fifo
            );

            // 2. Inicializa o Núcleo
            NucleodeSimulacao nucleo = new NucleodeSimulacao(gerencMemoria);

            // 3. Cria Processo e Thread
            PCB processo = new PCB(idProcesso: 1, instanteCriacao: 0);
            TCB thread = new TCB(idThread: 101, processoBase: processo);
            thread.SurtoCpuRestante = 2; // Exige 2 ciclos de CPU

            nucleo.listaProcessos.Add(processo);
            nucleo.filaProntos.Add(thread);

            // 4. Início da Simulação via método do Núcleo
            nucleo.Log.Registrar(nucleo.ClockAtual, "INICIO", "SISTEMA", "Simulação iniciada.");

            // Faz o primeiro despacho (sai da fila de prontos para execução)
            if (nucleo.filaProntos.Count > 0)
            {
                // Como ThreadEmExecucao tem private set, chamamos a lógica através de um método do núcleo ou iniciamos o ciclo
                // Para o teste manual, simulamos a instrução diretamente:
            }

            // SIMULAÇÃO 1: Primeira tentativa de acesso (Falta de Página) 
            Console.WriteLine("\n--- Instrução 1: Acesso ao endereço 2050 ---");
            // Vamos temporariamente colocar a thread em execução pelo fluxo do núcleo:
            nucleo.Executar();

            // 5. Exibe o relatório final
            Console.WriteLine("\n");
            nucleo.Metricas.VisualizarRelatorio(nucleo.ClockAtual);
        }
    }
}