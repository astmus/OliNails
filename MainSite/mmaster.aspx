<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mmaster.aspx.cs" Inherits="MainSite.mmaster" UICulture="ru" Culture="ru-RU" %>

<%@ Register Assembly="MainSite" Namespace="MainSite.Controls" TagPrefix="controls" %>

<!DOCTYPE html>
<meta name="viewport" content="width=device-width">
<meta name="MobileOptimized" content="width" />
<html xmlns="http://www.w3.org/1999/xhtml">
<script>
    function addNewNailDate(totalMinutes) {
        PageMethods.AddNewNailDate(totalMinutes, onSuccess, onError);
    }

    function onSuccess(result) {
        window.location.href = "SelectSercvices.aspx";
    }

    function onError(result) {        
    }
</script>
<script src="scripts/AlterDatePrompt.js"></script>
<head runat="server">
    <title></title>
    <style>
        a {
            text-decoration: none;
            display: block;
            margin-top: 30%;
            height: 70%;
        }

        input {            
            height: 55px;
            width: 100%;
            border: 1px solid;            
            margin-top: 3px;            
            text-decoration: none;
            color: white;
            border-color:#111111;
        }

            input.reserved {
                background-color: #bd82fa;
            }

            input.notreserved {
                background-color: #646464;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server" EnablePageMethods="true" />
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <controls:NailDateCalendar OnSelectionChanged="scheduler_SelectionChanged" Style="width: 100%; height: 100vw" ID="scheduler" runat="server" Font-Size="16" FirstDayOfWeek="Monday" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" Font-Names="Arial">
                        <NextPrevStyle BackColor="#646464" />
                        <DayHeaderStyle Font-Size="18" Height="30" BackColor="#333333" BorderColor="#111111" Font-Bold="True" />
                        <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
                        <SelectedDayStyle BorderColor="#8d23fb" BorderWidth="1" BackColor="Transparent" Font-Bold="True" ForeColor="White" />
                        <TitleStyle Font-Size="24" BackColor="#333333" Height="40" Font-Bold="True" BorderColor="#111111" />
                        <TodayDayStyle BorderColor="#4CAF50" BorderWidth="1" ForeColor="#4CAF50" />
                        <WeekendDayStyle BorderColor="#111111" />
                        <DayStyle BorderColor="#111111" />
                    </controls:NailDateCalendar>
                    <asp:Panel Width="100%" Visible="false" runat="server" BackColor="#333333" ID="buttonsPanel">
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
