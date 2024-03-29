﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Configuration;

public partial class Sugar_pgeAddressforvoucher : System.Web.UI.Page
{
    #region data section
    string qry = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    string cs = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qryAccountList = string.Empty;
    int flag = 0;
    int count = 0;
    int counts = 0;
    static WebControl objAsp = null;
    SqlConnection con = null;
    SqlCommand cmd = null;
    SqlTransaction myTran = null;
    #endregion

    #region text and label data Declaration
    string AddressLine1 = string.Empty;
    string AddressLine2 = string.Empty;
    string AddressLine3 = string.Empty;
    string AddressLine4 = string.Empty;
    string other = string.Empty;
    string BillFooter = string.Empty;
    string bankdetail = string.Empty;
    string dbbackup = string.Empty;
    #endregion

    #region Head part Declaration
    string Created_By = string.Empty;
    string Modified_By = string.Empty;
    string Created_Date = string.Empty;
    string Modified_Date = string.Empty;
    string retValue = string.Empty;
    string strRev = string.Empty;

    int Company_Code = 0;
    int Year_Code = 0;
    string Head_Update = string.Empty;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "tblvoucherheadaddress";
            user = Session["user"].ToString();
            if (!Page.IsPostBack)
            {
                hdnfcompanycode.Value = Session["Company_Code"].ToString();
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    string qry = "Select * from tblvoucherheadaddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                    DataSet ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string comcode = Session["Company_Code"].ToString();
                        if (hdnfcompanycode.Value != comcode)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records company code & current company code is not same !')", true);

                        }
                        else
                        {

                            hdnfcompanycode.Value = ds.Tables[0].Rows[0]["Company_Code"].ToString();
                            DataTable dt = new DataTable();
                            txtAddressLine1.Text = ds.Tables[0].Rows[0]["AL1"].ToString();
                            txtAddressLine2.Text = ds.Tables[0].Rows[0]["AL2"].ToString();
                            txtAddressLine3.Text = ds.Tables[0].Rows[0]["AL3"].ToString();
                            txtAddressLine4.Text = ds.Tables[0].Rows[0]["AL4"].ToString();
                            txtOtherDetail.Text = ds.Tables[0].Rows[0]["Other"].ToString();
                            txtBillFooter.Text = ds.Tables[0].Rows[0]["BillFooter"].ToString();
                            txtbankdetail.Text = ds.Tables[0].Rows[0]["bankdetail"].ToString();
                            txtdbbackup.Text = ds.Tables[0].Rows[0]["dbbackup"].ToString();
                        }
                    }

                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
        }
        catch
        {
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfcompanycode.Value != Session["Company_Code"].ToString())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Selected Records company code & current company code is not same!')", true);
                return;

            }
            DataSet ds = new DataSet();

            #region -Head part declearation
            AddressLine1 = txtAddressLine1.Text;
            AddressLine2 = txtAddressLine2.Text;
            AddressLine3 = txtAddressLine3.Text;
            AddressLine4 = txtAddressLine4.Text;
            other = txtOtherDetail.Text;
            BillFooter = txtBillFooter.Text;
            bankdetail = txtbankdetail.Text;
            dbbackup = txtdbbackup.Text;
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string Head_Fields = string.Empty;
            string Head_Values = string.Empty;
            int flag = 0;
            string op = string.Empty;
            string returnmaxno = string.Empty;
            #endregion

            string s = clsCommon.getString("select ID from tblvoucherheadaddress where Company_Code=" + Convert.ToInt32(Session["Company_Code"]).ToString());
            if (s == "0")
            {
                Head_Fields = Head_Fields + "ID,";
                Head_Values = Head_Values + "'" + 1 + "',";
                Head_Fields = Head_Fields + "AL1,";
                Head_Values = Head_Values + "'" + AddressLine1 + "',";
                Head_Fields = Head_Fields + "AL2,";
                Head_Values = Head_Values + "'" + AddressLine2 + "',";
                Head_Fields = Head_Fields + "AL3,";
                Head_Values = Head_Values + "'" + AddressLine3 + "',";
                Head_Fields = Head_Fields + "AL4,";
                Head_Values = Head_Values + "'" + AddressLine4 + "',";
                Head_Fields = Head_Fields + "Other,";
                Head_Values = Head_Values + "'" + other + "',";
                Head_Fields = Head_Fields + "Company_Code,";
                Head_Values = Head_Values + "'" + Company_Code + "',";
                Head_Fields = Head_Fields + "bankdetail";
                Head_Values = Head_Values + "'" + bankdetail + "',";
                Head_Fields = Head_Fields + "dbbackup";
                Head_Values = Head_Values + "'" + dbbackup + "'";

                flag = 1;
                qry = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";


                Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                thred.Start(); //Thread Operation Start
                thred.Join();

                if (count == 1)
                {
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert(' Record Successfully Saved!')", true);
            }
            else
            {
                string s1 = clsCommon.getString("select ID from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (s1 != string.Empty)
                {
                    #region Create Update Query
                    Head_Update = Head_Update + "AL1=";
                    Head_Update = Head_Update + "'" + AddressLine1 + "',";
                    Head_Update = Head_Update + "AL2=";
                    Head_Update = Head_Update + "'" + AddressLine2 + "',";
                    Head_Update = Head_Update + "AL3=";
                    Head_Update = Head_Update + "'" + AddressLine3 + "',";
                    Head_Update = Head_Update + "AL4=";
                    Head_Update = Head_Update + "'" + AddressLine4 + "',";
                    Head_Update = Head_Update + "Other=";
                    Head_Update = Head_Update + "'" + other + "',";
                    Head_Update = Head_Update + "BillFooter=";
                    Head_Update = Head_Update + "'" + BillFooter + "',";
                    Head_Update = Head_Update + "bankdetail=";
                    Head_Update = Head_Update + "'" + bankdetail + "',";
                    Head_Update = Head_Update + "dbbackup=";
                    Head_Update = Head_Update + "'" + dbbackup + "'";
                    #endregion
                }
                flag = 2;
                qry = "update " + tblHead + " set " + Head_Update + " where Company_Code='" + Company_Code + "' ";
                Thread thred = new Thread(() => { count = DataStore(qry, flag); });
                thred.Start();
                thred.Join();
                if (count == 2)
                {
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
                }
            }
        }

        catch (Exception)
        {
            throw;
        }
    }

    #region DataStore
    private int DataStore(string Query, int flag)
    {
        int count = 0;
        try
        {
            //Connection open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            myTran = con.BeginTransaction();

            if (flag == 1)
            {
                cmd = new SqlCommand(Query, con, myTran);
                cmd.ExecuteNonQuery();
                myTran.Commit();
                Thread.Sleep(100);

                count = 1;
            }
            else if (flag == 2)
            {
                if (Query != "")
                {
                    cmd = new SqlCommand(Query, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                myTran.Commit();
                Thread.Sleep(100);
                count = 2;
            }

            return count;
        }
        catch
        {
            if (myTran != null)
            {
                myTran.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('check Entry AND Try Again !')", true);

            }
            return count;

        }
        finally
        {
            con.Close();
        }

    }
    #endregion

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region [fetchrecord]
    private bool fetchRecord(string qry)
    {
        try
        {

            bool recordExist = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {

                        txtAddressLine1.Text = dt.Rows[0]["AL1"].ToString();
                        txtAddressLine2.Text = dt.Rows[0]["AL2"].ToString();
                        txtAddressLine3.Text = dt.Rows[0]["AL3"].ToString();
                        txtAddressLine4.Text = dt.Rows[0]["AL4"].ToString();
                        txtOtherDetail.Text = dt.Rows[0]["Other"].ToString();
                    }
                }

            }
            recordExist = true;
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion



}