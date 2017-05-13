<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjectDowntownInventory.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>
    <script type="text/javascript">
    $(function () {
        $("[id*=SubmitItemAdd]").bind("click", function () {
            var it = {};
            it.ITEM = $("[id*=ItemAddTxt]").val();
            $.ajax({
                type: "POST",
                url: "Default.aspx/addInventory",
                data: '{it: ' + JSON.stringify(i) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Item has been added successfully.");
                    window.location.reload();
                }
            });
            return false;
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="Auth" runat="server" class="col-lg-8 col-lg-offset-2">
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            Username:
        </td>
        <td>
            <asp:TextBox ID="txtUsername" runat="server" Text="" CssClass="form-control input-lg " />
        </td>
    </tr>
    <tr>
        <td>
            Password:
        </td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control input-lg" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="SubmitButton" Text="Submit" runat="server" OnClick="SubmitButton_Click" CssClass="form-control btn-block"/>
        </td>
    </tr>
</table>
<hr />
</div>
<div id="tbl" runat="server" class ="col-lg-8 col-lg-offset-2">

    <div class="col-lg-8 col-lg-offset-2">
        <asp:TextBox ID="ItemAddTxt" runat="server" CssClass ="form-control input-sm col-lg-10"></asp:TextBox>
        <asp:Button ID="SubmitItemAdd" Text="Submit Item" runat="server" CssClass="form-control btn-default" OnClick="SubmitItemAdd_Click"/>
        <div id="SuccessMessage" runat="server" visible="false"><p>Item added Successfully.</p></div>
    </div>
    <br />
    <br />
    <div>
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-striped" OnRowDeleting="gvUsers_RowDeleting" >
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID"/>
        <asp:BoundField DataField="ITEM" HeaderText="ITEM" />
        <asp:TemplateField HeaderText="QUANTITY ON HAND">
            <ItemTemplate>
                <asp:TextBox ID="txtRowQTY" runat="server" Text='<%# Eval("QUANTITY") %>' />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtRowQTY" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
            </ItemTemplate>
         </asp:TemplateField>
        <asp:TemplateField HeaderText="UNITS">
            <ItemTemplate>
                <asp:TextBox ID="txtRowUNITS" runat="server" Text='<%# Eval("UNITS") %>' />
            </ItemTemplate>
         </asp:TemplateField>
        <asp:TemplateField HeaderText="ORDER MORE">
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" Checked='<%# Eval("ORDER_MORE") %>' />
            </ItemTemplate>
         </asp:TemplateField>
        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />

    </Columns>
    </asp:GridView>
        <div id="SaveSuccessMsg" runat="server" visible="false"><p>Changes Saved...</p></div>
        <asp:Button ID="btnSaveChanges" Text="Save Changes" runat="server" CssClass="form-control btn-default" OnClick="btnSaveChanges_Click"/>

    </div>
</div>
</asp:Content>
