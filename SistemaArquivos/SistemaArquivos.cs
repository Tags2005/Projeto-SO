using Projeto_SO.SistemaArquivos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projeto_SO.SistemaArquivos
{
    // Sistema de arquivos simulado em memória (diretório plano)
    // Operações: criar, abrir, ler, escrever, fechar e remover.
    public class SistemaArquivos
    {
        private readonly Dictionary<string, FCB> diretorio = new Dictionary<string, FCB>();
        private readonly RegistradorLog? log;
        private readonly Func<int> obterClock;
        private int proximoIdArquivo = 1;

        public IReadOnlyCollection<FCB> Arquivos => diretorio.Values;

        public SistemaArquivos(RegistradorLog? log = null, Func<int>? obterClock = null)
        {
            this.log = log;
            this.obterClock = obterClock ?? (() => 0);
        }

        // Cria um arquivo vazio
        public FCB Criar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do arquivo é obrigatório.", nameof(nome));
            }

            if (diretorio.ContainsKey(nome))
            {
                throw new IOException($"O arquivo '{nome}' já existe.");
            }

            FCB fcb = new FCB(proximoIdArquivo++, nome, obterClock());
            diretorio[nome] = fcb;

            log?.Registrar(obterClock(), "ARQ_CRIAR", $"FCB {fcb.IdArquivo}", $"Arquivo '{nome}' criado.");
            return fcb;
        }

        public int Abrir(PCB processo, string nome)
        {
            FCB fcb = ObterArquivo(nome);

            int fd = 3; // 0, 1 e 2 (entrada, saída e erro padrão)
            while (processo.TabelaArquivosAbertos.ContainsKey(fd))
            {
                fd++;
            }

            processo.TabelaArquivosAbertos[fd] = new DescritorArquivo(fcb);
            fcb.RegistrarAbertura(obterClock());

            log?.Registrar(obterClock(), "ARQ_ABRIR", $"PCB {processo.IdProcesso}",
                $"Arquivo '{nome}' aberto (fd {fd}).");
            return fd;
        }

        // Lê até 'quantidade' bytes a partir da posição atual do descritor
        public byte[] Ler(PCB processo, int fd, int quantidade)
        {
            DescritorArquivo descritor = ObterDescritor(processo, fd);

            byte[] resultado = descritor.Arquivo.Ler(descritor.Posicao, quantidade, obterClock());
            descritor.Posicao += resultado.Length;

            log?.Registrar(obterClock(), "ARQ_LER", $"PCB {processo.IdProcesso}",
                $"Leu {resultado.Length} byte(s) de '{descritor.Arquivo.Nome}' (fd {fd}).");
            return resultado;
        }

        // Escreve bytes na posição atual do descritor
        public int Escrever(PCB processo, int fd, byte[] dados)
        {
            DescritorArquivo descritor = ObterDescritor(processo, fd);

            int escritos = descritor.Arquivo.Escrever(descritor.Posicao, dados, obterClock());
            descritor.Posicao += escritos;

            log?.Registrar(obterClock(), "ARQ_ESCREVER", $"PCB {processo.IdProcesso}",
                $"Escreveu {escritos} byte(s) em '{descritor.Arquivo.Nome}' (fd {fd}).");
            return escritos;
        }

        // Fecha o descritor do processo
        public void Fechar(PCB processo, int fd)
        {
            DescritorArquivo descritor = ObterDescritor(processo, fd);

            processo.TabelaArquivosAbertos.Remove(fd);
            descritor.Arquivo.RegistrarFechamento();

            log?.Registrar(obterClock(), "ARQ_FECHAR", $"PCB {processo.IdProcesso}",
                $"Arquivo '{descritor.Arquivo.Nome}' fechado (fd {fd}).");
        }

        // Remove o arquivo do diretório
        // Não permite remover se ainda estiver aberto
        public void Remover(string nome)
        {
            FCB fcb = ObterArquivo(nome);

            if (fcb.EstaAberto)
            {
                throw new IOException($"O arquivo '{nome}' está aberto e não pode ser removido.");
            }

            diretorio.Remove(nome);

            log?.Registrar(obterClock(), "ARQ_REMOVER", $"FCB {fcb.IdArquivo}", $"Arquivo '{nome}' removido.");
        }

        private FCB ObterArquivo(string nome)
        {
            if (!diretorio.TryGetValue(nome, out FCB? fcb))
            {
                throw new FileNotFoundException($"Arquivo '{nome}' não encontrado.");
            }
            return fcb;
        }

        private static DescritorArquivo ObterDescritor(PCB processo, int fd)
        {
            if (processo.TabelaArquivosAbertos.TryGetValue(fd, out object? obj) && obj is DescritorArquivo descritor)
            {
                return descritor;
            }
            throw new InvalidOperationException($"Descritor {fd} inválido para o processo {processo.IdProcesso}.");
        }
    }
}