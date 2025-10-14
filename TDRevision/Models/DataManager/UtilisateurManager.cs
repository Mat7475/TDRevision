using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDRevision.Models;
using TDRevision.Models.EntityFramework;
using TDRevision.Models.Repository;

public class UtilisateurManager : IDataRepository<Utilisateur,int,string>
{
    private readonly AppDbContext _context;

    public UtilisateurManager(AppDbContext context) 
    {
        _context = context;
    }

    public async Task AddAsync(Utilisateur entity)
    {
        await _context.Utilisateurs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async  Task DeleteAsync(Utilisateur entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Utilisateur>> GetAllAsync()
    {
        return await _context.Utilisateurs.Include(c => c.Commandes).ToListAsync();
    }

    public async Task<Utilisateur> GetByIdAsync(int id)
    {
        return await _context.Utilisateurs.Include(c => c.Commandes).FirstOrDefaultAsync(c => c.IdUtilisateur == id);
    }

    public Task<Commande> GetByKeyAsync(string str)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Utilisateur entityToUpdate, Utilisateur entity)
    {
        _context.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }
}
