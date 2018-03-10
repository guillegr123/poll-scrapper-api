using CnModels = PollScrapperApi.ContemosNosotros.Models;

namespace PollScrapperApi.Models
{
    public class DiputadoPreferencia
    {
        public int Codigo { set; get; }
        public string Nombre { set; get; }
        public Departamento Departamento { set; get; }
        public string AbreviaturaPartido { set; get; }
        public int Votos { set; get; }

        public CnModels.Diputado CnInfo { set; get; }
    }
}
