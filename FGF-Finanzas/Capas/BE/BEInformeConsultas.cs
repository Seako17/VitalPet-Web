using System;
using System.Collections.Generic;

namespace FGF_Finanzas.Capas.BE
{
    public class BEInformeConsultas
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string DniVeterinario { get; set; }
        public string NombreVeterinario { get; set; }
        public DateTime Generado { get; set; }
        public List<BEInformeVeterinario> Detalle { get; set; } = new List<BEInformeVeterinario>();
        public List<BEInformeMes> Meses { get; set; } = new List<BEInformeMes>();
    }

    public class BEInformeMes
    {
        public int Mes { get; set; }
        public int Atendidos { get; set; }
    }

    public class BEInformeVeterinario
    {
        public string Dni { get; set; }
        public string Veterinario { get; set; }
        public int Turnos { get; set; }
        public int Atendidos { get; set; }
        public int Cancelados { get; set; }
        public int Ausentes { get; set; }
        public int Pendientes { get; set; }
        public decimal Porcentaje => Turnos == 0 ? 0 : Math.Round(100m * Atendidos / Turnos, 1);
    }
}
