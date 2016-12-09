<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="MainSite.Pages.News" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Статистика</title>
    <style>
        .editdelete:link {
            background: rgba(0, 0, 0, 0.00);
            color: #000000;
        }

            .editdelete:link:hover {
                background: rgba(0, 0, 0, 0.00);
                color: #000000;
            }
    </style>
    <link rel="stylesheet" type="text/css" href="../TableStyle.css" />
    <link rel="stylesheet" type="text/css" href="../SiteMenu.css" />
</head>
<body>
    <ul>
        <li><a href="../OwnControl.aspx">Расписание</a></li>
        <li><a href="../EditServices.aspx">Редактирование услуг</a></li>
        <li><a href="Report.aspx">Статистика</a></li>
        <li><a class="active" href="News.aspx">Новости</a></li>
    </ul>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" Style="width: 100%" AutoGenerateColumns="False" ShowFooter="True" DataSourceID="NailDataSource" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="id">
                <Columns>
                    <asp:BoundField DataField="id" HeaderStyle-Width="20px" HeaderText="id" ReadOnly="True">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="сообщение">
                        <EditItemTemplate>
                            <asp:TextBox ID="textBox1" TextMode="MultiLine" runat="server" Text='<%# Bind("message") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("message") %>'></asp:Label>
                        </ItemTemplate>
                        <ControlStyle Width="100%" />
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:TextBox ID="newMessage" Style="width: 100%" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="управление" ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle CssClass="editdelete" />
                        <ItemStyle Width="130px" />
                        <FooterTemplate>
                            <asp:Button runat="server" Text="Добавить" CommandName="Insert" ID="Add" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle CssClass="header"></HeaderStyle>
                <PagerStyle CssClass="pager"></PagerStyle>
                <RowStyle CssClass="rows"></RowStyle>
            </asp:GridView>
            <asp:SqlDataSource ID="NailDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" SelectCommand="SELECT * FROM [News]" UpdateCommand="UPDATE News SET message = @message WHERE (id = @id)" DeleteCommand="DELETE FROM News WHERE (id = @id)">
                <UpdateParameters>
                    <asp:Parameter Name="message" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
