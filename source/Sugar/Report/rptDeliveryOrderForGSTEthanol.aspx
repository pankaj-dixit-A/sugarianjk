<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDeliveryOrderForGSTEthanol.aspx.cs" Inherits="Sugar_Report_rptDeliveryOrderForGSTEthanol" MasterPageFile="~/MasterPage3.master"%>


<%@ MasterType VirtualPath="~/MasterPage3.master" %>
<%@ Register Assembly="CrystalDecisions.Web,Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.2.61/jspdf.min.js"></script>
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <title></title>
    <script type="text/javascript">
        function sendPdfToWatsapp(content, fileName, instanceid, accesstoken, message, mobile, authKey) {
            whatsappApi(content, fileName, instanceid, accesstoken, message, mobile, authKey);
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
