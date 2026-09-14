<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consulta.aspx.cs" Inherits="FGF_Finanzas.Consulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Consultas/Consultas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-container-main">
        <div class="main-title">
            <h2>Agendar Consulta</h2>
        </div>
        <div class="form-card">
            <h2 class="form-subtitle">datos para la consulta</h2>
            <div class="form-group">
                <label for="dropMascota">Mascota:</label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="dropMascota" ></asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="txtMotivo">Motivo:</label>
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-input" Placeholder="Ej: Vacunación"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtFecha">Fecha y hora:</label>
                <asp:TextBox TextMode="DateTime" runat="server" ID="txtFecha" CssClass="form-input"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtVeterinario">Fecha de nacimiento:</label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="txtVeterinario"></asp:DropDownList>

            </div>
            <asp:Label runat="server" ID="lblError" ForeColor="Red" CssClass="lblError" />
            <div class="form-buttons">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-cancelar" UseSubmitBehavior="false" />
                <asp:Button ID="btnAgendar" runat="server" Text="Registrar" CssClass="btn btn-registrar" OnClick="btnAgendar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
