<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSercvices.aspx.cs" Inherits="MainSite.SelectSercvices" UICulture="ru" Culture="ru-RU" %>

<%@ Register TagPrefix="control" TagName="services" Src="~/SelectServicesSheet.ascx" %>
<!DOCTYPE html>
<meta name="viewport" content="width=device-width, initial-scale=1">
<html xmlns="http://www.w3.org/1999/xhtml">    
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="TableStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <control:services ID="services" runat="server" />
    </form>
</body>
</html>
