using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEConsulta
    {
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