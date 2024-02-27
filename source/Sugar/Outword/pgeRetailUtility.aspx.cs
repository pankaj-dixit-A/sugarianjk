using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Threading;
using System.Data.SqlClient;

public partial class Sugar_Outword_pgeRetailUtility : System.Web.UI.Page
{
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null;
    string cs = string.Empty;
    string qryCommon = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        qryCommon = "qryRetailSale";
        cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
        con = new SqlConnection(cs);
        if (!IsPostBack)
        {
            BindDummyRow();

        }
        SetFocus(btnAdd);
    }
    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("Doc_No");
        dummy.Columns.Add("Doc_Date");
        dummy.Columns.Add("Tran_Type");
        dummy.Columns.Add("partyname");
        dummy.Columns.Add("Delivered");
        dummy.Columns.Add("brokername");
        dummy.Columns.Add("millshortname");
        dummy.Columns.Add("Brand_Code_Name");
        dummy.Columns.Add("Qty");
        dummy.Columns.Add("Rate");
        dummy.Columns.Add("Net_Value");
        dummy.Columns.Add("ack");
        dummy.Columns.Add("EwayBillNo");
        dummy.Columns.Add("IsDelete");
        dummy.Columns.Add("Retailid");

        dummy.Rows.Add();
        gvCustomers.DataSource = dummy;
        gvCustomers.DataBind();
    }
    [WebMethod]
    public static string GetCustomers(string searchTerm, int pageIndex, string Trantype, int PageSize, int Company_Code, int year)
    {
        string searchtxt = "";
        string delimStr = "";
        char[] delimiter = delimStr.ToCharArray();
        string words = "";
        string[] split = null;
        string name = string.Empty;

        searchtxt = searchTerm;
        words = searchTerm;
        split = words.Split(delimiter);
        foreach (var s in split)
        {
            string aa = s.ToString();
            // name += "Doc_No Like '%" + aa + "%'or";
            name += "( partyname like '%" + aa + "%' or Doc_No like '%" + aa + "%' or Doc_Date like '%" + aa + "%' or Grand_total like '%" + aa + "%' or Retailid like '%" + aa + "%'or millshortname like '%" + aa + "%' ) and";


        }
        name = name.Remove(name.Length - 3);


        string query = "SELECT ROW_NUMBER() OVER ( order by Doc_No ASC) AS RowNumber,Doc_No,convert(varchar(10),Doc_Date,103) as Doc_Date,CashCredit as Tran_Type,"
      + "partyname as Party_Name ,case when Delivered='1' then 'No' else 'Yes' end as Delivered,brokername,millshortname,Brand_Code_Name,Qty,Rate,Net_Value,ack,EwayBillNo,IsDelete,Retailid FROM qryRetailSale   where " + name + " and CashCredit='" + Trantype + "' and Company_Code=" + Company_Code + " " +
      " and Year_Code=" + year + " order by Doc_No desc, Doc_Date desc";

        SqlCommand cmd = new SqlCommand(query);
        cmd.CommandType = CommandType.Text;
        return GetData(cmd, pageIndex, PageSize).GetXml();

    }

    private static DataSet GetData(SqlCommand cmd, int pageIndex, int PageSize)
    {

        string RecordCount = "";
        string cs1 = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;

        using (SqlConnection con = new SqlConnection(cs1))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {

                cmd.Connection = con;
                sda.SelectCommand = cmd;
                DataSet dsreturn = new DataSet();
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    int number = 1;
                    DataTable dtnew = new DataTable();
                    dtnew = ds.Tables[0];
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        dtnew.Rows[i][0] = number;
                        number = number + 1;

                    }
                    string f1 = " RowNumber >=(" + pageIndex + " -1) * (" + PageSize + "+1) and RowNumber<=";
                    string f2 = "(((" + pageIndex + " -1) * " + PageSize + " +1) +" + PageSize + ")-1";

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "Doc_Date desc");
                    if (results.Count() > 0)
                    {
                        DataTable dt1 = results.CopyToDataTable();
                        dt1.TableName = "Customers";
                        DataTable dt = new DataTable("Pager");
                        dt.Columns.Add("PageIndex");
                        dt.Columns.Add("PageSize");
                        dt.Columns.Add("RecordCount");
                        dt.Rows.Add();
                        RecordCount = ds.Tables[0].Rows.Count.ToString();

                        dt.Rows[0]["PageIndex"] = pageIndex;
                        dt.Rows[0]["PageSize"] = PageSize;
                        dt.Rows[0]["RecordCount"] = RecordCount;

                        dsreturn = new DataSet();
                        dsreturn.Tables.Add(dt1);
                        dsreturn.Tables.Add(dt);

                        return dsreturn;
                    }
                    else
                    {
                        return dsreturn;
                    }


                }
            }
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
}