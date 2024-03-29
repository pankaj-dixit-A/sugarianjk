﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;
using System.Web.Security;
using System.Xml.Linq;
using System.Drawing.Printing;
using System.Net;
using System.Text;
using System.Net.Mail;


public partial class Reports_rptSugarBalanceStocks : System.Web.UI.Page
{

    string f = "../GSReports/SugarStock.htm";
    string f_Main = "../Report/rptSugarBalanceStocks";
    string email = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    int pos;
    DataSet d2;
    DataTable binddata;
    DataTable dtabind;
    PagedDataSource adsource;
    string tblPrefix = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {

                    lblCompanyName.Text = Session["Company_Name"].ToString();

                    Bindata();
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }

            }





        }
        catch (NullReferenceException ne)
        {
            //Response.Write("<script>alert('Please login');</script>");
            //Response.Redirect("~/pgeloginForm.aspx", false);
        }
    }
    private void Bindata()
    {
        try
        {
            //using (clsDataProvider obj = new clsDataProvider())
            //{
            DataSet ds = new DataSet();
            //string qry = "select distinct Tender_No,Tender_DateConverted as Tender_Date,millshortname as millname,season,Grade,Quantal as Quantal,Mill_Rate as Mill_Rate ,Purc_Rate as Purc_Rate,Lifting_DateConverted as Lifting_Date,tenderdoshortname as doname  " +
            //    " from qrytenderdobalanceview  where BALANCE!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by millshortname,Tender_No";
            string qry = "select distinct Tender_No,Tender_DateConverted as Tender_Date,millshortname as millname,season,Grade,Quantal as Quantal,Mill_Rate as Mill_Rate ,Purc_Rate as Purc_Rate,Lifting_DateConverted as Lifting_Date,tenderdoshortname as doname  " +
                 " from qrytenderdobalanceview  where BALANCE!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                 " order by millshortname,Tender_No";
          
            
            
            
            //ds = obj.GetDataSet(qry);
            ds = clsDAL.SimpleQuery(qry);

            //string qry1 = "select ID as ID,Buyer,buyername as buyerbrokerfullname,Sale_Rate as Sale_Rate,Buyer_Quantal,DESPATCH as despatchqty,BALANCE as balance,Tender_No from qrytenderdobalanceview where"
            //    + " Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by ID ";
            string qry1 = "select ID as ID,Buyer,buyername as buyerbrokerfullname,Sale_Rate as Sale_Rate,Buyer_Quantal,DESPATCH as despatchqty,BALANCE as balance,Tender_No from qrytenderdobalanceview where"
               + " Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + 
                " order by ID ";
            
            
            
            d2 = clsDAL.SimpleQuery(qry1);
            dtabind = d2.Tables[0];
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    DataList1.DataSource = dt;
                    DataList1.DataBind();
                }
            }
            //}


        }
        catch
        {
            throw;
        }
        finally
        {
            clsDAL.CloseConnection();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
      server control at run time. */
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            //using (clsDataProvider obj = new clsDataProvider())
            //{
            DataList dtl = (DataList)e.Item.FindControl("dtl");
            LinkButton lnktenderNo = (LinkButton)e.Item.FindControl("lbkTenderNo");
            Label lblDispatch1 = (Label)e.Item.FindControl("lblDispatch1");
            Label lblMillLot = (Label)e.Item.FindControl("lblMillLot");
            Label lblBalance1 = (Label)e.Item.FindControl("lblBalance1");
            DataSet ds2 = new DataSet();
            double totaldes = 0.00;
            DataView view1 = new DataView(dtabind, "Tender_No='" + lnktenderNo.Text + "'", "Tender_No", DataViewRowState.CurrentRows);

            DataTable dtAcData = view1.ToTable(true, "ID", "Buyer", "buyerbrokerfullname", "Sale_Rate", "Buyer_Quantal", "despatchqty", "balance",
         "Tender_No");
            if (dtAcData.Rows.Count > 0)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Buyer", typeof(Int32)));
                dt.Columns.Add(new DataColumn("buyerbrokerfullname", typeof(string)));
                dt.Columns.Add(new DataColumn("Sale_Rate", typeof(double)));
                dt.Columns.Add(new DataColumn("Buyer_Quantal", typeof(double)));
                dt.Columns.Add(new DataColumn("despatchqty", typeof(double)));
                dt.Columns.Add(new DataColumn("balance", typeof(double)));

                if (dtAcData.Rows.Count > 0)
                {

                    for (int i = 0; i < dtAcData.Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = dtAcData.Rows[i]["ID"].ToString();
                        string BuyerCode = dtAcData.Rows[i]["Buyer"].ToString();
                        string buyerCityName = clsCommon.getString("select CityName from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + BuyerCode + "  and Ac_Name_E !='SELF'");
                        if (buyerCityName == "0")
                        {
                            buyerCityName = string.Empty;
                        }
                        dr["buyerbrokerfullname"] = dtAcData.Rows[i]["buyerbrokerfullname"].ToString() + " , " + buyerCityName;
                        dr["Sale_Rate"] = dtAcData.Rows[i]["Sale_Rate"].ToString();
                        dr["Buyer_Quantal"] = dtAcData.Rows[i]["Buyer_Quantal"].ToString();
                        double despqty = Convert.ToDouble(dtAcData.Rows[i]["despatchqty"].ToString());
                        dr["despatchqty"] = despqty;
                        double balance = Convert.ToDouble(dtAcData.Rows[i]["balance"].ToString());
                        dr["balance"] = balance;
                        if (balance != 0)
                        {
                            dt.Rows.Add(dr);
                        }

                        totaldes += despqty;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //lblQntlGrandTotal.Text = clsCommon.getString("select SUM(Buyer_Quantal) from " + tblPrefix + "qryTenderList where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                        lblBalance1.Text = Convert.ToString(dt.Compute("SUM(balance)", string.Empty));

                        double millLot = Convert.ToDouble(lblMillLot.Text);
                        double bal = millLot - Convert.ToDouble(lblBalance1.Text);
                        lblDispatch1.Text = bal.ToString();
                        dtl.DataSource = dt;
                        dtl.DataBind();
                    }

                }
            }
            //}
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btnMail_Click(object sender, EventArgs e)
    {

        #region pdf code success
        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=pankaj.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        //pnlMain.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 0, 0, 0, 0);
        //pnlMain.Style.Add("font-size", "10px");



        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
        #endregion
        try
        {
            email = txtEmail.Text.ToString();
            CreateHtml();
            string mailFrom = Session["EmailId"].ToString();
            string smtpPort = "587";
            string emailPassword = Session["EmailPassword"].ToString();
            MailMessage msg = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(clsGV.Email_Address, 587);
            SmtpServer.Host = clsGV.Email_Address;
            msg.From = new MailAddress(mailFrom);
            msg.To.Add(email);
            msg.Body = "Sugar Stock";
            msg.Attachments.Add(new Attachment(Server.MapPath(f)));
            msg.IsBodyHtml = true;
            //msg.Body = "<html><body>" + txtEmail.Text.ToString() + "</body></html>";
            msg.Subject = "Sugar Stock Report  " + DateTime.Now.ToString("dd/MM/yyyy");
            msg.IsBodyHtml = true;
            if (smtpPort != string.Empty)
            {
                SmtpServer.Port = Convert.ToInt32(smtpPort);
            }
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
            SmtpServer.Send(msg);
        }

        catch (Exception e1)
        {
            //Response.Write("mail err:" + e1);
            Response.Write("<script>alert('Error sending Mail');</script>");
            return;
        }
        Response.Write("<script>alert('Mail sent successfully');</script>");
    }

    private void CreateHtml()
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        pnlMain.RenderControl(tw);
        string s = sw.ToString();
        try
        {
            using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(s);
                }
            }
        }
        catch (Exception ee)
        {
            f = f_Main + ".htm";
            using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(s);
                }
            }
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        //using (WebClient client = new WebClient())
        //{
        //    client.Headers[HttpRequestHeader.Cookie] =
        //        System.Web.HttpContext.Current.Request.Headers["Cookie"];

        //    string htmlCode = client.DownloadString(HttpContext.Current.Request.Url.AbsoluteUri);
        //   // client.DownloadFile(HttpContext.Current.Request.Url.AbsoluteUri, @"C:\\localfile.html");
        //}

        //WebRequest req = HttpWebRequest.Create("http://google.com");
        //req.Method = "GET";

        //string source;
        //using (StreamReader reader = new StreamReader(req.GetRequestStream())) 
        //{
        //    source = reader.ReadToEnd();
        //}


        WebClient client = new WebClient();

        Stream data = client.OpenRead(HttpContext.Current.Request.Url.AbsoluteUri);
        StreamReader reader = new StreamReader(data);
        string s = reader.ReadToEnd();
        data.Close();
        reader.Close();

        using (FileStream fs = new FileStream("D:\\grid.htm", FileMode.Create))
        {
            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            {
                w.WriteLine(s);
            }
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }
    protected void DataList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    //protected void btnfirst_Click(object sender, EventArgs e)
    //{
    //    pos = 0;
    //    Bindata();
    //}

    //protected void btnprevious_Click(object sender, EventArgs e)
    //{
    //    pos = (int)this.ViewState["vs"];
    //    pos -= 1;
    //    this.ViewState["vs"] = pos;
    //    Bindata();
    //}
    //protected void btnnext_Click(object sender, EventArgs e)
    //{
    //    pos = (int)this.ViewState["vs"];
    //    pos += 1;
    //    this.ViewState["vs"] = pos;
    //    Bindata();
    //}
    //protected void btnlast_Click(object sender, EventArgs e)
    //{
    //    pos = adsource.PageCount - 1;
    //    Bindata();
    //}
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/force-download";
        Response.AddHeader("content-disposition", "attachment; filename=Print.xls");
        Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
        Response.Write("<head>");
        Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-     8\">");
        Response.Write("<!--[if gte mso 9]><xml>");
        Response.Write("<x:ExcelWorkbook>");
        Response.Write("<x:ExcelWorksheets>");
        Response.Write("<x:ExcelWorksheet>");
        Response.Write("<x:Name>Report Data</x:Name>");
        Response.Write("<x:WorksheetOptions>");
        Response.Write("<x:Print>");
        Response.Write("<x:ValidPrinterInfo/>");
        Response.Write("</x:Print>");
        Response.Write("</x:WorksheetOptions>");
        Response.Write("</x:ExcelWorksheet>");
        Response.Write("</x:ExcelWorksheets>");
        Response.Write("</x:ExcelWorkbook>");
        Response.Write("</xml>");
        Response.Write("<![endif]--> ");
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        pnlMain.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.Write("</head>");
        Response.Flush();
        //StringBuilder StrHtmlGenerate = new StringBuilder();
        //StringBuilder StrExport = new StringBuilder();
        //StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        //StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        //StrExport.Append("<DIV  style='font-size:12px;'>");
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter tw = new HtmlTextWriter(sw);
        //pnlMain.RenderControl(tw);
        //string sim = sw.ToString();
        //StrExport.Append(sim);
        //StrExport.Append("</div></body></html>");
        //string strFile = "report.xlsm";
        //string strcontentType = "application/excel";
        //Response.ClearContent();
        //Response.ClearHeaders();
        //Response.BufferOutput = true;
        //Response.ContentType = strcontentType;
        //Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        //Response.Write(StrExport.ToString());
        //Response.Flush();
        //Response.Close();
        //Response.End();
    }

    protected void lnkTenderNo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkTenderNo = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkTenderNo.NamingContainer;
            string No = lnkTenderNo.Text;
            string accode = lnkTenderNo.Text;
            Session["TN_NO"] = No;
            Int16 Action = 1;
            //Int32 counts = Convert.ToInt32(clsCommon.getString("SELECT tenderid  from nt_1_tender where tender_no=" + accode +
            //    " and Year_Code=" + Session["year"].ToString() + " and Company_Code=" + Session["Company_Code"].ToString()));



            Int32 counts = Convert.ToInt32(clsCommon.getString("SELECT tenderid  from nt_1_tender where tender_no=" + accode +
               "  and Company_Code=" + Session["Company_Code"].ToString()));
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tnjhj", "javascript:TN();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:TN('" + accode + "','" + counts + "','"+Action+"')", true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}