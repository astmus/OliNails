<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="Materials.aspx.cs" Inherits="MainSite.Pages.Materials" UICulture="ru" Culture="ru-RU" %>

<%@ Register TagPrefix="MainSite" TagName="SelectServices" Src="~/SelectServicesSheet.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Материалы</title>
    <link rel="stylesheet" type="text/css" href="../TableStyle.css" />
    <link rel="stylesheet" type="text/css" href="../SiteMenu.css" />
    <style>
        .hidden {
            display: none;
        }
    </style>
    <script>
        function selectRow(row, e) {
            var input = row.cells[1].children[0]
            if (e.srcElement.parentNode == row)
                input.checked = !input.checked
            //row.className == "selectedrow" ? false : true;
            row.className = input.checked ? "selectedrow" : "rows"
            e.stopPropagation();
        }
    </script>
</head>
<body>
    <ul>
        <li><a href="../OwnControl.aspx">Расписание</a></li>
        <li><a href="../EditServices.aspx">Редактирование услуг</a></li>
        <li><a href="Report.aspx">Статистика</a></li>
        <li><a href="News.aspx">Новости</a></li>
        <li><a class="active" href="Materials.aspx">Материалы</a></li>
    </ul>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" /> 
        <div>
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <asp:GridView ID="materialTable" ShowFooter="true" OnRowCreated="materialTable_RowCreated" OnRowCommand="materialTable_RowCommand" OnRowEditing="materialTable_RowEditing" OnRowUpdating="materialTable_RowUpdating" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="materialDataSource" SelectedRowStyle-CssClass="selectedrow" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                            <Columns>
                                <asp:TemplateField HeaderText="имя" SortExpression="name">
                                    <FooterTemplate>
                                        <asp:TextBox style="width:auto" ID="nameBox" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="дата" SortExpression="startTime">
                                    <EditItemTemplate>
                                        <asp:TextBox Width="70" ID="sinceTimeBox" runat="server" Text='<%# Bind("startTime", "{0:dd.MM.yyyy}") %>'></asp:TextBox>
                                        <asp:Button runat="server" ID="selDateBut" Text="выбрать" />
                                        <ajaxToolkit:CalendarExtender ID="TxtDate_CalendarExtender" runat="server"
                                            Enabled="True" PopupButtonID="selDateBut"
                                            TargetControlID="sinceTimeBox" Format="dd.MM.yyyy"/>s
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="sinceTimeBox"  Width="70" runat="server" Text='<%# Bind("startTime") %>'></asp:TextBox>
                                        <asp:Button runat="server" ID="selDateBut" Text="выбрать" />
                                        <ajaxToolkit:CalendarExtender ID="calendarExtender" runat="server"
                                            Enabled="True" PopupButtonID="selDateBut"
                                            TargetControlID="sinceTimeBox" Format="dd.MM.yyyy"/>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("startTime", "{0:dd.MM.yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  HeaderText="цена" SortExpression="price">                                    
                                    <EditItemTemplate>
                                        <asp:TextBox ID="priceTextBox" Width="50" AutoPostBack="false" runat="server" Text='<%# Bind("price") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="priceLabel" runat="server" Text='<%# Bind("price") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="priceTextBox" Width="50" runat="server" Text='<%# Bind("price") %>' ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ед." SortExpression="amount">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="amountTextBox" Width="50" runat="server" Text='<%# Bind("amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="amountLabel" runat="server" Text='<%# Bind("amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="amountTextBox" Width="50" Text='<%# Bind("amount") %>' runat="server"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="услуги" ItemStyle-HorizontalAlign="Center" SortExpression="procedures">
                                    <ItemTemplate>
                                        <asp:Label ID="proceduresLabel" runat="server" Text='<%# Eval("procedures") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button runat="server" Text="Добавить" CommandName="Insert" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="True" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                                    <FooterStyle CssClass="hidden"></FooterStyle>
                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                    <ItemStyle CssClass="hidden"></ItemStyle>
                                    <ControlStyle CssClass="ruleRowButton"></ControlStyle>
                                    <FooterStyle CssClass="hidden"></FooterStyle>
                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                    <ItemStyle CssClass="hidden"></ItemStyle>
                                </asp:CommandField>
                                <asp:CommandField HeaderText="управление" ShowEditButton="True" EditText="изм." UpdateText="принять" CancelText="отмена" ControlStyle-CssClass="ruleRowButton" NewText="начать новый" ShowInsertButton="True">
                                    <ControlStyle CssClass="ruleRowButton"></ControlStyle>
                                </asp:CommandField>
                            </Columns>
                            <AlternatingRowStyle CssClass="rows altrow" />
                            <HeaderStyle CssClass="header"></HeaderStyle>
                            <PagerStyle CssClass="pager"></PagerStyle>
                            <RowStyle CssClass="rows"></RowStyle>
                            <SelectedRowStyle CssClass="selectedrow"></SelectedRowStyle>
                        </asp:GridView>
                        <asp:SqlDataSource ID="materialDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" SelectCommand="SELECT * FROM [MaterialsWithRelatedProcedures] WHERE expiredTime IS NULL order by startTime desc" UpdateCommand="UPDATE Materials SET startTime = @startTime, price = @price, amount = @amount WHERE (id = @ID)" InsertCommand="INSERT INTO Materials(name, startTime, price, amount) VALUES (@name,@startTime,@price,@amount)" OnInserting="materialDataSource_Inserting">
                            <UpdateParameters>
                                <asp:Parameter Name="price" Type="Int16" />
                                <asp:Parameter Name="amount" Type="Int16" />
                                <asp:Parameter Name="startTime" Type="DateTime" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="name" Type="String" />
                                <asp:Parameter Name="price" Type="Int16" />
                                <asp:Parameter Name="amount" Type="Int16" />
                                <asp:Parameter Name="startTime" Type="DateTime" />
                            </InsertParameters>
                        </asp:SqlDataSource>
                    </td>
                    <td style="vertical-align: top">
                        <asp:GridView Style="width: 100%" ID="useMaterialTable" runat="server" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AutoGenerateColumns="False" DataKeyNames="materialId" DataSourceID="useMaterialDataSource">
                            <Columns>
                                <asp:BoundField DataField="name" ItemStyle-HorizontalAlign="Left" HeaderText="услуга" SortExpression="name" />
                                <asp:BoundField DataField="useCount" HeaderText="исп-ий" SortExpression="useCount" />
                                <asp:BoundField DataField="totalPrice" HeaderText="стоимость" SortExpression="totalPrice" />
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <PagerStyle CssClass="pager" />
                            <RowStyle CssClass="rows" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="useMaterialDataSource" CacheKeyDependency="materialsCacheKey" EnableCaching="true" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" SelectCommand="select materialId, serviceId = max(serviceId) , name = ISNULL(name,N'суммарно'), useCount = SUM(useCount), totalPrice = sum(totalPrice) from MaterialUsedCount where ([materialId] = @materialId) group by rollup(name),materialId">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="materialTable" Name="materialId" PropertyName="SelectedValue" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView Style="width: 100%" Visible="false" ID="selectServicesTable" OnRowDataBound="table_RowDataBound" runat="server" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="servicesDataSource">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:TemplateField HeaderText="привязанные услуги" SortExpression="name">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectedServiceBox" AutoPostBack="false" runat="server" Text='<%# Bind("name") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="pos" HeaderText="pos" SortExpression="pos" Visible="False" />
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <PagerStyle CssClass="pager" />
                            <RowStyle CssClass="rows" />
                        </asp:GridView>
                        <asp:SqlDataSource EnableCaching="true" ID="servicesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" SelectCommand="SELECT [id], [name], [pos] FROM [Services] WHERE ([isObsolete] = @isObsolete) ORDER BY [pos]">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="false" Name="isObsolete" Type="Boolean" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
