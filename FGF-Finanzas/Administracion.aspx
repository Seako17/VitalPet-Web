<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Administracion.aspx.cs" Inherits="FGF_Finanzas.Administracion" %>

<asp:Content ID="EstilosAdministracion" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionUsuarios/GestionUsuariosStyles.css" rel="stylesheet" />
    <link href="Content/Administracion.css" rel="stylesheet" />
    <link href="Content/MenuAdministracion.css?v=1" rel="stylesheet" />
</asp:Content>
<asp:Content ID="ContenidoAdministracion" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2><asp:Label ID="lblTituloPanel" runat="server" Text="Panel de Administrador" /></h2>
    </div>
    <div class="main-layout administracion-layout">
        <aside class="sidebar">
            <h3><asp:Label ID="lblTituloMenu" runat="server" Text="Menú" /></h3>
            <ul id="menuSecciones" runat="server"></ul>
        </aside>
    </div>
</asp:Content>
