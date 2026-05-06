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
            }
        }

        protected void LogoutBtn_Click(object sender, EventArgs e)
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

            Response.Redirect("~/Default.aspx");
        }
    }
}