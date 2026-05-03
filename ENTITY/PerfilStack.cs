
namespace ENTITY
{
    public class PerfilStack : BaseEntity
    {
        public int Id { get; set; }
        public string NombrePerfil { get; set; }
        
        public System.Collections.Generic.List<Herramienta> Herramientas { get; set; } = new System.Collections.Generic.List<Herramienta>();
    }
}
