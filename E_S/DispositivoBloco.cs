using Projeto;
using Projeto_SO.E_S;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.E_S
{
    // Dispositivo de bloco, o tempo de serviço é fixo por requisição
    public class DispositivoBloco : DispositivoES
    {
        public override TipoDispositivo Tipo => TipoDispositivo.Bloco;

        public DispositivoBloco(int idDispositivo, string nome, int tempoServicoBase)
            : base(idDispositivo, nome, tempoServicoBase)
        {
        }

        protected override int CalcularTempoServico(RequisicaoES requisicao)
        {
            return TempoServicoBase;
        }
    }
}