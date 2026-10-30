<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Atenciones.aspx.cs" Inherits="FGF_Finanzas.Atenciones" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Atenciones/Atenciones.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2>Consultas pendientes de atención</h2>
    </div>

    <div class="main-layout">
        <div class="eventos__container">
            <h3><asp:Label ID="lblSubtitulo" runat="server" class="lblSubtitle" Text="Consultas sin atender" /></h3>

            <asp:Label ID="lblMensaje" runat="server" class="lblError" />

            <div class="table-responsive">
                <asp:GridView ID="gvConsultas" runat="server" AutoGenerateColumns="False" CssClass="table"
                    DataKeyNames="idConsulta" OnSelectedIndexChanged="gvConsultas_SelectedIndexChanged"
                    SelectedRowStyle-CssClass="fila-seleccionada" ShowHeaderWhenEmpty="True"
                    EmptyDataText="No hay consultas pendientes de atención.">
                    <Columns>
                        <asp:BoundField DataField="idConsulta" HeaderText="N°" />
                        <asp:BoundField DataField="mascota" HeaderText="Mascota" />
                        <asp:BoundField DataField="veterinario" HeaderText="Veterinario/a" />
                        <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                        <asp:BoundField DataField="fechaHora" HeaderText="Fecha y hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
                    </Columns>
                </asp:GridView>
            </div>

            <div>
                <asp:Button ID="btnAtender" class="btn" runat="server" Text="Atender" OnClick="btnAtender_Click" />
            </div>
        </div>
    </div>
</asp:Content>
