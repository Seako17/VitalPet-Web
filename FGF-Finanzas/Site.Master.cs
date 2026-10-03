using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.DAL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Site : MasterPage, IIdiomaObserver
    {
        BLLUsuario bllUsuario = new BLLUsuario(); BEUsuario usuario;
        BLLRol BLLRol = new BLLRol();

        public string NombreFormulario => "Site.Master";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComboIdiomas();
                IdiomaManager.Instancia.Notificar();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IdiomaManager.Instancia.Suscribir(this);
            Admin.Visible = false;
            WebMaster.Visible = false;
            Cliente.Visible = false;
            HttpCookie cookie = Request.Cookies["UserSessionFGF"];
            if (cookie != null)
            {
                string nombreUsuario = cookie.Value;
                var usuarios = bllUsuario.ObtenerUsuarios();

                foreach (DataRow dr in usuarios.Rows)
                {
                    if (dr[3].ToString() == nombreUsuario)
                    {
                        usuario = new BEUsuario(dr);
                    }
                }

                if (usuario != null)
                {
                    SessionManager.Login(usuario);
                }
            }
            if (SessionManager.IsLogged())
            {
                
                BLLDigitoVerificador bllDV = new BLLDigitoVerificador();
                var lista = bllDV.CompararDigito();
                if (lista != null && lista.Count > 0)
                {
                    if (SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x =>x.Nombre == "DV")))
                    {
                        Session["InconsistenciasDetectadas"] = lista;
                        Response.Redirect("~/DV_Form.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    else
                    {
                        Limpiar_Session();
                        FormsAuthentication.SignOut();
                        string mensaje = IdiomaManager.Instancia.ObtenerTexto("Errores", "ERR_SISTEMA_MANTENIMIENTO").Replace("'", "\\'");
                        string script = "alert('" + mensaje + "');  window.location.href = 'Default.aspx';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaInconsistencia", script, true);
                        return;
                    }
                }

                if(SessionManager.Instancia != null && SessionManager.Instancia.Usuario != null)
                {
                    if (SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "UsuarioGestionar")) || SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "FamiliaGestionar")) || SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "PerfilGestionar")))
                    {
                        Admin.Visible = true;
                    }
                    if (SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "BackUpRestore")))
                    {
                        WebMaster.Visible = true;
                    }
                    if (SessionManager.Instancia.Usuario.Rol.TienePermiso(CodigosPermiso.ObtenerPermisos().Find(x => x.Nombre == "MascotaRegistrar")))
                    {
                        Cliente.Visible = true;
                    }
                }
            }
            else
            {
                FormsAuthentication.SignOut();
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            IdiomaManager.Instancia.Desuscribir(this);
        }

        private void CargarComboIdiomas()
        {
            var lista = IdiomaManager.Instancia.ObtenerIdiomasDisponibles();
            rptIdiomas.DataSource = lista;
            rptIdiomas.DataBind();

            var actual = lista.Find(i => i.Codigo == IdiomaManager.Instancia.IdiomaActual);
            if (actual == null)
            {
                IdiomaManager.Instancia.IdiomaActual = BLLTraduccion.IDIOMA_POR_DEFECTO;
                actual = lista.Find(i => i.Codigo == BLLTraduccion.IDIOMA_POR_DEFECTO);
            }

            if (actual != null)
            {
                lblIdiomaSeleccionado.Text = actual.Nombre;
            }
        }

        private void Limpiar_Session()
        {
            SessionManager.LogOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["UserSessionFGF"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserSessionFGF");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            BLLEvento bLLEvento = new BLLEvento();
            bLLEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Cerrar Sesión", 5));
            Limpiar_Session();

            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }

        public void ActualizarIdioma(string codigoIdioma, IDictionary<string, string> traducciones)
        {
            BasePage.AplicarTraducciones(this.Controls, traducciones);
        }

        protected void rptIdiomas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CambiarIdioma")
            {
                string nuevoIdioma = e.CommandArgument.ToString();
                IdiomaManager.Instancia.IdiomaActual = nuevoIdioma;
                if (SessionManager.IsLogged())
                {
                    SessionManager.Instancia.Usuario.Idioma = nuevoIdioma;
                    bllUsuario.CambiarIdiomaPreferido(SessionManager.Instancia.Usuario.DNI, nuevoIdioma);
                }

                CargarComboIdiomas();
            }
        }
    }
}