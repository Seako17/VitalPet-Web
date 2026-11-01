<%@ Page Title="Tienda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Marketplace.aspx.cs" Inherits="FGF_Finanzas.Marketplace" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server"><link href="Content/Marketplace.css" rel="stylesheet" /></asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
<section class="market-wrap">
    <header class="market-heading">
        <div><span class="market-kicker"><asp:Literal ID="litKicker" runat="server" /></span><h1><asp:Literal ID="litTitulo" runat="server" /></h1><p><asp:Literal ID="litDescripcion" runat="server" /></p></div>
        <a class="market-cart" href="Carrito.aspx"><asp:Literal ID="litCarrito" runat="server" /> <span><asp:Label ID="lblCantidadCarrito" runat="server" Text="0" /></span></a>
    </header>
    <asp:Label ID="lblEstadoOperacion" runat="server" CssClass="market-message" EnableViewState="false" />
    <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand" OnItemDataBound="rptProductos_ItemDataBound">
        <HeaderTemplate><div class="product-grid"></HeaderTemplate>
        <ItemTemplate>
            <article class="product-card">
                <div class="product-photo"><asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("Imagen"))) %>'><img src='<%#: Eval("Imagen") %>' alt='<%#: Eval("Nombre") %>' /></asp:PlaceHolder><asp:PlaceHolder runat="server" Visible='<%# String.IsNullOrEmpty(Convert.ToString(Eval("Imagen"))) %>'><span class="product-placeholder">VITALPET</span></asp:PlaceHolder></div>
                <div class="product-info"><h2><%#: Eval("Nombre") %></h2><p><%#: Eval("Descripcion") %></p><small><asp:Literal ID="litDisponibilidad" runat="server" /></small>
                    <div class="product-bottom"><strong>$<%# Eval("Precio", "{0:N2}") %></strong><div class="product-actions"><asp:TextBox ID="txtCantidad" runat="server" Text="1" TextMode="Number" min="1" max="99" CssClass="quantity" /><asp:LinkButton ID="btnAgregar" runat="server" CssClass="market-button" CommandName="Agregar" CommandArgument='<%# Eval("IdProducto") %>' CausesValidation="false" /></div></div>
                </div>
            </article>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
    <asp:Panel ID="pnlVacio" runat="server" CssClass="market-empty"><h2><asp:Literal ID="litVacioTitulo" runat="server" /></h2><p><asp:Literal ID="litVacioDescripcion" runat="server" /></p></asp:Panel>
</section>
</asp:Content>
