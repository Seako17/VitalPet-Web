<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Consulta.aspx.cs" Inherits="FGF_Finanzas.Consulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Consultas/Consultas.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-container-main">
        <div class="main-title">
            <h2>AGENDAR CONSULTAS</h2>
        </div>
        <div class="form-card">
            <h2 class="form-subtitle">Datos para la Consulta</h2>

            <div class="form-group">
                <label for="dropMascota">Mascota:</label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="dropMascota"
                    AutoPostBack="true" OnSelectedIndexChanged="Horario_Changed">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtMotivo">Motivo:</label>
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-input" Placeholder="Ej: Vacunación"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtMotivo">Fecha y Hora:</label>
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
                <label for="ddlVeterinario">Veterinario/a:</label>
                <asp:DropDownList runat="server" CssClass="form-input" ID="ddlVeterinario"></asp:DropDownList>
            </div>

            <asp:Label runat="server" ID="lblError" ForeColor="Red" CssClass="lblError" />
            <asp:Label runat="server" ID="lblOk" CssClass="lblOk" />

            <div class="form-buttons">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-cancelar"
                    UseSubmitBehavior="false" CausesValidation="false" OnClick="btnCancelar_Click" />
                <asp:Button ID="btnAgendar" runat="server" Text="Registrar" CssClass="btn btn-registrar" OnClick="btnAgendar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
