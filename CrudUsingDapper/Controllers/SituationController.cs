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
    public class SituationController : ControllerBase
    {
        private ISituation _situationService;

        public SituationController(ISituation situationService)
        {
            _situationService = situationService;
        }

        // GET: api/Situation
        [HttpGet]
        public IEnumerable<Situation_client> Get()
        {
            return _situationService.Gets();
        }

        // GET api/Situation/5
        [HttpGet("{id}")]
        public Situation_client Get(int id)
        {
            return _situationService.Get(id);
        }

        // POST api/<SituationController>
        [HttpPost]
        public Situation_client Post([FromBody] Situation_client situation)
        {
            if (ModelState.IsValid)
            {
                return _situationService.Insert(situation);
            }

            return null;
        }

        // PUT api/<SituationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SituationController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return _situationService.Delete(id);
        }
    }
}
