using System;
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


public partial class Sugar_Report_rptUtrReport_Print : System.Web.UI.Page
{
    string AL1 = string.Empty;
    string AL2 = string.Empty;
    string AL3 = string.Empty;
    string AL4 = string.Empty;
    string other = string.Empty;
    int billno;
    int company_code;
    int year_code;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
    string ac_code;
    string utr_no;
          string millcode;
    string AcType = string.Empty;
    string mail = string.Empty;
    string doc_no = string.Empty;
    string tenderno = string.Empty;
    string imagepath = string.Empty;
    ReportDocument rprt1 = new ReportDocument();
    ReportDocument rprt2 = new ReportDocument();
    ReportDocument rpt = new ReportDocument();
    string company_name = string.Empty;
    string amount = string.Empty;
    string utrnonew = string.Empty;
    string qry = string.Empty;
    string utr_date = string.Empty;
    string Mill_Name = string.Empty;
    string Mill_City_Name = string.Empty;
    string MillBank_Ac_No = string.Empty;
    string Utr = string.Empty;
    string Narration_Header = string.Empty;
    string Narration_Footer = string.Empty;
    string Mill_Mobile_No = string.Empty;
    string Moblie_Number = string.Empty;
    string MillBankName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            company_name = Session["Company_Name"].ToString();
            doc_no = Request.QueryString["docno"];
            millcode = Request.QueryString["millcode"];
            if (!IsPostBack)
            {
               
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                string mill = Request.QueryString["mill"];
                //txtEmail.Text = clsCommon.getString("Select Email_Id,Email_Id_cc from qrymstaccountmaster where  Ac_Code='" + millcode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                //txtEmail.Text = clsCommon.getString("Select Email_Id_cc from qrymstaccountmaster where  Ac_Code='" + millcode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                //txtEmail.Text = "select Email_Id,Email_Id_cc from qrymstaccountmaster where  Ac_Code='" + millcode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                qry = "select Email_Id,Email_Id_cc,whatsup_no from qrymstaccountmaster where  Ac_Code='" + millcode + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        if (dt1.Rows.Count > 0)
                        {
                            string Email_Id = dt1.Rows[0]["Email_Id"].ToString();
                            string Email_Id_cc = dt1.Rows[0]["Email_Id_cc"].ToString();
                            string whatsup_no = dt1.Rows[0]["whatsup_no"].ToString();

                            if (Email_Id_cc != string.Empty)
                            {
                                txtEmail.Text = Email_Id + "," + Email_Id_cc;
                            }
                            else
                            {
                                txtEmail.Text = Email_Id;
                            }
                            txtWhatsapp.Text = whatsup_no;

                        }
                    }
                }
            }
            //amount = clsCommon.getString("select amount from nt_1_utr where utrid=" + doc_no + 
            //    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
            //    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

            amount = clsCommon.getString("select amount from nt_1_utr where utrid=" + doc_no +
              " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
           //   + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));


            //utrnonew = clsCommon.getString("select utr_no from nt_1_utr where utrid=" + doc_no +
            //    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) 
            //    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));


            utrnonew = clsCommon.getString("select utr_no from nt_1_utr where utrid=" + doc_no +
             " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
//             + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

            DataTable dt = GetData();
            SqlDataAdapter da = new SqlDataAdapter();
            rpt.Load(Server.MapPath("cryUtr_Print.rpt"));
            rpt.SetDataSource(dt);


            cryUtr_Print.ReportSource = rpt;
            rpt.DataDefinition.FormulaFields["companyname"].Text = "\"" + company_name + "\"";

            rpt.DataDefinition.FormulaFields["AL1"].Text = "\"" + AL1 + "\"";
            rpt.DataDefinition.FormulaFields["AL2"].Text = "\"" + AL2 + "\"";
            rpt.DataDefinition.FormulaFields["AL3"].Text = "\"" + AL3 + "\"";
            rpt.DataDefinition.FormulaFields["Al4"].Text = "\"" + AL4 + "\"";
            rpt.DataDefinition.FormulaFields["other"].Text = "\"" + other + "\"";


            imagepath = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='L' and Company_Code='"
                 + Session["Company_Code"].ToString() + "'");
            String path = Server.MapPath("") + "\\" + imagepath;

            // string imagepath1 = path.Replace("Sugar", "Images");
            imagepath = path.Replace("Report", "Images");

            string sign = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='S' and Company_Code='"
              + Session["Company_Code"].ToString() + "'");
            String logopath = Server.MapPath("") + "\\" + sign;

            // string logopath1 = path.Replace("Sugar", "Images");
            sign = logopath.Replace("Report", "Images");

            rpt.DataDefinition.FormulaFields["img"].Text = "\"" + imagepath + "\"";
            rpt.DataDefinition.FormulaFields["sign"].Text = "\"" + sign + "\"";
            // rprt.DataDefinition.FormulaFields["todate"].Text = datefrom;
            //rprt.DataDefinition.FormulaFields["Date"].Text = "\"Reverse Charge Report From " + datefrom + " To " + dateto + "\"";
            cryUtr_Print.RefreshReport();
        }
        catch (Exception)
        {

            throw;
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
                SqlCommand cmd = new SqlCommand("select * from qryutrhead where  utrid in(" + doc_no + ")", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }

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
                }
            }
            return dt;
        }
        catch
        {
            return null;
        }
    }

    protected void btnWhatsApp_Click(object sender, EventArgs e)
    {
        string mobileNumber = txtWhatsapp.Text.Trim();
        DataTable dt = GetData();
        utr_date = dt.Rows[0]["doc_dateConverted"].ToString();
        Mill_Name = dt.Rows[0]["Ac_Name_E"].ToString();
        Mill_City_Name = dt.Rows[0]["millcityname"].ToString();
        Utr = dt.Rows[0]["utr_no"].ToString();
        Narration_Header = dt.Rows[0]["narration_header"].ToString();
        Narration_Footer = dt.Rows[0]["narration_footer"].ToString(); 


        if (mobileNumber == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Enter Whatsapp No !');", true);
            return;
        }
        string bill_no = doc_no;
        var base64String = "";
        string uploadedFileName = "";
        DataTable table = new DataTable();
        table.Columns.Add("mobno", typeof(string));
        table.Columns.Add("filename", typeof(string));
        string[] names = mobileNumber.Split(',');
        for (int i = 0; i < names.Length; i++)
            table.Rows.Add(new object[] { names[i] });
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


                string message = " Hello, " + Environment.NewLine + " Greetings from  *" + WaTitle + "*" + Environment.NewLine +
                       "" + Environment.NewLine + " *PAYMENT DETAILS:* " + Environment.NewLine + " *REF NO:* " + bill_no + "" + Environment.NewLine + " *DATE:* " + utr_date + "" + Environment.NewLine +
                       " *PAYMENT TO:* " + Mill_Name + "" + Environment.NewLine + " *CITY:* " + Mill_City_Name + "" + Environment.NewLine + 
                       " *AMOUNT:* " + amount + "" + Environment.NewLine + " *UTR NO:* " + Utr + "" + Environment.NewLine +
                       " *DETAIL 1:* " + Narration_Header + "" + Environment.NewLine + " *DETAIL 2:* " + Narration_Footer + "" + Environment.NewLine +
                       "" + Environment.NewLine + " FOR MORE DETAILS, PLEASE OPEN THE ATTACHED PDF FILE " + Environment.NewLine +
                       " IN CASE OF ANY PROBLEM, PLEASE CALL ON  *" + Mobile_No + "*";
                //string message = "Mill:" + millshortname + " DONo:" + DONo + " TruckNo:" + Lorryno + " Qty:" + Qty + " Grade:" + grade + " BillTo:" + buyershortname + ",City:" + buyercityname + " ShipTo:" + Shiptoshortname + ",City:" + Shiptocityname + ".pdf";

 
                string Url = "https://wawatext.com/api/send.php?number=91" + Moblie_Number + "&type=text&message=" + message + "&instance_id=" + instanceid + "&access_token=" + accesstoken + "";
                //string Url = "https://wawatext.com/api/send.php?number=91";
                //string Url = "";
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
                StreamReader reder = new StreamReader(resp.GetResponseStream());
                respString = reder.ReadToEnd();
                reder.Close();
                resp.Close();





            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "api-call", "javascript:sendPdfToWatsapp('" + bill_no + "','" + base64String + "', '" + uploadedFileName + "','" + instanceid + "','" + accesstoken + "','PAYMENT DETAIL FROM " + WaTitle + "','" + Moblie_Number + "', 'LATASOFTWARE','" + txtWhatsapp.Text + "');", true);

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
            string filepath = @"D:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("D:\\pdffiles");
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
            string filepath = @"D:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("D:\\pdffiles");
            }
            string pdfname = filepath + "\\UTR.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);

            if (txtEmail.Text != string.Empty)
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txtEmail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "UTR";
                Attachment attachment = new Attachment(pdfname, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
               // msg.Body = "UTR";
                msg.Body = "Dear Sir/Madam," + Environment.NewLine +
                         "We have made payment as follows." + Environment.NewLine +
                         "Amount:" + amount + ", Bill No:" + utrnonew + "" + Environment.NewLine +
                         "Kindly credit the same to our account." + Environment.NewLine +
                         "Thanks & Regards," + Environment.NewLine +
                         Session["Company_Name"].ToString();
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "Payment Details from " + Session["Company_Name"].ToString();
                //msg.Subject = "DOC.No:";
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
            }


        }
        catch (Exception e1)
        {
            Response.Write("Mail err:" + e1);
            return;
        }
        Response.Write("<script>alert('Mail Send successfully');</script>");

    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        //rprt1.Close();
        //rprt1.Clone();
        //rprt1.Dispose();
        //GC.Collect();
        this.cryUtr_Print.ReportSource = null;

        cryUtr_Print.Dispose();

        if (rpt != null)
        {

            rpt.Close();

            rpt.Dispose();

            rpt = null;

        }

        GC.Collect();

        GC.WaitForPendingFinalizers();
    }
}