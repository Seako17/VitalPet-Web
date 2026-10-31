<%@ Page Title="Mi carrito" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="FGF_Finanzas.Carrito" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server"><link href="Content/Marketplace.css" rel="stylesheet" /></asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
<section class="market-wrap checkout-wrap">
    <a class="back-link" href="Marketplace.aspx"><asp:Literal ID="litSeguirComprando" runat="server" /></a>
    <span class="market-kicker"><asp:Literal ID="litKicker" runat="server" /></span>
    <h1><asp:Literal ID="litTitulo" runat="server" /></h1>
    <asp:Label ID="lblMensaje" runat="server" CssClass="market-message" EnableViewState="false" />
    <asp:GridView ID="gvCarrito" runat="server" AutoGenerateColumns="False" DataKeyNames="IdProducto" CssClass="cart-table cart-items" GridLines="None" OnRowCommand="gvCarrito_RowCommand" OnRowDataBound="gvCarrito_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Producto">
                <ItemTemplate>
                    <div class="cart-product">
                        <asp:PlaceHolder runat="server" Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("Imagen"))) %>'><img src='<%#: Eval("Imagen") %>' alt='<%#: Eval("Nombre") %>' /></asp:PlaceHolder>
                        <span class="cart-product-placeholder" runat="server" visible='<%# String.IsNullOrEmpty(Convert.ToString(Eval("Imagen"))) %>'>VitalPet</span>
                        <div class="cart-product-info"><strong><%#: Eval("Nombre") %></strong><small>$<%# Eval("Precio", "{0:N2}") %> <asp:Literal ID="litPrecioUnitario" runat="server" /></small><p><%#: Eval("Descripcion") %></p></div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cantidad">
                <ItemTemplate>
                    <div class="cart-quantity-control">
                        <asp:TextBox ID="txtCantidad" runat="server" Text='<%# Eval("Cantidad") %>' TextMode="Number" min="1" max="99" CssClass="cart-quantity-input" aria-label="Cantidad" AutoPostBack="true" CausesValidation="false" OnTextChanged="txtCantidad_TextChanged" />
                        <asp:Literal ID="litEstadoStockCantidad" runat="server" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="${0:N2}" />
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate><asp:LinkButton ID="btnEliminar" runat="server" CssClass="remove-button cart-delete-button" CommandName="Eliminar" CommandArgument='<%# Eval("IdProducto") %>' CausesValidation="false" /></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="cart-summary"><span><asp:Literal ID="LitTotal" runat="server" /></span><strong>$<asp:Label ID="lblTotal" runat="server" Text="0,00" /></strong></div>
    <asp:Panel ID="pnlPago" runat="server" CssClass="payment-panel">
        <div class="payment-title"><h2><asp:Literal ID="litPagoTitulo" runat="server" /></h2><span><asp:Literal ID="litPagoNotaTitulo" runat="server" /></span></div>
        <div class="payment-grid">
            <div class="field-wide"><asp:Label ID="litNombreTitular" runat="server" AssociatedControlID="txtTitular" /><asp:TextBox ID="txtTitular" runat="server" MaxLength="80" autocomplete="cc-name" /></div>
            <div class="field-wide"><asp:Label ID="litNumeroTarjeta" runat="server" AssociatedControlID="txtTarjeta" /><asp:TextBox ID="txtTarjeta" runat="server" MaxLength="16" autocomplete="off" inputmode="numeric" CssClass="card-number-input" /><asp:RegularExpressionValidator ID="valTarjeta" runat="server" ControlToValidate="txtTarjeta" ValidationExpression="^[0-9]{16}$" CssClass="field-error" Display="Dynamic" /></div>
            <div><asp:Label ID="litVencimiento" runat="server" AssociatedControlID="txtVencimiento" /><asp:TextBox ID="txtVencimiento" runat="server" MaxLength="5" autocomplete="cc-exp" /></div>
            <div><asp:Label ID="litCvv" runat="server" AssociatedControlID="txtCvv" /><asp:TextBox ID="txtCvv" runat="server" TextMode="Password" MaxLength="4" autocomplete="cc-csc" inputmode="numeric" /></div>
        </div>
        <asp:Button ID="btnComprar" runat="server" CssClass="market-button buy-button" OnClick="btnComprar_Click" />
        <p class="payment-note"><asp:Literal ID="litNotaPago" runat="server" /></p>
    </asp:Panel>
</section>
</asp:Content>
