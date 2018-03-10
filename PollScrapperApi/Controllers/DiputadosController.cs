using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using PollScrapperApi.Models;
using CnModels = PollScrapperApi.ContemosNosotros.Models;
using Newtonsoft.Json;

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
            try
            {
                return Ok(new Respuesta<IList<DiputadoPreferencia>>(await ObtenerPreferenciasUnidas()));
            }
            catch (Exception)
            {
            }
            return NotFound();
        }

        private async Task<IList<DiputadoPreferencia>> ObtenerPreferenciasUnidas()
        {
            var taskPrefs = ObtenerPreferencias();
            var taskDiputadosCn = ObtenerDiputadosCnAsync();
            await Task.WhenAll(new Task[] { taskPrefs, taskDiputadosCn });

            var diputadosCn = taskDiputadosCn.Result.Data;

            var listaDiputados = new List<DiputadoPreferencia>();

            foreach (var diputadoPref in taskPrefs.Result)
            {
                diputadoPref.CnInfo
                    = diputadosCn.FirstOrDefault(x =>
                        string.Equals(x.DepartamentoNombre, diputadoPref.Departamento.NombreDepartamento, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(x.PartidoNombre, diputadoPref.AbreviaturaPartido, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(x.DiputadoNombre, diputadoPref.Nombre, StringComparison.OrdinalIgnoreCase));

                listaDiputados.Add(diputadoPref);
            }

            return listaDiputados;
        }

        private async Task<IEnumerable<DiputadoPreferencia>> ObtenerPreferencias()
        {
            var urlDeptos = "http://elecciones2018.tse.gob.sv/data/results/r/0/1/6.json";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(urlDeptos))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    dynamic json = JObject.Parse(data);
                    return ObtenerDetalles(json);
                }
            }
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

        private async Task<CnModels.RespuestaDiputados> ObtenerDiputadosCnAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync("https://contemosnosotros.org/staging/api"))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        return JsonConvert.DeserializeObject<CnModels.RespuestaDiputados>(data);
                    }
                    else
                    {
                        return new CnModels.RespuestaDiputados();
                    }
                }
            }
        }
    }
}
