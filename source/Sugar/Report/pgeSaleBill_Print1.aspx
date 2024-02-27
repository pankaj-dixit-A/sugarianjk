<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pgeSaleBill_Print1.aspx.cs" Inherits="Sugar_Report_pgeSaleBill_Print1" %>

<%@ Register Assembly="CrystalDecisions.Web,Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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

        function Print1() {
            debugger;
            var dvReport = document.getElementById("Div1");
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
    
</head>  
<body>
    <form id="form1" runat="server">
        <div id="printReady">
            <asp:Button ID="btnprintdialog" Text="Print" runat="server" OnClientClick="return Print()" />
            <asp:Button ID="btnPDF" runat="server" Text="Open PDF" Width="80px" OnClientClick="return CheckEmail();"
                OnClick="btnPDF_Click" />
            <asp:Button ID="btnMail" runat="server" Text="Mail PDF" Width="80px" OnClientClick="return CheckEmail();"
                OnClick="btnMail_Click" />
            <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
            
             <br />
            <br /> 
     <asp:Button runat="server" ID="btnWhatsApp" Text="Sale Bill To" CssClass="btnHelp" Height="24px"
                    Width="80px" OnClick="btnWhatsApp_Click" /> 
             <asp:TextBox runat="server" ID="txtWhatsapp" Width="150px" placeholder="Enter MobNo" OnTextChanged="txtWhatsapp_TextChanged"  AutoPostBack="True" ></asp:TextBox>
                  <asp:Button runat="server" ID="btnTransportWhatsApp" Text="Transport" CssClass="btnHelp" Height="24px"
                    Width="80px" OnClick="btnTransportWhatsApp_Click" /> 
             <asp:TextBox runat="server" ID="txtTransportWhatsapp" Width="150px" placeholder="Enter MobNo"></asp:TextBox>
               <asp:Button runat="server" ID="btnDriverWhatsapp" Text="Driver" CssClass="btnHelp" Height="24px"
                    Width="80px" OnClick="btnDriverWhatsapp_Click" /> 
             <asp:TextBox runat="server" ID="txtDriverWhatsapp" Width="150px" placeholder="Enter MobNo"></asp:TextBox>
     <asp:Button runat="server" ID="btnrefWhatsApp" Text="Refer By" CssClass="btnHelp" Height="24px"
                    Width="80px" OnClick="btnrefWhatsApp_Click" />    
             <asp:TextBox runat="server" ID="txtrefWhatsapp" Width="150px" placeholder="Enter MobNo"></asp:TextBox>
          
                  <asp:Button runat="server" ID="btnShippedWhatsApp" Text="Shipped To" CssClass="btnHelp" Height="24px"
                    Width="140px" OnClick="btnShippedWhatsApp_Click" />
             <asp:TextBox runat="server" ID="txtShippedWhatsapp" Width="150px" placeholder="Enter MobNo"></asp:TextBox>
        
            <CR:CrystalReportViewer ID="crySaleBill_Print1" runat="server" AutoDataBind="false" />
        </div>

    </form>
</body>
</html>
