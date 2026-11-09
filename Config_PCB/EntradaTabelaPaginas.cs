namespace Projeto_SO.Config_PCB
{
    public class EntradaTabelaPaginas
    {
        public int NumeroMoldura { get; set; }
        public bool Presente { get; set; }

        public EntradaTabelaPaginas(int numeroMoldura, bool presente)
        {
            NumeroMoldura = numeroMoldura;
            Presente = presente;
        }
    }
}