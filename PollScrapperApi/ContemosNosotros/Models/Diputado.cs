using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollScrapperApi.ContemosNosotros.Models
{
    public class Diputado
    {
        [JsonProperty("dpto_id")]
        public int DepartamentoId { set; get; }

        [JsonProperty("dpto_nombre")]
        public string DepartamentoNombre { set; get; }

        [JsonProperty("partido_id")]
        public int PartidoId { set; get; }

        [JsonProperty("partido_nombre")]
        public string PartidoNombre { set; get; }

        [JsonProperty("diputado_id")]
        public int DiputadoId { set; get; }

        [JsonProperty("diputado_nombre")]
        public string DiputadoNombre { set; get; }
    }
}
