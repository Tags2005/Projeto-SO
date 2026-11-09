using System;
using System.Collections.Generic;
using Projeto_SO.Config_PCB;
using Projeto_SO.Config_GerencMem;

namespace Projeto_SO
{
    public class GerenciadorMemoria
    {
        public Moldura[] molduras { get; private set; }
        public IPoliticaSubstituicaoPagina politicaSubstituicao { get; private set; }
        public int TamanhoPagina { get; private set; }
        public int TempoAtendimentoFaltaPagina { get; private set; }

        // Construtor
        public GerenciadorMemoria(int totalMolduras, int tamanhoPagina, int tempoAtendimento, IPoliticaSubstituicaoPagina politica)
        {
            if (tamanhoPagina <= 0)
            {
                throw new ArgumentException("O tamanho da página deve ser estritamente maior que zero.", nameof(tamanhoPagina));
            }

            this.TamanhoPagina = tamanhoPagina;
            this.TempoAtendimentoFaltaPagina = tempoAtendimento;
            this.politicaSubstituicao = politica;

            molduras = new Moldura[totalMolduras];
            for (int i = 0; i < totalMolduras; i++)
            {
                molduras[i] = new Moldura(i);
            }
        }

        public bool AcessarMemoria(PCB processo, int enderecoLogico, List<PCB> listaProcessos)
        {
            if (TamanhoPagina <= 0)
            {
                throw new InvalidOperationException("O tamanho da página deve ser maior que zero.");
            }

            int numeroPagina = enderecoLogico / TamanhoPagina;

            if (processo.TabelaPaginas.EstaPresente(numeroPagina))
            {
                return true;
            }

            TratarFaltaPagina(processo, numeroPagina, listaProcessos);
            return false;
        }

        public void TratarFaltaPagina(PCB processo, int numeroPagina, List<PCB> listaProcessos)
        {
            int indiceMolduraAlocada = -1;

            for (int i = 0; i < molduras.Length; i++)
            {
                if (molduras[i].EstaLivre)
                {
                    indiceMolduraAlocada = i;
                    break;
                }
            }

            if (indiceMolduraAlocada == -1)
            {
                indiceMolduraAlocada = politicaSubstituicao.SelecionarMolduraVitima(molduras);
                Moldura molduraVitima = molduras[indiceMolduraAlocada];

                if (molduraVitima.IdProcessoDono.HasValue && molduraVitima.IdPaginaAlocada.HasValue)
                {
                    int idProcessoAntigo = molduraVitima.IdProcessoDono.Value;
                    int paginaAntiga = molduraVitima.IdPaginaAlocada.Value;

                    if (listaProcessos != null)
                    {
                        PCB processoAntigo = listaProcessos.Find(p => p.IdProcesso == idProcessoAntigo);
                        if (processoAntigo != null)
                        {
                            processoAntigo.TabelaPaginas.DesmapearPagina(paginaAntiga);
                        }
                    }
                }

                molduraVitima.Liberar();
            }

            molduras[indiceMolduraAlocada].Alocar(numeroPagina, processo.IdProcesso);

            processo.TabelaPaginas.MapearPagina(numeroPagina, indiceMolduraAlocada, presente: true);

            politicaSubstituicao.RegistrarUsoMoldura(indiceMolduraAlocada);
        }
    }
}