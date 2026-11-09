using System.Collections.Generic;

namespace Projeto_SO.Config_PCB
{
    public class RegistradoresSimulados
    {
        public Dictionary<string, int> Registradores { get; private set; }

        public RegistradoresSimulados()
        {
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