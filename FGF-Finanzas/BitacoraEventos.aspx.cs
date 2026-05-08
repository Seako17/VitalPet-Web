using FGF_Finanzas.Capas.BLL;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class BitacoraEventos : System.Web.UI.Page
    {
        BLLEvento bllEvento = new BLLEvento();
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarGrilla(bllEvento.ObtenerEventos());
        }
        private void cargarGrilla(DataTable eventos)
        {
            GridViewEventos.DataSource = eventos;
            GridViewEventos.DataBind();
        }

        protected void btnAplicar_Click(object sender, EventArgs e)
        {
            string usuario = string.Empty;
            DateTime fecha = DateTime.MinValue;
            string modulo = string.Empty;
            string evento = string.Empty;
            int criticidad = 0;

            if(nombreUsuario.Text == string.Empty)
            {
                usuario = null;
            }
            else
            {
                usuario = nombreUsuario.Text.Trim();
            }
            if (conFecha.Checked == false)
            {
                fecha = DateTime.MinValue;
            }
            else
            {
                fecha = Convert.ToDateTime(fechaFiltro.Value);
            }
            if(moduloFiltro.Value == "Todos")
            {
                modulo = null;
            }
            else
            {
                modulo = moduloFiltro.Value;
            }
            if (eventoFiltro.Value == "Todos")
            {
                evento = null;
            }
            else
            {
                evento = eventoFiltro.Value;
            }
            if (criticidadFiltro.Value == "Sin")
            {
                criticidad = 0;
            }
            else
            {
                criticidad = Convert.ToInt32(criticidadFiltro.Value);
            }
            cargarGrilla(bllEvento.ObtenerEventosFiltrados(usuario, fecha, modulo, evento, criticidad)); 

        }
    }
}