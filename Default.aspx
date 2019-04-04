<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Prueba_Tecnica.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <form id="form1" runat="server">
        <div class="container">
            <div class="well">
                 <div class="col-md-12  "  style="margin-bottom :20px">
                     <asp:Label ID="LabelInfo" runat="server" Text="Label"></asp:Label>
                      </div>
                <div class="col-md-6 " style="margin-bottom :20px">
                    <div class="input-group ">
                        <asp:Label ID="Label1" CssClass="input-group-addon " runat="server" Text="Documento"></asp:Label>
                        <asp:TextBox ID="TextBoxDocumento" CssClass="form-control " runat="server"></asp:TextBox>
                        <div class="input-group-btn ">
                            <asp:Button ID="ButtonBuscar" CssClass="btn btn-info " runat="server" Text="Buscar" OnClick="ButtonBuscar_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="input-group ">
                        <asp:Label ID="Label2" CssClass="input-group-addon " runat="server" Text="Cliente"></asp:Label>
                        <asp:DropDownList ID="DropDownListCliente"  CssClass="form-control " runat="server" DataSourceID="SqlClientes" DataTextField="Name_Customer" DataValueField="ID_Customer"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlClientes" runat="server" ConnectionString="<%$ ConnectionStrings:ProyectoConnectionString %>" SelectCommand="SELECT [ID_Customer], [Name_Customer] FROM [Customer]"></asp:SqlDataSource>
                        <%--<asp:TextBox ID="TextBoxCliente" CssClass="form-control " runat="server"></asp:TextBox>--%>
                      
                    </div>
                </div>
                <div class="col-md-12 "  style="margin-bottom :20px">
                   <div class="input-group ">
                        <asp:Label ID="Label3" CssClass="input-group-addon " runat="server" Text="Producto"></asp:Label>
                        <asp:DropDownList ID="DropDownListProductos"  CssClass="form-control " runat="server" DataSourceID="SqlPorductos" DataTextField="Name_Item" DataValueField="ID_Item"></asp:DropDownList>
                          <asp:SqlDataSource ID="SqlPorductos" runat="server" ConnectionString="<%$ ConnectionStrings:ProyectoConnectionString %>" SelectCommand="SELECT [ID_Item], [Name_Item] FROM [Item]"></asp:SqlDataSource>
                          <asp:Label ID="Label4" CssClass="input-group-addon " runat="server"   Text="Cantidad"></asp:Label>
                          <asp:TextBox ID="TextBoxCantidad" CssClass="form-control " TextMode="Number" runat="server"></asp:TextBox>
                         <asp:Label ID="Label5" CssClass="input-group-addon " runat="server" Text="Precio"></asp:Label>
                          <asp:TextBox ID="TextBoxPrecio" CssClass="form-control " TextMode="Number" runat="server"></asp:TextBox>
                        <div class="input-group-btn ">
                            <asp:Button ID="ButtonAgregarItem" CssClass="btn btn-success  " runat="server" Text="Guardar" OnClick="ButtonAgregarItem_Click" />
                           <asp:Button ID="ButtonEliminar" CssClass="btn btn-danger  " runat="server" Text="Eliminar_Doc" OnClick="ButtonEliminar_Click"  />
                        </div>
                    </div>
                </div>
                <div class="col-md-12 ">
                    <asp:GridView ID="GridView1" CssClass ="table table-hover table-bordered table-condensed" runat="server" >


                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
