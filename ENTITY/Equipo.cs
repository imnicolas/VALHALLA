
namespace ENTITY
{
    public class Equipo : BaseEntity
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public string NroSerie { get; set; }
        public string Estado { get; set; }
        public int? IdUsuario { get; set; }
    }
}
