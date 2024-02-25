using ApiBank.Web.Models;

namespace ApiBank.Web.Repositories.Interfaces
{
    public interface IContaRepository
    {
        Task<List<Conta>> GetAll();

        Task<Conta> GetForId(int id);

        Task<Conta> Insert(Conta conta);

        Task<Conta> Update(Conta conta);

        Task<bool> Delete(int id);
    }
}
