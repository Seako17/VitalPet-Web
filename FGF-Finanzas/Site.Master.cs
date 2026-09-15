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
                    if (SessionManager.Instancia.Usuario.Rol == "Web Master")
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
                        string script= @"alert('El sistema se encuentra en mantenimiento.');  window.location.href = 'Default.aspx';";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaInconsistencia", script, true);
                        return;
                    }
                }

                if(SessionManager.Instancia != null && SessionManager.Instancia.Usuario != null)
                {
                    if (SessionManager.Instancia.Usuario.Rol == "Admin")
                    {
                        Admin.Visible = true;
                    }
                    if (SessionManager.Instancia.Usuario.Rol == "Web Master")
                    {
                        WebMaster.Visible = true;
                    }
                    if (SessionManager.Instancia.Usuario.Rol == "Cliente")
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
            AplicarTraducciones(this.Controls, traducciones);
        }
        private void AplicarTraducciones(ControlCollection controls, IDictionary<string, string> traducciones)
        {
            foreach (Control c in controls)
            {
                if (!string.IsNullOrEmpty(c.ID) && traducciones.TryGetValue(c.ID, out string texto))
                {
                    switch (c)
                    {
                        case HyperLink hl:
                            hl.Text = texto;
                            break;
                        case Button btn:
                            btn.Text = texto;
                            break;
                        case Label lbl:
                            lbl.Text = texto;
                            break;
                    }
                }

                if (c.HasControls())
                {
                    AplicarTraducciones(c.Controls, traducciones);
                }
            }
        }

        protected void rptIdiomas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CambiarIdioma")
            {
                IdiomaManager.Instancia.IdiomaActual = e.CommandArgument.ToString();
                CargarComboIdiomas();
            }
        }
    }
}