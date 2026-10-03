<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Perfiles.aspx.cs" Inherits="FGF_Finanzas.Gestion_Perfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionPerfiles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2>Panel de Administrador</h2>
    </div>
    <div id="contenedor-alertas"></div>
    <div class="main-layout">
        <aside class="sidebar">
            <h3>Menú</h3>
            <ul>
                <li><a href="Gestion-Usuarios.aspx">Gestión Usuarios</a></li>
                <li><a href="BitacoraEventos.aspx">Bitácora eventos</a></li>
                <li class="active"><a href="Gestion-Perfiles.aspx">Gestión Perfiles</a></li>
                <li><a href="Gestion-Familias.aspx">Gestión Familias</a></li>
                <li><a href="Backup-Restore.aspx">Backup / Restore</a></li>
            </ul>

        </aside>
        <section class="gestion-perfiles__container">
            <h3>Gestión de Perfiles</h3>
            <div class="perfiles-layout">
                <div class="columna-lista">
                    <h4>Perfiles</h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstPerfiles"
                            runat="server"
                            CssClass="listbox-perfiles"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-lista">
                    <h4>Familias</h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstFamilias"
                            runat="server"
                            CssClass="listbox-perfiles"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-lista">
                    <h4>Permisos</h4>
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
