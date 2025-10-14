using Front.Models;
using System.Net.Http.Json;
using TDRevision.Models;
using TDRevision.Models.DTO;

namespace TDRevision.BlazorApp.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/Commande";

        public CommandeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CommandeDTO>> GetAllCommandesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CommandeDTO>>($"{BaseUrl}/GetAll");
                return response ?? new List<CommandeDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des commandes: {ex.Message}");
                return new List<CommandeDTO>();
            }
        }

        public async Task<Commande?> GetCommandeByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Commande>($"{BaseUrl}/GetByID/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de la commande {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Commande> CreateCommandeAsync(Commande commande)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Post", commande);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Commande>()
                ?? throw new Exception("Erreur lors de la création de la commande");
        }

        public async Task UpdateCommandeAsync(int id, Commande commande)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Update/{id}", commande);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCommandeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}