using System.Collections.Generic;
using System.Threading.Tasks;
using CrudUsingDapper.IServices;
using CrudUsingDapper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClient _clientService;

        public ClientController(IClient clientService)
        {
            _clientService = clientService;
        }

        // GET: api/Client
        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return _clientService.Gets();
        }

        // GET api/Client/5
        [HttpGet("{id}")]
        public Client Get(int id)
        {
            return _clientService.Get(id);
        }

        // POST api/Client
        [HttpPost]
        public Client Post([FromBody] Client client)
        {
            if(ModelState.IsValid)
            {
                return _clientService.Insert(client);
            }

            return null;
        }

        // PUT api/Client/5
        [HttpPut("{id}")]
        public Client Put(int id, [FromBody] Client client)
        {
            if (ModelState.IsValid)
            {
                return _clientService.Update(client);
            }

            return null;
        }

        // DELETE api/Client/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return _clientService.Delete(id);
        }
    }
}
