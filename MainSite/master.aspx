<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="master.aspx.cs" Inherits="MainSite.master" UICulture="ru" Culture="ru-RU" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="MainSite" Namespace="MainSite" TagPrefix="main" %>

<!DOCTYPE html>
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">

    function selectRow(row)
    {        
        console.log("selectRow runned")
        var input = row.cells[0].children[0]
        row.cells[0].children[0].checked = !row.cells[0].children[0].checked
        price = parseInt(row.cells[1].children[0].innerHTML)
        var priceLabel = document.getElementById('totalPrice');
        var newPrice;
        if (input.checked){
            row.className = "selectedrow"
            newPrice = price + parseInt(priceLabel.innerHTML)
        }
        else{
            row.className = "rows"
            newPrice = parseInt(priceLabel.innerHTML) - price
        }
        priceLabel.innerHTML = newPrice
        document.getElementById('OkButton').disabled = newPrice == 0
    }
    
    function applyHandlers()
    {
        var modal = document.getElementById('Panl1');
        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function() {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function(event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    }

    function showModal(event)
    {
       <%-- var label = document.getElementById("nailDateLabel");        
        label.innerHTML = event.target.getAttribute('time');

        document.getElementById("<%=hiddenField.ClientID%>").value = label.innerHTML;--%>
       
        var modal = document.getElementById('Panl1');
        modal.style.display = "block";
        <%=clientName.ClientID%>.focus();
    }

    function applyMask()
    {
        return;
        var keyCode = ('which' in event) ? event.which : event.keyCode; 
        if (keyCode == 8 || keyCode == 229) return true;

        switch (<%=phone.ClientID%>.value.length) {
            case 1:
                <%=phone.ClientID%>.value = "("+<%=phone.ClientID%>.value
                break;
            case 4:
                <%=phone.ClientID%>.value = <%=phone.ClientID%>.value+")"
                break;
            case 8:
            case 11:
                <%=phone.ClientID%>.value = <%=phone.ClientID%>.value + "-"
                break;            
        } 
    }

    function validatePhone(event)
    {   
        var keyCode = ('which' in event) ? event.which : event.keyCode;
        //return ((<%=phone.ClientID%>.value.length > 4 && <%=phone.ClientID%>.value.length <= 13));
        console.log(keyCode);
        if (<%=phone.ClientID%>.value.length == 4 && (keyCode == 8 || keyCode == 229)) return false;
        if (<%=phone.ClientID%>.value.length == 13 && (keyCode != 8 && keyCode != 229)) return false;
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Расписание</title>
    <link rel="stylesheet" type="text/css" href="Modal.css" />
    <link rel="stylesheet" type="text/css" href="TableStyle.css" />
</head>
<body onload="applyHandlers();">

    <form id="form1" runat="server">
        <asp:HiddenField ID="hiddenField" runat="server" />
        <!-- The Modal -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div style="width:100%;text-align:center">
        <asp:Panel style="display:inline-block" runat="server" ID="mainPanel">
        </asp:Panel>
        </div>
        <div runat="server" id="Panl1" class="modal">
            <table id="dialogTable" runat="server" class="modal-content">
                <tr>
                    <td style="text-align: center" colspan="2">
                        <asp:Label Text="Дата" ID="nailDateLabel" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Text="Имя" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="clientName" ValidationGroup="nailValid" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите имя" ControlToValidate="clientName" ValidationGroup="valid" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlToValidate="clientName" IsValidEmpty="False" ValidationExpression="^[a-zA-Zа-яА-Я ]{3,20}$" ErrorMessage="Введите имя (3 - 20 букв)" ValidationGroup="valid" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="phoneText" Text="Телефон" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="phone" onkeyup="applyMask()" onkeydown="return validatePhone(event)" runat="server" Text="+380" ValidationGroup="nailValid" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите телефон" ControlToValidate="phone" ValidationGroup="valid" />
                        <asp:RegularExpressionValidator ID="MaskedEditValidator2" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlToValidate="phone"
                            IsValidEmpty="False" ValidationExpression="\+[0-9]{12}" ErrorMessage="Введите только цифры"
                            ValidationGroup="valid" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: justify" colspan="2">                        
                        <asp:GridView DataKeyNames="id" ID="GridView1"  OnRowDataBound="GridView1_RowDataBound" runat="server" AutoGenerateColumns="False" DataSourceID="NailDataSource" CssClass="mydatagrid" HeaderStyle-CssClass="header" SelectedRowStyle-CssClass="selectedrow" RowStyle-CssClass="rows" AllowSorting="True">
                            <Columns>                               
                                <asp:TemplateField HeaderText="название" SortExpression="name">                                    
                                    <ItemTemplate>
                                        <asp:Label Visible="false" ID="procedureIdLabel"  runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        <asp:Label Visible="false" ID="procedureAbbreviation"  runat="server" Text='<%# Eval("abbreviation") %>'></asp:Label>
                                        <asp:CheckBox runat="server" ID="procedureRowSelect" />
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                    </ItemTemplate>                                    
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="цена" SortExpression="price">                                    
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:c0}",Eval("price")) %>'></asp:Label>
                                    </ItemTemplate>                                                                        
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="время" SortExpression="duration">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="newDuration" Width="40" runat="server" Text='<%# Bind("duration") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("duration") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="addDuration" Width="40" runat="server"></asp:TextBox>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField> --%>                                                              
                            </Columns>
                            <HeaderStyle CssClass="header"></HeaderStyle>
                            <PagerStyle CssClass="pager"></PagerStyle>
                            <RowStyle CssClass="rows"></RowStyle>
                        </asp:GridView>
                        <asp:SqlDataSource ID="NailDataSource" runat="server" OnSelecting="NailDataSource_Selecting" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" ProviderName="<%$ ConnectionStrings:dbConnectionSctring.ProviderName %>" SelectCommand="GetServicesForEdit" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter Name="localTime" Type="DateTime" />
                            </SelectParameters>                            
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: justify" colspan="2">
                        Сумма: <label id="totalPrice">0</label> руб. 
                    </td>
                </tr>
                <tr>
                    <td style="text-align: justify" colspan="2">
                        <main:TagButton ID="OkButton" CausesValidation="true" ValidationGroup="valid" runat="server" Text="Отправить" OnClick="AddNailDate" />
                        <input type="button" class="close" value="Закрыть" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
