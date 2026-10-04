<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Perfiles.aspx.cs" Inherits="FGF_Finanzas.Gestion_Perfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionPerfiles.css" rel="stylesheet" />
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
                <li class="active"><asp:Label ID="lblMenuPerfiles" runat="server" Text="Gestión Perfiles" /></li>
                <li><a href="Gestion-Familias.aspx"><asp:Label ID="lblMenuFamilias" runat="server" Text="Gestión Familias" /></a></li>
                <li><asp:HyperLink ID="lnkMenuBackup" runat="server" NavigateUrl="Backup-Restore.aspx">Backup/Restore</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuIdiomas" runat="server" NavigateUrl="Gestion-Idioma.aspx">Gestión Idiomas</asp:HyperLink></li>
            </ul>

        </aside>
        <section class="gestion-perfiles__container">
            <h3><asp:Label ID="lblTituloPerfiles" runat="server" Text="Gestión de Perfiles" /></h3>
            <div class="perfiles-layout">
                <div class="columna-lista">
                    <h4><asp:Label ID="lblPerfiles" runat="server" Text="Perfiles" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstPerfiles"
                            runat="server"
                            CssClass="listbox-perfiles"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-lista">
                    <h4><asp:Label ID="lblFamilias" runat="server" Text="Familias" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstFamilias"
                            runat="server"
                            CssClass="listbox-perfiles"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-lista">
                    <h4><asp:Label ID="lblPermisos" runat="server" Text="Permisos" /></h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstPermisos"
                            runat="server"
                            CssClass="listbox-perfiles"></asp:ListBox>
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
                        ID="btnEliminarPerfil"
                        runat="server"
                        Text="Eliminar perfil" OnClick="btnEliminarPerfil_Click" />
                    <asp:Button
                        ID="btnEliminarComponente"
                        runat="server"
                        Text="Eliminar componente" OnClick="btnEliminarComponente_Click" />
                    <div class="separador-botones"></div>
                    <asp:TextBox
                        ID="txtNombrePerfil"
                        runat="server"
                        CssClass="txt-nombre-perfil"
                        placeholder="Nombre de perfil">
                    </asp:TextBox>
                    <asp:Button
                        ID="btnAgregarPerfil"
                        runat="server"
                        Text="Agregar perfil" OnClick="btnAgregarPerfil_Click" />
                </aside>
            </div>
        </section>
    </div>
</asp:Content>
