<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="master.aspx.cs" Inherits="MainSite.master" UICulture="ru" Culture="ru-RU" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="MainSite" Namespace="MainSite" TagPrefix="main" %>
<%@ Register TagPrefix="MainSite" TagName="SelectServices" Src="~/SelectServicesSheet.ascx" %>
<!DOCTYPE html>
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">

    function selectRow(row) {
        console.log("selectRow runned")
        var input = row.cells[0].children[0]
        row.cells[0].children[0].checked = !row.cells[0].children[0].checked
        price = parseInt(row.cells[1].children[0].innerHTML)
        var priceLabel = document.getElementById('totalPrice');
        var newPrice;
        if (input.checked) {
            row.className = "selectedrow"
            newPrice = price + parseInt(priceLabel.innerHTML)
        }
        else {
            row.className = "rows"
            newPrice = parseInt(priceLabel.innerHTML) - price
        }
        priceLabel.innerHTML = newPrice
        document.getElementById('OkButton').disabled = newPrice == 0
    }

    function applyHandlers() {
        var modal = document.getElementById('Panl1');
        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    }

    function showModal(event) {
       <%-- var label = document.getElementById("nailDateLabel");        
        label.innerHTML = event.target.getAttribute('time');

        document.getElementById("<%=hiddenField.ClientID%>").value = label.innerHTML;--%>

        var modal = document.getElementById('Panl1');
        modal.style.display = "block";
        <%--<%=clientName.ClientID%>.focus();--%>
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Расписание</title>
    <link rel="stylesheet" type="text/css" href="Modal.css" />
    <link rel="stylesheet" type="text/css" href="TableStyle.css" />
</head>
<body onload="applyHandlers();">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hiddenField" runat="server" />
        <!-- The Modal -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div style="width: 100%; text-align: center">
            <asp:Panel Style="display: inline-block" runat="server" ID="mainPanel">
            </asp:Panel>
        </div>
        <div runat="server" id="Panl1" class="modal">
            <MainSite:SelectServices ID="srvTable" runat="server" class="modal-content" />
        </div>
    </form>
</body>
</html>
