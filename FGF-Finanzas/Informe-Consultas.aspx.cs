using FGF_Finanzas.Capas.BE;
using FGF_Finanzas.Capas.BLL;
using FGF_Finanzas.Capas.Servicios;
using FGF_Finanzas.Capas.Servicios.Cambio_Idioma;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FGF_Finanzas
{
    public partial class InformeConsultas : PaginaSegura
    {
        private readonly BLLInformeConsultas informes = new BLLInformeConsultas();
        private bool exportando;
        protected override Permiso PermisoRequerido => CodigosPermiso.ObtenerPermisos().Find(p => p.Nombre == "InformeVer");
        private string Idioma => IdiomaManager.Instancia.IdiomaActual;
        private CultureInfo Cultura => CultureInfo.GetCultureInfo(Idioma);
        private string T(string clave) { return ObtenerMensaje(clave); }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLogged() || !TieneAcceso(SessionManager.Instancia.Usuario)) return;
            if (!IsPostBack && Request.QueryString["pdf"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["pdf"], out id)) AbrirPdf(id);
                else MostrarErrorVisible(lblMensaje, "ERR_INFORME_NO_EXISTE");
                if (exportando) return;
            }
            if (!IsPostBack)
            {
                for (int mes = 1; mes <= 12; mes++) ddlMes.Items.Add(new ListItem("", mes.ToString()));
                ddlMes.SelectedValue = DateTime.Today.Month.ToString();
                txtAnio.Text = DateTime.Today.Year.ToString();
                txtAnio.Attributes["max"] = DateTime.Today.Year.ToString();
                try
                {
                    ddlVeterinario.Items.Add(new ListItem("", ""));
                    foreach (DataRow fila in informes.ObtenerVeterinarios().Rows)
                        ddlVeterinario.Items.Add(new ListItem(fila["NombreCompleto"].ToString(), fila["dni"].ToString()));
                }
                catch (Exception ex) { MostrarErrorVisible(lblMensaje, ex); }
            }
        }

        protected void Generar(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblMensaje);
            try
            {
                int anio, mes;
                if (!int.TryParse(txtAnio.Text, out anio) || !int.TryParse(ddlMes.SelectedValue, out mes)) throw new BECustomException("ERR_INFORME_PERIODO");
                int id = informes.Generar(anio, mes, ddlVeterinario.SelectedValue);
                ViewState["InformeId"] = id;
                MostrarMensajeVisible(lblMensaje, "informeGuardado");
            }
            catch (Exception ex) { MostrarErrorVisible(lblMensaje, ex); }
        }

        protected void SeleccionarInforme(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblMensaje);
            int id;
            ViewState["InformeId"] = int.TryParse(ddlInformes.SelectedValue, out id) ? (object)id : null;
        }

        protected void PeriodoCambiado(object sender, EventArgs e)
        {
            LimpiarMensajeVisible(lblMensaje);
            int anio, mes;
            if (int.TryParse(txtAnio.Text, out anio) && anio == DateTime.Today.Year &&
                int.TryParse(ddlMes.SelectedValue, out mes) && mes > DateTime.Today.Month)
                ddlMes.SelectedValue = DateTime.Today.Month.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (exportando || !SessionManager.IsLogged() || !TieneAcceso(SessionManager.Instancia.Usuario)) return;
            Title = T("lblTitulo");
            lblCriterio.Text = T("criterio");
            int anio;
            bool anioValido = int.TryParse(txtAnio.Text, out anio) && anio >= 2000 && anio <= DateTime.Today.Year;
            txtAnio.Attributes["max"] = DateTime.Today.Year.ToString();
            foreach (ListItem mes in ddlMes.Items)
            {
                mes.Text = Cultura.DateTimeFormat.GetMonthName(int.Parse(mes.Value));
                mes.Enabled = anioValido && (anio < DateTime.Today.Year || int.Parse(mes.Value) <= DateTime.Today.Month);
            }
            if (ddlVeterinario.Items.Count > 0) ddlVeterinario.Items[0].Text = T("todos");
            btnPdf.Visible = false;
            try
            {
                ddlInformes.Items.Clear();
                ddlInformes.Items.Add(new ListItem(T("seleccioneInforme"), ""));
                foreach (DataRow fila in informes.Listar().Rows)
                    ddlInformes.Items.Add(new ListItem("#" + fila["IdInforme"] + " — " + Cultura.DateTimeFormat.GetMonthName((int)fila["Mes"]) + " " + fila["Anio"] + " — " +
                        (string.IsNullOrEmpty(fila["NombreVeterinario"].ToString()) ? T("todos") : fila["NombreVeterinario"].ToString()) + " — " + ((DateTime)fila["Generado"]).ToString("g", Cultura), fila["IdInforme"].ToString()));
                if (ViewState["InformeId"] != null)
                {
                    string id = ViewState["InformeId"].ToString();
                    if (ddlInformes.Items.FindByValue(id) == null) throw new BECustomException("ERR_INFORME_NO_EXISTE");
                    ddlInformes.SelectedValue = id;
                    btnPdf.NavigateUrl = "~/Informe-Consultas.aspx?pdf=" + id;
                    btnPdf.Visible = true;
                }
            }
            catch (Exception ex) { MostrarErrorVisible(lblMensaje, ex); }
        }

        private void AbrirPdf(int id)
        {
            try
            {
                var informe = informes.Obtener(id);
                byte[] pdf = new ServicioInformePdf().Generar(informe, T, Idioma, Server.MapPath("~/Content/logoVitalPet.png"));
                Response.Clear(); Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline; filename=InformeConsultas-" + informe.Id + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(pdf); exportando = true; Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex) { MostrarErrorVisible(lblMensaje, ex); }
        }

        protected override void Render(HtmlTextWriter writer) { if (!exportando) base.Render(writer); }
    }
}
