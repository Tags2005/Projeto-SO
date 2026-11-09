
using Projeto_SO.Config_PCB;

namespace Projeto_SO.Config_GerencMem
{
    public class PoliticaSubstituicaoFIFO : IPoliticaSubstituicaoPagina
    {
        public List<int> ordemMolduras = new List<int>();

        public void RegistrarUsoMoldura(int indiceMoldura)
        {
            if (!ordemMolduras.Contains(indiceMoldura))
            {
                ordemMolduras.Add(indiceMoldura);
            }
        }

        public int SelecionarMolduraVitima(Moldura[] molduras)
        {
            if (ordemMolduras.Count == 0)
            {
                return 0;
            }

            int indiceVitima = ordemMolduras[0];

            ordemMolduras.RemoveAt(0);

            return indiceVitima;
        }
    }


}