<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BitacoraEventos.aspx.cs" Inherits="FGF_Finanzas.BitacoraEventos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Bitácora de Eventos</h2>
    <div class="eventos__container">
        <asp:GridView ID="GridViewEventos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
            <Columns>
                <asp:BoundField DataField="DNI" HeaderText="Usuario"  />
                <asp:BoundField DataField="fechaHora" HeaderText="Fecha y Hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"/>
                <asp:BoundField DataField="modulo" HeaderText="Módulo" />
                <asp:BoundField DataField="evento" HeaderText="Evento" />
                <asp:BoundField DataField="criticidad" HeaderText="Criticidad" />
            </Columns>
        </asp:GridView>
        <div class="eventos__filtros">

            <asp:Label ID="Label1" for="nombreUsuario" runat="server" Text="Nombre de Usuario:"></asp:Label>
            <asp:TextBox ID="nombreUsuario" runat="server"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="Fecha:"></asp:Label>
            <input type="date" id="fechaFiltro" runat="server" />
            <asp:Label ID="Label3" runat="server" Text="Módulo:"></asp:Label>
            <select id="moduloFiltro" runat="server">
                <option value="Todos">Todos</option>
                <option value="Usuarios">Usuarios</option>
                <option value="Sistema">Sistema</option>
                <option value="Reportes">En Desarrollo</option>
            </select>
            <asp:Label ID="Label4" for="eventoFiltro" runat="server" Text="Evento:"></asp:Label>
            <select id="eventoFiltro" runat="server">
                <option value="Todos">Todos</option>
                <option value="Iniciar Sesion">Iniciar Sesión</option>
                <option value="Romper Todo">Romper Todo</option>
                <option value="Reportes">En Desarrollo</option>
            </select>
            <select id="criticidadFiltro" runat="server">
                <option value="Sin">Sin criticidad</option>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
            </select>
            <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" OnClick="btnAplicar_Click" />
            <asp:CheckBox ID="conFecha" runat="server" Text="Filtrar con Fecha" />
        </div>
    </div>
</asp:Content>
