using Projeto_SO.E_S;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.E_S
{
    // Dispositivo de caractere, o tempo é proporcional a quantidade transferida
    public class DispositivoCaractere : DispositivoES
    {
        public override TipoDispositivo Tipo => TipoDispositivo.Caractere;

        public DispositivoCaractere(int idDispositivo, string nome, int tempoServicoBase)
            : base(idDispositivo, nome, tempoServicoBase)
        {
        }

        protected override int CalcularTempoServico(RequisicaoES requisicao)
        {
            return TempoServicoBase * requisicao.Quantidade;
        }
    }
}