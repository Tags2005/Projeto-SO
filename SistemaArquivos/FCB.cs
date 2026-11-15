using System;
using System.Text;
using System.Collections.Generic;

namespace Projeto_SO.SistemaArquivos
{
    // Bloco de Controle de Arquivo (File Control Block)
    public class FCB
    {
        public int IdArquivo { get; private set; }
        public string Nome { get; private set; }

        private readonly List<byte> dados = new List<byte>();

        public int Tamanho => dados.Count;

        public int InstanteCriacao { get; private set; }
        public int InstanteUltimoAcesso { get; private set; }

        // Quantidade de aberturas ativas (impede remoção enquanto > 0)
        public int ContadorAberturas { get; private set; }

        public bool EstaAberto => ContadorAberturas > 0;

        public FCB(int idArquivo, string nome, int instanteCriacao)
        {
            IdArquivo = idArquivo;
            Nome = nome;
            InstanteCriacao = instanteCriacao;
            InstanteUltimoAcesso = instanteCriacao;
        }

        public void RegistrarAbertura(int clock)
        {
            ContadorAberturas++;
            InstanteUltimoAcesso = clock;
        }

        public void RegistrarFechamento()
        {
            if (ContadorAberturas > 0)
            {
                ContadorAberturas--;
            }
        }

        // Lê até 'quantidade' bytes a partir de 'posicao'
        public byte[] Ler(int posicao, int quantidade, int clock)
        {
            int disponiveis = Tamanho - posicao;
            int total = Math.Max(0, Math.Min(quantidade, disponiveis));

            InstanteUltimoAcesso = clock;
            return total == 0 ? Array.Empty<byte>() : dados.GetRange(posicao, total).ToArray();
        }

        // Escreve a partir de 'posicao', sobrescrevendo e/ou estendendo o arquivo
        public int Escrever(int posicao, byte[] bytes, int clock)
        {
            foreach (byte b in bytes)
            {
                if (posicao < dados.Count)
                {
                    dados[posicao] = b;
                }
                else
                {
                    dados.Add(b);
                }
                posicao++;
            }

            InstanteUltimoAcesso = clock;
            return bytes.Length;
        }
    }
}