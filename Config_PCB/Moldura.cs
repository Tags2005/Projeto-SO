namespace Projeto_SO.Config_PCB
{
    //Representa um bloco físico (frame) de tamanho fixo na memória RAM simulada.
    public class Moldura
    {
        public int IdMoldura { get; private set; }
        public int? IdPaginaAlocada { get; set; }
        public int? IdProcessoDono { get; set; }

        //retorna true se a moldura não contiver nenhuma página alocada
        public bool EstaLivre => !IdPaginaAlocada.HasValue;

        public Moldura(int idMoldura)
        {
            IdMoldura = idMoldura;
            IdPaginaAlocada = null;
            IdProcessoDono = null;
        }

        // Associa uma página e seu processo correspondente a esta moldura física.
        public void Alocar(int idPagina, int idProcesso)
        {
            IdPaginaAlocada = idPagina;
            IdProcessoDono = idProcesso;
        }

        //Desassocia a página atual, tornando a moldura física novamente disponível
        public void Liberar()
        {
            IdPaginaAlocada = null;
            IdProcessoDono = null;
        }


    }
}