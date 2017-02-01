<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mmaster.aspx.cs" Inherits="MainSite.mmaster" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        a{
            text-decoration:none;
            display:block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Calendar Style="width: 100%; height: 100vw;" ID="Calendar1" FirstDayOfWeek="Monday" runat="server" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" Font-Size="40" Font-Names="Arial">
                <DayHeaderStyle BackColor="#333333" BorderColor="#111111" Font-Bold="True" />                
                <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
                <SelectedDayStyle BorderColor="#111111" BackColor="#bd82fa" Font-Bold="True" ForeColor="White" />                
                <TitleStyle BackColor="#333333" Font-Bold="True" BorderColor="#111111" />
                <TodayDayStyle BackColor="#4CAF50" ForeColor="White" BorderColor="#111111" />
                <WeekendDayStyle BackColor="#404040" BorderColor="#111111" />
                <DayStyle BorderColor="#111111" />
        </asp:Calendar>
    </div>
    </form>
</body>
</html>
