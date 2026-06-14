<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backup-Restore.aspx.cs" Inherits="FGF_Finanzas.Backup_Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/BackupStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2>Panel de Administrador</h2>
    </div>

    <div class="main-layout">
        <aside class="sidebar">
            <h3>Menú</h3>
            <ul>
                <li>Gestión Usuarios</li>
                <li><a href="BitacoraEventos.aspx">Bitácora eventos</a></li>
                <li>Gestión Perfiles</li>
                <li>Gestión Familias</li>
                <li class="active">Backup/Restore</li>
            </ul>
        </aside>
        <div class="backup-restore__container">
            <div class="mensaje-container">
                <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje"></asp:Label>
            </div>
            <div class="backup-card">
                <h3>Backup</h3>
                <p>Pulse el botón para generar un backup de la base de datos y descargarlo en su computadora</p>
                <asp:Button ID="btnBackup" runat="server" Text="Descargar backup" OnClick="btnBackup_Click" CssClass="btn-action" />
            </div>

            <div class="restore-card">
                <h3>Restore</h3>
                <p>Suba un archivo ".bak" y pulse el botón para restaurar la base de datos a partir de ese backup.</p>
                <div class="restore-actions">
                    <label for="<%= fileRestore.ClientID %>" class="btn-upload">

                        <asp:Image ID="imgClip" runat="server" ImageUrl="~/Content/BackupRestore/clip.png" CssClass="img-clip" AlternateText="Icono Clip" />

                        <span>Subir .BAK</span>
                    </label>

                    <asp:FileUpload ID="fileRestore" runat="server" Style="display: none;" onchange="updateFileName(this)" accept=".bak"/>
                    <span id="fileNameLabel" class="file-name-text"></span>

                    <asp:Button ID="btnRestore" runat="server" Text="Restaurar" CssClass="btn-action" OnClick="btnRestore_Click" />
                </div>
            </div>


        </div>

    </div>
</asp:Content>
