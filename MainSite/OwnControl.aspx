<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OwnControl.aspx.cs" Inherits="MainSite.OwnControl"  UICulture="ru" Culture="ru-RU" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="ownform" runat="server">
        <asp:Button runat="server" style="display: none" ID="b2" OnClick="OnUpdateNialDateClick"/>
        <asp:Table runat="server" EnableTheming="True">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Panel DefaultButton="b2" runat="server" ID="mainPanel">
            
                    </asp:Panel>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Table ID="detailDataTable" Visible="false" runat="server" >
                        <asp:TableRow>
                            <asp:TableCell> Дата: </asp:TableCell>
                            <asp:TableCell> <asp:Literal runat="server" ID="seletedDate"/> </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>                            
                            <asp:TableCell ColumnSpan="2"> <asp:Calendar runat="server" ID="dateCalendar" SelectionMode="Day" OnSelectionChanged="DateSelectionChanged" ShowGridLines="False"/>
                            </asp:TableCell>  
                            <asp:TableCell VerticalAlign="Top"><asp:ListBox AutoPostBack="True" runat="server" OnSelectedIndexChanged="availableTimes_SelectedIndexChanged" ID="availableTimes"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell> Имя </asp:TableCell>
                            <asp:TableCell ColumnSpan="2"> <asp:TextBox runat="server" AutoPostBack="false" ID="clientName" /> </asp:TableCell>                            
                            <asp:TableCell> </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell> Телефон </asp:TableCell>
                            <asp:TableCell ColumnSpan="2"> <asp:TextBox runat="server" AutoPostBack="false" ID="clientPhone" /> </asp:TableCell>                            
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
