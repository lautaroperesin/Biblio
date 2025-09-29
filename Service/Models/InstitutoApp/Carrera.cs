namespace Service.Models.InstitutoApp
{
    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;


        public override string ToString()
        {
            return Nombre;
        }
    }
}
