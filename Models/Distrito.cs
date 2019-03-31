
namespace GlobalCityManager.Models
{
    public partial class Distrito
    {
        public Distrito(string pNombre)
        {
            Nombre = pNombre;
        }
        public string Nombre { get; set; }
    }
}
