using Projeto_SO.Config_PCB;
namespace Projeto_SO.Config_GerencMem
{
    public interface IPoliticaSubstituicaoPagina
    {
        int SelecionarMolduraVitima(Moldura[] molduras);
        void RegistrarUsoMoldura(int indiceMoldura);
    }
}