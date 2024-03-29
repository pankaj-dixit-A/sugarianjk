﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using System.Drawing.Printing;
//using System.Printing;
using System.Net;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using iTextSharp.tool.xml;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Data.SqlClient;


public partial class Sugar_Report_rptDeliveryOrderForGST : System.Web.UI.Page
{
    string AL1 = string.Empty;
    string AL2 = string.Empty;
    string AL3 = string.Empty;
    string AL4 = string.Empty;
    string other = string.Empty;
    string billfoter = string.Empty;
    int billno;
    int company_code;
    int year_code;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
    string ac_code;
    string utr_no;
    string AcType = string.Empty;
    string mail = string.Empty;
    string doc_no = string.Empty;
    string tenderno = string.Empty;
    string imagepath = string.Empty;
    ReportDocument rprt1 = new ReportDocument();
    ReportDocument rprt2 = new ReportDocument();
    ReportDocument rpt = new ReportDocument();
    string company_name = string.Empty;
    string bss = "";
    string paymentto = "";

    string DoType = string.Empty;
    string MillCode = string.Empty;

    string millshortname = string.Empty;
    string DONo = string.Empty;
    string Lorryno = string.Empty;
    string Qty = string.Empty;
    string grade = string.Empty;
    string buyershortname = string.Empty;
    string buyercityname = string.Empty;
    string Shiptoshortname = string.Empty;
    string Shiptocityname = string.Empty;
    string getpassname = string.Empty;
    string getpasscityname = string.Empty;
    string shiptoname = string.Empty;
    string dodocdate = string.Empty;
    string millrate = string.Empty;
    string tenderDate = string.Empty;
    string salebillcityname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tenderno = Request.QueryString["tenderno"];
            company_name = Session["Company_Name"].ToString();
            doc_no = Request.QueryString["do_no"];
            paymentto = Request.QueryString["Paymentto"];
            company_code = Convert.ToInt32(Session["Company_Code"].ToString());
            year_code = Convert.ToInt32(Session["year"].ToString());

            //DataTable dt = GetData();
            //string TotalAmount = dt.Rows[0]["Mill_AmtWO_TCS"].ToString();
            //string tdsrate = dt.Rows[0]["PurchaseTDSRate"].ToString();
            //string mill_rate = dt.Rows[0]["mill_rate"].ToString();
            //string quantal = dt.Rows[0]["quantal"].ToString();
            //double basicamt = Convert.ToDouble(mill_rate) * Convert.ToDouble(quantal);
            //double tdsamount = basicamt * Convert.ToDouble(tdsrate) / 100;
            //string amount = Convert.ToString(Convert.ToDouble(TotalAmount) - tdsamount);
            //string CompanyMail = Session["EmailId"].ToString();
            bss = Request.QueryString["bss"];

            //  string inWords = clsNoToWord.ctgword(amount);
            SqlDataAdapter da = new SqlDataAdapter();
            //    rpt.Load(Server.MapPath("cryDeliveryOrderForGST.rpt"));
            //   rpt.SetDataSource(dt);


            string tenderDate = clsCommon.getString("select Tender_DateConverted from qrytenderhead where Tender_No='" + tenderno +
                "' and Company_Code='" + Session["Company_Code"] + "'");

            string sellnoteno = clsCommon.getString("select Sell_Note_No from qrytenderhead where Tender_No='" + tenderno +
                "' and Company_Code='" + Session["Company_Code"] + "'");



            string tenderdoname = clsCommon.getString("select tenderdoname from qrytenderhead where Tender_No='" + tenderno +
                "' and Company_Code='" + Session["Company_Code"] + "' ");
            // " and Year_Code='" + Session["year"].ToString() + "'");

            DataSet ds = clsDAL.SimpleQuery("select * from qrydohead where doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
              " and Year_Code='" + Session["year"].ToString() + "'");
             
            dodocdate = ds.Tables[0].Rows[0]["doc_dateConverted"].ToString();
            millshortname = ds.Tables[0].Rows[0]["millshortname"].ToString();
            millrate = ds.Tables[0].Rows[0]["mill_rate"].ToString();
            //string narration1 = ds.Tables[0].Rows[0]["narration1"].ToString() + ds.Tables[0].Rows[0]["narration2"].ToString();
            //string narration2 = ds.Tables[0].Rows[0]["narration3"].ToString() + "(" + tenderdoname + ")";

            string narration1 = ds.Tables[0].Rows[0]["narration1"].ToString();
            string narration2 = ds.Tables[0].Rows[0]["narration3"].ToString();

            //  string dodocdate = clsCommon.getString("select doc_dateConverted from qrydohead where doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
            //" and Year_Code='" + Session["year"].ToString() + "'");
            //   select * from qrydohead where  doid in

            string utrnarration = clsCommon.getString("select Narration from qrydodetail where detail_Id=1 and doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
             " and Year_Code='" + Session["year"].ToString() + "'");

            string qry = "select * from tblvoucherheadaddress where Company_Code='" + Session["Company_Code"].ToString() + "'";
            DataSet ds1 = clsDAL.SimpleQuery(qry);
            if (ds1 != null)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    AL1 = dt1.Rows[0]["AL1"].ToString();
                    AL2 = dt1.Rows[0]["AL2"].ToString();
                    AL3 = dt1.Rows[0]["AL3"].ToString();
                    AL4 = dt1.Rows[0]["AL4"].ToString();
                    other = dt1.Rows[0]["Other"].ToString();
                    billfoter = dt1.Rows[0]["BillFooter"].ToString();
                }
            }


            //  cryDeliveryOrderForGST.ReportSource = rpt;
            string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            SqlConnection dbConnection = new SqlConnection(strcon);
            DataSet myDataSet_ForReport = new DataSet();
            SqlCommand cmd_for_sp = new SqlCommand("sputrreport", dbConnection);
            cmd_for_sp.CommandType = CommandType.StoredProcedure;
            cmd_for_sp.Parameters.Add("@docno", SqlDbType.Int).Value = doc_no;
            cmd_for_sp.Parameters.Add("@companycode", SqlDbType.Int).Value = company_code;
            cmd_for_sp.Parameters.Add("@yearcode", SqlDbType.Int).Value = year_code;


            SqlDataAdapter get_data_via_adapter = new SqlDataAdapter(cmd_for_sp);

            get_data_via_adapter.Fill(myDataSet_ForReport);

            myDataSet_ForReport.DataSetName = "dsDeliveryOrderGST.xsd";
            myDataSet_ForReport.Tables[0].TableName = "DeliveryOrderGST"; //based on datatable name in .xsd
            myDataSet_ForReport.Tables[1].TableName = "qrydodetail";

            DataTable dt = myDataSet_ForReport.Tables[0];

            string TotalAmount = dt.Rows[0]["Mill_AmtWO_TCS"].ToString();
            string tdsrate = dt.Rows[0]["PurchaseTDSRate"].ToString();
            string mill_rate = dt.Rows[0]["mill_rate"].ToString();
            string quantal = dt.Rows[0]["quantal"].ToString();

            millshortname = dt.Rows[0]["millshortname"].ToString();
            DONo = dt.Rows[0]["doc_no"].ToString();
            Lorryno = dt.Rows[0]["truck_no"].ToString();
            Qty = dt.Rows[0]["quantal"].ToString();
            grade = dt.Rows[0]["grade"].ToString();
            buyershortname = dt.Rows[0]["billtoshortname"].ToString();
            buyercityname = dt.Rows[0]["salebillcityname"].ToString();
            Shiptoshortname = dt.Rows[0]["shiptoshortname"].ToString();
            Shiptocityname = dt.Rows[0]["shiptocityname"].ToString();

            double basicamt = Convert.ToDouble(mill_rate) * Convert.ToDouble(quantal);
            double tdsamount = basicamt * Convert.ToDouble(tdsrate) / 100;
            string amount = Convert.ToString(Convert.ToDouble(TotalAmount) - tdsamount);
            string CompanyMail = Session["EmailId"].ToString();
            //string inWords = clsNoToWord.ctgword(amount);

            string inWords = clsNoToWord.ctgword(TotalAmount);
            DoType = dt.Rows[0]["desp_type"].ToString();
            #region [Whatsapp]
            getpassname = dt.Rows[0]["getpassname"].ToString();
            shiptoname = dt.Rows[0]["shiptoname"].ToString();
            #endregion
            if (!IsPostBack)
            {
                string mill = Request.QueryString["mill"];
                if (DoType == "DO")
                {
                    paymentto = mill;
                }
                txtEmail.Text = clsCommon.getString("Select Email_Id from qrymstaccountmaster where  Ac_Code='" + paymentto + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string ccmail = clsCommon.getString("Select Email_Id_cc from qrymstaccountmaster where  Ac_Code='" + paymentto + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (ccmail != "0")
                {
                    txtEmail.Text = txtEmail.Text + "," + ccmail;
                }
                string Tender_DO = clsCommon.getString("select Tender_DO from qrytenderhead where Tender_No='" + tenderno +
                    "' and Company_Code='" + Session["Company_Code"] + "' ");
                // " and Year_Code='" + Session["year"].ToString() + "'");
                txttenderdomail.Text = clsCommon.getString("Select Email_Id from qrymstaccountmaster where  Ac_Code='" + Tender_DO + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                txtWhatsapp.Text = clsCommon.getString("Select whatsup_no from qrymstaccountmaster where  Ac_Code='" + paymentto + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


            }

            rpt.Load(Server.MapPath("../Report/cryDeliveryOrderForGST.rpt"));
            rpt.DataDefinition.FormulaFields["companyname"].Text = "\"" + company_name + "\"";
            rpt.DataDefinition.FormulaFields["printhead"].Text = "\"Y\"";
            rpt.DataDefinition.FormulaFields["narr1"].Text = "\"" + narration1 + "\"";
            rpt.DataDefinition.FormulaFields["narr2"].Text = "\"" + narration2 + "\"";

            rpt.DataDefinition.FormulaFields["word"].Text = "\"" + inWords + "\"";
            rpt.DataDefinition.FormulaFields["tenderDate"].Text = "\"" + tenderDate + "\"";
            rpt.DataDefinition.FormulaFields["sellnoteno"].Text = "\"" + sellnoteno + "\"";
            rpt.DataDefinition.FormulaFields["docdate"].Text = "\"" + dodocdate + "\"";
            rpt.DataDefinition.FormulaFields["AL1"].Text = "\"" + AL1 + "\"";
            rpt.DataDefinition.FormulaFields["AL2"].Text = "\"" + AL2 + "\"";
            rpt.DataDefinition.FormulaFields["AL3"].Text = "\"" + AL3 + "\"";
            rpt.DataDefinition.FormulaFields["Al4"].Text = "\"" + AL4 + "\"";
            rpt.DataDefinition.FormulaFields["note"].Text = "\"" + billfoter + "\"";
            rpt.DataDefinition.FormulaFields["utrnarration"].Text = "\"" + utrnarration + "\"";


            rpt.DataDefinition.FormulaFields["Cmail"].Text = "\"" + CompanyMail + "\"";
            rpt.DataDefinition.FormulaFields["other"].Text = "\"" + other + "\"";
            rpt.DataDefinition.FormulaFields["bss"].Text = "\"" + bss + "\"";
            imagepath = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='L' and Company_Code='"
             + Session["Company_Code"].ToString() + "'");
            String path = Server.MapPath("") + "\\" + imagepath;


            imagepath = path.Replace("Report", "Images");

            string sign = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='S' and Company_Code='"
              + Session["Company_Code"].ToString() + "'");
            String logopath = Server.MapPath("") + "\\" + sign;


            sign = logopath.Replace("Report", "Images");

            rpt.DataDefinition.FormulaFields["img"].Text = "\"" + imagepath + "\"";
            rpt.DataDefinition.FormulaFields["sign"].Text = "\"" + sign + "\"";
            rpt.SetDataSource(myDataSet_ForReport);


            cryDeliveryOrderForGST.ReportSource = rpt;
        }
        catch (Exception)
        {

            throw;
        }
        if (!IsPostBack)
        {
            #region [Whatsapp]
            if (bss == "Y")
            {
                string MobileNo = clsCommon.getString("Select whatsup_no from qrymstaccountmaster where  Ac_Name_E='" + getpassname + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                txtWhatsapp.Text = MobileNo;
            }
            if (bss == "N")
            {
                string MobileNo = clsCommon.getString("Select whatsup_no from qrymstaccountmaster where  Ac_Name_E='" + shiptoname + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                txtWhatsapp.Text = MobileNo;
            }

            #endregion
        }
    }

    private DataTable GetData()
    {
        try
        {
            DataTable dt = new DataTable();
            string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand("select * from qrydohead where  doid in(" + doc_no + ")", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            DoType = dt.Rows[0]["desp_type"].ToString();


            string qry = "select * from tblvoucherheadaddress where Company_Code='" + Session["Company_Code"].ToString() + "'";
            DataSet ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    AL1 = dt1.Rows[0]["AL1"].ToString();
                    AL2 = dt1.Rows[0]["AL2"].ToString();
                    AL3 = dt1.Rows[0]["AL3"].ToString();
                    AL4 = dt1.Rows[0]["AL4"].ToString();
                    other = dt1.Rows[0]["Other"].ToString();
                    billfoter = dt1.Rows[0]["BillFooter"].ToString();
                }
            }



            return dt;
        }
        catch
        {
            return null;
        }
    }
   

    private string GetDefaultPrinter()
    {
        PrinterSettings settings = new PrinterSettings();

        foreach (string printer in PrinterSettings.InstalledPrinters)
        {
            settings.PrinterName = printer;
            if (settings.IsDefaultPrinter)
            {
                return printer;
            }
        }
        return string.Empty;
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath=@"D:\pdffiles\cryChequePrinting.pdf";
            string filepath = @"C:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("C:\\PDFFiles");
            }
            string pdfname = filepath + "\\UTR.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);

            //open PDF File
            //System.Diagnostics.Process.Start(pdfname);
            // string FilePath = Server.MapPath("javascript1-sample.pdf");

            WebClient User = new WebClient();

            Byte[] FileBuffer = User.DownloadData(pdfname);

            if (FileBuffer != null)
            {

                Response.ContentType = "application/pdf";

                Response.AddHeader("content-length", FileBuffer.Length.ToString());

                Response.BinaryWrite(FileBuffer);

            }
        }
        catch (Exception e1)
        {
            Response.Write("PDF err:" + e1);
            return;
        }
        //   Response.Write("<script>alert('PDF successfully Generated');</script>");

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath = @"D:\ashwini\bhavani10012019\accowebBhavaniNew\PAN\cryChequePrinting.pdf";
            string filepath = @"C:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("C:\\PDFFiles");
            }
            string pdfname = filepath + "\\D.O.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);
            DataSet ds = clsDAL.SimpleQuery("select * from qrydohead where doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
              " and Year_Code='" + Session["year"].ToString() + "'");
            string Do = ds.Tables[0].Rows[0]["doc_no"].ToString();
            string Truck_No = ds.Tables[0].Rows[0]["truck_no"].ToString();
            string DOQntl = ds.Tables[0].Rows[0]["quantal"].ToString();
            string salebillname = ds.Tables[0].Rows[0]["salebillname"].ToString();
            string grade = ds.Tables[0].Rows[0]["grade"].ToString();
            string season = ds.Tables[0].Rows[0]["season"].ToString();
            string sbno = ds.Tables[0].Rows[0]["SB_No"].ToString();

            if (txtEmail.Text != string.Empty && txtEmail.Text != "0")
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txtEmail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "D.O";
                Attachment attachment = new Attachment(pdfname, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "D.O";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "DO.No:" + Do + " Truck No:" + Truck_No + " Qt:" + DOQntl + " " + salebillname + " " + grade + " " + season + " SB_No:" + sbno;
                //msg.IsBodyHtml = true;
                if (smtpPort != string.Empty)
                {
                    SmtpServer.Port = Convert.ToInt32(smtpPort);
                }
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                SmtpServer.Send(msg);
                attachment.Dispose();
                if (File.Exists(pdfname))
                {
                    File.Delete(pdfname);
                }
                Response.Write("<script>alert('Mail Send successfully');</script>");
            }
            else
            {
                Response.Write("<script>alert('Enter Email Id');</script>");
            }
        }
        catch (Exception e1)
        {
            Response.Write("Mail err:" + e1);
            return;
        }


    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        //rprt1.Close();
        //rprt1.Clone();
        //rprt1.Dispose();
        //GC.Collect();
        this.cryDeliveryOrderForGST.ReportSource = null;

        cryDeliveryOrderForGST.Dispose();

        if (rpt != null)
        {

            rpt.Close();

            rpt.Dispose();

            rpt = null;

        }

        GC.Collect();

        GC.WaitForPendingFinalizers();
    }
    protected void btntenderDo_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath = @"D:\ashwini\bhavani10012019\accowebBhavaniNew\PAN\cryChequePrinting.pdf";
            string filepath = @"C:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("C:\\PDFFiles");
            }
            string pdfname = filepath + "\\D.O.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);
            DataSet ds = clsDAL.SimpleQuery("select * from qrydohead where doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
              " and Year_Code='" + Session["year"].ToString() + "'");
            string Do = ds.Tables[0].Rows[0]["doc_no"].ToString();
            string Truck_No = ds.Tables[0].Rows[0]["truck_no"].ToString();
            string DOQntl = ds.Tables[0].Rows[0]["quantal"].ToString();
            string millname = ds.Tables[0].Rows[0]["millname"].ToString();
            string grade = ds.Tables[0].Rows[0]["grade"].ToString();
            string season = ds.Tables[0].Rows[0]["season"].ToString();

            if (txttenderdomail.Text != string.Empty)
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txttenderdomail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "D.O";
                Attachment attachment = new Attachment(pdfname, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "D.O";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "DO.No:" + Do + " " + Truck_No + " Qt:" + DOQntl + " " + millname + " " + grade + " " + season;
                //msg.IsBodyHtml = true;
                if (smtpPort != string.Empty)
                {
                    SmtpServer.Port = Convert.ToInt32(smtpPort);
                }
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                SmtpServer.Send(msg);
                attachment.Dispose();
                if (File.Exists(pdfname))
                {
                    File.Delete(pdfname);
                }
                Response.Write("<script>alert('Mail Send successfully');</script>");
            }
            else
            {
                Response.Write("<script>alert('Enter Email Id');</script>");
            }
        }
        catch (Exception e1)
        {
            Response.Write("Mail err:" + e1);
            return;
        }

    }

    protected void btnWhatsApp_Click(object sender, EventArgs e)
    {
      
        DataSet ds = clsDAL.SimpleQuery("select * from qrydohead where doid='" + doc_no + "' and Company_Code='" + Session["Company_Code"] + "' " +
             " and Year_Code='" + Session["year"].ToString() + "'");
        bss = Request.QueryString["bss"];
        string salebillname = "";
        if (bss == "Y")
        {
            salebillname = ds.Tables[0].Rows[0]["getpassname"].ToString();
            salebillcityname = ds.Tables[0].Rows[0]["getpasscityname"].ToString();
        }
        else
        {
            salebillname = ds.Tables[0].Rows[0]["shiptoname"].ToString();
            salebillcityname = ds.Tables[0].Rows[0]["shiptocityname"].ToString();
        }
        getpassname = ds.Tables[0].Rows[0]["getpassname"].ToString();
        getpasscityname = ds.Tables[0].Rows[0]["getpasscityname"].ToString();
        string mobileNumber = txtWhatsapp.Text.Trim();
        if (mobileNumber == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Enter Whatsapp No !');", true);
            return;
        }
        string bill_no = "DoNo:" + DONo;
        var base64String = "";
        string uploadedFileName = "";
        DataTable table = new DataTable();
        table.Columns.Add("mobno", typeof(string));
        table.Columns.Add("filename", typeof(string));
        string[] names = mobileNumber.Split(',');
        for (int i = 0; i < names.Length; i++)
            table.Rows.Add(new object[] { names[i] });
        string Moblie_Number = string.Empty;
        string respString = string.Empty;
        string instanceid = clsCommon.getString("select Instance_Id from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        string accesstoken = clsCommon.getString("select Access_token from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        string WaTitle = clsCommon.getString("select WaTitle from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        string Mobile_No = clsCommon.getString("select Mobile_No from tblWhatsAppURL where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

        string filepath = @"C:\PDFFILES\DO";
        if (!System.IO.Directory.Exists(filepath))
        {
            System.IO.Directory.CreateDirectory(filepath);
        }
        string DoDate = DateTime.Now.ToString();
        string DDate = DoDate.Trim();
        uploadedFileName = "DO.pdf";

        string pdfname = filepath + "\\" + uploadedFileName;
        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);
        var fileBytes = File.ReadAllBytes(pdfname);
        base64String = System.Convert.ToBase64String(fileBytes);
        if (table.Rows.Count > 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Moblie_Number = table.Rows[i]["mobno"].ToString();


                string message = " Hello, " + Environment.NewLine + "Greetings from *" + WaTitle + "*" + Environment.NewLine + " DELIVERY ORDER " + Environment.NewLine +
                      " DATE: " + dodocdate + "  " + Environment.NewLine + " MILL NAME: " + millshortname + "" + Environment.NewLine +
                      " D.O NO: " + DONo + "  " + Environment.NewLine + " TRUCK NO: " + Lorryno + "  " + Environment.NewLine +
                      " QUANTITY: " + Qty + "  " + Environment.NewLine + " GRADE: " + grade + " " + Environment.NewLine +
                      " TENDER DATE: " + tenderDate + " " + Environment.NewLine + " BUYER: " + getpassname + " " + Environment.NewLine + " BUYER CITY: " + getpasscityname + "  " + Environment.NewLine +
                      " SHIPPED TO: " + salebillname + " " + Environment.NewLine + " SHIPPED TO CITY: " + salebillcityname + "" + Environment.NewLine +
                      "  " + Environment.NewLine + " FOR MORE DETAILS, PLEASE OPEN THE ATTACHED PDF FILE " + Environment.NewLine +
                      " IN CASE OF ANY PROBLEM, PLEASE CALL ON *" + Mobile_No + "*";


                //https://wawatext.com/api/send.php?number=84933313xxx&type=text&message=test%20message&instance_id=629D9E80CA75D&access_token=6c596a733f89149b69a1c334f12f65b1 

                //string Url = "https://wawatext.com/api/send.php?number=91" + mobileNumber + "&type=text&message=" + message + "&instance_id=62A08B1183599&access_token=6c596a733f89149b69a1c334f12f65b1";
                string Url = "https://wawatext.com/api/send.php?number=91" + Moblie_Number + "&type=text&message=" + message + "&instance_id=" + instanceid + "&access_token=" + accesstoken + "";
                //string Url = "https://wawatext.com/api/send.php?number=91";
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
                StreamReader reder = new StreamReader(resp.GetResponseStream());
                respString = reder.ReadToEnd();
                reder.Close();
                resp.Close();





            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "api-call", "javascript:sendPdfToWatsapp('" + bill_no + "','" + base64String + "', '" + uploadedFileName + "','" + instanceid + "','" + accesstoken + "','DO from new updated whatsapp api','" + Moblie_Number + "', 'LATASOFTWARE','" + txtWhatsapp.Text + "');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "api-call", "javascript:sendPdfToWatsapp('" + bill_no + "','" + base64String + "', '" + uploadedFileName + "','" + instanceid + "','" + accesstoken
                + "','DELIVER ORDER FROM " + WaTitle + " DO NO: " + DONo + " TRUCK NO: " + Lorryno + "','" + Moblie_Number + "', 'LATASOFTWARE','" + txtWhatsapp.Text + "');", true);

            string str = respString;
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace(":", "");
            str = str.Replace(",", "");
            str = str.Replace("\"", "");
            string sub2 = "success";
            bool b = str.Contains(sub2);

            string sub4 = "error";
            bool s = str.Contains(sub4);

            if (b)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Successfully Sent!');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Cloud Not Sent!');", true);
            }
        }

    }
}