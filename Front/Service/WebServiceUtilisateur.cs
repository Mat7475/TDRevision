using Front.Models;
using System.Net.Http.Json;

namespace Front.Service
{
    public class WebServiceUtilisateur : IService<Utilisateur, int, string>
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public WebServiceUtilisateur(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = $"https://localhost:7112/api/{endpoint}";
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Utilisateur>>($"{_endpoint}/GetAll");
        }

        public async Task<Utilisateur> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Utilisateur>($"{_endpoint}/GetByID/{id}");
        }

        public async Task UpdateAsync(int id, Utilisateur entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/Update/{id}", entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Utilisateur> GetByKeyAsync(string str)
        {
            return await _httpClient.GetFromJsonAsync<Utilisateur>($"{_endpoint}/GetByString/{str}");
        }

        public async Task AddAsync(Utilisateur entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/Post", entity);
            response.EnsureSuccessStatusCode();
            await response.Content.ReadFromJsonAsync<Utilisateur>();
        }
    }
}
