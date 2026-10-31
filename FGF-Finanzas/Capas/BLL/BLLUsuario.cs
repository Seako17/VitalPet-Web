using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
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
        BLLDigitoVerificador bllDigitoVerificador;
        BLLRol bllRol;
        public BLLUsuario()
        {
            dalUsuario = new DALUsuario();
            bllEvento = new BLLEvento();
            bllDigitoVerificador = new BLLDigitoVerificador();
            bllRol = new BLLRol();
        }

        public void ValidarUsuario(string dni, string usuario, string nombre, string apellido, string contraseña, string confirmacion)
        {
            if (string.IsNullOrWhiteSpace(dni)) throw new BECustomException("ERR_DNI_OBLIGATORIO");
            if (!Regex.IsMatch(dni, @"^\d{8}$")) throw new BECustomException("ERR_DNI_8_DIGITOS");

            if (string.IsNullOrWhiteSpace(nombre)) throw new BECustomException("ERR_NOMBRE_OBLIGATORIO");
            if (!Regex.IsMatch(nombre, @"^[A-Za-z]{3,}(\s[A-Za-z]{3,})*$")) throw new BECustomException("ERR_NOMBRE_INVALIDO");

            if (string.IsNullOrWhiteSpace(apellido)) throw new BECustomException("ERR_APELLIDO_OBLIGATORIO");
            if (!Regex.IsMatch(apellido, @"^[A-Za-z]{3,}(\s[A-Za-z]{3,})*$")) throw new BECustomException("ERR_APELLIDO_INVALIDO");

            if (string.IsNullOrWhiteSpace(usuario)) throw new BECustomException("ERR_USUARIO_OBLIGATORIO");

            if (!Regex.IsMatch(contraseña, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,20}$"))
            {
                throw new BECustomException("ERR_PASSWORD_INVALIDA");
            }
            if (contraseña != confirmacion) throw new BECustomException("ERR_PASSWORDS_NO_COINCIDEN");

            DataTable dt = ObtenerUsuarios();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["dni"].ToString() == dni)
                {
                    throw new BECustomException("ERR_DNI_YA_REGISTRADO");
                }
                if (dr[3].ToString() == usuario)
                {
                    throw new BECustomException("ERR_USUARIO_YA_EXISTENTE");
                }
            }
        }

        public DataTable ObtenerUsuarios()
        {
            DataTable usuarios = dalUsuario.ObtenerUsuarios();

            usuarios.Columns.Add("RolTemp", typeof(Rol));

            foreach (DataRow item in usuarios.Rows)
            {
                if (item["rol"] != DBNull.Value)
                {
                    int idRol = Convert.ToInt32(item["rol"]);

                    Rol rol = bllRol.ObtenerCompleto(new Rol(idRol));

                    item["RolTemp"] = rol;
                }
            }

            usuarios.Columns.Remove("rol");

            usuarios.Columns["RolTemp"].ColumnName = "rol";

            return usuarios;
        }



        public void ActualizarIntentosUsuario(BEUsuario usuario, bool sistemaIntegro)
        {
            if (sistemaIntegro)
            {
                dalUsuario.ActualizarIntentosYBloqueo(usuario.DNI, usuario.Intento, usuario.Bloqueado);
                bllDigitoVerificador.InicializarTablaCompleta("Usuario");
            }
        }

        public void IniciarSesion(string usuario, string contraseña)
        {
            if (SessionManager.IsLogged())
                throw new BECustomException("ERR_SESION_YA_INICIADA");

            BEUsuario user = null;
            foreach (DataRow item in ObtenerUsuarios().Rows)
            {
                if (item["usuario"].ToString() == usuario)
                {
                    user = new BEUsuario(item);
                    break;
                }
            }

            if (user == null)
                throw new BECustomException("ERR_CREDENCIALES_INVALIDAS");

            if (user.Bloqueado == true)
                throw new BECustomException("ERR_USUARIO_BLOQUEADO");

            bool sistemaIntegro = bllDigitoVerificador.ValidarIntegridadDelSistema();

            if (Encriptacion.Encriptar(contraseña).Equals(user.Contraseña))
            {
                user.Intento = 0;
                ActualizarIntentosUsuario(user, sistemaIntegro);
                SessionManager.Login(user);
                IdiomaManager.Instancia.IdiomaActual = user.Idioma;
                bllEvento.AgregarEvento(new BEEvento(user, DateTime.Now, "Usuarios", "Iniciar Sesión", 5));
                return;
            }

            user.Intento++;
            if (user.Intento >= 3)
            {
                user.Bloqueado = true;
                user.Intento = 0;
            }

            ActualizarIntentosUsuario(user, sistemaIntegro);

            throw new BECustomException("ERR_CREDENCIALES_INVALIDAS");
        }

        public BEUsuario ConsultaIndividual(string dni)
        {
            return dalUsuario.ConsultaIndividual(dni);
        }



        public void AgregarUsuario(BEUsuario usuario)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_REGISTRO");
            }

            string encriptado = Encriptacion.Encriptar(usuario.Contraseña);
            usuario.Contraseña = encriptado;
            dalUsuario.AgregarUsuario(usuario);
            if(SessionManager.IsLogged())
            {
                bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Registrar Usuario", 4));
            }
            else
            {
                bllEvento.AgregarEvento(new BEEvento(usuario, DateTime.Now, "Usuarios", "Registrar Usuario", 4));
            }
            bllDigitoVerificador.InicializarTablaCompleta("Usuario");
        }

        public void ActualizarUsuario(BEUsuario usuario)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_ACTUALIZACION");
            }
            dalUsuario.Actualizar(usuario);
            bllDigitoVerificador.InicializarTablaCompleta("Usuario");
        }

        public void CambiarContraseña(string contraseñaActual, string nuevaContraseña)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new BECustomException("ERR_SISTEMA_INCONSISTENCIA_PASSWORD");
            }

            if (!Regex.IsMatch(nuevaContraseña, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,20}$"))
            {
                throw new BECustomException("ERR_PASSWORD_NUEVA_INVALIDA");
            }

            if (contraseñaActual == nuevaContraseña) throw new BECustomException("ERR_PASSWORD_IGUAL_A_ACTUAL");

            string actualEncriptada = Encriptacion.Encriptar(contraseñaActual);
            if (actualEncriptada != SessionManager.Instancia.Usuario.Contraseña) throw new BECustomException("ERR_PASSWORD_ACTUAL_INCORRECTA");
            string nuevaEncriptada = Encriptacion.Encriptar(nuevaContraseña);
            dalUsuario.ActualizarContraseña(SessionManager.Instancia.Usuario.DNI, nuevaEncriptada);
            SessionManager.Instancia.Usuario.Contraseña = nuevaEncriptada;
            bllDigitoVerificador.InicializarTablaCompleta("Usuario");

            bllEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Cambiar Contraseña", 3));
        }
        public void CambiarIdiomaPreferido(string dni, string codigoIdioma)
        {
            dalUsuario.ActualizarIdioma(dni, codigoIdioma);
            bllDigitoVerificador.InicializarTablaCompleta("Usuario");
        }
        public DataTable ObtenerUsuariosPuros()
        {
            DataTable usuarios = dalUsuario.ObtenerUsuariosPuros();

            usuarios.Columns.Add("RolTemp", typeof(Rol));

            foreach (DataRow item in usuarios.Rows)
            {
                if (item["rol"] != DBNull.Value)
                {
                    int idRol = Convert.ToInt32(item["rol"]);

                    Rol rol = bllRol.ObtenerCompleto(new Rol(idRol));

                    item["RolTemp"] = rol;
                }
            }

            usuarios.Columns.Remove("rol");

            usuarios.Columns["RolTemp"].ColumnName = "rol";

            return usuarios;
        }

        public void ImportarUsuarios(DataTable dt)
        {
            string[] columnas = {
                "DNI", "Nombre", "Apellido", "Usuario",
                "Contraseña", "Intento", "Bloqueado", "Mail", "Rol"};

            foreach (string columna in columnas)
            {
                if (!dt.Columns.Contains(columna))
                    throw new Exception("Falta la columna " + columna + " en el archivo XML.");
            }

            DataTable guardados = ObtenerUsuarios();
            Dictionary<string, int> rolesExistentes = dalUsuario.ObtenerRolesExistentes().AsEnumerable()
                .ToDictionary(
                    fila => fila["nombre"].ToString().Trim(),
                    fila => Convert.ToInt32(fila["id_rol"]),
                    StringComparer.OrdinalIgnoreCase);

            List<BEUsuario> usuariosImportados = new List<BEUsuario>();

            HashSet<string> dnis = new HashSet<string>();
            HashSet<string> usuarios = new HashSet<string>();
            HashSet<string> mails = new HashSet<string>();

            foreach (DataRow fila in dt.Rows)
            {
                BEUsuario usuario = new BEUsuario
                {
                    DNI = fila["DNI"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    Usuario = fila["Usuario"].ToString(),
                    Contraseña = fila["Contraseña"].ToString(),
                    Intento = Convert.ToInt32(fila["Intento"]),
                    Bloqueado = Convert.ToBoolean(fila["Bloqueado"]),
                    Mail = fila["Mail"].ToString()
                };
                string nombreRolImportado = fila["Rol"].ToString().Trim();

                // 1. Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(usuario.DNI) ||
                    string.IsNullOrWhiteSpace(usuario.Nombre) ||
                    string.IsNullOrWhiteSpace(usuario.Apellido) ||
                    string.IsNullOrWhiteSpace(usuario.Usuario) ||
                    string.IsNullOrWhiteSpace(usuario.Contraseña) ||
                    string.IsNullOrWhiteSpace(usuario.Mail) ||
                    string.IsNullOrWhiteSpace(nombreRolImportado))
                {
                    throw new Exception("Existen usuarios con campos obligatorios vacíos.");
                }

                // 2. Validar DNI
                if (!usuario.DNI.All(char.IsDigit) ||
                    (usuario.DNI.Length != 7 && usuario.DNI.Length != 8))
                {
                    throw new Exception("El DNI " + usuario.DNI + " no tiene un formato válido.");
                }

                // 3. Validar correo electrónico
                try
                {
                    MailAddress mail = new MailAddress(usuario.Mail);

                    if (mail.Address != usuario.Mail.Trim())
                        throw new Exception();
                }
                catch
                {
                    throw new Exception("El correo " + usuario.Mail + " no es válido.");
                }

                // 4. Validar intento
                if (usuario.Intento < 0)
                    throw new Exception("El usuario " + usuario.Usuario + " tiene un número de intentos inválido.");

                // 5. Validar contraseña
                if (string.IsNullOrWhiteSpace(usuario.Contraseña))
                    throw new Exception("El usuario " + usuario.Usuario + " no tiene contraseña.");

                // 6. Validar rol
                int idRol;
                if (!rolesExistentes.TryGetValue(nombreRolImportado, out idRol))
                    throw new Exception("El rol " + nombreRolImportado + " del usuario " + usuario.Usuario + " no existe.");
                usuario.Rol = new Rol(idRol);

                // 7. Validar duplicados dentro del XML
                if (!dnis.Add(usuario.DNI))
                    throw new Exception("El DNI " + usuario.DNI + " está duplicado en el XML.");

                if (!usuarios.Add(usuario.Usuario.ToLowerInvariant()))
                    throw new Exception("El usuario " + usuario.Usuario + " está duplicado en el XML.");

                if (!mails.Add(usuario.Mail.ToLowerInvariant()))
                    throw new Exception("El correo " + usuario.Mail + " está duplicado en el XML.");

                // 8. Validar existencia en la base de datos
                foreach (DataRow existente in guardados.Rows)
                {
                    if (existente["DNI"].ToString() == usuario.DNI)
                        throw new Exception("El DNI " + usuario.DNI + " ya está registrado.");

                    if (existente["Usuario"].ToString().Equals(
                        usuario.Usuario, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("El nombre de usuario " + usuario.Usuario + " ya está registrado.");

                    if (existente["Mail"].ToString().Equals(
                        usuario.Mail, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("El correo " + usuario.Mail + " ya está registrado.");
                }

                usuariosImportados.Add(usuario);
            }

            foreach (BEUsuario usuario in usuariosImportados)
            {
                AgregarUsuario(usuario);
            }
        }
    }
}
