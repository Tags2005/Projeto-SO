using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_GerencMem;

namespace Projeto_SO
{
    // Motor principal da simulação. Controla o tempo (clock lógico),
    // o escalonamento das threads, as filas de prontos/bloqueados e os ciclos de execução
    public class NucleodeSimulacao
    {
        public int ClockAtual { get; private set; }
        public List<PCB> listaProcessos;
        public List<TCB> filaProntos;
        public TCB ThreadEmExecucao { get; private set; }
        public GerenciadorMemoria GerencMemoria { get; private set; }
        public RegistradorLog Log { get; private set; }
        public CalculadorMetricas Metricas { get; private set; }

        private PriorityQueue<TCB, int> filaBloqueados = new PriorityQueue<TCB, int>();

        // Construtor
        public NucleodeSimulacao(GerenciadorMemoria gerencMemoria)
        {
            ClockAtual = 0;
            listaProcessos = new List<PCB>();
            filaProntos = new List<TCB>();
            Log = new RegistradorLog();
            Metricas = new CalculadorMetricas();
            GerencMemoria = gerencMemoria;
        }

        // Loop principal do simulador. Processa despacho de threads, avanço de clock,
        // decremento de CPU e verificação de desbloqueios até que não restem tarefas
        public void Executar()
        {
            Log.Registrar(ClockAtual, "INICIO", "SISTEMA", "Simulação iniciada.");

            while (filaProntos.Count > 0 || ThreadEmExecucao != null)
            {
                // Se a CPU estiver livre e houver thread pronta, faz o despacho
                if (ThreadEmExecucao == null && filaProntos.Count > 0)
                {
                    ThreadEmExecucao = filaProntos[0];
                    filaProntos.RemoveAt(0);

                    Metricas.RegistrarTrocaContexto(custoContexto: 1);
                    ThreadEmExecucao.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Executando;
                    Log.Registrar(ClockAtual, "DESPACHO", $"TCB {ThreadEmExecucao.IdThread}", "Thread assumiu a CPU.");
                }

                AvisarClock(1);

                // Processa a execução da thread atual na CPU

                if (ThreadEmExecucao != null)
                {
                    ThreadEmExecucao.SurtoCpuRestante--;

                    // Verifica se a thread concluiu todo o seu tempo de processamento
                    if (ThreadEmExecucao.SurtoCpuRestante <= 0)
                    {
                        ThreadEmExecucao.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Finalizado;
                        Log.Registrar(ClockAtual, "FINALIZACAO", $"TCB {ThreadEmExecucao.IdThread}", "Thread concluiu a execução.");

                        Metricas.RegistrarFinalizacaoThread(ClockAtual - ThreadEmExecucao.ProcessoBase.InstanteCriacao, 0);

                        // Se todas as threads do processo pai terminaram, finaliza o processo
                        PCB processoPai = ThreadEmExecucao.ProcessoBase;
                        if (processoPai != null && processoPai.Threads.Count > 0)
                        {
                            bool processoConcluido = processoPai.Threads.All(t => t.Estado == Config_PCB.EnumEstadoThread.EstadoThread.Finalizado);
                            if (processoConcluido)
                            {
                                processoPai.Estado = Config_PCB.Enum.EstadoProcesso.Finalizado;
                                Log.Registrar(ClockAtual, "FINALIZACAO", $"PCB {processoPai.IdProcesso}", "Processo finalizado (todas as threads encerradas).");
                            }
                        }

                        ThreadEmExecucao = null;
                    }
                }
                // Verifica se há threads bloqueadas cujo tempo de I/O ou falta de página já expirou
                VerificarDesbloqueios();
            }
        }

        // Simula uma instrução de solicitação de memória realizada pela thread em execução.
        public void ProcessarAcessoMemoria(int enderecoLogico)
        {
            if (ThreadEmExecucao == null) return;

            bool hit = GerencMemoria.AcessarMemoria(ThreadEmExecucao.ProcessoBase, enderecoLogico, listaProcessos);
            Metricas.RegistrarAcessoMemoria();

            if (!hit)
            {
                Metricas.RegistrarFaltaPagina();
                BloquearThreadPorFaltaPagina(ThreadEmExecucao, GerencMemoria.TempoAtendimentoFaltaPagina);
            }
        }

        //Avança o tempo do clock lógico do sistema.
        public void AvisarClock(int novosPassos = 1)
        {
            ClockAtual += novosPassos;
        }

        //Registra no log a leitura de um arquivo de configuração de carga de trabalho
        public void CarregarCargaTrabalho(string caminhoArquivo)
        {
            Log.Registrar(ClockAtual, "CARGA", "SISTEMA", $"Carga de trabalho lida de: {caminhoArquivo}");
        }

        // Move a thread para o estado Bloqueado e agenda seu retorno à fila de prontos no futuro
        public void BloquearThreadPorFaltaPagina(TCB thread, int tempoAtendimento)
        {
            thread.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Bloqueado;
            Log.Registrar(ClockAtual, "BLOQUEIO", $"TCB {thread.IdThread}", "Thread bloqueada por falta de página.");

            int tempoDesbloqueio = ClockAtual + tempoAtendimento;
            filaBloqueados.Enqueue(thread, tempoDesbloqueio);

            // Libera a CPU imediatamente
            ThreadEmExecucao = null;
        }

        // Revisa a fila de bloqueados e move as threads cujo tempo de espera terminou de volta para a fila de prontos.
        public void VerificarDesbloqueios()
        {
            while (filaBloqueados.Count > 0)
            {
                if (filaBloqueados.TryPeek(out TCB thread, out int tempoDesbloqueio))
                {
                    if (ClockAtual >= tempoDesbloqueio)
                    {
                        filaBloqueados.Dequeue();
                        thread.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Pronto;
                        filaProntos.Add(thread);

                        Log.Registrar(ClockAtual, "DESBLOQUEIO", $"TCB {thread.IdThread}", "Página carregada. Thread pronta para execução.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}