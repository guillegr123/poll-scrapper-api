using System.Collections.Generic;

namespace PollScrapperApi.ContemosNosotros.Models
{
    public class RespuestaDiputados
    {
        public IList<Diputado> Data { set; get; }

        public RespuestaDiputados()
        {
            Data = new List<Diputado>();
        }
    }
}
