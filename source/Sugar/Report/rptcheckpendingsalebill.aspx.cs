using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sugar_Report_rptcheckpendingsalebill : System.Web.UI.Page
{
    DataSet ds;
    DataTable dt;
    DataSet dsbind = new DataSet();
    DataSet dsitembind = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            this.bindData();
        }
    }

    private void bindData()
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            //lblcityName.Text = clsCommon.getString("select [city_name_e] from " + cityMasterTable + " where [city_code]=" + cityCode + " and [company_code]=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string qry = "";
            qry = "SELECT doc_no as DO_NO,doc_dateConverted as doc_Date,Do_Date_Conv as Do_Date,quantal,salebillname,millname,TRUCK_NO FROM qrydohead where  (SB_NO is null or SB_No=0)  and desp_type!='DO' and purc_no!=0 and SaleBillTo!=0 and SaleBillTo!=2 and SaleBillTo!=GETPASSCODE and Company_Code="
                + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())+" order by millname,doc_no";
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dtlAcList.DataSource = dt;
                        dtlAcList.DataBind();
                        lblqntltotal.Text = Convert.ToString(dt.Compute("SUM(quantal)", string.Empty));
                    }
                }
            }


        }
        catch (Exception eec)
        {
            Response.Write(eec.Message);
        }
    }
    protected void lbkTenderNo_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkTenderNo = (LinkButton)sender;
            DataListItem item = (DataListItem)lnkTenderNo.NamingContainer;
            string No = lnkTenderNo.Text;
            // Session["TN_NO"] = No;
            int do_id = Convert.ToInt32(clsCommon.getString("select doid from nt_1_deliveryorder where doc_no=" + No +
                     " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" +
                     " " + Session["year"].ToString() + ""));

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:DoOPen('" + do_id + "')", true);
            //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tnjhj", "javascript:TN();", true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    protected void btnchangedate_Click(object sender, EventArgs e)
    {
        string changedate = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd"); ;
        string qry = "SELECT doid as DO_NO,doc_date,Purchase_Date,voucher_no,saleidtable,doc_no FROM qrydohead where  (SB_NO is null or SB_No=0)  and desp_type!='DO' and purc_no!=0 and SaleBillTo!=0 and SaleBillTo!=2 and SaleBillTo!=GETPASSCODE and Company_Code="
                  + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        DataSet ds = clsDAL.SimpleQuery(qry);
        DataTable dt = new DataTable();
        DataRow dr;
        DataTable Maindt = new DataTable();
        Maindt.Columns.Add("Querys", typeof(string));
        dr = Maindt.NewRow();
        if (ds != null)
        {
            dt = ds.Tables[0];
            string dono = "";
            string doc_no = "";
            string psno = "";
            string saleid = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dono = dono + dt.Rows[i]["DO_NO"].ToString() + ",";
                doc_no = doc_no + dt.Rows[i]["doc_no"].ToString() + ",";
                psno = psno + dt.Rows[i]["voucher_no"].ToString() + ",";
                saleid = saleid + dt.Rows[i]["saleidtable"].ToString() + ",";

            }
            dono = dono.Remove(dono.Length - 1);
            doc_no = doc_no.Remove(doc_no.Length - 1);
            psno = psno.Remove(psno.Length - 1);
            saleid = saleid.Remove(saleid.Length - 1);

            string qry1 = "update nt_1_deliveryorder set doc_date='" + changedate + "',Purchase_Date='" + changedate + "' where doid in(" + dono + ")";
            string qry2 = "update nt_1_sugarsale set doc_date='" + changedate + "' where saleid in(" + saleid + ")";
            string qry3 = "update nt_1_sugarpurchase set doc_date='" + changedate + "' where doc_no in(" + psno +
                    ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            string qry4 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='PS' and doc_no in(" + psno +
                   ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            string qry5 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='SB' and saleid in(" + saleid +
                ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            string qry6 = "update nt_1_gledger set doc_date='" + changedate + "' where TRAN_TYPE='DO' and doc_no in(" + doc_no +
                 ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            dr = Maindt.NewRow();
            dr["Querys"] = qry1;
            Maindt.Rows.Add(dr);

            dr = Maindt.NewRow();
            dr["Querys"] = qry2;
            Maindt.Rows.Add(dr);

            dr = Maindt.NewRow();
            dr["Querys"] = qry3;
            Maindt.Rows.Add(dr);

            dr = Maindt.NewRow();
            dr["Querys"] = qry4;
            Maindt.Rows.Add(dr);

            dr = Maindt.NewRow();
            dr["Querys"] = qry5;
            Maindt.Rows.Add(dr);

            dr = Maindt.NewRow();
            dr["Querys"] = qry6;
            Maindt.Rows.Add(dr);

            string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(2, Maindt);


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            Response.Redirect("rptcheckpendingsalebill.aspx");
        }


    }
}
