using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FGF_Finanzas.Capas.BE
{
    public class BEUsuario
    {
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int Intento { get; set; }
        public bool Bloqueado { get; set; }
        public BEUsuario()
        {

        }
        public BEUsuario(string dni, string nombre, string apellido, string username, string password)
        {
            DNI = dni;
            Nombre = nombre;
            Apellido = apellido;
            Usuario = username;
            Contraseña = password;
            Intento = 0;
            Bloqueado = false;
        }

        public BEUsuario(DataRow dr)
        {
            DNI = dr[0].ToString();
            Nombre = dr[1].ToString();
            Apellido = dr[2].ToString();
            Usuario = dr[3].ToString();
            Contraseña = dr[4].ToString();
            Intento = int.Parse(dr[5].ToString());
            Bloqueado = Convert.ToBoolean(dr[6]);
        }
    }
}