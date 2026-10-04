<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Familias.aspx.cs" Inherits="FGF_Finanzas.Gestion_Familias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionFamiliasStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2><asp:Label ID="lblTituloPanel" runat="server" Text="Panel de Administrador" /></h2>
    </div>

    <div id="contenedor-alertas"></div>

    <div class="main-layout">
        <aside class="sidebar">
            <h3><asp:Label ID="lblTituloMenu" runat="server" Text="Menú" /></h3>
            <ul>
                <li><a href="Gestion-Usuarios.aspx"><asp:Label ID="lblMenuUsuarios" runat="server" Text="Gestión Usuarios" /></a></li>
                <li><asp:HyperLink ID="lnkMenuBitacora" runat="server" NavigateUrl="BitacoraEventos.aspx">Bitácora eventos</asp:HyperLink></li>
                <li><a href="Gestion-Perfiles.aspx"><asp:Label ID="lblMenuPerfiles" runat="server" Text="Gestión Perfiles" /></a></li>
                <li class="active"><asp:Label ID="lblMenuFamilias" runat="server" Text="Gestión Familias" /></li>
                <li><asp:HyperLink ID="lnkMenuBackup" runat="server" NavigateUrl="Backup-Restore.aspx">Backup/Restore</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuIdiomas" runat="server" NavigateUrl="Gestion-Idioma.aspx">Gestión Idiomas</asp:HyperLink></li>
            </ul>
        </aside>
        <section class="gestion-familias__container">
            <h3><asp:Label ID="lblTituloFamilias" runat="server" Text="Gestión de Familias" /></h3>
            <div class="familias-layout">
                <div class="columna-familia">
                    <h4><asp:Label ID="lblFamiliaBase" runat="server" Text="Familia base" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox runat="server" ID="lstFamiliaBase"
                            CssClass="listbox-familias"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-familia">
                    <h4><asp:Label ID="lblFamiliaAgregar" runat="server" Text="Familia a agregar" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstFamiliaAgregar"
                            runat="server"
                            CssClass="listbox-familias"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-permisos">
                    <h4><asp:Label ID="lblPermisos" runat="server" Text="Permisos" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstPermisos"
                            runat="server"
                            CssClass="listbox-familias"></asp:ListBox>
                    </div>
                </div>
                <aside class="botones-operaciones">
                    <asp:Button
                        ID="btnAsignarPermiso"
                        runat="server"
                        Text="Asignar permiso" OnClick="btnAsignarPermiso_Click" />
                    <asp:Button
                        ID="btnAsignarFamilia"
                        runat="server"
                        Text="Asignar familia" OnClick="btnAsignarFamilia_Click" />
                    <asp:Button
                        ID="btnEliminarFamilia"
                        runat="server"
                        Text="Eliminar familia" OnClick="btnEliminarFamilia_Click" />
                    <asp:Button
                        ID="btnEliminarComponente"
                        runat="server"
                        Text="Eliminar componente" OnClick="btnEliminarComponente_Click" />
                    <div class="separador-botones"></div>
                    <asp:TextBox
                        ID="txtNombreFamilia"
                        runat="server"
                        CssClass="txt-nombre-familia"
                        placeholder="Nombre de familia">
                    </asp:TextBox>
                    <asp:Button
                        ID="btnAgregarFamilia"
                        runat="server"
                        Text="Agregar familia" OnClick="btnAgregarFamilia_Click" />
                </aside>
            </div>
        </section>
    </div>
</asp:Content>
