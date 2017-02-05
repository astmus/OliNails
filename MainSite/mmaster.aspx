<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mmaster.aspx.cs" Inherits="MainSite.mmaster" UICulture="ru" Culture="ru-RU" %>

<%@ Register assembly="MainSite" namespace="MainSite.Controls" tagprefix="controls" %>

<!DOCTYPE html>
<meta name="viewport" content="width=device-width">
<meta name="MobileOptimized" content="width" />
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
        <controls:NailDateCalendar Style="width:100%; height:100vw" ID="scheduler" runat="server" Font-Size="16" FirstDayOfWeek="Monday" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" Font-Names="Arial">
            <NextPrevStyle BackColor="#646464" />                
            <DayHeaderStyle Font-Size="18" Height="30" BackColor="#333333" BorderColor="#111111" Font-Bold="True" />                
            <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
            <SelectedDayStyle BorderColor="#bd82fa" BorderWidth="5" Font-Bold="True" ForeColor="White" />                
            <TitleStyle Font-Size="24" BackColor="#333333" Height="40" Font-Bold="True" BorderColor="#111111" />
            <TodayDayStyle BackColor="#4CAF50" ForeColor="White" BorderColor="#111111" />
            <WeekendDayStyle BackColor="#404040" BorderColor="#111111" />
            <DayStyle BorderColor="#111111" />
        </controls:NailDateCalendar>       
    </div>
    </form>
</body>
</html>
