namespace PollScrapperApi.Models
{
    public class Departamento
    {
        public int CodDepartamento { set; get; }
        public string NombreDepartamento { set; get; }

        public Departamento(dynamic dValue)
        {
            CodDepartamento = dValue.customCode;
            NombreDepartamento = dValue.name;
        }
    }
}
