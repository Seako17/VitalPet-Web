<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backup-Restore.aspx.cs" Inherits="FGF_Finanzas.Backup_Restore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Gestión de base de datos</h3>
        <asp:Button ID="btnBackup" runat="server" Text="Descargar backup" OnClick="btnBackup_Click" />
        <br />
        <br />
        <hr />
        <br />
        <asp:FileUpload ID="fileRestore" runat="server" />
        <br />
        <asp:Button ID="btnRestore" runat="server" Text="Restaurar desde backup" />
        <br />
        <br />
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        <br />
    </div>
</asp:Content>
