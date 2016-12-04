<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditServices.aspx.cs" Inherits="MainSite.EditServices" UICulture="ru" Culture="ru-RU"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Редактирование услуг</title>  
    <link rel="stylesheet" type="text/css" href="SiteMenu.css" />
    <link rel="stylesheet" type="text/css" href="TableStyle.css" />
</head>
<body>
    <ul>
        <li><a href="OwnControl.aspx">Расписание</a></li>
        <li><a class="active" href="EditServices.aspx">Редактирование услуг</a></li>
        <li><a href="Pages/Report.aspx">Статистика</a></li>
        <li><a href="#about">About</a></li>
    </ul>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />        
        <div>
            <asp:GridView DataKeyNames="id" ID="GridView1" runat="server" ShowFooter="True" OnRowUpdated="GridView1_RowUpdated" AutoGenerateColumns="False" DataSourceID="NailDataSource" CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowSorting="True" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="id" SortExpression="id">
                        <EditItemTemplate>
                            <asp:Label ID="TextBox5" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="название" SortExpression="name">
                        <EditItemTemplate>
                            <asp:TextBox ID="newName" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="addName" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="цена" SortExpression="price">
                        <EditItemTemplate><%-- (Eval("sinceDate") as DateTime?)?.ToString("dd.MM.yyyy")--%>
                            <asp:Label ID="Label2" runat="server"   Text='<%# Eval("price") %>'></asp:Label> изменится на
                            <asp:TextBox ID="priceFrom" Width="30" runat="server" Text='<%# Bind("newPrice") %>'></asp:TextBox><br />
                             с <asp:TextBox Width="70" Text='<%#  Bind("sinceDate", "c {0:dd.MM.yyyy}") %>' ID="sinceDate" runat="server" /><asp:Button runat="server" ID="selDateBut" Text="выбрать" />
                            <ajaxToolkit:CalendarExtender ID="TxtDate_CalendarExtender" runat="server" 
                                      Enabled="True" PopupButtonID="selDateBut" 
                                      TargetControlID="sinceDate" Format="dd.MM.yyyy">
                            </ajaxToolkit:CalendarExtender>                            
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# String.Format("{0:c0}",Eval("price")) %>'></asp:Label>&nbsp;                            
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("sinceDate", "c {0:dd.MM.yyyy}") %>' Visible='<%# Eval("newPrice") is Int16 %>'></asp:Label>
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("newPrice", "{0:c0}") %>' Visible='<%# Eval("newPrice") is Int16 %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="addPrice"  Width="40" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="время" SortExpression="duration">
                        <EditItemTemplate>
                            <asp:TextBox ID="newDuration" Width="40" runat="server" Text='<%# Bind("duration") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server"   Text='<%# Eval("duration") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox  ID="addDuration" Width="40"  runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="сокращение" SortExpression="abbreviation">
                        <EditItemTemplate>
                            <asp:TextBox ID="newAbbreviation" Width="40" runat="server" Text='<%# Bind("abbreviation") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("abbreviation") %>'></asp:Label>
                        </ItemTemplate>   
                        <FooterTemplate>
                            <asp:TextBox ID="addAbbreviation" Width="40" runat="server"></asp:TextBox>
                        </FooterTemplate> 
                        <FooterStyle HorizontalAlign="Center" />                    
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="от даты" SortExpression="abbreviation">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("sinceDate") %>'></asp:TextBox>
                            <asp:Button Text="выбрать" ID="selButton" runat="server" />
                            <ajaxToolkit:CalendarExtender Format="dd.MM.yyyy" ID="cldExtTermin" runat="server" PopupButtonID="selButton" TargetControlID="TextBox13" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" BackColor="Transparent" Text='<%# Bind("sinceDate", "{0:dd.MM.yyyy}") %>'></asp:Label>
                        </ItemTemplate>   
                        <FooterTemplate>                            
                            <asp:TextBox ID="addSinceDate" BackColor="Transparent" Width="70" Text='<%# Bind("sinceDate", "{0:dd.MM.yyyy}") %>' runat="server"></asp:TextBox>                            
                        </FooterTemplate> 
                        <FooterStyle HorizontalAlign="Center" />                    
                    </asp:TemplateField>--%>
                    <asp:TemplateField ShowHeader="True" HeaderText="управление">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Сохр."></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Отмен."></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Изм."></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Удал."></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button runat="server" Text="Добавить" CommandName="Insert" ID="Add" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ControlStyle CssClass="ruleRowButton" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="header"></HeaderStyle>
                <PagerStyle CssClass="pager"></PagerStyle>
                <RowStyle CssClass="rows"></RowStyle>
            </asp:GridView>
            <asp:SqlDataSource ID="NailDataSource" runat="server" DeleteCommand="update Services set isObsolete = 1 where id = @id" OnUpdating="NailDataSource_Updating"  OnUpdated="NailDataSource_Updated" OnSelecting="NailDataSource_Selecting" ConnectionString="<%$ ConnectionStrings:dbConnectionSctring %>" ProviderName="<%$ ConnectionStrings:dbConnectionSctring.ProviderName %>" SelectCommand="GetServicesForEdit" InsertCommand="INSERT INTO Services(name, price, duration, abbreviation) VALUES (@name, @price, @duration, @abbreviation)" SelectCommandType="StoredProcedure" UpdateCommand="UpdateServiceProc" UpdateCommandType="StoredProcedure">
                <DeleteParameters>
                    <asp:Parameter Name="id" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="name" />
                    <asp:Parameter Name="price" DefaultValue="0" />
                    <asp:Parameter Name="duration"  DefaultValue="0" />
                    <asp:Parameter Name="abbreviation" />
                </InsertParameters>                               
                <SelectParameters>                    
                    <asp:Parameter Name="localTime" Type="DateTime" />                    
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    <asp:ControlParameter ControlID="GridView1" Name="id" PropertyName="SelectedValue" Type="Int16" />
                    <asp:ControlParameter ControlID="GridView1" Name="name" PropertyName="SelectedValue" Type="String" Size="35"/>
                    <asp:ControlParameter ControlID="GridView1" Name="duration" PropertyName="SelectedValue" Type="Int16" />
                    <asp:ControlParameter ControlID="GridView1" Name="abbreviation" PropertyName="SelectedValue" Type="String" Size="3" />
                    <asp:ControlParameter ControlID="GridView1" Name="price" DefaultValue="0" PropertyName="SelectedValue" Type="Int16" />
                    <asp:ControlParameter ControlID="GridView1" Name="sinceDate" PropertyName="SelectedValue" Type="DateTime" />
                    <asp:Parameter Direction="InputOutput" Name="result" Type="String" Size="30"/>
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
