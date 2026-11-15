using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO
{
    // Módulo estatístico da simulação. Acumula contadores e calcula métricas de desempenho,
    // como taxa de faltas de página, trocas de contexto e tempo médio de retorno
    public class CalculadorMetricas
    {
        //Contador do número total de acessos à memória realizados pelas threads
        public int TempoTotalAcessosMemoria { get; private set; }

        //Tempo total acumulado de sobrecarga (overhead) decorrente das trocas de contexto
        public int TempoSobrecargaTrocaContexto { get; private set; }

        //Contador total de Faltas de Página (Page Faults) ocorridas 
        public int QuantidadeFaltasPaginas { get; private set; }

        //Contador do número total de trocas de contexto de CPU efetuadas
        public int QuantidadeTrocaContextos { get; private set; }

        // Listas para ajudar a realizar o cálculo das médias finais!
        private List<int> temposRetorno = new List<int>();
        private List<int> temposEspera = new List<int>();

        //Métodos 

        public void RegistrarFaltaPagina() => QuantidadeFaltasPaginas++;
        public void RegistrarAcessoMemoria() => TempoTotalAcessosMemoria++;
        public void RegistrarTrocaContexto(int custoContexto)
        {
            QuantidadeTrocaContextos++;
            TempoSobrecargaTrocaContexto += custoContexto;
        }

        public void RegistrarFinalizacaoThread(int tempoRetorno, int tempoEspera)
        {
            temposRetorno.Add(tempoRetorno);
            temposEspera.Add(tempoEspera);
        }

        //Cálculo de quanto tempo, em média, os processos/threads levam da chegada até a finalização
        public double ObterTempoMedioRetorno()
        {
            // Se a lista não estiver vazia, o LINQ .Average() calcula a média automaticamente
            return temposRetorno.Count > 0 ? temposRetorno.Average() : 0;
        }

        //Calcula a proporção de acessos à memória que resultaram em falta de página
        public double ObterTaxaFaltaPagina()
        {
            return TempoTotalAcessosMemoria > 0
                ? (double)QuantidadeFaltasPaginas / TempoTotalAcessosMemoria
                : 0;
        }

        public void VisualizarRelatorio(int clockFinal)
        {
            Console.WriteLine("****** RELATÓRIO DE MÉTRICAS ******");
            Console.WriteLine($"Trocas de Contexto: {QuantidadeTrocaContextos}");
            Console.WriteLine($"Faltas de Página: {QuantidadeFaltasPaginas} (Taxa: {ObterTaxaFaltaPagina():P2})");
            Console.WriteLine($"Tempo Médio de Retorno: {ObterTempoMedioRetorno():F2} unidades de tempo");
        }

    }
}
