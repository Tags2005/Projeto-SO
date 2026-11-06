
using static System.Net.WebRequestMethods;

//A primeira moldura a ser ocupada com uma página será a primeira a ser
// escolhida como vítima quando for necessário liberar espaço na RAM   

namespace Projeto_SO.Config_GerencMem
{
    public class PoliticaSubstituicaoFIFO : IPoliticaSubstituicaoPagina
    {
        // Lista de histórico que armazena os índices das molduras na ordem em que foram alocadas.
        // O índice na posição 0 representa a moldura carregada há mais tempo - a mais antiga
        public List<int> ordemMolduras = new List<int>();

        public void RegistrarUsoMoldura(int indiceMoldura)
        {
            // Evita duplicidades na fila caso a mesma moldura seja registrada novamente sem necessidade
            if (!ordemMolduras.Contains(indiceMoldura))
            {
                ordemMolduras.Add(indiceMoldura);
            }
        }

        // Seleciona e remove da fila a moldura que está carregada há mais tempo
        public int SelecionarMolduraVitima()
        {
            //se a fila estiver vazia por algum erro, retorna a primeira moldura - índice 0
            if (ordemMolduras.Count == 0)
            {
                return 0; 
            }

            // O elemento na posição 0 é o mais antigo
            int indiceVitima = ordemMolduras[0];

            // Remove a moldura escolhida do histórico da fila
            ordemMolduras.RemoveAt(0);

            // Retorna o índice para que o GerenciadorMemoria faça a desalocação
            return indiceVitima;
        }
        public void RemoverMoldura(int indiceMoldura)
        {
            ordemMolduras.Remove(indiceMoldura);
        }

    }
}