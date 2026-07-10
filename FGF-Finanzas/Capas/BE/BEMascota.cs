using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEMascota
    {
        public BEUsuario usuario { get; set; }
        public string nombre { get; set; }
        public string especie { get; set; }
        public string raza { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public BEMascota(BEUsuario usuario, string nombre, string especie, string raza, DateTime fecha)
        {
            this.usuario = usuario;
            this.nombre = nombre;
            this.especie = especie;
            this.raza = raza;
            this.fechaNacimiento = fecha;
        }
    }
}