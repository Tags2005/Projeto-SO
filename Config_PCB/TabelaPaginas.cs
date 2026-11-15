
namespace Projeto_SO.Config_PCB
{
    // Gerencia o mapeamento de endereços virtuais (páginas) para endereços físicos (molduras na RAM)
    // de um processo específico.
    public class TabelaPaginas
    {
        //Dicionário que armazena a relação[Número da Página Virtual->Entrada da Tabela]
        private Dictionary<int, EntradaTabelaPaginas> entradas = new Dictionary<int, EntradaTabelaPaginas>();

        //Verifica se a página virtual existe no dicionário da tabela de páginas
        public bool ContemPagina(int numeroPagina) => entradas.ContainsKey(numeroPagina);

        // Verifica se a página virtual está carregada na memória RAM física (bit Presente = true)
        public bool EstaPresente(int numeroPagina)
        {
            return entradas.ContainsKey(numeroPagina) && entradas[numeroPagina].Presente;
        }

        //Retorna o índice da moldura na RAM se a página estiver presente; caso contrário, -1
        public int ObterNumeroMoldura(int numeroPagina)
        {
            if (EstaPresente(numeroPagina))
            {
                return entradas[numeroPagina].NumeroMoldura;
            }
            return -1;
        }

        public void MapearPagina(int numeroPagina, int numeroMoldura, bool presente = true)
        {
            entradas[numeroPagina] = new EntradaTabelaPaginas(numeroMoldura, presente);
        }

        // Desmapea a página (desmarca o bit de presença), indicando que ela foi removida da RAM física.
        public void DesmapearPagina(int numeroPagina)
        {
            if (entradas.ContainsKey(numeroPagina))
            {
                entradas[numeroPagina].Presente = false;
            }
        }
    }
}