<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="master.aspx.cs" Inherits="MainSite.master" UICulture="ru" Culture="ru-RU" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="MainSite" Namespace="MainSite" TagPrefix="main" %>

<!DOCTYPE html>
<script type="text/javascript">
    function applyMask()
    {
        
    }

    function validatePhone(event)
    {
        var keyCode = ('which' in event) ? event.which : event.keyCode; 
        if (keyCode != 8)
        switch (<%=phone.ClientID%>.value.length) {
            case 0:
                <%=phone.ClientID%>.value = "("+<%=phone.ClientID%>.value
                break;
            case 4:
                 <%=phone.ClientID%>.value = <%=phone.ClientID%>.value+")"
                break;
            case 8:
                <%=phone.ClientID%>.value = <%=phone.ClientID%>.value + "-"
                break;            
        }
        return <%=phone.ClientID%>.value.length <= 12 || keyCode == 8;
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Расписание</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnPopup" runat="server" Style="display: none" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <ajax:ModalPopupExtender ID="mp1" runat="server" TargetControlID="btnPopup" CancelControlID="CancelButton" PopupControlID="Panl1">
        </ajax:ModalPopupExtender>
        <asp:Panel runat="server" ID="mainPanel">
            
        </asp:Panel>
        <asp:Panel BackColor="Wheat" ID="Panl1" runat="server" CssClass="Popup" Style="display: none">
            <table >
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
                        <asp:Label Text="Телефон" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="phone" onkeyup="applyMask()" onkeydown="return validatePhone(event)" runat="server" ValidationGroup="nailValid" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите телефон" ControlToValidate="phone" ValidationGroup="nailValid" />
                        <asp:RegularExpressionValidator ID="MaskedEditValidator2" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlToValidate="phone"
                            IsValidEmpty="False" ValidationExpression="\([0-9]{3}\)[0-9]{3}\-[0-9]{4}" ErrorMessage="Неверный формат"
                            ValidationGroup="nailValid" />                        
                    </td>
                </tr>
                <tr>
                    <td style="text-align: justify" colspan="2">
                        <main:TagButton ID="OkButton" CausesValidation="true" ValidationGroup="nailValid" runat="server" Text="Отправить" OnClick="AddNailDate" />
                        <asp:Button ID="CancelButton" runat="server" Text="Закрыть" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
