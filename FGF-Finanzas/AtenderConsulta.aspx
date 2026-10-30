<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AtenderConsulta.aspx.cs" Inherits="FGF_Finanzas.AtenderConsulta" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Atenciones/Atenciones.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container-main">
        <div class="main-title">
            <h2><asp:Label ID="lblTitulo" runat="server" Text="ATENDER CONSULTA" /></h2>
        </div>

        <div class="form-card">
            <h2 class="form-subtitle"><asp:Label ID="lblSubtitulo" class="lblSubtitle" runat="server" Text="Datos de la atención" /></h2>

            <div class="form-group">
                <label for="txtMascota"><asp:Label ID="lblMascota" runat="server" Text="Mascota:" /></label>
                <asp:TextBox ID="txtMascota" runat="server" CssClass="form-input" ReadOnly="true" />
            </div>

            <div class="form-group">
                <label for="txtVeterinario"><asp:Label ID="lblVeterinario" runat="server" Text="Veterinario/a:" /></label>
                <asp:TextBox ID="txtVeterinario" runat="server" CssClass="form-input" ReadOnly="true" />
            </div>

            <div class="form-group">
                <label for="txtMotivo"><asp:Label ID="lblMotivo" runat="server" Text="Motivo:" /></label>
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-input" ReadOnly="true" />
            </div>

            <div class="form-group">
                <label for="txtFechaHora"><asp:Label ID="lblFechaHora" runat="server" Text="Fecha y hora:" /></label>
                <asp:TextBox ID="txtFechaHora" runat="server" CssClass="form-input" ReadOnly="true" />
            </div>

            <div class="form-group">
                <label for="txtDiagnostico"><asp:Label ID="lblDiagnostico" runat="server" Text="Diagnóstico:" /></label>
                <asp:TextBox ID="txtDiagnostico" runat="server" class="form-input txtArea" TextMode="MultiLine" Rows="3" />
            </div>

            <div class="form-group">
                <label for="txtObservaciones"><asp:Label ID="lblObservaciones" runat="server" Text="Observaciones:" /></label>
                <asp:TextBox ID="txtObservaciones" runat="server" class="form-input txtArea" TextMode="MultiLine" Rows="3" />
            </div>

            <asp:Label ID="lblError" runat="server" class="lblError" />

            <div class="form-buttons">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn"
                    UseSubmitBehavior="false" CausesValidation="false" OnClick="btnCancelar_Click" />
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar atención" CssClass="btn" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
