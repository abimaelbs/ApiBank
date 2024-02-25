using ApiBank.Web.Models;
using ApiBank.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ApiBank.Web.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")] 
    [ApiController]
    [ApiVersion("1.0")]    
    public class ContaController : ControllerBase
    {
        private readonly IContaRepository _contaRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CONTAS_KEY = "CONTAS";

        public ContaController(IContaRepository contaRepository, IMemoryCache memoryCache)
        {
            _contaRepository = contaRepository; 
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<List<Conta>>> BuscarTodos() 
        {
            if(_memoryCache.TryGetValue(CONTAS_KEY, out List<Conta> lsContas))
            {
                return Ok(lsContas);                     
            }

            List<Conta> contas = await _contaRepository.GetAll();

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions 
            { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)                
            };

            _memoryCache.Set(CONTAS_KEY, contas, memoryCacheEntryOptions);

            return Ok(contas);
        }

        [HttpGet("{id}")]        
        public async Task<ActionResult<Conta>> BuscarPorId(int id)
        {
            if (_memoryCache.TryGetValue(CONTAS_KEY, out List<Conta> lsContas))
            {
                return Ok(lsContas.Where(x => x.Id == id));
            }

            var conta = await _contaRepository.GetForId(id);
            return Ok(conta);
        }

        [HttpPost]
        public async Task<ActionResult<Conta>> Cadastrar([FromBody]Conta conta)
        {
            var retorno = await _contaRepository.Insert(conta);
            return Ok(retorno);
        }

        [HttpPut]
        public async Task<ActionResult<Conta>> Atualizar([FromBody]Conta conta)
        {            
            var retorno = await _contaRepository.Update(conta);
            return Ok(retorno);
        }

        [HttpDelete]
        public async Task<ActionResult<Conta>> Excluir(int id)
        {
            bool retorno = await _contaRepository.Delete(id);
            return Ok(retorno);
        }

        [HttpGet("ImportarDados")]        
        public async Task<ActionResult> ImportarDados()
        {
            const string url = "https://brasilapi.com.br/api/feriados/v1/2023";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                var dados = await response.Content.ReadAsStringAsync();
                var contas = JsonSerializer.Deserialize<List<Conta>>(dados, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                foreach (var item in contas)
                {
                    await _contaRepository.Insert(item);
                }

                return Ok(contas);
            }
        }
    }
}
