<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="MainSite.Pages.Report" UICulture="ru" Culture="ru-RU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Статистика</title>
    <link rel="stylesheet" type="text/css" href="../TableStyle.css" />
    <link rel="stylesheet" type="text/css" href="../SiteMenu.css" />
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
</head>
<body>
    <ul>
        <li><a href="../OwnControl.aspx">Расписание</a></li>
        <li><a href="../EditServices.aspx">Редактирование услуг</a></li>
        <li><a class="active" href="Report.aspx">Статистика</a></li>
        <li><a href="News.aspx">Новости</a></li>
        <li><a href="Materials.aspx">Материалы</a></li>
    </ul>
    <form id="form1" runat="server">
        <div>
            <asp:Calendar FirstDayOfWeek="Monday" Style="display: inline-block" ID="dateFrom" runat="server" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" CellPadding="5" Font-Names="Arial">
                <DayHeaderStyle BackColor="#333333" BorderColor="#111111" Font-Bold="True" />                
                <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
                <SelectedDayStyle BorderColor="#111111" BackColor="#bd82fa" Font-Bold="True" ForeColor="White" />                
                <TitleStyle BackColor="#333333" Font-Bold="True" BorderColor="#111111" />
                <TodayDayStyle BackColor="#4CAF50" ForeColor="White" BorderColor="#111111" />
                <WeekendDayStyle BackColor="#404040" BorderColor="#111111" />
                <DayStyle BorderColor="#111111" />
            </asp:Calendar>
            <asp:Calendar FirstDayOfWeek="Monday" Style="display: inline-block" ID="dateTo" runat="server" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" CellPadding="5" Font-Names="Arial">
                <DayHeaderStyle BackColor="#333333" BorderColor="#111111" Font-Bold="True" />                
                <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
                <SelectedDayStyle BorderColor="#111111" BackColor="#bd82fa" Font-Bold="True" ForeColor="White" />                
                <TitleStyle BackColor="#333333" Font-Bold="True" BorderColor="#111111" />
                <TodayDayStyle BackColor="#4CAF50" ForeColor="White" BorderColor="#111111" />
                <WeekendDayStyle BackColor="#404040" BorderColor="#111111" />
                <DayStyle BorderColor="#111111" />
            </asp:Calendar>
            <div style="background: #BBBBBB; vertical-align: top; padding-top: 5px; display: inline-block">
                <div>
                    Количество визитов:
                    <asp:Literal runat="server" Text="0" ID="countOfVisitors" />
                </div>
                <div style="padding-top: 5px">
                    <asp:Label runat="server" Text="Поиск" /><br />
                    <asp:TextBox runat="server" ID="searchParam" />&nbsp
        <asp:Button runat="server" OnClick="OnFindClick" Style="display: inline-block;" Text="найти" />
                </div>
            </div>
            <asp:GridView ID="GridView1" runat="server" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound" AutoGenerateColumns="False" ShowFooter="True" DataSourceID="NailDataSource" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                <Columns>
                    <asp:BoundField DataField="nailDateId" HeaderText="id" ReadOnly="true" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" >
                        <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                        <ItemStyle CssClass="hiddencol"></ItemStyle>
                        <FooterStyle CssClass="hiddencol"></FooterStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StartTime" HeaderText="Дата" SortExpression="StartTime" DataFormatString="{0:dd.MM.yyyy HH:mm}" ReadOnly="True" />
                    <asp:BoundField DataField="duration" HeaderText="Время" SortExpression="duration" ReadOnly="True" />
                    <asp:BoundField DataField="realDuration" HeaderText="Факт.время" SortExpression="realDuration"/>
                    <asp:BoundField DataField="ClientName" HeaderText="Имя" SortExpression="ClientName" />
                    <asp:BoundField DataField="ClientPhone" HeaderText="Телефон" SortExpression="ClientPhone" />
                    <asp:BoundField DataField="procedures" HeaderText="Процедуры" ReadOnly="True" SortExpression="procedures" />
                    <asp:BoundField DataField="price" HeaderText="Стоимость" SortExpression="price" ReadOnly="True" />
                    <asp:BoundField DataField="tips" HeaderText="Чай" SortExpression="tips" />
                    <asp:TemplateField ShowHeader="True" HeaderText="Управление">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Сохранить"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Отмена"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Изменить"></asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle CssClass="ruleRowButton" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="rows altrow" />
                <HeaderStyle CssClass="header"></HeaderStyle>
                <PagerStyle CssClass="pager"></PagerStyle>
                <RowStyle CssClass="rows"></RowStyle>
            </asp:GridView>

            <asp:SqlDataSource ID="NailDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" OnSelecting="NailDataSource_Selecting" SelectCommand="SELECT * FROM [FullNailDatesInfo] WHERE ([StartTime] &lt; @StartTime and [StartTime] &gt;= @from and [StartTime] &lt; @to) order by StartTime">
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
