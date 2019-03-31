
namespace GlobalCityManager.Models
{
    public partial class Region
    {
        public Region(string pNombre)
        {
            Nombre = pNombre;
        }
        public string Nombre { get; set; }
    }
}
