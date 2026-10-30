<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consulta.aspx.cs" Inherits="FGF_Finanzas.Consulta" ViewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Consultas/Consultas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-container-main">
        <div class="main-title">
            <h2><asp:Label ID="lblTitulo" runat="server" Text="AGENDAR CONSULTAS" /></h2>
        </div>
        <div class="form-card">
            <asp:Panel ID="pnlFormulario" runat="server" CssClass="datos-consulta">
            <h2 class="form-subtitle"><asp:Label ID="lblSubtitulo" runat="server" Text="Datos para la Consulta" /></h2>

            <div class="form-group">
                <label for="dropMascota"><asp:Label ID="lblMascota" runat="server" Text="Mascota:" /></label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="dropMascota"
                    AutoPostBack="true" OnSelectedIndexChanged="Horario_Changed">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtMotivo"><asp:Label ID="lblMotivo" runat="server" Text="Motivo:" /></label>
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-input" Placeholder="Ej: Vacunación"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtMotivo"><asp:Label ID="lblFechaHora" runat="server" Text="Fecha y Hora:" /></label>
                <div class="fecha-hora-row">
                    <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-input"
                        AutoPostBack="true" OnTextChanged="Horario_Changed"></asp:TextBox>

                    <div class="hora-wrap">
                        <asp:DropDownList ID="ddlHora" runat="server" CssClass="form-input ddl-cuatro-filas"
                            AutoPostBack="true" OnSelectedIndexChanged="Horario_Changed"
                            onfocus="this.size=4;"
                            onblur="this.size=1;"
                            onchange="this.size=1; this.blur();">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>


            <div class="form-group">
                <label for="ddlVeterinario"><asp:Label ID="lblVeterinario" runat="server" Text="Veterinario/a:" /></label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="ddlVeterinario"></asp:DropDownList>
            </div>

            <asp:Label runat="server" ID="lblError" ForeColor="Red" CssClass="lblError" />
            <asp:Label runat="server" ID="lblOk" CssClass="lblOk" />

            <div class="form-buttons">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-cancelar"
                    UseSubmitBehavior="false" CausesValidation="false" OnClick="btnCancelar_Click" />
                <asp:Button ID="btnAgendar" runat="server" Text="Agendar" CssClass="btn btn-registrar" OnClick="btnAgendar_Click" />
            </div>
            </asp:Panel>
            <asp:Panel ID="pnlAviso" runat="server" Visible="false" CssClass="datos-consulta" role="region">
                <h2 class="form-subtitle"><asp:Label ID="lblAviso" runat="server" Text="La mascota ya tiene consultas agendadas" /></h2>
                <div class="tabla-pendientes">
                    <asp:GridView ID="grillaPendientes" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="turnos-pendientes">
                        <Columns>
                            <asp:BoundField DataField="fechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="nombreVeterinario" HeaderText="Veterinario/a" />
                            <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                        </Columns>
                    </asp:GridView>
                </div>
                <p><asp:Label ID="lblConfirmarAviso" runat="server" Text="¿Querés agendar igualmente una nueva consulta?" /></p>
                <div class="form-buttons">
                    <asp:Button ID="btnVolver" runat="server" Text="Volver al formulario" CssClass="btn" CausesValidation="false" OnClick="btnVolver_Click" />
                    <asp:Button ID="btnConfirmar" runat="server" Text="Agendar igualmente" CssClass="btn" OnClick="btnConfirmar_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>

</asp:Content>
