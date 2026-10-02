<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Usuarios.aspx.cs" Inherits="FGF_Finanzas.Gestion_Usuarios" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionUsuarios/GestionUsuariosStyles.css" rel="stylesheet" />
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
                <li class="active"><asp:Label ID="lblMenuUsuarios" runat="server" Text="Gestión Usuarios" /></li>
                <li><asp:HyperLink ID="lnkMenuBitacora" runat="server" NavigateUrl="BitacoraEventos.aspx">Bitácora eventos</asp:HyperLink></li>
                <li><asp:Label ID="lblMenuPerfiles" runat="server" Text="Gestión Perfiles" /></li>
                <li><asp:Label ID="lblMenuFamilias" runat="server" Text="Gestión Familias" /></li>
                <li><asp:HyperLink ID="lnkMenuBackup" runat="server" NavigateUrl="Backup-Restore.aspx">Backup/Restore</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuIdiomas" runat="server" NavigateUrl="Gestion-Idioma.aspx">Gestión Idiomas</asp:HyperLink></li>
            </ul>
        </aside>

        <div class="gestion-usuarios__container">
            <h3><asp:Label ID="lblSubtituloUsuarios" runat="server" Text="Gestión de usuarios" /></h3>
            <div class="area-datos">
                <div class="tabla-usuarios">
                    <asp:GridView ID="dgvUsuarios" runat="server" AutoGenerateColumns="False"
                        ShowHeaderWhenEmpty="true" EmptyDataText="No hay usuarios para mostrar." OnSelectedIndexChanged="dgvUsuarios_SelectedIndexChanged" SelectedRowStyle-CssClass="fila-seleccionada">
                        <Columns>
                            <asp:BoundField DataField="DNI" HeaderText="DNI" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="mail" HeaderText="Email" />
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                            <asp:BoundField DataField="Rol" HeaderText="Rol" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="fila-intermedia">
                    <div class="filtro-activos">
                        <asp:RadioButtonList ID="rblFiltroTodosActivos" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblFiltroTodosActivos_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="Bloqueados">Bloqueados</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Todos">Todos</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <div class="control-encriptado">
                        <asp:CheckBox ID="chkVerEncriptado" runat="server" Text="Ver datos encriptados" AutoPostBack="true" OnCheckedChanged="chkVerEncriptado_CheckedChanged" />
                    </div>

                    <div class="campo-modo">
                        <asp:Label ID="lblModo" runat="server" AssociatedControlID="txtModo" Text="Modo:" />
                        <asp:TextBox ID="txtModo" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="formulario-inputs">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblDni" runat="server" AssociatedControlID="txtDni" Text="DNI:" /></td>
                            <td>
                                <asp:TextBox ID="txtDni" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" Text="Nombre:" /></td>
                            <td>
                                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblApellido" runat="server" AssociatedControlID="txtApellido" Text="Apellido:" /></td>
                            <td>
                                <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="Email:" /></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNombreUsuario" runat="server" AssociatedControlID="txtNombreUsuario" Text="Nombre usuario:" /></td>
                            <td>
                                <asp:TextBox ID="txtNombreUsuario" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRol" runat="server" AssociatedControlID="ddlRol" Text="Rol:" /></td>
                            <td>
                                <asp:DropDownList ID="ddlRol" runat="server">
                                    <asp:ListItem Text="-- Seleccionar Rol --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Web Master" Value="Web Master"></asp:ListItem>
                                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                    <asp:ListItem Text="Cliente" Value="Cliente"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>

                    <aside class="botones-operaciones">
                        <asp:Button ID="btnCrear" runat="server" Text="Crear" OnClick="btnCrear_Click" />
                        <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" OnClick="btnDesbloquear_Click" />
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" />
                        <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" OnClick="btnAplicar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </aside>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
