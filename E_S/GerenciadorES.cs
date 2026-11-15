using Projeto_SO.Config_PCB;
using Projeto_SO.Config_PCB.Enum;
using Projeto_SO.Config_PCB.EnumEstadoThread;
using Projeto_SO.E_S;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.E_S
{
    // Gerencia os dispositivos de E/S: registro, filas, bloqueio das threads solicitantes e o tratamento de interrupções simuladas
    public class GerenciadorES
    {
        private readonly Dictionary<int, DispositivoES> dispositivos = new Dictionary<int, DispositivoES>();
        private readonly RegistradorLog? log;
        private readonly Func<int> obterClock;

        // Thread terminou E/S e precisa voltar para fila de prontos
        public event Action<TCB>? ThreadDesbloqueada;

        public IReadOnlyCollection<DispositivoES> Dispositivos => dispositivos.Values;

        // obterClock = clock atual
        public GerenciadorES(RegistradorLog? log = null, Func<int>? obterClock = null)
        {
            this.log = log;
            this.obterClock = obterClock ?? (() => 0);
        }

        public void RegistrarDispositivo(DispositivoES dispositivo)
        {
            if (dispositivos.ContainsKey(dispositivo.IdDispositivo))
            {
                throw new ArgumentException($"Já existe um dispositivo com id {dispositivo.IdDispositivo}.");
            }

            dispositivo.InterrupcaoGerada += TratarInterrupcao;
            dispositivos[dispositivo.IdDispositivo] = dispositivo;
        }

        //thread bloqueada entra na fila do dispositivo
        public void SolicitarES(TCB thread, int idDispositivo, int quantidade = 1)
        {
            if (!dispositivos.TryGetValue(idDispositivo, out DispositivoES? dispositivo))
            {
                throw new KeyNotFoundException($"Dispositivo {idDispositivo} não encontrado.");
            }

            thread.Estado = EstadoThread.Bloqueado;
            dispositivo.Enfileirar(new RequisicaoES(thread, quantidade, obterClock()));

            log?.Registrar(obterClock(), "ES_SOLICITACAO", $"TCB {thread.IdThread}",
                $"Solicitou E/S no dispositivo '{dispositivo.Nome}' (qtd: {quantidade}).");
        }

        public void Avancar()
        {
            foreach (DispositivoES dispositivo in dispositivos.Values)
            {
                dispositivo.Avancar();
            }
        }

        // marca a thread como Pronta e avisa o núcleo
        private void TratarInterrupcao(DispositivoES dispositivo, RequisicaoES requisicao)
        {
            requisicao.Thread.Estado = EstadoThread.Pronto;

            log?.Registrar(obterClock(), "ES_INTERRUPCAO", $"DISP {dispositivo.Nome}",
                $"E/S concluída. TCB {requisicao.Thread.IdThread} pronta para execução.");

            ThreadDesbloqueada?.Invoke(requisicao.Thread);
        }
    }
}
