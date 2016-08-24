<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="master.aspx.cs" Inherits="MainSite.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="MainSite" Namespace="MainSite" TagPrefix="main" %>
<!DOCTYPE html>
<script type="text/javascript">
    function validateNailsData()
    {   
        var result = <%=clientName.ClientID%>.value.search(/[a-zA-Zа-яА-Я].{2,10}/) != -1;
        result = <%=phone.ClientID%>.value.search(/^[(]?[0-9]{3}[)]?[-]?[0-9]{3}[-]?[0-9]{4}$/im) != -1 && result;
        document.getElementById("OkButton").disabled = !result
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
        <div>
            <asp:Table BorderStyle="Outset" CellPadding="5" BorderWidth="1" CellSpacing="0" ID="mainTable" runat="server">
                <asp:TableHeaderRow BorderColor="Gray">
                    <asp:TableHeaderCell ColumnSpan="8">Расписание</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow BorderStyle="Outset" BorderColor="Gray" ID="daysHeader" BorderWidth="1" BackColor="Gray">
                    <asp:TableHeaderCell ForeColor="White" BackColor="Black"><div style="padding:5px">время</div></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="first"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="second"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="third"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="fourth"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="fifth"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="sixth"></asp:TableHeaderCell>
                    <asp:TableHeaderCell ForeColor="White" Width="50" ID="seventh"></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <asp:Panel BackColor="Wheat" ID="Panl1" runat="server" CssClass="Popup" style="display: none">
            <table>
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
                        <asp:TextBox ID="clientName"  ValidationGroup="Group1" onkeyup="validateNailsData(); return false;" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Введите имя" ControlToValidate="clientName" ValidationGroup="Group1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Text="Телефон" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="phone" onkeyup="validateNailsData(); return false;" runat="server" ValidationGroup="MKE" />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server"
                            TargetControlID="phone"
                            Mask="(999)999-9999"
                            ClearMaskOnLostFocus="false"
                            MessageValidatorTip="true"
                            MaskType="None"
                            InputDirection="LeftToRight"
                            AcceptNegative="Left"
                            DisplayMoney="Left" Filtered="-"
                            ErrorTooltipEnabled="True" />
                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" Display="Dynamic" runat="server" ForeColor="Red"
                            ControlExtender="MaskedEditExtender2"
                            ControlToValidate="phone"
                            IsValidEmpty="False" ValidationExpression="\([0-9]{3}\)[0-9]{3}\-[0-9]{4}"
                            EmptyValueMessage="Введите телефон"
                            InvalidValueMessage="Введите телефон"
                            EmptyValueBlurredText="Введите телефон"
                            InvalidValueBlurredMessage="Введите телефон"
                            ValidationGroup="MKE" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: justify" colspan="2">
                        <main:TagButton ID="OkButton" Enabled="false" runat="server" Text="Отправить" OnClick="AddNailDate"/>
                        <asp:Button ID="CancelButton" runat="server" Text="Закрыть" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
