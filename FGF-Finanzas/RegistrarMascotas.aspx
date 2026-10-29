<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarMascotas.aspx.cs" Inherits="FGF_Finanzas.RegistrarMascotas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/registrarMascotasStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container-main">
        <div class="main-title">
            <h2><asp:Label ID="lblTituloMascota" runat="server" Text="Registrar Mascota" /></h2>
        </div>
        <div class="form-card">
            <h2 class="form-subtitle"><asp:Label ID="lblSubtituloDatos" runat="server" Text="DATOS DE LA MASCOTA" /></h2>
            <div class="form-group">
                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="Nombre:" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-input" Placeholder="Nombre"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblEspecie" runat="server" AssociatedControlID="txtEspecie" Text="Especie:" />
                <asp:TextBox ID="txtEspecie" runat="server" CssClass="form-input" Placeholder="Especie"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblRaza" runat="server" AssociatedControlID="txtRaza" Text="Raza:" />
                <asp:TextBox ID="txtRaza" runat="server" CssClass="form-input" Placeholder="Raza"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblFechaNacimiento" runat="server" AssociatedControlID="txtFechaNacimiento" Text="Fecha de nacimiento:" />
                <input type="date" id="txtFechaNacimiento" runat="server" class="form-input" />
            </div>
            <asp:Label runat="server" ID="lblError" ForeColor="Red" CssClass="lblError" />
            <div class="form-buttons">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-cancelar" UseSubmitBehavior="false" />
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn btn-registrar" OnClick="btnRegistrar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
