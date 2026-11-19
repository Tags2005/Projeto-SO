using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_SO.SistemaArquivos
{
    // Representa um diretório na estrutura hierárquica do Sistema de Arquivos
    public class Diretorio
    {
        public string Nome { get; private set; }
        public Diretorio? Pai { get; private set; }

        // Dicionário de subdiretórios: Nome do diretório -> Objeto Diretorio
        public Dictionary<string, Diretorio> Subdiretorios { get; } = new Dictionary<string, Diretorio>();

        // Dicionário de arquivos contidos neste diretório: Nome do arquivo -> Objeto FCB
        public Dictionary<string, FCB> Arquivos { get; } = new Dictionary<string, FCB>();

        public Diretorio(string nome, Diretorio? pai = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do diretório não pode ser vazio.", nameof(nome));
            }

            Nome = nome;
            Pai = pai;
        }

        // Retorna o caminho absoluto deste diretório (ex: /docs/trabalhos)
        public string ObterCaminhoAbsoluto()
        {
            if (Pai == null || Pai.Nome == "/")
            {
                return "/" + (Nome == "/" ? "" : Nome);
            }
            return Pai.ObterCaminhoAbsoluto() + "/" + Nome;
        }
    }
}