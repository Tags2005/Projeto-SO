using System;
using System.IO;

namespace Projeto_SO
{
    public class RegistradorLog
    {
        private readonly string caminhoFicheiro;

        public RegistradorLog(string caminhoArquivo = "simulacao_log.txt")
        {
            caminhoFicheiro = caminhoArquivo;

            // Limpa o arquivo de log anterior ao iniciar uma nova execução
            if (File.Exists(caminhoFicheiro))
            {
                File.Delete(caminhoFicheiro);
            }
        }

        public void Registrar(int clock, string evento, string entidade, string mensagem)
        {
            string linhaLog = $"[{clock:D4}] [{evento}] [{entidade}] - {mensagem}";
            Console.WriteLine(linhaLog);
            File.AppendAllText(caminhoFicheiro, linhaLog + Environment.NewLine);
        }
    }
}