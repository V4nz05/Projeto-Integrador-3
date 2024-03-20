using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Clima
{
    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("condicao")]
    public string Condicao { get; set; }

    [JsonProperty("min")]
    public int Min { get; set; }

    [JsonProperty("max")]
    public int Max { get; set; }

    [JsonProperty("indice_uv")]
    public int IndiceUv { get; set; }

    [JsonProperty("condicao_desc")]
    public string CondicaoDesc { get; set; }
}

public class ClimaRoot
{
    [JsonProperty("cidade")]
    public string Cidade { get; set; }

    [JsonProperty("estado")]
    public string Estado { get; set; }

    [JsonProperty("atualizado_em")]
    public string AtualizadoEm { get; set; }

    [JsonProperty("clima")]
    public List<Clima> Clima { get; set; }
}

public class DadosOnda
{
    [JsonProperty("vento")]
    public double Vento { get; set; }

    [JsonProperty("direcao_vento")]
    public string DirecaoVento { get; set; }

    [JsonProperty("direcao_vento_desc")]
    public string DirecaoVentoDesc { get; set; }

    [JsonProperty("altura_onda")]
    public double AlturaOnda { get; set; }

    [JsonProperty("direcao_onda")]
    public string DirecaoOnda { get; set; }

    [JsonProperty("direcao_onda_desc")]
    public string DirecaoOndaDesc { get; set; }

    [JsonProperty("agitacao")]
    public string Agitacao { get; set; }

    [JsonProperty("hora")]
    public string Hora { get; set; }
}

public class Onda
{
    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("dados_ondas")]
    public List<DadosOnda> DadosOndas { get; set; }
}

public class OndaRoot
{
    [JsonProperty("cidade")]
    public string Cidade { get; set; }

    [JsonProperty("estado")]
    public string Estado { get; set; }

    [JsonProperty("atualizado_em")]
    public string AtualizadoEm { get; set; }

    [JsonProperty("ondas")]
    public List<Onda> Ondas { get; set; }
}

public class ApiService
{
    private HttpClient _client;

    public ApiService()
    {
        _client = new HttpClient();
    }

    public async Task<ClimaRoot> GetClimaDataAsync(string cityId)
    {
        var response = await _client.GetAsync($"https://brasilapi.com.br/api/cptec/v1/clima/previsao/{cityId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var climaData = JsonConvert.DeserializeObject<ClimaRoot>(content);
        return climaData;
    }

    public async Task<OndaRoot> GetOndaDataAsync(string cityId)
    {
        var response = await _client.GetAsync($"https://brasilapi.com.br/api/cptec/v1/ondas/{cityId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var ondaData = JsonConvert.DeserializeObject<OndaRoot>(content);
        return ondaData;
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var apiService = new ApiService();

        var climaData = await apiService.GetClimaDataAsync("2535");
        var ondaData = await apiService.GetOndaDataAsync("2535");

        Console.WriteLine($"Dados de clima para {climaData.Cidade}, {climaData.Estado}");
        foreach (var clima in climaData.Clima)
        {
            Console.WriteLine($"Data: {clima.Data}, Condição: {clima.Condicao}, Mínima: {clima.Min}, Máxima: {clima.Max}");
        }

        Console.WriteLine();

        Console.WriteLine($"Dados de ondas para {ondaData.Cidade}, {ondaData.Estado}");
        foreach (var onda in ondaData.Ondas)
        {
            Console.WriteLine($"Data: {onda.Data}");
            foreach (var dadosOnda in onda.DadosOndas)
            {
                Console.WriteLine($"Hora: {dadosOnda.Hora}, Vento: {dadosOnda.Vento}, Direção do Vento: {dadosOnda.DirecaoVento}, Altura da Onda: {dadosOnda.AlturaOnda}");
            }
        }
    }
}