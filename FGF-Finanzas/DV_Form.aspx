<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DV_Form.aspx.cs" Inherits="FGF_Finanzas.DV_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="Content/DV_Styles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="barra-titulo">
            <h2>¡Inconsistencia de Datos!</h2>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="GridInconsistencias" runat="server" AutoGenerateColumns="False" CssClass="table" Visible="false">
                <Columns>
                    <asp:BoundField DataField="NombreTabla" HeaderText="Tabla Afectada" />
                    <asp:BoundField DataField="IdRegistro" HeaderText="ID Registro" />
                    <asp:BoundField DataField="TipoFalla" HeaderText="Diagnóstico de la Inconsistencia" />
                </Columns>
            </asp:GridView>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje"></asp:Label>

        <asp:FileUpload ID="fileRestore" runat="server" Style="display: none;" accept=".bak" />


        <div class="botones">
            <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClick="btnSalir_Click" class="btn-action" />
            <asp:Button ID="btnInicializar" runat="server" Text="Recalcular" OnClick="btnRecalcular_Click" class="btn-action" />
            <asp:Button ID="btnRestore" runat="server" Text="Restaurar BD" OnClick="btnRestore_Click" class="btn-action" />
            <label for="<%= fileRestore.ClientID %>" id="lblBak" class="btn-upload">
                <asp:Image ID="imgClip" runat="server" ImageUrl="~/Content/BackupRestore/clip.png" CssClass="img-clip" AlternateText="Icono Clip" />
                <span>Subir .BAK</span>
            </label>
        </div>

    </form>
</body>
</html>
