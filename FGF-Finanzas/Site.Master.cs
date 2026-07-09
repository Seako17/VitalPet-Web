using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
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
    public partial class Site : System.Web.UI.MasterPage
    {
        BLLUsuario bllUsuario = new BLLUsuario(); BEUsuario usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (SessionManager.IsLogged())
            {
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

                BLLDigitoVerificador bllDV = new BLLDigitoVerificador();
                var lista = bllDV.CompararDigito();
                if (lista != null && lista.Count > 0)
                {
                    if (SessionManager.Instancia.Usuario.Rol == "Web Master")
                    {
                        Session["InconsistenciasDetectadas"] = lista;
                        Response.Redirect("~/DV_Form.aspx");
                    }
                    else
                    {
                        string mensajeScript = "alert('El sistema se encuentra en mantenimiento');";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertaInconsistencia", mensajeScript, true);
                        Limpiar_Session();

                    }
                }
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

            Response.Redirect("~/Default.aspx");
        }
    }
}