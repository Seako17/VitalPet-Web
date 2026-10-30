using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEConsulta
    {
        public const string Agendada = "Agendada";
        public const string Atendida = "Atendida";
        public const string Cancelada = "Cancelada";
        public int idConsulta { get; set; }
        public string estado { get; set; } = Agendada;
        public string diagnostico { get; set; }
        public string nombreMascota { get; set; }
        public string nombreVeterinario { get; set; }
        public int idMascota { get; set; }
        public BEUsuario veterinario { get; set; }
        public string motivo { get; set; }
        public DateTime fechaHora { get; set; }

        public BEConsulta(int idMascota, BEUsuario veterinario, string motivo, DateTime fechaHora)
        {
            this.idMascota = idMascota;
            this.veterinario = veterinario;
            this.motivo = motivo;
            this.fechaHora = fechaHora;
        }
        public BEConsulta()
        {
            
        }
    }
}
