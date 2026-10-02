<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backup-Restore.aspx.cs" Inherits="FGF_Finanzas.Backup_Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/BackupRestore/BackupStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2><asp:Label ID="lblTituloPanel" runat="server" Text="Panel de Administrador" /></h2>
    </div>

    <div class="main-layout">
        <aside class="sidebar">
            <h3><asp:Label ID="lblTituloMenu" runat="server" Text="Menú" /></h3>
            <ul>
                <li><asp:HyperLink ID="lnkMenuUsuarios" runat="server" NavigateUrl="Gestion-Usuarios.aspx">Gestión Usuarios</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuBitacora" runat="server" NavigateUrl="BitacoraEventos.aspx">Bitácora eventos</asp:HyperLink></li>
                <li><asp:Label ID="lblMenuPerfiles" runat="server" Text="Gestión Perfiles" /></li>
                <li><asp:Label ID="lblMenuFamilias" runat="server" Text="Gestión Familias" /></li>
                <li class="active"><asp:Label ID="lblMenuBackup" runat="server" Text="Backup/Restore" /></li>
            </ul>
        </aside>
        <div class="backup-restore__container">
            <div class="mensaje-container">
                <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje"></asp:Label>
            </div>
            <div class="backup-card">
                <h3><asp:Label ID="lblTituloBackup" runat="server" Text="Backup" /></h3>
                <p><asp:Label ID="lblDescripcionBackup" runat="server" Text="Pulse el botón para generar un backup de la base de datos y descargarlo en su computadora" /></p>
                <asp:Button ID="btnBackup" runat="server" Text="Descargar backup" OnClick="btnBackup_Click" CssClass="btn-action" />
            </div>

            <div class="restore-card">
                <h3><asp:Label ID="lblTituloRestore" runat="server" Text="Restore" /></h3>
                <p><asp:Label ID="lblDescripcionRestore" runat="server" Text="Suba un archivo &quot;.bak&quot; y pulse el botón para restaurar la base de datos a partir de ese backup." /></p>
                <div class="restore-actions">
                    <asp:Label ID="lblBak" runat="server" AssociatedControlID="fileRestore" CssClass="btn-upload">

                        <asp:Image ID="imgClip" runat="server" ImageUrl="~/Content/BackupRestore/clip.png" CssClass="img-clip" AlternateText="Icono Clip" />

                        <span><asp:Label ID="lblSubirBak" runat="server" Text="Subir .BAK" /></span>
                    </asp:Label>

                    <asp:FileUpload ID="fileRestore" runat="server" Style="display: none;" onchange="updateFileName(this)" accept=".bak"/>
                    <span id="fileNameLabel" class="file-name-text"></span>

                    <asp:Button ID="btnRestore" runat="server" Text="Restaurar" CssClass="btn-action" OnClick="btnRestore_Click" />
                </div>
            </div>


        </div>

    </div>
</asp:Content>
