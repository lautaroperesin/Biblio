using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Models.InstitutoApp
{
    public class Alumno
    {
        public int Id { get; set; }
        public string ApellidoNombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Eliminado { get; set; } = false;

        public ICollection<InscriptoCarrera> InscripcionesACarreras { get; set; } = new List<InscriptoCarrera>();

        [NotMapped]
        public string Inscripto_a_Carrera
        {
            get
            {
                return string.Join(", ", InscripcionesACarreras.Select(x => x.Carrera?.Nombre));
            }
        }


        public override string ToString()
        {
            return ApellidoNombre;
        }

    }
}
