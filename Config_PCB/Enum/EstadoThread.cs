namespace Projeto_SO.Config_PCB.EnumEstadoThread
{
    //Representa os estados do ciclo de vida de uma Thread(TCB) individual
    public enum EstadoThread
    {
        Novo,   //A thread foi instanciada, mas ainda não entrou na fila de prontos
        Pronto, //A thread está na fila de prontos aguardando para assumir a CPU
        Executando, //A thread está ocupando a CPU e consumindo seu surto de processamento
        Bloqueado,  //A thread foi suspensa aguardando a resolução de um Page Fault
        Finalizado  //A thread concluiu todo o seu tempo de processamento necessário
    }
}