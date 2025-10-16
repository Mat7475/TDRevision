using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDRevision.Models;
using TDRevision.Models.EntityFramework;
using TDRevision.Models.Repository;

namespace TDRevision.Models.DataManager;
public class CommandeManager : IDataRepository<Commande,int,string>
{
    private readonly AppDbContext _context;

    public CommandeManager(AppDbContext context) 
    {
        _context = context;
    }

    public async Task AddAsync(Commande entity)
    {
        await _context.Commandes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async  Task DeleteAsync(Commande entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Commande>> GetAllAsync()
    {
        return await _context.Commandes.Include(c => c.CommandeNav).ToListAsync();
    }

    public async Task<Commande> GetByIdAsync(int id)
    {
        return await _context.Commandes.Include(c => c.CommandeNav).FirstOrDefaultAsync(c => c.IdCommande == id);
    }

    public async Task<Commande> GetByKeyAsync(string str)
    {
        return await _context.Commandes.FirstOrDefaultAsync(c => c.NomArticle.ToLower().Contains(str.ToLower()));
    }

    public async Task UpdateAsync(Commande entityToUpdate, Commande entity)
    {
        _context.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }
}
