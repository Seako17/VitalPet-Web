<%@ Page Title="Gestión de productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gestion-Productos.aspx.cs" Inherits="FGF_Finanzas.Gestion_Productos" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server"><link href="Content/EventosStyles.css" rel="stylesheet" /><link href="Content/Marketplace.css" rel="stylesheet" /><link href="Content/MenuAdministracion.css?v=1" rel="stylesheet" /></asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
<div class="barra-titulo"><h2><asp:Literal ID="litPanelTitulo" runat="server" /></h2></div>
<div class="market-admin">
    <aside class="sidebar"><h3><asp:Literal ID="litAdministracion" runat="server" /></h3><ul>
        <li runat="server" data-permiso="UsuarioGestionar"><a href="Gestion-Usuarios.aspx"><asp:Literal ID="litMenuUsuarios" runat="server" /></a></li>
        <li runat="server" data-permiso="BitacoraVer"><a href="BitacoraEventos.aspx"><asp:Literal ID="litMenuBitacora" runat="server" /></a></li>
        <li runat="server" data-permiso="PerfilGestionar"><a href="Gestion-Perfiles.aspx"><asp:Literal ID="litMenuPerfiles" runat="server" /></a></li>
        <li runat="server" data-permiso="FamiliaGestionar"><a href="Gestion-Familias.aspx"><asp:Literal ID="litMenuFamilias" runat="server" /></a></li>
        <li runat="server" data-permiso="BackUpRestore"><a href="Backup-Restore.aspx"><asp:Literal ID="litMenuBackup" runat="server" /></a></li>
        <li runat="server" data-permiso="IdiomaGestionar"><a href="Gestion-Idioma.aspx"><asp:Literal ID="litMenuIdiomas" runat="server" /></a></li>
        <li class="active" runat="server" data-permiso="ProductoGestionar"><asp:Literal ID="litGestionMenu" runat="server" /></li>
    </ul></aside>
    <section class="admin-products">
        <span class="market-kicker"><asp:Literal ID="litKicker" runat="server" /></span><h1><asp:Literal ID="litTitulo" runat="server" /></h1>
        <asp:Label ID="lblMensaje" runat="server" CssClass="market-message" EnableViewState="false" />
        <asp:Panel ID="pnlModoEdicion" runat="server" CssClass="edit-mode" Visible="false"><asp:Literal ID="litModoEdicion" runat="server" /></asp:Panel>
        <div class="product-editor"><h2><asp:Label ID="lblFormulario" runat="server" /></h2><asp:HiddenField ID="hfIdProducto" runat="server" />
            <div class="editor-grid">
                <div><asp:Label ID="litNombre" runat="server" AssociatedControlID="txtNombre" /><asp:TextBox ID="txtNombre" runat="server" MaxLength="120" /></div>
                <div><asp:Label ID="litPrecio" runat="server" AssociatedControlID="txtPrecio" /><asp:TextBox ID="txtPrecio" runat="server" TextMode="Number" step="0.01" min="0.01" /></div>
                <div class="field-wide"><asp:Label ID="litDescripcion" runat="server" AssociatedControlID="txtDescripcion" /><asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" MaxLength="1000" /></div>
                <div><asp:Label ID="litStock" runat="server" AssociatedControlID="txtStock" /><asp:TextBox ID="txtStock" runat="server" TextMode="Number" min="0" /></div>
                <div><asp:Label ID="litImagen" runat="server" AssociatedControlID="fuImagen" /><asp:FileUpload ID="fuImagen" runat="server" /></div>
            </div>
            <div class="editor-actions"><asp:Button ID="btnGuardar" runat="server" CssClass="market-button" OnClick="btnGuardar_Click" /><asp:Button ID="btnLimpiar" runat="server" CssClass="market-button" CausesValidation="false" OnClick="btnLimpiar_Click" /></div>
        </div>
        <div class="table-responsive"><asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false" CssClass="table product-admin-table" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCommand="gvProductos_RowCommand" OnRowDataBound="gvProductos_RowDataBound">
            <Columns><asp:BoundField DataField="Nombre" HeaderText="Producto" /><asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="${0:N2}" /><asp:BoundField DataField="Stock" HeaderText="Stock" /><asp:TemplateField HeaderText="Acciones"><ItemTemplate><div class="product-row-actions"><asp:LinkButton ID="btnEditar" runat="server" CssClass="back-link" CommandName="Editar" CommandArgument='<%# Eval("IdProducto") %>' CausesValidation="false" /><asp:LinkButton ID="btnEliminar" runat="server" CssClass="remove-button" CommandName="Eliminar" CommandArgument='<%# Eval("IdProducto") %>' CausesValidation="false" /></div></ItemTemplate></asp:TemplateField></Columns>
        </asp:GridView></div>
    </section>
</div>
</asp:Content>
