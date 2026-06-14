<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BitacoraEventos.aspx.cs" Inherits="FGF_Finanzas.BitacoraEventos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/EventosStyles.css" rel="stylesheet" />
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
                <li class="active">Bitácora eventos</li>
                <li>Gestión Perfiles</li>
                <li>Gestión Familias</li>
                <li><a href="Backup-Restore.aspx">Backup/Restore</a></li>
            </ul>
        </aside>
        <div class="eventos__container">
            <h3>Bitácora de Eventos</h3>

            <div class="table-responsive">
                <asp:GridView ID="GridViewEventos" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:BoundField DataField="DNI" HeaderText="Usuario" />
                        <asp:BoundField DataField="fechaHora" HeaderText="Fecha y Hora" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="modulo" HeaderText="Módulo" />
                        <asp:BoundField DataField="evento" HeaderText="Evento" />
                        <asp:BoundField DataField="criticidad" HeaderText="Criticidad" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="eventos__filtros">
                <asp:TextBox ID="nombreUsuario" runat="server" placeholder="Nombre usuario"></asp:TextBox>
                <input type="date" id="fechaFiltro" runat="server" />
                <select id="moduloFiltro" runat="server">
                    <option value="">Módulo</option>
                    <option value="Usuarios">Usuarios</option>
                    <option value="">En desarollo ...</option>
                </select>
                <select id="eventoFiltro" runat="server">
                    <option value="">Evento</option>
                    <option value="Iniciar Sesión">Iniciar Sesión</option>
                    <option value="Registrar Usuario">Registrar Usuario</option>
                    <option value="Cerrar Sesión">Cerrar Sesión</option>
                </select>
                <select id="criticidadFiltro" runat="server">
                    <option value="0">Criticidad</option>
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                </select>

                <div class="filtros-acciones">
                    <asp:Button ID="btnAplicar" runat="server" Text="Filtrar" CssClass="btn-filtro" OnClick="btnAplicar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn-filtro" OnClick="btnLimpiar_Click" />
                    <asp:CheckBox ID="conFecha" runat="server" Text="Filtrar con fecha" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
