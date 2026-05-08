<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FGF_Finanzas.Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <% 
        HttpCookie cookie = Request.Cookies["UserSessionFGF"];
        if (cookie != null) 
        { 
    %>
        <p>Bienvenido de nuevo, <%: cookie.Value %></p>
    <% 
        }
        else if(FGF_Finanzas.Capas.Servicios.SessionManager.Instancia != null)
        {
            %>
            <p>Bienvenido <%=FGF_Finanzas.Capas.Servicios.SessionManager.Instancia.Usuario.Usuario.ToString() %></p>
        <% }
        else
        {
    %>
        <p>Bienvenido.</p>
    <% } %>
    </main>

</asp:Content>

