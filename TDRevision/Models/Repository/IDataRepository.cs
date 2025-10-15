using Microsoft.AspNetCore.Mvc;

namespace TDRevision.Models.Repository
{
    public interface IDataRepository<Tentity, Tidentifier, TKey> : ReadableRepository<Tentity, Tidentifier>, WriteableRepository<Tentity>, SearchableRepository<Tentity, TKey>
    {
    }

    public interface ReadableRepository<Tentity,Tidentifier>  
    {
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task<Tentity> GetByIdAsync(int id);
    }

    public interface SearchableRepository<Tentity, TKey>
    {
        Task<Tentity> GetByKeyAsync(TKey str);
    }

    public interface WriteableRepository<Tentity>  
    {
        Task AddAsync(Tentity entity);
        Task UpdateAsync(Tentity entityToUpdate,Tentity entity);
        Task DeleteAsync(Tentity entity);
    }

}
