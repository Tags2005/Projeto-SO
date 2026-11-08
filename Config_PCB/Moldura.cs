namespace Projeto_SO.Config_PCB
{
    public class Moldura
    {
        public int IdMoldura { get; private set; }
        public int? IdPaginaAlocada { get; set; }
        public int? IdProcessoDono { get; set; }

        public bool EstaLivre => !IdPaginaAlocada.HasValue;

        public Moldura(int idMoldura)
        {
            IdMoldura = idMoldura;
            IdPaginaAlocada = null;
            IdProcessoDono = null;
        }

        public void Alocar(int idPagina, int idProcesso)
        {
            IdPaginaAlocada = idPagina;
            IdProcessoDono = idProcesso;
        }

        public void Liberar()
        {
            IdPaginaAlocada = null;
            IdProcessoDono = null;
        }


    }
}