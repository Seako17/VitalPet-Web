using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class Consulta : System.Web.UI.Page
    {
        BLLMascota bllMascota = new BLLMascota();
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = bllMascota.ObtenerMascotasDeUsuario(SessionManager.Instancia.Usuario);

            dropMascota.DataSource = dt;
            dropMascota.DataTextField = "nombre";
            dropMascota.DataBind();
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {

        }
    }
}