<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registrarse.aspx.cs" Inherits="FGF_Finanzas.Registrarse" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <h2><%: Title %>.</h2>


    <div class="form-horizontal">
        <hr />

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DniUsuario" CssClass="col-md-2 control-label">DNI</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DniUsuario" CssClass="form-control" />
            </div>
        </div>
                 
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Nombre" CssClass="col-md-2 control-label">Nombre</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Nombre" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Apellido" CssClass="col-md-2 control-label">Apellido</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Apellido" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Nombre de usuario</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Contraseña</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" ClientIDMode="Static" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirmar contraseña</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />

                <div class="col-md-10">
                    <asp:Label runat="server" ID="lblError" CssClass="text-danger"/>
                    <br />
                </div>
            </div>
            <div class="col-md-10">
                <asp:CheckBox class="checkbox" ID="checkBoxContraseña" runat="server" Checked="false" onclick="mostrarContraseña(this);"/>
                <asp:Label ID="checkBoxLbl" runat="server" AssociatedControlID="checkBoxContraseña" CssClass="col-md-2 control-label">Mostrar contraseña</asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Registrarse" class="boton" />
            </div>
        </div>
    </div>
</asp:Content>