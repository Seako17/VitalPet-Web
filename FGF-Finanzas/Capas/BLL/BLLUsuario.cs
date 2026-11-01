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
            AgregarUsuario(usuario, false);
        }

        private void AgregarUsuario(BEUsuario usuario, bool contraseñaYaEncriptada)
        {
            if (!bllDigitoVerificador.ValidarIntegridadDelSistema())
            {
                throw new BECustomException("ERR_SISTEMA_MANTENIMIENTO_REGISTRO");
            }

            if (!contraseñaYaEncriptada)
                usuario.Contraseña = Encriptacion.Encriptar(usuario.Contraseña);
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
            string[] columnasRequeridas = { "DNI", "Nombre", "Apellido", "Usuario", "Contraseña", "Intento", "Bloqueado", "Mail", "Rol" };
            var columnasFaltantes = columnasRequeridas.Except(
                dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName),
                StringComparer.OrdinalIgnoreCase);

            if (columnasFaltantes.Any())
                throw new BECustomException("ERR_XML_COLUMNAS", string.Join(", ", columnasFaltantes));

            Dictionary<int, string> rolesExistentes = dalUsuario.ObtenerRolesExistentes().AsEnumerable()
                .ToDictionary(fila => Convert.ToInt32(fila["id_rol"]), fila => fila["nombre"].ToString().Trim());

            DataTable guardados = ObtenerUsuarios();
            HashSet<string> dnisExistentesBD = new HashSet<string>(guardados.AsEnumerable().Select(r => r["DNI"].ToString().Trim()));

            HashSet<string> dnisXml = new HashSet<string>();
            HashSet<string> usuariosXml = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> mailsXml = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            List<BEUsuario> usuariosImportados = new List<BEUsuario>();

            foreach (DataRow fila in dt.Rows)
            {
                foreach (string col in columnasRequeridas)
                {
                    if (fila[col] == DBNull.Value || fila[col] == null || string.IsNullOrWhiteSpace(fila[col].ToString()))
                        throw new BECustomException("ERR_XML_CAMPO", col);
                }

                string dni = fila["DNI"].ToString().Trim();
                string nombre = fila["Nombre"].ToString().Trim();
                string apellido = fila["Apellido"].ToString().Trim();
                string usuarioNombre = fila["Usuario"].ToString().Trim();
                string contrasena = fila["Contraseña"].ToString().Trim();
                string mailTexto = fila["Mail"].ToString().Trim();
                string rolTexto = fila["Rol"].ToString().Trim();

                if (!dni.All(char.IsDigit) || (dni.Length != 7 && dni.Length != 8)) throw new BECustomException("ERR_XML_DNI", dni);

                try
                {
                    MailAddress mailAddress = new MailAddress(mailTexto);
                    if (mailAddress.Address != mailTexto) throw new Exception();
                }
                catch
                {
                    throw new BECustomException("ERR_XML_MAIL", mailTexto);
                }

                if (!int.TryParse(fila["Intento"].ToString(), out int intento) || intento < 0) throw new BECustomException("ERR_XML_INTENTOS", usuarioNombre);

                bool bloqueado;
                string valorBloqueado = fila["Bloqueado"].ToString().Trim();
                if (valorBloqueado == "1") bloqueado = true;
                else if (valorBloqueado == "0") bloqueado = false;
                else if (!bool.TryParse(valorBloqueado, out bloqueado)) throw new BECustomException("ERR_XML_BLOQUEADO", usuarioNombre);

                int idRol;
                if (int.TryParse(rolTexto, out idRol))
                {
                    if (!rolesExistentes.ContainsKey(idRol))
                        throw new BECustomException("ERR_XML_ROL_ID", idRol, usuarioNombre);
                }
                else
                {
                    KeyValuePair<int, string> rolPorNombre = rolesExistentes.FirstOrDefault(
                        rol => rol.Value.Equals(rolTexto, StringComparison.OrdinalIgnoreCase));
                    if (rolPorNombre.Key == 0)
                        throw new BECustomException("ERR_XML_ROL", rolTexto, usuarioNombre);

                    idRol = rolPorNombre.Key;
                }

                if (!dnisXml.Add(dni)) throw new BECustomException("ERR_XML_DNI_DUPLICADO", dni);

                if (!usuariosXml.Add(usuarioNombre)) throw new BECustomException("ERR_XML_USUARIO_DUPLICADO", usuarioNombre);

                if (!mailsXml.Add(mailTexto)) throw new BECustomException("ERR_XML_MAIL_DUPLICADO", mailTexto);

                var otrosUsuarios = guardados.AsEnumerable()
                    .Where(r => r["DNI"].ToString().Trim() != dni);

                if (otrosUsuarios.Any(r => r["Usuario"].ToString().Trim().Equals(usuarioNombre, StringComparison.OrdinalIgnoreCase)))
                    throw new BECustomException("ERR_XML_USUARIO_EXISTE", usuarioNombre);

                if (otrosUsuarios.Any(r => r["Mail"].ToString().Trim().Equals(mailTexto, StringComparison.OrdinalIgnoreCase)))
                    throw new BECustomException("ERR_XML_MAIL_EXISTE", mailTexto);

                BEUsuario usuario = new BEUsuario
                {
                    DNI = dni,
                    Nombre = nombre,
                    Apellido = apellido,
                    Usuario = usuarioNombre,
                    Contraseña = contrasena,
                    Intento = intento,
                    Bloqueado = bloqueado,
                    Mail = mailTexto,
                    Rol = new Rol(idRol)
                };

                usuariosImportados.Add(usuario);
            }

            foreach (BEUsuario usuario in usuariosImportados)
            {
                bool contraseñaYaEncriptada = Regex.IsMatch(usuario.Contraseña, @"\A[0-9a-fA-F]{64}\z");
                if (dnisExistentesBD.Contains(usuario.DNI))
                {
                    if (!contraseñaYaEncriptada)
                        usuario.Contraseña = Encriptacion.Encriptar(usuario.Contraseña);

                    ActualizarUsuario(usuario);
                }
                else
                {
                    AgregarUsuario(usuario, contraseñaYaEncriptada);
                }
            }
        }
    }
}
