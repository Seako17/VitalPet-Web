<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FGF_Finanzas.Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Content/Login/LoginStyles.css" rel="stylesheet" />
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="login-container">
        <h2>¡Bienvenido!</h2>

        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>

        <div class="input-group">
            <asp:TextBox runat="server" ID="UserName" CssClass="form-input" Placeholder="Usuario" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                CssClass="validation-error" ErrorMessage="El campo de nombre de usuario es obligatorio." Display="Dynamic" />
        </div>

        <div class="input-group">
            <div class="password">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-input" Placeholder="Contraseña" ClientIDMode="Static" />
                <button type="button" id="togglePassword" class="mostrarContraseña" onclick="togglePasswordIcon()"></button>
            </div>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                CssClass="validation-error" ErrorMessage="El campo de contraseña es obligatorio." Display="Dynamic" />
            <asp:Label runat="server" ID="lblError" CssClass="validation-error" />
        </div>

        <div class="remember-group">
            <asp:CheckBox runat="server" ID="RememberMe" ClientIDMode="Static" />
            <label for="RememberMe">Recordar cuenta</label>
        </div>

        <div class="action-group">
            <asp:Button class="btn-submit" runat="server" OnClick="LogIn" Text="Iniciar sesión" />
        </div>
    </div>

</asp:Content>
