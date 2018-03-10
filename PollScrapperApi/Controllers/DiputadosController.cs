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
    public class DiputadosController : Controller
    {
        // GET api/values
        [HttpGet]
        [Route("preferencias")]
        public async Task<IActionResult> Get()
        {
            var urlDeptos = "http://elecciones2018.tse.gob.sv/data/results/r/0/1/6.json";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(urlDeptos))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        dynamic json = JObject.Parse(data);
                        return Ok(new Respuesta<IEnumerable<DiputadoPreferencia>>(ObtenerDetalles(json)));
                    }
                }
            }
            return NotFound();
        }

        private IEnumerable<DiputadoPreferencia> ObtenerDetalles(dynamic json)
        {
            foreach (var d in json.children)
            {
                var departamentoValue = d.Value;
                foreach (var candidato in d.Value.optionsRegister)
                {
                    yield return new DiputadoPreferencia
                    {
                        Codigo = candidato.ballotCode,
                        Nombre = candidato.ballotName,
                        AbreviaturaPartido = candidato.partyAbbr,
                        Votos = candidato.amount,
                        Departamento = new Departamento(departamentoValue)
                    };
                }
            }
        }
    }
}
