using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using PollScrapperApi.Models;

namespace PollScrapperApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class DataController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var urlDeptos = "http://elecciones2018.tse.gob.sv/data/navigation/root.json";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(urlDeptos))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        dynamic json = JObject.Parse(data);
                        return Ok(new Respuesta<IEnumerable<Detalle>>(ObtenerDetalles(json)));
                    }
                }
            }
            return NotFound();
        }

        private IEnumerable<Detalle> ObtenerDetalles(dynamic jsonDeptos)
        {
            foreach (var d in jsonDeptos.subRegions)
            {
                yield return new Detalle
                {
                    CodDepartamento = d.Value.customCode,
                    NombreDepartamento = d.Value.name
                };
            }
        }

        /*
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
