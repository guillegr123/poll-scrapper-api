namespace PollScrapperApi.Models
{
    public class Respuesta<TModel>
    {
        public TModel Datos { set; get; }

        public Respuesta(TModel datos)
        {
            Datos = datos;
        }
    }
}
