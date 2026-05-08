using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEEvento
    {
    
        public BEUsuario usuario { get; set; }
        public DateTime fechaHora { get; set; }
        public string modulo { get; set; }
        public string evento { get; set; }
        public int criticidad { get; set; }
        public BEEvento(BEUsuario usuario, DateTime fechaHora, string modulo, string evento, int criticidad)
        {
            this.usuario = usuario;
            this.fechaHora = fechaHora;
            this.modulo = modulo;
            this.evento = evento;
            this.criticidad = criticidad;
        }
    }
}