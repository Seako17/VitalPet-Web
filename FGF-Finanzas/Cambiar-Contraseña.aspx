<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cambiar-Contraseña.aspx.cs" Inherits="FGF_Finanzas.Cambiar_Contraseña" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionUsuarios/CambiarContraseñaStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="barra-titulo">
        <h2>Cambiar contraseña</h2>
    </div>

    <div class="main-layout">
        <div class="cambiar-contraseña__container">
            <h3>Complete los datos:</h3>
            
            <div class="control-form">
                <div class="password-wrapper">
                    <asp:TextBox ID="txtContraseñaActual" runat="server" TextMode="Password" class="campo-input" placeholder="Contraseña actual" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" id="btnToggleActual" class="mostrarContraseña" onclick="togglePasswordIcon('txtContraseñaActual', 'btnToggleActual')"></button>
                </div>
            </div>
            
            <div class="control-form">
                <div class="password-wrapper">
                    <asp:TextBox ID="txtNuevaContraseña" runat="server" TextMode="Password" class="campo-input" placeholder="Nueva contraseña" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" id="btnToggleNueva" class="mostrarContraseña" onclick="togglePasswordIcon('txtNuevaContraseña', 'btnToggleNueva')"></button>
                </div>
            </div>
            
            <div class="control-form">
                <div class="password-wrapper">
                    <asp:TextBox ID="txtConfirmarContraseña" runat="server" TextMode="Password" class="campo-input" placeholder="Confirmar nueva contraseña" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" id="btnToggleConfirmar" class="mostrarContraseña" onclick="togglePasswordIcon('txtConfirmarContraseña', 'btnToggleConfirmar')"></button>
                </div>
            </div>
            
            <div class="botones">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn" OnClick="btnCancelar_Click" />
                <asp:Button ID="btnCambiar" runat="server" Text="Cambiar contraseña" CssClass="btn" OnClick="btnCambiar_Click" />
            </div>
        </div>
    </div>

</asp:Content>