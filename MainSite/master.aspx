<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="master.aspx.cs" Inherits="MainSite.master" UICulture="ru" Culture="ru-RU" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="MainSite" Namespace="MainSite" TagPrefix="main" %>

<!DOCTYPE html>
<script type="text/javascript">

    
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
        var label = document.getElementById("nailDateLabel");        
        label.innerHTML = event.target.getAttribute('time');

        document.getElementById("<%=hiddenField.ClientID%>").value = label.innerHTML;
       
        var modal = document.getElementById('Panl1');
        modal.style.display = "block";
        <%=clientName.ClientID%>.focus();
    }

    function applyMask()
    {
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
        if (keyCode == 8 || keyCode == 229) return true;
        return <%=phone.ClientID%>.value.length <= 13;
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Расписание</title>
    <link rel="stylesheet" type="text/css" href="Modal.css" />
</head>
<body onload="applyHandlers();">
    
    <form id="form1" runat="server">
        <asp:HiddenField ID="hiddenField" runat="server" />
    <!-- The Modal -->        
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>
        <asp:Panel runat="server" ID="mainPanel">
        </asp:Panel>
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите имя" ControlToValidate="clientName" ValidationGroup="nailValid" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlToValidate="clientName" IsValidEmpty="False" ValidationExpression="^[a-zA-Zа-яА-Я ]{3,20}$" ErrorMessage="Введите имя (3 - 20 букв)" ValidationGroup="nailValid" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="phoneText" Text="Телефон" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="phone" onkeyup="applyMask()" onkeydown="return validatePhone(event)" runat="server" ValidationGroup="nailValid" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите телефон" ControlToValidate="phone" ValidationGroup="nailValid" />
                        <asp:RegularExpressionValidator ID="MaskedEditValidator2" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlToValidate="phone"
                            IsValidEmpty="False" ValidationExpression="\([0-9]{3}\)[0-9]{3}\-[0-9]{2}\-[0-9]{2}" ErrorMessage="Неверный формат"
                            ValidationGroup="nailValid" />
                    </td>
                </tr>
                <%--<tr>
                    <td style="text-align: justify" colspan="2">
                        <input type="radio" title="гель лак" />                        
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: justify" colspan="2">
                        <main:TagButton ID="OkButton" CausesValidation="true" ValidationGroup="nailValid" runat="server" Text="Отправить" OnClick="AddNailDate" />
                        <input type="button" class="close" value="Закрыть"/>            
                    </td>
                </tr>
                
            </table>
        </div>
    </form>
</body>
</html>
