using System;
using System.Collections.Generic;
using System.Text;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_GerencMem;

namespace Projeto_SO
{
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

        public NucleodeSimulacao(GerenciadorMemoria gerencMemoria)
        {
            ClockAtual = 0;
            listaProcessos = new List<PCB>();
            filaProntos = new List<TCB>();
            Log = new RegistradorLog();
            Metricas = new CalculadorMetricas();
            GerencMemoria = gerencMemoria;
        }

        // Métodos 
        public void Executar()
        {
            Log.Registrar(ClockAtual, "INICIO", "SISTEMA", "Simulação iniciada.");

            while (filaProntos.Count > 0 || ThreadEmExecucao != null)
            {
                if (ThreadEmExecucao == null && filaProntos.Count > 0)
                {
                    ThreadEmExecucao = filaProntos[0];
                    filaProntos.RemoveAt(0);
                    Metricas.RegistrarTrocaContexto(custoContexto: 1);
                    ThreadEmExecucao.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Executando;
                    Log.Registrar(ClockAtual, "DESPACHO", $"TCB {ThreadEmExecucao.IdThread}", "Thread assumiu a CPU.");
                }

                AvisarClock(1);

                if (ThreadEmExecucao != null)
                {
                    ThreadEmExecucao.SurtoCpuRestante--;

                    if (ThreadEmExecucao.SurtoCpuRestante <= 0)
                    {
                        ThreadEmExecucao.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Finalizado;
                        Log.Registrar(ClockAtual, "FINALIZACAO", $"TCB {ThreadEmExecucao.IdThread}", "Thread concluiu a execução.");

                        Metricas.RegistrarFinalizacaoThread(ClockAtual - ThreadEmExecucao.ProcessoBase.InstanteCriacao, 0);

                        ThreadEmExecucao = null;
                    }
                }
                VerificarDesbloqueios();
            }
        }

        public void ProcessarAcessoMemoria(int enderecoLogico)
        {
            if (ThreadEmExecucao == null) return;

            bool hit = GerencMemoria.AcessarMemoria(ThreadEmExecucao.ProcessoBase, enderecoLogico);
            Metricas.RegistrarAcessoMemoria();

            if (!hit)
            {
                Metricas.RegistrarFaltaPagina();
                BloquearThreadPorFaltaPagina(ThreadEmExecucao, GerencMemoria.TempoAtendimentoFaltaPagina);
            }
        }

        public void AvisarClock(int novosPassos = 1)
        {
            ClockAtual += novosPassos;
        }

        public void CarregarCargaTrabalho(string caminhoArquivo)
        {
            Log.Registrar(ClockAtual, "CARGA", "SISTEMA", $"Carga de trabalho lida de: {caminhoArquivo}");
        }

        public void BloquearThreadPorFaltaPagina(TCB thread, int tempoAtendimento)
        {
            thread.Estado = Config_PCB.EnumEstadoThread.EstadoThread.Bloqueado;
            Log.Registrar(ClockAtual, "BLOQUEIO", $"TCB {thread.IdThread}", "Thread bloqueada por falta de página.");

            int tempoDesbloqueio = ClockAtual + tempoAtendimento;
            filaBloqueados.Enqueue(thread, tempoDesbloqueio);

            ThreadEmExecucao = null;
        }

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
