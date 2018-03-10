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

        public DiputadoPreferencia(dynamic candidato, dynamic departamentoValue)
        {
            Codigo = candidato.ballotCode;
            Nombre = candidato.ballotName;
            AbreviaturaPartido = candidato.partyAbbr;
            Votos = candidato.amount;
            Departamento = new Departamento(departamentoValue);

            Nombre = Nombre.Replace("&Ntilde;", "Ñ");
        }
    }
}
