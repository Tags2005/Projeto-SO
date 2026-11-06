using Projeto_SO.Config_PCB;

public interface IPoliticaSubstituicaoPagina
{
    int SelecionarMolduraVitima();
    void RegistrarUsoMoldura(int indiceMoldura);
    void RemoverMoldura(int indiceMoldura); 
}
