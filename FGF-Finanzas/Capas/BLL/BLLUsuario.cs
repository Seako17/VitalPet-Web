using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FGF_Finanzas.Capas.BLL
{
    public class BLLUsuario
    {
        DALUsuario dalUsuario;
        BLLEvento bllEvento;
        public BLLUsuario()
        {
            dalUsuario = new DALUsuario();
            bllEvento = new BLLEvento();
        }

        public void ValidarUsuario(string dni, string usuario, string nombre, string apellido, string contraseña, string confirmacion)
        {
            if (string.IsNullOrWhiteSpace(dni)) throw new Exception("El campo de DNI es obligatorio.");
            if (!Regex.IsMatch(dni, @"^\d{8}$")) throw new Exception("El DNI debe contener 8(ocho) dígitos.");

            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("El campo de Nombre es obligatorio.");
            if (!Regex.IsMatch(nombre, @"^[A-Za-z]{3,}(\s[A-Za-z]{3,})*$")) throw new Exception("Ingrese su/s nombre/s correctamente.");

            if (string.IsNullOrWhiteSpace(apellido)) throw new Exception("El campo de Apellido es obligatorio.");
            if (!Regex.IsMatch(apellido, @"^[A-Za-z]{3,}(\s[A-Za-z]{3,})*$")) throw new Exception("Ingrese su/s apellido/s correctamente.");

            if (string.IsNullOrWhiteSpace(usuario)) throw new Exception("El campo de Usuario es obligatorio.");

            if (!Regex.IsMatch(contraseña, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,20}$"))
            {
                throw new Exception("La contraseña debe tener entre 8 y 20 caracteres, e incluir al menos una mayúscula, una minúscula, un número y un carácter especial (@*_/#$%).");
            }
            if (contraseña != confirmacion) throw new Exception("La contraseña y la contraseña de confirmación no coinciden.");

            DataTable dt = ObtenerUsuarios();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["dni"].ToString() == dni)
                {
                    throw new Exception("DNI ya registrado.");
                }
                if (dr[3].ToString() == usuario)
                {
                    throw new Exception("Usuario ya existente.");
                }
            }
        }

        public DataTable ObtenerUsuarios()
        {
            return dalUsuario.ObtenerUsuarios();
        }

        public void AgregarUsuario(BEUsuario usuario)
        {
            string encriptado = Encriptacion.Encriptar(usuario.Contraseña);
            usuario.Contraseña = encriptado;
            dalUsuario.AgregarUsuario(usuario);
            bllEvento.AgregarEvento(new BEEvento(usuario, DateTime.Now, "Usuarios", "Registrar Usuario", 4));
        }

        public void IniciarSesion(string usuario, string contraseña)
        {
            if (SessionManager.IsLogged()) throw new Exception("Ya ha iniciado sesión.");

            BEUsuario user = null;

            foreach (DataRow item in dalUsuario.ObtenerUsuarios().Rows)
            {
                if (item["usuario"].ToString() == usuario)
                {
                    user = new BEUsuario(item);
                }
            }

            if (user == null) throw new Exception("Credenciales incorrectas.");

            if (user.Bloqueado == true) throw new Exception("Usuario bloqueado");

            if (!Encriptacion.Encriptar(contraseña).Equals(user.Contraseña))
            {
                user.Intento++;

                if (user.Intento == 3)
                {
                    user.Bloqueado = true;
                    user.Intento = 0;
                    dalUsuario.Actualizar(user);
                }
                else { dalUsuario.Actualizar(user); }

                throw new Exception("Credenciales incorrectas.");
            }
            else
            {
                user.Intento = 0;
                dalUsuario.Actualizar(user);
                SessionManager.Login(user);

                //SessionManager.Idioma = user.Idioma_516MF;
                
                bllEvento.AgregarEvento(new BEEvento(user,DateTime.Now,"Usuarios","Iniciar Sesión",5));
            }
        }

        public BEUsuario ConsultaIndividual(string dni)
        {
            return dalUsuario.ConsultaIndividual(dni);
        }

        public void ActualizarUsuario(BEUsuario usuario)
        {
            dalUsuario.Actualizar(usuario);
        }

        public void CambiarContraseña(string contraseñaActual, string nuevaContraseña)
        {
            if (!Regex.IsMatch(nuevaContraseña, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,20}$"))
            {
                throw new Exception("La contraseña nueva debe tener entre 8 y 20 caracteres, e incluir al menos una mayúscula, una minúscula, un número y un carácter especial (@*_/#$%).");
            }

            if (contraseñaActual == nuevaContraseña) throw new Exception("La nueva contraseña no puede ser igual a la actual.");

            string actualEncriptada = Encriptacion.Encriptar(contraseñaActual);
            if (actualEncriptada != SessionManager.Instancia.Usuario.Contraseña) throw new Exception("La contraseña actual es incorrecta.");
            string nuevaEncriptada = Encriptacion.Encriptar(nuevaContraseña);

            dalUsuario.ActualizarContraseña(SessionManager.Instancia.Usuario.DNI, nuevaEncriptada);
            SessionManager.Instancia.Usuario.Contraseña = nuevaEncriptada;
            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Cambiar Contraseña", 3));
        }
    }
}