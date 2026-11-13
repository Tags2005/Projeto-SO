using System.Collections.Generic;

namespace Projeto_SO.Config_PCB
{
    // Simula o conjunto de registradores de uso geral da CPU (R0 a R3) para cada thread.
    public class RegistradoresSimulados
    {
        //Dicionário contendo o nome do registrador e seu valor inteiro atual
        public Dictionary<string, int> Registradores { get; private set; }

        public RegistradoresSimulados()
        {
            // Inicializa os registradores genéricos com valor zero
            Registradores = new Dictionary<string, int>
            {
                { "R0", 0 },
                { "R1", 0 },
                { "R2", 0 },
                { "R3", 0 }
            };
        }
    }
}