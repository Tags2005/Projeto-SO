
namespace Projeto_SO.Config_PCB
{
    //Aqui é realizada a verificação dos endereços virtuais (número da página)
    //para os endereços físicos na memória RAM (número da moldura)
    public class TabelaPaginas
    {
        private Dictionary<int, EntradaTabelaPaginas> entradas = new Dictionary<int, EntradaTabelaPaginas>();

        public bool ContemPagina(int numeroPagina) => entradas.ContainsKey(numeroPagina);

        public bool EstaPresente(int numeroPagina)
        {
            return entradas.ContainsKey(numeroPagina) && entradas[numeroPagina].Presente;
        }

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

        public void DesmapearPagina(int numeroPagina)
        {
            if (entradas.ContainsKey(numeroPagina))
            {
                entradas[numeroPagina].Presente = false;
            }
        }
    }
}