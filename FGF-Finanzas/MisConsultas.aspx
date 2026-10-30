<%@ Page Title="Mis consultas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MisConsultas.aspx.cs" Inherits="FGF_Finanzas.MisConsultas" ViewStateEncryptionMode="Always" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <link href="Content/EventosStyles.css" rel="stylesheet" />
    <link href="Content/Consultas/MisConsultas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo"><h2><asp:Label ID="lblTitulo" runat="server" Text="Mis consultas" /></h2></div>
    <div class="mis-consultas">
        <div class="eventos__container">
            <div class="eventos__filtros">
                <asp:Label ID="lblEstado" runat="server" Text="Estado:" AssociatedControlID="ddlEstado" />
                <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="EstadoChanged">
                    <asp:ListItem Value="">Todas</asp:ListItem>
                    <asp:ListItem Value="Agendada">Agendadas</asp:ListItem>
                    <asp:ListItem Value="Atendida">Atendidas</asp:ListItem>
                    <asp:ListItem Value="Cancelada">Canceladas</asp:ListItem>
                </asp:DropDownList>
                <asp:HyperLink ID="lnkNueva" runat="server" NavigateUrl="~/Consulta.aspx" Text="Nueva consulta" CssClass="btn-filtro" />
            </div>
            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-consulta" role="status" />
            <div class="table-responsive">
                <asp:GridView ID="grilla" runat="server" AutoGenerateColumns="false" CssClass="table" GridLines="None"
                    OnRowCommand="ConsultaCommand" OnRowDataBound="ConsultaBound"
                    EmptyDataText="No hay consultas para el estado seleccionado.">
                    <Columns>
                        <asp:BoundField DataField="nombreMascota" HeaderText="Mascota" />
                        <asp:BoundField DataField="fechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="nombreVeterinario" HeaderText="Veterinario/a" />
                        <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                        <asp:TemplateField HeaderText="Estado"><ItemTemplate><asp:Literal ID="litEstado" runat="server" Mode="Encode" /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones"><ItemTemplate>
                            <asp:LinkButton ID="btnDetalle" runat="server" CommandName="Detalle" CommandArgument='<%# Eval("idConsulta") %>' Text="Ver detalle" CausesValidation="false" />
                            <asp:LinkButton ID="btnCancelarConsulta" runat="server" CommandName="CancelarConsulta" CommandArgument='<%# Eval("idConsulta") %>' Text="Cancelar cita" CausesValidation="false" />
                        </ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="pnlDetalle" runat="server" Visible="false" CssClass="detalle-consulta" EnableViewState="false">
                <h3><asp:Label ID="lblDetalle" runat="server" Text="Detalle de la atención" /></h3>
                <p><asp:Literal ID="litResumen" runat="server" Mode="Encode" /></p>
                <h4><asp:Label ID="lblDiagnostico" runat="server" Text="Diagnóstico" /></h4>
                <p class="diagnostico"><asp:Literal ID="litDiagnostico" runat="server" Mode="Encode" /></p>
                <asp:Button ID="btnCerrar" runat="server" Text="Cerrar detalle" CssClass="btn-filtro" OnClick="CerrarDetalle" CausesValidation="false" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
