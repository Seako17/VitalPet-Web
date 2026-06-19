<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Usuarios.aspx.cs" Inherits="FGF_Finanzas.Gestion_Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionUsuarios/GestionUsuariosStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2>Panel de Administrador</h2>
    </div>

    <div class="main-layout">
        <aside class="sidebar">
            <h3>Menú</h3>
            <ul>
                <li class="active">Gestión Usuarios</li>
                <li><a href="BitacoraEventos.aspx">Bitácora eventos</a></li>
                <li>Gestión Perfiles</li>
                <li>Gestión Familias</li>
                <li><a href="Backup-Restore.aspx">Backup/Restore</a></li>
            </ul>
        </aside>

        <div class="gestion-usuarios__container">
    <h3>Gestión de usuarios</h3>
    
    <div class="area-datos">
        
        <asp:GridView ID="dgvUsuarios" runat="server" AutoGenerateColumns="False" 
            ShowHeaderWhenEmpty="true" EmptyDataText="No hay usuarios registrados.">
            <Columns>
                <asp:BoundField DataField="DNI" HeaderText="DNI" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                <asp:BoundField DataField="Rol" HeaderText="Rol" />
            </Columns>
        </asp:GridView>

        <div class="fila-intermedia">
            <div class="filtro-activos">
                <asp:RadioButtonList ID="rblFiltroTodosActivos" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="Activos">Activos</asp:ListItem>
                    <asp:ListItem Value="Todos">Todos</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            
            <div class="campo-modo">
                <label>Modo:</label>
                <asp:TextBox ID="txtModo" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>

        <div class="formulario-inputs">
            <table>
                <tr>
                    <td><label>DNI:</label></td>
                    <td><asp:TextBox ID="txtDni" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Nombre:</label></td>
                    <td><asp:TextBox ID="txtNombre" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Apellido:</label></td>
                    <td><asp:TextBox ID="txtApellido" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Email:</label></td>
                    <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Nombre usuario:</label></td>
                    <td><asp:TextBox ID="txtNombreUsuario" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><label>Rol:</label></td>
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
        </div>

    </div> <aside class="botones-operaciones">
        <asp:Button ID="btnCrear" runat="server" Text="Crear" />
        <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" />
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
        <asp:Button ID="btnActDes" runat="server" Text="Activar/Desactivar" />
        <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
    </aside>
</div>
    </div>
</asp:Content>
