using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Default : System.Web.UI.Page
    {
        BLLUsuario bllUsuario = new BLLUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = "¡Bienvenido!";
            HttpCookie cookie = Request.Cookies["UserSessionFGF"];
            if(cookie != null)
            {
                BEUsuario usuario = new BEUsuario(bllUsuario.ObtenerUsuarios().AsEnumerable().Where(x=> x["usuario"].ToString() == cookie.Value.ToString()).FirstOrDefault());
                if(usuario != null)
                {
                    SessionManager.Login(usuario);
                    lblBienvenida.Text += $" {cookie.Value.ToString()}";
                }
            }
        }
    }
}