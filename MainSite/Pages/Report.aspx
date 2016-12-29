<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="MainSite.Pages.Report" UICulture="ru" Culture="ru-RU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Статистика</title>
    <link rel="stylesheet" type="text/css" href="../TableStyle.css" />
    <link rel="stylesheet" type="text/css" href="../SiteMenu.css" />
</head>
<body>
    <ul>
        <li><a href="../OwnControl.aspx">Расписание</a></li>
        <li><a href="../EditServices.aspx">Редактирование услуг</a></li>
        <li><a class="active" href="Report.aspx">Статистика</a></li>
        <li><a href="News.aspx">Новости</a></li>
    </ul>
    <form id="form1" runat="server">
        <div>
            <asp:Calendar FirstDayOfWeek="Monday" Style="display: inline-block" ID="dateFrom" runat="server"></asp:Calendar>
            <asp:Calendar FirstDayOfWeek="Monday" Style="display: inline-block" ID="dateTo" runat="server"></asp:Calendar>
            <div style="background: #BBBBBB; vertical-align: top; padding-top: 5px; display: inline-block">
                <div>
                    Количество визитов: <asp:Literal runat="server" Text="0" ID="countOfVisitors" />
                </div>
                <div style="padding-top: 5px">
                    <asp:Label runat="server" Text="Поиск" /><br />
                    <asp:TextBox runat="server" ID="searchParam" />&nbsp
        <asp:Button runat="server" OnClick="OnFindClick" Style="display: inline-block;" Text="найти" />
                </div>
            </div>
            <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound" AutoGenerateColumns="False" ShowFooter="true" DataSourceID="NailDataSource" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                <Columns>
                    <asp:BoundField DataField="StartTime" HeaderText="Дата" SortExpression="StartTime" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                    <asp:BoundField DataField="duration" HeaderText="Время" SortExpression="duration" />
                    <asp:BoundField DataField="ClientName" HeaderText="Имя" SortExpression="ClientName" />
                    <asp:BoundField DataField="ClientPhone" HeaderText="Телефон" SortExpression="ClientPhone" />
                    <asp:BoundField DataField="procedures" HeaderText="Процедуры" ReadOnly="True" SortExpression="procedures" />
                    <asp:BoundField DataField="price" HeaderText="Стоимость" SortExpression="price" />
                    <asp:BoundField DataField="tips" HeaderText="Чай" SortExpression="tips" />
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource ID="NailDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" OnSelecting="NailDataSource_Selecting" SelectCommand="SELECT * FROM [FullNailDatesInfo2] WHERE ([StartTime] &lt; @StartTime and [StartTime] &gt;= @from and [StartTime] &lt; @to) order by StartTime">
                <SelectParameters>
                    <asp:Parameter Name="StartTime" Type="DateTime" />
                    <asp:Parameter Name="from" />
                    <asp:Parameter Name="to" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
