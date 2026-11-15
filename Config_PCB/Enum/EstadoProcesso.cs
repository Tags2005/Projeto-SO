namespace Projeto_SO.Config_PCB.Enum
{
    // Representa os estados do ciclo de vida de um Processo (PCB) no sistema operacional
    public enum EstadoProcesso
    {
        Novo,   //O processo foi criado, mas ainda está sendo carregado no sistema
        Pronto, //O processo está pronto para ter suas threads escalonadas
        Executando, //O processo possui ao menos uma thread executando na CPU
        Bloqueado,  //O processo está bloqueado, aguardando algum evento externo (como I/O) para continuar  
        Finalizado  //Todas as threads do processo terminaram e seus recursos foram liberado
    }
}