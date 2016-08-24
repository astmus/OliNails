<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addNailDate.aspx.cs" Inherits="MainSite.addNailDate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <table>
                <tr>
                    <td>
                        <asp:Label Text="Телефон" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="phone" runat="server" ValidationGroup="MKE" />
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
                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server"
                            ControlExtender="MaskedEditExtender2"
                            ControlToValidate="phone"
                            IsValidEmpty="False" ValidationExpression="\([0-9]{3}\)[0-9]{3}\-[0-9]{4}"
                            EmptyValueMessage="Введите телефон"
                            InvalidValueMessage="Введите телефон"
                            EmptyValueBlurredText="Введите телефон"
                            InvalidValueBlurredMessage="Введите телефон"
                            ValidationGroup="MKE" OnMaskedEditServerValidator="MaskedEditValidator2_MaskedEditServerValidator" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Text="Имя" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="clientName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="OkButton" Enabled="false" runat="server" Text="Отправить" />                       
                    </td>
                    <td>
                        <asp:Button ID="CancelButton" runat="server" Text="Закрыть" />
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
