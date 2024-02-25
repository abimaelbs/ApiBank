using ApiBank.Web.Database;
using ApiBank.Web.Models;
using ApiBank.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiBank.Web.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly ApiBankContext _dbContext;
        public ContaRepository(ApiBankContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<List<Conta>> GetAll()
        {
            return await _dbContext.Contas.ToListAsync();
        }

        public async Task<Conta> GetForId(int id)
        {
            return await _dbContext.Contas.FirstAsync(c => c.Id == id);
        }

        public async Task<Conta> Insert(Conta conta)
        {
            await _dbContext.Contas.AddAsync(conta);
            await _dbContext.SaveChangesAsync();

            return conta;
        }

        public async Task<Conta> Update(Conta conta)
        {
            var contaReturn = await GetForId(conta.Id);

            if (contaReturn == null) 
                throw new Exception($"Id de conta '{conta.Id}' não existente na base de dados.");

            _dbContext.Contas.Update(conta);
            await _dbContext.SaveChangesAsync();

            return conta;
        }

        public async Task<bool> Delete(int id)
        {
            var contaReturn = await GetForId(id);

            if (contaReturn == null)
                throw new Exception($"Id de conta '{id}' não existente na base de dados.");

            _dbContext.Contas.Remove(contaReturn);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
