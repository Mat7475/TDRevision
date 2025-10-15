using Front.Models;
using System.Net.Http.Json;

namespace Front.Service
{
    public class WebServiceCommande : IService<Commande, int, string>
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public WebServiceCommande(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7112/api/");
            _endpoint = endpoint;
        }

        public async Task<IEnumerable<Commande>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Commande>>($"{_endpoint}/GetAll");
        }

        public async Task<Commande> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Commande>($"{_endpoint}/GetByID/{id}");
        }

        public async Task UpdateAsync(int id, Commande entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/Update/{id}", entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Commande> GetByKeyAsync(string str)
        {
            return await _httpClient.GetFromJsonAsync<Commande>($"{_endpoint}/GetByString/{str}");
        }

        public async Task AddAsync(Commande entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/Post", entity);
            response.EnsureSuccessStatusCode();
            await response.Content.ReadFromJsonAsync<Commande>();
        }
    }
}
