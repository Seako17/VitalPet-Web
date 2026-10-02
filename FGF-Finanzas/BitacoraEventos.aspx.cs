using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class BitacoraEventos : BasePage
    {
        BLLEvento bllEvento = new BLLEvento();
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarGrilla(bllEvento.ObtenerEventos());
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            TraducirColumna(GridViewEventos, 0, "GridViewEventos_Header_Usuario", "Usuario");
            TraducirColumna(GridViewEventos, 1, "GridViewEventos_Header_FechaHora", "Fecha y Hora");
            TraducirColumna(GridViewEventos, 2, "GridViewEventos_Header_Modulo", "Módulo");
            TraducirColumna(GridViewEventos, 3, "GridViewEventos_Header_Evento", "Evento");
            TraducirColumna(GridViewEventos, 4, "GridViewEventos_Header_Criticidad", "Criticidad");

            TraducirOpciones(moduloFiltro,
                new[] { "moduloFiltro_Modulo", "moduloFiltro_Usuarios", "moduloFiltro_Administrador", "moduloFiltro_Clientes", "moduloFiltro_EnDesarrollo" },
                new[] { "Módulo", "Usuarios", "Administrador", "Clientes", "En desarollo ..." });

            TraducirOpciones(eventoFiltro,
                new[] { "eventoFiltro_Evento", "eventoFiltro_IniciarSesion", "eventoFiltro_RegistrarUsuario", "eventoFiltro_CerrarSesion", "eventoFiltro_CambiarContrasena", "eventoFiltro_DesbloquearUsuario", "eventoFiltro_ModificarUsuario", "eventoFiltro_HacerBackup", "eventoFiltro_HacerRestore", "eventoFiltro_RegistrarMascota" },
                new[] { "Evento", "Iniciar Sesión", "Registrar Usuario", "Cerrar Sesión", "Cambiar Contraseña", "Desbloquear Usuario", "Modificar Usuario", "Hacer Backup", "Hacer Restore", "Registrar Mascota" });

            TraducirOpciones(criticidadFiltro,
                new[] { "criticidadFiltro_Criticidad", "criticidadFiltro_1", "criticidadFiltro_2", "criticidadFiltro_3", "criticidadFiltro_4", "criticidadFiltro_5" },
                new[] { "Criticidad", "1 (Crítica)", "2 (Importante)", "3 (Media)", "4 (Baja)", "5 (Mínima)" });
        }

        private string TextoTraducido(string clave, string porDefecto)
        {
            string texto = IdiomaManager.Instancia.ObtenerTexto(NombreFormulario, clave);
            return texto == "[" + clave + "]" ? porDefecto : texto;
        }

        private void TraducirColumna(GridView grilla, int columna, string clave, string porDefecto)
        {
            string texto = TextoTraducido(clave, porDefecto);
            grilla.Columns[columna].HeaderText = texto;

            if (grilla.HeaderRow != null && columna < grilla.HeaderRow.Cells.Count)
                grilla.HeaderRow.Cells[columna].Text = texto;
        }

        private void TraducirOpciones(HtmlSelect control, string[] claves, string[] textosPorDefecto)
        {
            for (int i = 0; i < control.Items.Count && i < claves.Length; i++)
            {
                control.Items[i].Text = TextoTraducido(claves[i], textosPorDefecto[i]);
            }
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            nombreUsuario.Text = string.Empty;
            fechaFiltro.Value = string.Empty;
            moduloFiltro.SelectedIndex = 0;
            eventoFiltro.SelectedIndex = 0;
            criticidadFiltro.SelectedIndex = 0;
            conFecha.Checked = false;
        }
    }
}