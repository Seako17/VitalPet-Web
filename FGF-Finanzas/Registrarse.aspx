<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registrarse.aspx.cs" Inherits="FGF_Finanzas.Registrarse" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Content/RegistrarStyles.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="barra-titulo">
        <h1>Registrarse</h1>
    </div>

    <div class="contenedor-registro">
        <h2>INGRESE SUS DATOS</h2>
        <hr class="linea-subrayado" />

        <div class="formulario-datos">

            <div class="input-group">
                <asp:TextBox runat="server" ID="DniUsuario" CssClass="entrada-formulario" Placeholder="DNI" />
            </div>

            <div class="input-group">
                <asp:TextBox runat="server" ID="Nombre" CssClass="entrada-formulario" Placeholder="Nombre" />
            </div>

            <div class="input-group">
                <asp:TextBox runat="server" ID="Apellido" CssClass="entrada-formulario" Placeholder="Apellido" />
            </div>

            <div class="input-group">
                <asp:TextBox runat="server" ID="Email" TextMode="Email" CssClass="entrada-formulario" Placeholder="Email" />
            </div>

            <div class="input-group">
                <asp:TextBox runat="server" ID="UserName" CssClass="entrada-formulario" Placeholder="Nombre usuario" />
            </div>

            <div class="input-group">
                <div class="password">
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="entrada-formulario" Placeholder="Contraseña" ClientIDMode="Static" ></asp:TextBox>
                    <button type="button" id="togglePassword" class="mostrarContraseña" onclick="togglePasswordIcon('Password', 'togglePassword')"></button>
                </div>
            </div>

            <div class="input-group">
                <div class="password">
                    <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="entrada-formulario" Placeholder="Confirmar contraseña" ClientIDMode="Static" ></asp:TextBox>
                    <button type="button" id="toggleConfirmPassword" class="mostrarContraseña" onclick="togglePasswordIcon('ConfirmPassword', 'toggleConfirmPassword')"></button>
                </div>
                <asp:Label runat="server" ID="lblError" CssClass="lblError" />
            </div>

            <div class="botones">
                <asp:Button class="boton" runat="server" ID="BtnCancelar" Text="Cancelar" PostBackUrl="~/Default.aspx" CauseValidation="false" />
                <asp:Button class="boton" runat="server" ID="BtnRegistrar" OnClick="CreateUser_Click" Text="Registrarse" />
            </div>

        </div>
    </div>
</asp:Content>
