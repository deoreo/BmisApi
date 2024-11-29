namespace BmisApi.Services
{
    public interface ICrudService<TGet,TGetAll, TCreate, TUpdate>
    {
        Task<TGet?> GetByIdAsync(int id);
        Task<TGet> CreateAsync(TCreate request);
        Task DeleteAsync(int id);
        Task<TGet?> UpdateAsync(TUpdate request, int id);
        Task<TGetAll> GetAllAsync();
        Task<TGetAll> Search(string name);
    }
}
