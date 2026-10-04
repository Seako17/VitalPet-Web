<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Familias.aspx.cs" Inherits="FGF_Finanzas.Gestion_Familias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionFamiliasStyles.css" rel="stylesheet" />
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
        <section class="gestion-familias__container">
            <h3>Gestión de Familias</h3>
            <div class="familias-layout">
                <div class="columna-familia">
                    <h4>Familia base</h4>
                    <div class="lista-contenedor">
                        <asp:ListBox runat="server" ID="lstFamiliaBase"
                            CssClass="listbox-familias"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-familia">
                    <h4>Familia a agregar</h4>
                    <div class="lista-contenedor">
                        <asp:ListBox
                            ID="lstFamiliaAgregar"
                            runat="server"
                            CssClass="listbox-familias"
                            AutoPostBack="true"></asp:ListBox>
                    </div>
                </div>
                <div class="columna-permisos">
                    <h4>Permisos</h4>
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
