using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json; // essa é um pacote que eu adicionei manualmente no nuget packages para conseruir ler o json
class Program
{
    static void Main(string[] args)
    {
        // URL do arquivo JSON com o valor de faturamento diário (importei para meu github) mas é o mesmo que foi passado no e-mail.
        string url = "https://raw.githubusercontent.com/Levixs/lendojson_cshap/main/dados.json";

        // cria um objeto WebClient para baixar o arquivo JSON
        WebClient webClient = new WebClient();

        try
        {
            // obtem o arquivo JSON e armazena em uma string
            string json = webClient.DownloadString(url);

            // converte o arquivo JSON para um array
            dynamic[] faturamento = JsonConvert.DeserializeObject<dynamic[]>(json);

            // obtem o menor valor de faturamento diário
            decimal menorFaturamento = faturamento.Min(dia => (decimal)dia.valor);

            // obtem o maior valor de faturamento diário
            decimal maiorFaturamento = faturamento.Max(dia => (decimal)dia.valor);

            // obtem a média mensal de faturamento diário (ignorando dias sem faturamento)
            decimal mediaMensal = faturamento.Where(dia => (decimal)dia.valor > 0)
                                             .Average(dia => (decimal)dia.valor);

            // obtem o número de dias em que o valor de faturamento diário foi superior à média mensal
            int diasAcimaDaMedia = faturamento.Count(dia => (decimal)dia.valor > mediaMensal);

            // mostra os resultados na tela
            Console.WriteLine($"Menor faturamento diário: {menorFaturamento:C}");
            Console.WriteLine($"Maior faturamento diário: {maiorFaturamento:C}");
            Console.WriteLine($"Dias com faturamento acima da média mensal: {diasAcimaDaMedia}");
            Console.WriteLine($"Média mensal de faturamento: {Math.Round(mediaMensal, 2):C}");
            Console.ReadKey();
        }
        catch (WebException ex)
        {
            Console.WriteLine($"Erro ao acessar o arquivo JSON: {ex.Message}");
        }
    }
}
