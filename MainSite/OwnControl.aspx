<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OwnControl.aspx.cs" Inherits="MainSite.OwnControl" UICulture="ru" Culture="ru-RU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    <link rel="stylesheet" type="text/css" href="SiteMenu.css" />
    <link rel="stylesheet" type="text/css" href="Styles/ScheduleTable.css" />
    <style>
        .hasNote{            
            border-style: solid;
            border-width: 15px 15px 0 0;
            border-color: #ceff00 transparent transparent transparent;
            width: 0px;
            height: 0px;            
            position: absolute;
            transform:translateY(-5px)
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

    </script>
</head>
<body>
    <ul>
        <li><a class="active" href="OwnControl.aspx">Расписание</a></li>
        <li><a href="EditServices.aspx">Редактирование услуг</a></li>
        <li><a href="Pages/Report.aspx">Статистика</a></li>
        <li><a href="Pages/News.aspx">Новости</a></li>
    </ul>
    <form id="ownform" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:Button runat="server" Style="display: none" ID="b2" OnClick="OnUpdateNialDateClick" />
        <asp:Table runat="server" CellSpacing="0" CellPadding="0" EnableTheming="True">
            <asp:TableRow>
                <asp:TableCell  VerticalAlign="Top">
                    <asp:Panel DefaultButton="b2" runat="server" ID="mainPanel">                        
                        <asp:Table runat="server" style="width: 100%">
                            <asp:TableRow>
                                <asp:TableCell BackColor="Red"><asp:Button OnClick="OnPrevMothClick" Style="width:100%" runat="server" Text="<< месяц"/></asp:TableCell>
                                <asp:TableCell BackColor="Gray"><asp:Button OnClick="OnPrevWeekClick" Style="width:100%" runat="server" Text="< неделя"/></asp:TableCell>
                                <asp:TableCell BackColor="Blue"><asp:Button OnClick="OnNextWeekClick" Style="width:100%" runat="server" Text="неделя >"/></asp:TableCell>
                                <asp:TableCell BackColor="Violet"><asp:Button OnClick="OnNextMonthClick" Style="width:100%" runat="server" Text="месяц >"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Table Style="display:none" ID="noteTable" runat="server" >
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="noteTitle">Заметка</asp:Label><br/>
                                <asp:TextBox ID="note" Style="width:100%" runat="server" TextMode="MultiLine" /><br/>
                                <asp:Button runat="server" Text="Сохранить заметку" OnClick="SaveNote" />
                                <asp:Button runat="server" Text="Удалить заметку" OnClick="OnDeleteNote_Click" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Table ID="detailDataTable" Visible="false" runat="server">
                        <asp:TableRow>
                            <asp:TableCell> Дата: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Literal runat="server" ID="seletedDate" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Calendar runat="server" ID="dateCalendar" SelectionMode="Day" OnSelectionChanged="DateSelectionChanged" ShowGridLines="False" />
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Top">
                                <asp:ListBox AutoPostBack="True" runat="server" OnSelectedIndexChanged="availableTimes_SelectedIndexChanged" ID="availableTimes" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell> Имя </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:TextBox runat="server" AutoPostBack="false" ID="clientName" />
                            </asp:TableCell>
                            <asp:TableCell> </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell> Телефон </asp:TableCell>
                            <asp:TableCell ColumnSpan="2">
                                <asp:TextBox runat="server" AutoPostBack="false" ID="clientPhone" />
                            </asp:TableCell>
                            <asp:TableCell> </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="servicesRow">
                            <asp:TableCell ColumnSpan="2"> Выбранные услуги </asp:TableCell>
                            <asp:TableCell> </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2">
                                <asp:Button runat="server" ID="myB" OnClick="OnUpdateNialDateClick" Text="обновить запись" />
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
