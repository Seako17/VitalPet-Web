using System.Collections.Generic;

namespace FGF_Finanzas.Capas.BE
{
    public class BEDisponibilidad
    {
        public bool Ok { get; set; }
        public string CodigoError { get; set; }
        public string Mensaje { get; set; }
        public int IdMascota { get; set; }
        public string DniVeterinario { get; set; }
        public string NombreVeterinario { get; set; }
        public string Fecha { get; set; }
        public List<string> HorariosLibres { get; set; }
        public int CantidadHorarios { get; set; }
        public string PrimerHorarioDisponible { get; set; }

        public BEDisponibilidad()
        {
            HorariosLibres = new List<string>();
        }
    }
}
