using Projeto_SO.E_S;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.E_S
{
    // Classe base abstrata de um dispositivo de E/S simulado, com fila própria de requisições
    public abstract class DispositivoES
    {
        public int IdDispositivo { get; private set; }
        public string Nome { get; private set; }

        //Tempo base em unidades de clock
        public int TempoServicoBase { get; private set; }

        // Classe do dispositivo
        public abstract TipoDispositivo Tipo { get; }

        private readonly Queue<RequisicaoES> filaRequisicoes = new Queue<RequisicaoES>();

        public RequisicaoES? RequisicaoAtual { get; private set; }
        public int TempoRestante { get; private set; }

        public bool Ocupado => RequisicaoAtual != null;
        public int TamanhoFila => filaRequisicoes.Count;

        // Interrupção simulada: disparada quando uma requisição termina
        public event Action<DispositivoES, RequisicaoES>? InterrupcaoGerada;

        protected DispositivoES(int idDispositivo, string nome, int tempoServicoBase)
        {
            if (tempoServicoBase <= 0)
            {
                throw new ArgumentException("O tempo de serviço deve ser maior que zero.", nameof(tempoServicoBase));
            }

            IdDispositivo = idDispositivo;
            Nome = nome;
            TempoServicoBase = tempoServicoBase;
        }
        protected abstract int CalcularTempoServico(RequisicaoES requisicao);

        // Coloca a requisição na fila do dispositivo
        public void Enfileirar(RequisicaoES requisicao)
        {
            filaRequisicoes.Enqueue(requisicao);
        }

        public RequisicaoES? Avancar()
        {
            if (RequisicaoAtual == null && filaRequisicoes.Count > 0)
            {
                RequisicaoAtual = filaRequisicoes.Dequeue();
                TempoRestante = CalcularTempoServico(RequisicaoAtual);
            }

            if (RequisicaoAtual == null)
            {
                return null;
            }

            TempoRestante--;

            if (TempoRestante <= 0)
            {
                RequisicaoES concluida = RequisicaoAtual;
                RequisicaoAtual = null;
                TempoRestante = 0;

                // Interrupção simulada

                InterrupcaoGerada?.Invoke(this, concluida);
                return concluida;
            }

            return null;
        }
    }
}