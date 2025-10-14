using Front.Models;
using TDRevision.Models;
using TDRevision.Models.DTO;

namespace TDRevision.BlazorApp.Services
{
    public interface ICommandeService
    {
        Task<List<CommandeDTO>> GetAllCommandesAsync();
        Task<Commande?> GetCommandeByIdAsync(int id);
        Task<Commande> CreateCommandeAsync(Commande commande);
        Task UpdateCommandeAsync(int id, Commande commande);
        Task DeleteCommandeAsync(int id);
    }
}