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
            Admin.Visible = false;
            WebMaster.Visible = false;
            Cliente.Visible = false;
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
                if (SessionManager.Instancia.Usuario.Rol=="Admin")
                {
                    Admin.Visible = true;
                }
                if(SessionManager.Instancia.Usuario.Rol=="Web Master")
                {
                    WebMaster.Visible = true;
                }
                if(SessionManager.Instancia.Usuario.Rol=="Cliente")
                {
                    Cliente.Visible = true;
                }
            }
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            BLLEvento bLLEvento = new BLLEvento();
            bLLEvento.AgregarEvento(new BEEvento(SessionManager.Instancia.Usuario, DateTime.Now, "Usuarios", "Cerrar Sesión", 5));
            SessionManager.LogOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["UserSessionFGF"] != null)
            {
                HttpCookie myCookie = new HttpCookie("UserSessionFGF");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}