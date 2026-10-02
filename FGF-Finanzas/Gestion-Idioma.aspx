<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Idioma.aspx.cs" Inherits="FGF_Finanzas.Gestion_Idioma" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Estilos-Gestion-Idioma.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="barra-titulo">
        <h2><asp:Label ID="lblTitulo" runat="server" Text="Gestión de Idiomas" /></h2>
    </div>
    <div id="contenedor-alertas"></div>

    <div class="main-layout">
        <aside class="sidebar">
            <h3><asp:Label ID="lblTituloMenu" runat="server" Text="Menú" /></h3>
            <ul>
                <li><asp:HyperLink ID="lnkMenuUsuarios" runat="server" NavigateUrl="Gestion-Usuarios.aspx">Gestión Usuarios</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuBitacora" runat="server" NavigateUrl="BitacoraEventos.aspx">Bitácora eventos</asp:HyperLink></li>
                <li><asp:HyperLink ID="lnkMenuBackup" runat="server" NavigateUrl="Backup-Restore.aspx">Backup/Restore</asp:HyperLink></li>
                <li class="active"><asp:Label ID="lblMenuIdiomas" runat="server" Text="Gestión Idiomas" /></li>
            </ul>
        </aside>

        <div class="gestion-idioma__container">
            <h3><asp:Label ID="lblSubtituloTraducciones" runat="server" Text="Traducciones del sistema" /></h3>

            <div class="area-datos">
                <div class="selectores">
                    <div class="campo-selector">
                        <asp:Label ID="lblIdiomaEditar" runat="server" AssociatedControlID="ddlIdioma" Text="Idioma a editar:" />
                        <asp:DropDownList ID="ddlIdioma" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlIdioma_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="campo-selector">
                        <asp:Label ID="lblFormulario" runat="server" AssociatedControlID="ddlFormulario" Text="Formulario:" />
                        <asp:DropDownList ID="ddlFormulario" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFormulario_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>

                <asp:Label ID="lblResumenTraduccion" runat="server" CssClass="resumen-traduccion" />

                <div class="tabla-etiquetas">
                    <asp:GridView ID="dgvTraducciones" runat="server" AutoGenerateColumns="False"
                        ShowHeaderWhenEmpty="true" EmptyDataText="No hay etiquetas para mostrar."
                        OnRowDataBound="dgvTraducciones_RowDataBound"
                        OnRowCommand="dgvTraducciones_RowCommand" CssClass="tabla-idioma">
                        <Columns>
                            <asp:BoundField DataField="ControlId" HeaderText="Control" />
                            <asp:TemplateField HeaderText="Texto traducido">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTexto" runat="server" Text='<%# Bind("Texto") %>' CssClass="input-texto" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnGuardarTraduccion" runat="server" CommandName="GuardarTraduccion"
                                        CssClass="btn-tabla" CausesValidation="false">Guardar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="paneles">
                <section class="panel">
                    <h4><asp:Label ID="lblTituloNuevoIdioma" runat="server" Text="Nuevo idioma" /></h4>
                    <p class="descripcion-panel">
                        <asp:Label ID="lblDescripcionNuevoIdioma" runat="server" Text="El idioma se crea copiando los textos en español por defecto (es-AR), que luego se editan de forma independiente." />
                    </p>
                    <div class="fila-campos">
                        <div class="campo">
                            <asp:Label ID="lblCodigoIdioma" runat="server" AssociatedControlID="txtCodigoIdioma" Text="Código:" />
                            <asp:TextBox ID="txtCodigoIdioma" runat="server" CssClass="input-campo" MaxLength="10" placeholder="pt-BR"></asp:TextBox>
                        </div>
                        <div class="campo">
                            <asp:Label ID="lblNombreIdioma" runat="server" AssociatedControlID="txtNombreIdioma" Text="Nombre:" />
                            <asp:TextBox ID="txtNombreIdioma" runat="server" CssClass="input-campo" MaxLength="50" placeholder="Portugués"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnCrearIdioma" runat="server" Text="Crear idioma" CssClass="btn-accion" OnClick="btnCrearIdioma_Click" CausesValidation="false" />
                    </div>
                </section>

                <section class="panel">
                    <h4><asp:Label ID="lblTituloIdiomasRegistrados" runat="server" Text="Idiomas registrados" /></h4>
                    <div class="tabla-idiomas">
                        <asp:GridView ID="dgvIdiomas" runat="server" AutoGenerateColumns="False"
                            ShowHeaderWhenEmpty="true" EmptyDataText="No hay idiomas registrados."
                            OnRowCommand="dgvIdiomas_RowCommand" CssClass="tabla-idioma">
                            <Columns>
                                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEliminarIdioma" runat="server" CommandName="EliminarIdioma"
                                            CommandArgument='<%# Bind("Codigo") %>' CssClass="btn-tabla btn-tabla-eliminar"
                                            CausesValidation="false"
                                            OnClientClick='return confirm("¿Eliminar el idioma <%# Bind("Codigo") %>? Se perderán todas sus traducciones.");'>Eliminar</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
