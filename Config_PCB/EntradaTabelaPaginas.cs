namespace Projeto_SO.Config_PCB
{
    // Representa uma entrada (linha) na Tabela de Páginas de um processo
    // Mapeia uma página virtual -> moldura física na RAM
    public class EntradaTabelaPaginas
    {
        //Número da moldura (frame) na memória RAM física onde a página está carregada
        public int NumeroMoldura { get; set; }
        //Indica se a página está presente na memória RAM   
        public bool Presente { get; set; }

        //Construtor
        public EntradaTabelaPaginas(int numeroMoldura, bool presente)
        {
            NumeroMoldura = numeroMoldura;
            Presente = presente;
        }
    }
}