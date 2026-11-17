using System;
using System.Collections.Generic;
using System.IO;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_PCB.Enum;

namespace Projeto_SO
{
    // Classe utilitária responsável por ler, validar e interpretar (parser)
    // os arquivos de carga de trabalho externos que definem processos, threads
    // e a sequência de instruções simuladas (CPU, MEM, ES, ARQ).
    public static class LeitorCargaTrabalho
    {
        // Lê um arquivo de carga de trabalho no caminho especificado e constrói
        // a estrutura de objetos PCB e TCB para a simulação
        public static List<PCB> Carregar(string caminhoArquivo)
        {
            // Valida a existência física do arquivo de carga
            if (!File.Exists(caminhoArquivo))
            {
                throw new FileNotFoundException($"Arquivo de carga de trabalho não encontrado: {caminhoArquivo}");
            }

            var processos = new Dictionary<int, PCB>();
            string[] linhas = File.ReadAllLines(caminhoArquivo);
            TCB? threadAtual = null;
            int numeroLinha = 0;

            foreach (string linha in linhas)
            {
                numeroLinha++;
                string l = linha.Trim();

                // Ignora linhas em branco e comentários iniciados por '#' ou '//'
                if (string.IsNullOrWhiteSpace(l) || l.StartsWith("#") || l.StartsWith("//"))
                    continue;

                string[] partes = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string tipo = partes[0].ToUpper();

                try
                {
                    // Formato: PROCESSO <IdProcesso> <InstanteCriacao>
                    if (tipo == "PROCESSO")
                    {
                        if (partes.Length < 3)
                            throw new FormatException($"Linha {numeroLinha}: Sintaxe inválida para PROCESSO. Esperado: PROCESSO <IdProcesso> <InstanteCriacao>");

                        int idProc = int.Parse(partes[1]);
                        int instCriacao = int.Parse(partes[2]);

                        // Cria e registra o PCB caso ainda não exista
                        if (!processos.ContainsKey(idProc))
                        {
                            processos[idProc] = new PCB(idProc, instCriacao);
                        }
                    }
                    // Formato: THREAD <IdThread> <IdProcesso> <Prioridade>
                    else if (tipo == "THREAD")
                    {
                        if (partes.Length < 4)
                            throw new FormatException($"Linha {numeroLinha}: Sintaxe inválida para THREAD. Esperado: THREAD <IdThread> <IdProcesso> <Prioridade>");

                        int idThread = int.Parse(partes[1]);
                        int idProc = int.Parse(partes[2]);
                        int prioridade = int.Parse(partes[3]);

                        // Vincula a thread ao processo base declarado previamente
                        if (processos.TryGetValue(idProc, out PCB? pcb))
                        {
                            threadAtual = new TCB(idThread, pcb, prioridade);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Linha {numeroLinha}: Processo ID {idProc} não foi declarado antes da Thread {idThread}.");
                        }
                    }
                    // Formato: INSTR <CPU|MEM|ES|ARQ> ...
                    else if (tipo == "INSTR")
                    {
                        if (threadAtual == null)
                            throw new InvalidOperationException($"Linha {numeroLinha}: Instrução declarada sem uma Thread ativa.");

                        if (partes.Length < 3)
                            throw new FormatException($"Linha {numeroLinha}: Sintaxe inválida para INSTR.");

                        string subTipo = partes[1].ToUpper();
                        var instrucao = new InstrucaoSimulada();

                        switch (subTipo)
                        {
                            // Instrução de CPU: INSTR CPU <Ticks>
                            case "CPU":
                                instrucao.Tipo = TipoInstrucao.CPU;
                                instrucao.Valor = int.Parse(partes[2]);
                                break;


                            // Instrução de Memória: INSTR MEM <EnderecoLogico>
                            case "MEM":
                            case "MEMORIA":
                                instrucao.Tipo = TipoInstrucao.MEMORIA;
                                instrucao.Valor = int.Parse(partes[2]);
                                break;


                            // Instrução de E/S: INSTR ES <IdDispositivo> [Quantidade] [NaoBloqueante]
                            case "ES":
                                instrucao.Tipo = TipoInstrucao.ES;
                                instrucao.IdDispositivo = int.Parse(partes[2]);
                                instrucao.Valor = partes.Length >= 4 ? int.Parse(partes[3]) : 1;
                                instrucao.NaoBloqueante = partes.Length >= 5 && bool.Parse(partes[4]);
                                break;


                            // Instruções do Sistema de Arquivos: INSTR ARQ <ABRIR|LER|ESCREVER|FECHAR>
                            case "ARQ":
                                instrucao.Tipo = TipoInstrucao.ARQ;
                                instrucao.AcaoArquivo = partes[2].ToUpper();

                                // INSTR ARQ ABRIR <CaminhoArquivo>
                                if (instrucao.AcaoArquivo == "ABRIR" && partes.Length >= 4)
                                {
                                    instrucao.CaminhoArquivo = partes[3];
                                }

                                // INSTR ARQ LER <fd> <QuantidadeBytes> ou INSTR ARQ ESCREVER <fd> <QuantidadeBytes>
                                else if ((instrucao.AcaoArquivo == "LER" || instrucao.AcaoArquivo == "ESCREVER") && partes.Length >= 4)
                                {
                                    instrucao.DescriptorFd = int.Parse(partes[3]);
                                    instrucao.Valor = partes.Length >= 5 ? int.Parse(partes[4]) : 1;
                                }

                                // INSTR ARQ FECHAR <fd>
                                else if (instrucao.AcaoArquivo == "FECHAR" && partes.Length >= 4)
                                {
                                    instrucao.DescriptorFd = int.Parse(partes[3]);
                                }
                                else
                                {
                                    throw new FormatException($"Linha {numeroLinha}: Instrução de arquivo '{instrucao.AcaoArquivo}' possui parâmetros insuficientes.");
                                }
                                break;

                            default:
                                throw new FormatException($"Linha {numeroLinha}: Tipo de instrução desconhecido '{subTipo}'.");
                        }

                        // Enfileira a instrução processada na fila de instruções da thread atual
                        threadAtual.Instrucoes.Enqueue(instrucao);
                    }
                    else
                    {
                        throw new FormatException($"Linha {numeroLinha}: Comando inválido ou não reconhecido '{tipo}'.");
                    }
                }
                catch (Exception ex)
                {
                    // Lança exceção interrompendo a simulação sem corromper a execução
                    throw new InvalidDataException($"[ERRO DE CARGA] Falha crítica na linha {numeroLinha} ('{linha}'): {ex.Message}", ex);
                }
            }

            return new List<PCB>(processos.Values);
        }
    }
}
