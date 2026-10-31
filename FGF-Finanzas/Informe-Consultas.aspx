<%@ Page Title="Informe de consultas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Informe-Consultas.aspx.cs" Inherits="FGF_Finanzas.InformeConsultas" ViewStateEncryptionMode="Always" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <link href="Content/EventosStyles.css" rel="stylesheet" />
    <link href="Content/InformeConsultas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo"><h2><asp:Label ID="lblTitulo" runat="server" Text="Informe mensual de consultas" /></h2></div>
    <div class="informe-consultas">
        <section class="eventos__container">
            <h3><asp:Label ID="lblNuevo" runat="server" Text="Generar informe" /></h3>
            <p><asp:Label ID="lblCriterio" runat="server" Text="El período corresponde al mes del turno. Los resultados se conservan como estaban al generar el informe." /></p>
            <div class="filtros-informe">
                <asp:Label ID="lblMes" runat="server" Text="Mes:" AssociatedControlID="ddlMes" /><asp:DropDownList ID="ddlMes" runat="server" />
                <asp:Label ID="lblAnio" runat="server" Text="Año:" AssociatedControlID="txtAnio" /><asp:TextBox ID="txtAnio" runat="server" TextMode="Number" min="2000" AutoPostBack="true" OnTextChanged="PeriodoCambiado" />
                <asp:Label ID="lblVeterinario" runat="server" Text="Veterinario/a:" AssociatedControlID="ddlVeterinario" /><asp:DropDownList ID="ddlVeterinario" runat="server" />
                <asp:Button ID="btnGenerar" runat="server" Text="Generar y guardar" OnClick="Generar" CssClass="btn-informe" />
            </div>
        </section>
        <section class="eventos__container">
            <h3><asp:Label ID="lblGuardados" runat="server" Text="Informes guardados" /></h3>
            <div class="filtros-informe">
                <asp:Label ID="lblElegir" runat="server" Text="Informe:" AssociatedControlID="ddlInformes" />
                <asp:DropDownList ID="ddlInformes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SeleccionarInforme" />
                <asp:HyperLink ID="btnPdf" runat="server" Text="Ver PDF" Target="_blank" rel="noopener" CssClass="btn-informe" Visible="false" />
            </div>
            <asp:Label ID="lblMensaje" runat="server" role="status" />
        </section>
    </div>
</asp:Content>
