<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDeliveryOrderForGST.aspx.cs" Inherits="Sugar_Report_rptDeliveryOrderForGST" %>

<%@ Register Assembly="CrystalDecisions.Web,Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Sugar/Scripts/whatsapp-api.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.2.61/jspdf.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    
    <script type="text/javascript">
        function sendPdfToWatsapp(bill_no, content, fileName, instanceid, accesstoken, message, mobile, authKey, mobtxt) {
            debugger;
            // var Opening_Bal =
            var string = mobtxt;
            // var string = document.getElementById("")
            //var string =  $("#<%=txtWhatsapp.ClientID %>").val() == "" ? 0 : $("#<%=txtWhatsapp.ClientID %>").val();
                 var stringArray = (new Function("return [" + string + "];")());
                 var strcountarry = stringArray.length;

                 for (var i = 0; i < strcountarry; i++) {
                     var filenamenew = stringArray[i] + fileName;
                     var authKey = '<%= Session["gitauthKey"] %>';
                 var authToken = '<%= Session["gitAuthToken"] %>';
                 var repo = '<%= Session["gitRepo"] %>';
                 whatsappApi(bill_no, content, filenamenew, instanceid, accesstoken, message, stringArray[i], authKey, authToken, repo);
             }

         }
        function Print() {
            debugger;
            var dvReport = document.getElementById("printReady");
            for (var i = 0; i < 8; i++) {
                if (i % 2 == 0) {
                    var frame1 = printReady.getElementsByTagName("iframe")[i];
                    if (navigator.appName.indexOf("Chrome") != -1) {
                        frame1.name = frame1.id;
                        window.frames[frame1.id].focus();
                        window.frames[frame1.id].print();
                    }
                    else {
                        var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                        frameDoc.print();
                    }
                }
            }

        }

    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Bill To Ship To same")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

    </script>
 </head>  
<body> 
    <form id="form1" runat="server"> 
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <div id="printReady">
                <asp:Button ID="btnprintdialog" Text="Print" runat="server" OnClientClick="return Print()" />
                <asp:Button ID="btnPDF" runat="server" Text="Open PDF" Width="80px" OnClientClick="return CheckEmail();"
                    OnClick="btnPDF_Click" />
                <asp:Button ID="btnMail" runat="server" Text="Mail PDF" Width="80px" OnClientClick="return CheckEmail();"
                    OnClick="btnMail_Click" />
                <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>

                <asp:Button ID="btntenderDo" runat="server" Text="Mail PDF" Width="80px" OnClientClick="return CheckEmail();"
                    OnClick="btntenderDo_Click" />
                <asp:TextBox runat="server" ID="txttenderdomail" Width="300px"></asp:TextBox>
                <asp:Button runat="server" ID="btnWhatsApp" Text="WhatsApp" CssClass="btnHelp" Height="24px"
                    Width="80px" OnClick="btnWhatsApp_Click" />
                <asp:TextBox runat="server" ID="txtWhatsapp" Width="150px" placeholder="Enter MobNo"></asp:TextBox>

                <CR:CrystalReportViewer ID="cryDeliveryOrderForGST" runat="server" AutoDataBind="false" />
       </div>
</form>
</body>

</html>