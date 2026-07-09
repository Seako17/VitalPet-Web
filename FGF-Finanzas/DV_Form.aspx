<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DV_Form.aspx.cs" Inherits="FGF_Finanzas.DV_Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>¡Inconsistencia de Datos!</h1>
            <p>Presione el botón para realizar un escaneo completo de la base de datos en busca de alteraciones externas.</p>

            <asp:GridView ID="GridInconsistencias" runat="server" AutoGenerateColumns="False" CssClass="grid-view" Visible="false">
                <Columns>
                    <asp:BoundField DataField="NombreTabla" HeaderText="Tabla Afectada" />
                    <asp:BoundField DataField="IdRegistro" HeaderText="ID Registro" />
                    <asp:BoundField DataField="TipoFalla" HeaderText="Diagnóstico de la Inconsistencia" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje"></asp:Label>

            <asp:FileUpload ID="fileRestore" runat="server" Style="display: none;" accept=".bak" />
            <label for="<%= fileRestore.ClientID %>" class="btn-upload">

                <asp:Image ID="imgClip" runat="server" ImageUrl="~/Content/BackupRestore/clip.png" CssClass="img-clip" AlternateText="Icono Clip" />

                <span>Subir .BAK</span>
            </label>

            <div class="botones">
                <asp:Button ID="btnSalir" runat="server" Text="Salir" OnClick="btnSalir_Click" />
                <asp:Button ID="btnInicializar" runat="server" Text="Recalcular" OnClick="btnRecalcular_Click" />
                <asp:Button ID="btnRestore" runat="server" Text="Restaurar BD" OnClick="btnRestore_Click" />
            </div>
        </div>
    </form>
</body>
</html>
