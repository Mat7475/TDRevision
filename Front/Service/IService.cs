namespace Front.Service
{
    public interface IService<Tentity, Tidentifier,TKey> : ServiceReadableRepository<Tentity, Tidentifier>, ServiceWriteableRepository<Tentity>, ServiceSearchableRepository<Tentity, TKey>
    {
    }

    public interface ServiceReadableRepository<Tentity, Tidentifier>
    {
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task<Tentity> GetByIdAsync(int id);
    }

    public interface ServiceSearchableRepository<Tentity, TKey>
    {
        Task<Tentity> GetByKeyAsync(TKey str);
    }

    public interface ServiceWriteableRepository<Tentity>
    {
        Task AddAsync(Tentity entity);
        Task UpdateAsync(int id, Tentity entity);
        Task DeleteAsync(int id );
    }
}
