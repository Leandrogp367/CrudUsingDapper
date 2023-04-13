using System.Collections.Generic;
using System.Threading.Tasks;
using CrudUsingDapper.IServices;
using CrudUsingDapper.Models;
using CrudUsingDapper.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudUsingDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private IType _typeSevice;

        public TypeController(IType typeService)
        {
            _typeSevice = typeService;
        }

        // GET: api/<TypeController>
        [HttpGet]
        public IEnumerable<Type_client> Get()
        {
            return _typeSevice.Gets();
        }

        // GET api/<TypeController>/5
        [HttpGet("{id}")]
        public Type_client Get(int id)
        {
            return _typeSevice.Get(id);
        }

        // POST api/<TypeController>
        [HttpPost]
        public Type_client Post([FromBody] Type_client type)
        {
            if (ModelState.IsValid)
            {
                return _typeSevice.Insert(type);
            }

            return null;
        }

        // PUT api/<TypeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TypeController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return _typeSevice.Delete(id);
        }
    }
}
