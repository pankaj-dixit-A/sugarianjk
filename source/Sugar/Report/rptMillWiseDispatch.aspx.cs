﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class Report_rptMillWiseDispatch : System.Web.UI.Page
{
    string Mill_Code = string.Empty;
    string fromDT = string.Empty;
    string toDT = string.Empty;
    string qry = string.Empty;
    string Tender_No = string.Empty;
    DataSet ds;
    DataTable dt;
    DataSet bind;
    DataTable bindmethode;
    string tblPrefix = string.Empty;
    string f = "";
    string f_Main = "../Report/";
    string Branch_Code = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        fromDT = Request.QueryString["fromDT"].ToString();
        toDT = Request.QueryString["toDT"].ToString();
        Branch_Code = Request.QueryString["Branch_Code"];
        if (!Page.IsPostBack)
        {
            user = Session["user"].ToString();
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                lblCompanyName.Text = Session["Company_Name"].ToString();
                this.BindData();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }

    private void BindData()
    {
        try
        {
            qry = "select doc_no ,doc_dateConverted as dodate,millShortName as millShortName,voucherbyshortname as VocBy,getpassshortname as GetPass" +
               ",mill_rate as MR,quantal as Qntl,sale_rate as SR,truck_no as lorry,Freight_Amount as frt,vasuli_amount1 AS vasuli,transportname as transport," +
               " doname as do,purc_no as tender,vasuliacname ,LoadingSms,mill_code from qrydohead where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "' and purc_no!=0 and  tran_type='DO'";


            bind = clsDAL.SimpleQuery(qry);

            bindmethode = bind.Tables[0];

            qry = "select distinct(mill_code) as MillCode,millName as MillName from qrydohead where tran_type='DO' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_date between '" + fromDT + "' and '" + toDT + "'  order by millName asc";

            ds = new DataSet();
            //ds = obj.GetDataSet(qry);
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Datalist1.DataSource = dt;
                    Datalist1.DataBind();
                }
                else
                {
                    Datalist1.DataSource = null;
                    Datalist1.DataBind();
                }
            }
            else
            {
                Datalist1.DataSource = null;
                Datalist1.DataBind();
            }
            //}
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            clsDAL.CloseConnection();
        }
    }
    protected void Datalist1_OnItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            //using (clsDataProvider obj = new clsDataProvider())
            //{
            DataList dtlDetails = (DataList)e.Item.FindControl("dtlDetails");
            Label lblMillCode = (Label)e.Item.FindControl("lblMillCode");
            Label lblQntlTotal = (Label)e.Item.FindControl("lblQntlTotal");
            string millcode = lblMillCode.Text;


            DataView view = new DataView(bindmethode, "mill_code='" + lblMillCode.Text + "'", "mill_code", DataViewRowState.CurrentRows);
            DataTable selectedvalue = view.ToTable(true, "doc_no", "dodate", "millShortName", "VocBy", "GetPass", "MR", "Qntl", "SR", "lorry", "frt", "vasuli", "transport",
                "do", "tender", "vasuliacname", "LoadingSms", "mill_code");

            if (selectedvalue.Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("#", typeof(string)));
                dt.Columns.Add(new DataColumn("dodate", typeof(string)));
                dt.Columns.Add(new DataColumn("millShortName", typeof(string)));
                dt.Columns.Add(new DataColumn("VocBy", typeof(string)));
                dt.Columns.Add(new DataColumn("GetPass", typeof(string)));
                dt.Columns.Add(new DataColumn("MR", typeof(string)));
                dt.Columns.Add(new DataColumn("Qntl", typeof(double)));
                dt.Columns.Add(new DataColumn("SR", typeof(string)));
                dt.Columns.Add(new DataColumn("lorry", typeof(string)));
                dt.Columns.Add(new DataColumn("frt", typeof(string)));
                dt.Columns.Add(new DataColumn("vasuli", typeof(string)));
                dt.Columns.Add(new DataColumn("transport", typeof(string)));
                dt.Columns.Add(new DataColumn("do", typeof(string)));
                dt.Columns.Add(new DataColumn("tender", typeof(string)));
                dt.Columns.Add(new DataColumn("LoadingSms", typeof(string)));
                dt.Columns.Add(new DataColumn("Vasuli_Ac_Name", typeof(string)));

                for (int i = 0; i < selectedvalue.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["#"] = selectedvalue.Rows[i]["doc_no"].ToString();
                    dr["dodate"] = selectedvalue.Rows[i]["dodate"].ToString();
                    dr["millShortName"] = selectedvalue.Rows[i]["millShortName"].ToString();
                    dr["VocBy"] = selectedvalue.Rows[i]["VocBy"].ToString();
                    dr["GetPass"] = selectedvalue.Rows[i]["GetPass"].ToString();
                    dr["MR"] = selectedvalue.Rows[i]["MR"].ToString();
                    dr["Qntl"] = selectedvalue.Rows[i]["Qntl"].ToString();
                    dr["SR"] = selectedvalue.Rows[i]["SR"].ToString();
                    dr["lorry"] = selectedvalue.Rows[i]["lorry"].ToString();
                    dr["frt"] = selectedvalue.Rows[i]["frt"].ToString();
                    dr["vasuli"] = selectedvalue.Rows[i]["vasuli"].ToString();
                    dr["transport"] = selectedvalue.Rows[i]["transport"].ToString();
                    dr["do"] = selectedvalue.Rows[i]["do"].ToString();
                    dr["tender"] = selectedvalue.Rows[i]["tender"].ToString();
                    dr["LoadingSms"] = selectedvalue.Rows[i]["LoadingSms"].ToString();
                    dr["Vasuli_Ac_Name"] = selectedvalue.Rows[i]["vasuliacname"].ToString();
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    lblfromtodate.Text = "Millwise Wise Dispatch Register From  <b>" + fromDT + "</b>  To  <b>" + toDT + "</b>";
                    lblQntlTotal.Text = Convert.ToString(dt.Compute("SUM(Qntl)", string.Empty));
                    dtlDetails.DataSource = dt;
                    dtlDetails.DataBind();
                }
                else
                {
                    dtlDetails.DataSource = null;
                    dtlDetails.DataBind();
                }
            }
            else
            {
                dtlDetails.DataSource = null;
                dtlDetails.DataBind();
            }
            //}
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        StringBuilder StrHtmlGenerate = new StringBuilder();
        StringBuilder StrExport = new StringBuilder();
        StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
        StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
        StrExport.Append("<DIV  style='font-size:12px;'>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter tw = new HtmlTextWriter(sw);
        pnlMain.RenderControl(tw);
        string sim = sw.ToString();
        StrExport.Append(sim);
        StrExport.Append("</div></body></html>");
        string strFile = "report.xls";
        string strcontentType = "application/excel";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.BufferOutput = true;
        Response.ContentType = strcontentType;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
        Response.Write(StrExport.ToString());
        Response.Flush();
        Response.Close();
        Response.End();
    }
}