<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OwnControl.aspx.cs" Inherits="MainSite.OwnControl" UICulture="ru" Culture="ru-RU" %>

<!DOCTYPE html>
<%@ Register TagPrefix="MainSite" TagName="SelectServices" Src="~/SelectServicesSheet.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <link rel="stylesheet" type="text/css" href="SiteMenu.css" />
    <link rel="stylesheet" type="text/css" href="Styles/ScheduleTable.css" />
    <link rel="stylesheet" type="text/css" href="TableStyle.css" />
    <style>
        .hidden {
            display: none;
        }

        .btn {
            background: #333;
            font-family: Arial;
            color: #ffffff;
            font-size: 14px;
            border: solid 0px;
            padding: 10px;
            text-decoration: none;
        }

            .btn:hover {
                background: #111;
                text-decoration: none;
            }

        .hasNote {
            border-style: solid;
            border-width: 15px 15px 0 0;
            border-color: #ceff00 transparent transparent transparent;
            width: 0px;
            height: 0px;
            position: absolute;
            transform: translateY(-5px);
        }
    </style>
    <script>
        function onSuccess(result) {
            document.getElementById('<%=noteTable.ID%>').style.display = "block";
            document.getElementById('<%=note.ID%>').value = result;
        }

        function onError(result) {
            alert(result);
        }

        function HandleIT(date) {
            PageMethods.GetNoteMessage(date, onSuccess, onError);
        }
        function selectMaterialRow(row, e) {
            var input = row.cells[1].children[0]

            if (e.srcElement.parentNode == row)
                input.checked = !input.checked
            //row.className == "selectedrow" ? false : true;
            row.className = input.checked ? "selectedrow" : "rows"
            e.stopPropagation();
        }
        function selectAll(grid) {
            var table = document.getElementById('<%= usedMaterialsTable.ClientID %>');

            for (var i = 0, row; row = table.rows[i]; i++) {
                console.log(row);
                console.log(row.cells[0].children != null);
                var check = row.cells[1].children[0];
                check.checked = true;
                row.className = "selectedrow";
            }
        }
    </script>
</head>
<body>
    <ul>
        <li><a class="active" href="OwnControl.aspx">Расписание</a></li>
        <li><a href="EditServices.aspx">Редактирование услуг</a></li>
        <li><a href="Pages/Report.aspx">Статистика</a></li>
        <li><a href="Pages/News.aspx">Новости</a></li>
        <li><a href="Pages/Materials.aspx">Материалы</a></li>
    </ul>
    <form id="ownform" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:Button runat="server" Style="display: none" ID="b2" OnClick="OnUpdateNialDateClick" />
        <asp:Table runat="server" CellSpacing="0" CellPadding="0" EnableTheming="True">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Panel DefaultButton="b2" runat="server" ID="mainPanel">
                        <asp:Table runat="server" CellPadding="0" CellSpacing="0" Style="width: 100%">
                            <asp:TableRow BorderStyle="Solid" BorderWidth="1" BorderColor="#111">
                                <asp:TableCell><asp:Button OnClick="OnPrevMothClick" CssClass="btn" Style="width:100%" runat="server" Text="<< месяц"/></asp:TableCell>
                                <asp:TableCell><asp:Button OnClick="OnPrevWeekClick" CssClass="btn" Style="width:100%" runat="server" Text="< неделя"/></asp:TableCell>
                                <asp:TableCell><asp:Button OnClick="OnNextWeekClick" CssClass="btn" Style="width:100%" runat="server" Text="неделя >"/></asp:TableCell>
                                <asp:TableCell><asp:Button OnClick="OnNextMonthClick" CssClass="btn" Style="width:100%" runat="server" Text="месяц >"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Table Style="display: none" ID="noteTable" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="noteTitle">Заметка</asp:Label><br />
                                <asp:TextBox ID="note" Style="width: 100%" runat="server" TextMode="MultiLine" /><br />
                                <asp:Button runat="server" Text="Сохранить заметку" OnClick="SaveNote" />
                                <asp:Button runat="server" Text="Удалить заметку" OnClick="OnDeleteNote_Click" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Table ID="detailDataTable" Visible="false" runat="server" CellSpacing="0" Style="display: inline">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Calendar Style="display: inline-block" runat="server" ID="dateCalendar" SelectionMode="Day" OnSelectionChanged="DateSelectionChanged" BackColor="#646464" BorderColor="#333333" ShowGridLines="true" DayNameFormat="Shortest" ForeColor="White" CellPadding="5" Font-Names="Arial">
                                    <DayHeaderStyle BackColor="#333333" BorderColor="#111111" Font-Bold="True" />
                                    <OtherMonthDayStyle BorderColor="#111111" ForeColor="#111111" />
                                    <SelectedDayStyle BorderColor="#111111" BackColor="#bd82fa" Font-Bold="True" ForeColor="White" />
                                    <TitleStyle BackColor="#333333" Font-Bold="True" BorderColor="#111111" />
                                    <TodayDayStyle BackColor="#4CAF50" ForeColor="White" BorderColor="#111111" />
                                    <WeekendDayStyle BackColor="#404040" BorderColor="#111111" />
                                    <DayStyle BorderColor="#111111" />
                                </asp:Calendar>
                                <asp:ListBox Style="vertical-align: top" AutoPostBack="True" runat="server" OnSelectedIndexChanged="availableTimes_SelectedIndexChanged" ID="availableTimes" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                Чаевые&nbsp<asp:TextBox runat="server" ID="tipsField" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <MainSite:SelectServices ID="nailDatePanel" runat="server" ConfirmButtonVisibility="false" />
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Top" Style="padding-top: 70px">
                                <asp:Table runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="2">
                                            <asp:GridView runat="server" ID="usedMaterialsTable" DataSourceID="materialsDataSource" OnRowDataBound="usedMaterialsTable_RowDataBound" Caption="Использованные материалы" ShowHeader="False" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" ItemStyle-CssClass="hidden" />
                                                    <asp:TemplateField HeaderText="selected" SortExpression="selected">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("selected") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                                                    <asp:BoundField DataField="startTime" DataFormatString="{0:dd.MM.yyyy}" HeaderText="startTime" SortExpression="startTime" />
                                                    <asp:CommandField Visible="false" ShowSelectButton="True" />
                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerStyle CssClass="pager" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="materialsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" SelectCommand="SELECT * FROM dbo.AvailableUsedMaterials(@nailDateId)">
                                                <SelectParameters>
                                                    <asp:SessionParameter DefaultValue="-1" Name="nailDateId" SessionField="selectedNailDateID" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableFooterRow>
                                        <asp:TableCell>
                                            <asp:Button runat="server" Text="Сохранить" OnClick="SaveUsedMaterialsForDate" Width="100%" />
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Button runat="server" Width="100%" Text="Выбрать все" UseSubmitBehavior="false" OnClientClick="selectAll();return false;" />
                                        </asp:TableCell>
                                    </asp:TableFooterRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Button runat="server" ID="myB" OnClick="OnUpdateNialDateClick" Text="обновить запись" />
                                <asp:Button runat="server" OnClick="HideDetailPanel" Text="скрыть" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Button runat="server" OnClick="OnDeleteNailDateClick" Text="отменить запись" />                                
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
</body>
</html>
