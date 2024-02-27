using System;
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
using System.Xml.Linq;

public partial class Sugar_Utility_pgeOther_Utility : System.Web.UI.Page
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
    string postdate = string.Empty;
    string inworddate = string.Empty;
    string outworddate = string.Empty;
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
    int year = 0;
    string Head_Update = string.Empty;
    string Head_Insert = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cs = ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString;
            con = new SqlConnection(cs);
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "post_date";
            user = Session["user"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Name from tblUser WHERE User_Name='" + user + "'");
                if (User_Type == "pankaj")
                {

                    txtFrom_Date.Text = clsGV.Start_Date;
                    txtTo_Date.Text = clsGV.To_date;




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

                        //txtDoc_Date.Text = dt.Rows[0]["Post_Date"].ToString();
                        //txtInwordDate.Text = dt.Rows[0]["Inword_Date"].ToString();
                        //txtOutWord.Text = dt.Rows[0]["Outword_Date"].ToString();

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

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["Year_Code"].ToString()) + "'";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

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

    protected void btnSalePosting_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();

            #region -Head part declearations
            postdate = DateTime.Parse(txtFrom_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            inworddate = DateTime.Parse(txtTo_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string Head_Fields = string.Empty;
            string Head_Values = string.Empty;
            int flag = 0;
            string op = string.Empty;
            string returnmaxno = string.Empty;

            string strRef = "";
            string Tran_Type = "SB";
            string ledger = "";
            #endregion

            //qry = "delete from " + tblPrefix + "GLEDGER where TRAN_TYPE='"
            //  + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            //ds = new DataSet();
            //ds = clsDAL.SimpleQuery(qry);



            qry = "";

            //qry = "select t1.*, nt_1_sugarsaledetails.item_code,nt_1_sugarsaledetails.ic,nt_1_systemmaster.Sale_AC,nt_1_companyparameters.* from nt_1_sugarsale t1 cross apply " +
            //    "(select top (1) nt_1_sugarsaledetails.* from nt_1_sugarsaledetails where nt_1_sugarsaledetails.saleid = t1.saleid) nt_1_sugarsaledetails  cross apply " +
            //    "(select nt_1_systemmaster.* from nt_1_systemmaster where nt_1_systemmaster.systemid=nt_1_sugarsaledetails.ic)nt_1_systemmaster cross apply " +
            //    "(select nt_1_companyparameters.* from nt_1_companyparameters where nt_1_companyparameters.Company_Code=t1.Company_Code and nt_1_companyparameters.Year_Code=t1.Year_Code)nt_1_companyparameters " +
            //    "  where t1.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and t1.Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            qry="select t1.*, nt_1_sugarsaledetails.item_code,nt_1_sugarsaledetails.ic,nt_1_sugarsaledetails.rate,nt_1_systemmaster.Sale_AC,nt_1_companyparameters.*, "+
                "nt_1_accountmaster.Short_Name as MillShort_Name,nt_1_accountmaster1.Short_Name as AcShort_Name, "+
                "nt_1_accountmaster2.Short_Name as UnitShort_Name,nt_1_accountmaster3.Short_Name as TransportShort_Name,nt_1_accountmaster4.accoid as Sale_Accoid " + 
                "from nt_1_sugarsale t1 cross apply (select top (1) nt_1_sugarsaledetails.* from nt_1_sugarsaledetails "+
                "where nt_1_sugarsaledetails.saleid = t1.saleid) nt_1_sugarsaledetails "+
                "cross apply (select nt_1_systemmaster.* from nt_1_systemmaster where nt_1_systemmaster.systemid=nt_1_sugarsaledetails.ic)nt_1_systemmaster "+ 
                "cross apply (select nt_1_companyparameters.* from nt_1_companyparameters where nt_1_companyparameters.Company_Code=t1.Company_Code "+
                "and nt_1_companyparameters.Year_Code=t1.Year_Code)nt_1_companyparameters "+
                "cross apply (select accode.* from nt_1_accountmaster accode where accode.accoid=t1.mc )nt_1_accountmaster "+
                "cross apply (select billto.* from nt_1_accountmaster billto where billto.accoid=t1.bt )nt_1_accountmaster1 "+
                "cross apply (select unit.* from nt_1_accountmaster unit where unit.accoid=t1.uc )nt_1_accountmaster2 "+
                "cross apply (select tarnsport.* from nt_1_accountmaster tarnsport where tarnsport.accoid=t1.tc )nt_1_accountmaster3 "+
                "cross apply (select Sale_ACid.* from nt_1_accountmaster Sale_ACid where Sale_ACid.Ac_Code=nt_1_systemmaster.Sale_AC and " +
                "Sale_ACid.company_code=nt_1_systemmaster.Company_Code  )nt_1_accountmaster4 "+
                "where t1.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and t1.Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet dsupdate = new DataSet();
            dsupdate = clsDAL.SimpleQuery(qry);

            if (dsupdate.Tables[0].Rows.Count > 0)
            {
                lbltotcount.Text = dsupdate.Tables[0].Rows.Count.ToString();

                for (int i = 0; i < dsupdate.Tables[0].Rows.Count; i++)
                {
                    lblcount.Text = string.Empty;
                    string MyDoc_No = dsupdate.Tables[0].Rows[i]["doc_no"].ToString();
                    Int32 saleid = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["saleid"].ToString());
                    string MyDoc_Date = DateTime.Parse(dsupdate.Tables[0].Rows[i]["doc_date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    Company_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Company_Code"].ToString());
                    Year_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Year_Code"].ToString());
                    double Bill_Amount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["Bill_Amount"]));
                    double TAXABLEAMOUNT = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["TaxableAmount"]));
                    double CGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["CGSTAmount"]));
                    double SGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["SGSTAmount"]));
                    double IGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["IGSTAmount"]));
                    double cash_advance = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["cash_advance"]));
                    double Roundoff = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["RoundOff"]));
                    double TCS_Amt = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["TCS_Amt"]));
                    double TDS_Amt = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["TDS_Amt"]));
                    Int32 Ac_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Ac_Code"].ToString());
                    Int32 Unit_Code=Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Unit_Code"].ToString());
                    string ac = dsupdate.Tables[0].Rows[i]["ac"].ToString();
                    Int32 SaleAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Sale_Ac"].ToString());
                    string item = dsupdate.Tables[0].Rows[i]["item_code"].ToString();
                    string SaleAcid = dsupdate.Tables[0].Rows[i]["Sale_Accoid"].ToString();
                    Int32 SaleCGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["CGSTAc"].ToString());
                    string SaleCGSTid = Session["SaleCGSTid"].ToString();
                    Int32 SaleSGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["SGSTAc"].ToString());
                    string SaleSGSTid = Session["SaleSGSTid"].ToString();
                    Int32 SaleIGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["IGSTAc"].ToString());
                    string SaleIGSTid = Session["SaleIGSTid"].ToString();
                    Int32 TRANSPORT_CODE = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Transport_Code"].ToString());
                    string tc = dsupdate.Tables[0].Rows[i]["tc"].ToString();
                    Int32 RoundOffAc_Code = Convert.ToInt32(Session["RoundOff"].ToString());
                    string RoundOffid = Session["RoundOffid"].ToString();
                    Int32 SaleTCSAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["SaleTCSAc"].ToString());
                    string SaleTCSAcid = Session["SaleTCSAcid"].ToString();
                    Int32 SaleTDSAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["SaleTDSAc"].ToString());
                    string SaleTDSacid = Session["SaleTDSacid"].ToString();

                    string MillShort_Name=dsupdate.Tables[0].Rows[i]["MillShort_Name"].ToString();
                    string UnitShort_Name=dsupdate.Tables[0].Rows[i]["UnitShort_Name"].ToString();
                    string TransportShort_Name=dsupdate.Tables[0].Rows[i]["TransportShort_Name"].ToString();
                    string AcShort_Name=dsupdate.Tables[0].Rows[i]["AcShort_Name"].ToString();

                   string Creditnara = ""; string Debitnara = "";
                   double rates = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["rate"]);
                   double finalsaleamonut = rates + ((rates * 5) / 100);
                   double NETQNTL = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["NETQNTL"]);
                   double LESS_FRT_RATE = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["LESS_FRT_RATE"]);
                   string LORRYNO = dsupdate.Tables[0].Rows[i]["LORRYNO"].ToString();
                   string PURCno = dsupdate.Tables[0].Rows[i]["PURCNO"].ToString();

                if (Ac_Code == Unit_Code) {
                    Creditnara = "" + MillShort_Name + "  " + NETQNTL + "  L:" + LORRYNO + "  PB:" + PURCno + "  R:" + finalsaleamonut + "  F:" + LESS_FRT_RATE + "";
                    Debitnara = "" + MillShort_Name + "  " + NETQNTL + "  L:" + LORRYNO + "  PB:" + PURCno + "  R:" + finalsaleamonut + "  F:" + LESS_FRT_RATE + "";
                }
                else {
                    Creditnara = "" + MillShort_Name + "  " + NETQNTL + "  L:" + LORRYNO + "  PB:" + PURCno + "  R:" + finalsaleamonut + "  F:" + LESS_FRT_RATE + " ShipToName::" + UnitShort_Name + "";
                    Debitnara = "" + MillShort_Name + "  " + NETQNTL + "  L:" + LORRYNO + "  PB:" + PURCno + "  R:" + finalsaleamonut + "  F:" + LESS_FRT_RATE + "  ShipToName::" + UnitShort_Name + "";
                }
                string saleaccountnarration = "" + MillShort_Name + ", SB:" + AcShort_Name + ", Qntl:" + NETQNTL +
                       ",L:" + LORRYNO + "";
                string TransportNarration = "" + NETQNTL + "  " + cash_advance + "  " + MillShort_Name + "  " + TransportShort_Name + "  Lorry:" + LORRYNO + "  Party:" + AcShort_Name + "";
                string TCSNarration = "TCS" + AcShort_Name + " " + MyDoc_No;
                string TDSNarration = "TDS" + AcShort_Name + " " + MyDoc_No;

                    Int32 Order_Code = 1;


                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        ledger = ledger + "<SaleHead doc_no='" + MyDoc_No + "' saleid='" + saleid + "' Company_Code='" + Company_Code + "' Year_Code='" + Year_Code + "'>";

                        //Credit Effect for Party
                        string rev = "";

                        if (Bill_Amount > 0)
                        {
                            ledger =ledger+ "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                            "UNIT_code='0' NARRATION='" + Debitnara + "' AMOUNT='" + Bill_Amount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                            "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                            "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                            Order_Code = Order_Code + 1;

                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleAc + "' " +
                                                           "UNIT_code='0' NARRATION='" + saleaccountnarration + "' AMOUNT='" + TAXABLEAMOUNT + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                           "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                           "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleAcid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";


                        }
                        if (CGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleCGSTAc + "' " +
                                                             "UNIT_code='0' NARRATION='" + Creditnara + "' AMOUNT='" + CGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                             "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                             "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleCGSTid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }
                        if (SGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='" + Creditnara + "' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleSGSTAc + "' " +
                                                              "UNIT_code='0' NARRATION='" + Creditnara + "' AMOUNT='" + SGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                              "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                              "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleSGSTid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }
                        if (IGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleIGSTAc + "' " +
                                                                "UNIT_code='0' NARRATION='" + Creditnara + "' AMOUNT='" + IGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleIGSTid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }
                        if (cash_advance > 0)
                        {
                            Order_Code = Order_Code + 1;
                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + TRANSPORT_CODE + "' " +
                                                                   "UNIT_code='0' NARRATION='" + TransportNarration + "' AMOUNT='" + cash_advance + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                   "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                   "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + tc + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }
                        if (Roundoff != 0)
                        {
                            if (Roundoff < 0)
                            {
                                Order_Code = Order_Code + 1;


                                ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + RoundOffAc_Code + "' " +
                                                                      "UNIT_code='0' NARRATION='" + Debitnara + "' AMOUNT='" + Roundoff + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                      "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                      "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + RoundOffid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                            }
                            else
                            {
                                Order_Code = Order_Code + 1;


                                ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + RoundOffAc_Code + "' " +
                                                                      "UNIT_code='0' NARRATION='" + Creditnara + "' AMOUNT='" + Roundoff + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                      "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                      "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + RoundOffid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                            }

                        }
                        if (TCS_Amt > 0)
                        {
                            Order_Code = Order_Code + 1;


                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleTCSAc + "' " +
                                                                 "UNIT_code='0' NARRATION='" + TCSNarration + "' AMOUNT='" + TCS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                 "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                 "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleTCSAcid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";

                            Order_Code = Order_Code + 1;


                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                                "UNIT_code='0' NARRATION='" + TCSNarration + "' AMOUNT='" + TCS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }
                        if (TDS_Amt > 0)
                        {
                            Order_Code = Order_Code + 1;

                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                                  "UNIT_code='0' NARRATION='" + TDSNarration + "' AMOUNT='" + TDS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";

                            Order_Code = Order_Code + 1;

                            ledger = ledger + "<Ledger TRAN_TYPE='SB' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + SaleTDSAc + "' " +
                                                                "UNIT_code='0' NARRATION='" + TDSNarration + "' AMOUNT='" + TDS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                "SORT_TYPE='SB' SORT_NO='" + MyDoc_No + "' ac='" + SaleTDSacid + "' vc='0' progid='6' tranid='0' saleid='" + saleid + "'/>";
                        }

                        ledger=ledger+"</SaleHead>";


                        string XMLReport = ledger.ToString();
                        XDocument xDoc = XDocument.Parse(XMLReport, LoadOptions.None);
                        XMLReport = xDoc.ToString(SaveOptions.DisableFormatting);
                        DataSet ds1 = new DataSet();
                        string spname = "SaleBill";
                        string xmlfile = XMLReport;
                        flag = 5;
                        #region [insert]
                        ds1 = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
                        ledger = "";
                       // lblcount.Text = dsupdate.Tables[0].Rows[i].ToString();
                       #endregion
                       

                      


                    }

                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Sucessfully Updated!!!!!');", true);
            return;

        }

        catch (Exception)
        {
            throw;
        }

    }
    protected void btnPurchasePosting_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();

            #region -Head part declearations

            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string Head_Fields = string.Empty;
            string Head_Values = string.Empty;
            int flag = 0;
            string op = string.Empty;
            string returnmaxno = string.Empty;

            string strRef = "";
            string Tran_Type = "PS";
            string purcledger = "";
            #endregion

            qry = " select t1.*, nt_1_sugarpurchasedetails.item_code,nt_1_sugarpurchasedetails.ic,nt_1_sugarpurchasedetails.rate,nt_1_systemmaster.Purchase_AC,nt_1_companyparameters.*, " +
                  " nt_1_accountmaster.Short_Name as MillShortName,nt_1_accountmaster1.Short_Name as PartyShortName,nt_1_accountmaster2.accoid as Purchase_Accoid " +
                  " from nt_1_sugarpurchase t1 cross apply (select top (1) nt_1_sugarpurchasedetails.* from nt_1_sugarpurchasedetails " +
                  " where nt_1_sugarpurchasedetails.purchaseid = t1.purchaseid) nt_1_sugarpurchasedetails " +
                  " cross apply (select nt_1_systemmaster.* from nt_1_systemmaster where nt_1_systemmaster.systemid=nt_1_sugarpurchasedetails.ic)nt_1_systemmaster " +
                  " cross apply (select nt_1_companyparameters.* from nt_1_companyparameters where nt_1_companyparameters.Company_Code=t1.Company_Code " +
                  " and nt_1_companyparameters.Year_Code=t1.Year_Code)nt_1_companyparameters " +
                  " cross apply (select accode.* from nt_1_accountmaster accode where accode.accoid=t1.mc )nt_1_accountmaster " +
                  " cross apply (select billto.* from nt_1_accountmaster billto where billto.accoid=t1.ac )nt_1_accountmaster1 " +
                  " cross apply (select Purchase_ACid.* from nt_1_accountmaster Purchase_ACid where Purchase_ACid.Ac_Code=nt_1_systemmaster.Purchase_AC and " +
                  " Purchase_ACid.company_code=nt_1_systemmaster.Company_Code  )nt_1_accountmaster2 " +
                  " where t1.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and t1.Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            DataSet dsupdate = new DataSet();
            dsupdate = clsDAL.SimpleQuery(qry);

            if (dsupdate.Tables[0].Rows.Count > 0)
            {
                lbltotcount.Text = dsupdate.Tables[0].Rows.Count.ToString();

                for (int i = 0; i < dsupdate.Tables[0].Rows.Count; i++)
                {
                    lblcount.Text = string.Empty;
                    string MyDoc_No = dsupdate.Tables[0].Rows[i]["doc_no"].ToString();
                    Int32 purchaseid = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["purchaseid"].ToString());
                    string MyDoc_Date = DateTime.Parse(dsupdate.Tables[0].Rows[i]["doc_date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    Company_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Company_Code"].ToString());
                    Year_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Year_Code"].ToString());
                    double Bill_Amount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["Bill_Amount"]));
                    double SUBTOTAL = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["SUBTOTAL"]));
                    double CGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["CGSTAmount"]));
                    double SGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["SGSTAmount"]));
                    double IGSTAmount = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["IGSTAmount"]));
                    double cash_advance = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["cash_advance"]));
                    //double Roundoff = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["RoundOff"]));
                    double TCS_Amt = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["TCS_Amt"]));
                    double TDS_Amt = Math.Abs(Convert.ToDouble(dsupdate.Tables[0].Rows[i]["TDS_Amt"]));
                    Int32 Ac_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Ac_Code"].ToString());
                    Int32 Unit_Code = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Unit_Code"].ToString());
                    string ac = dsupdate.Tables[0].Rows[i]["ac"].ToString();
                    Int32 Purchase_AC = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["Purchase_AC"].ToString());
                    string item = dsupdate.Tables[0].Rows[i]["item_code"].ToString();
                    string PurcAcid = dsupdate.Tables[0].Rows[i]["Purchase_Accoid"].ToString();
                    Int32 PurchaseCGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["PurchaseCGSTAc"].ToString());
                    string PurchaseCGSTid = Session["PurchaseCGSTid"].ToString();
                    Int32 PurchaseSGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["PurchaseSGSTAc"].ToString());
                    string PurchaseSGSTid = Session["PurchaseSGSTid"].ToString();
                    Int32 PurchaseIGSTAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["PurchaseIGSTAc"].ToString());
                    string PurchaseIGSTid = Session["PurchaseIGSTid"].ToString();
                    Int32 PurchaseTCSAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["PurchaseTCSAc"].ToString());
                    string PurchaseTCSAcid = Session["PurchaseTCSAcid"].ToString();
                    Int32 PurchaseTDSAc = Convert.ToInt32(dsupdate.Tables[0].Rows[i]["PurchaseTDSAc"].ToString());
                    string PurchaseTDSacid = Session["PurchaseTDSacid"].ToString();

                    string CreditNarration = ""; string DebitNarration = "";
                    double rates = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["rate"]);
                    double Quntal = Convert.ToDouble(dsupdate.Tables[0].Rows[i]["NETQNTL"]);
                    string LORRYNO = dsupdate.Tables[0].Rows[i]["LORRYNO"].ToString();
                    string Grade = dsupdate.Tables[0].Rows[i]["grade"].ToString();

                    string MillShortName = dsupdate.Tables[0].Rows[i]["MillShortName"].ToString();
                    string PartyShortName = dsupdate.Tables[0].Rows[i]["PartyShortName"].ToString();

                    CreditNarration = MillShortName + "L:" + LORRYNO + "G:" + Grade + " " + Quntal + " R:" + rates;
                    DebitNarration = MillShortName + " " + PartyShortName + "L:" + LORRYNO + "G:" + Grade + " " + Quntal + " R:" + rates;
                    var TCSNarration = "TCS" + PartyShortName + " " + MyDoc_No;
                    var TDSNarration = "TDS" + PartyShortName + " " + MyDoc_No;

                    Int32 Order_Code = 1;


                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        purcledger = purcledger + "<PurchaseHead doc_no='" + MyDoc_No + "' purchaseid='" + purchaseid + "' Company_Code='" + Company_Code + "' Year_Code='" + Year_Code + "'>";

                        //Credit Effect for Party
                        string rev = "";

                        if (Bill_Amount > 0)
                        {
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                                    "UNIT_code='0' NARRATION='" + CreditNarration + "' AMOUNT='" + Bill_Amount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                    "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                    "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='5' tranid='0'/>";

                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Purchase_AC + "' " +
                                                                   "UNIT_code='0' NARRATION='" + DebitNarration + "' AMOUNT='" + SUBTOTAL + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                   "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                   "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurcAcid + "' vc='0' progid='5' tranid='0'/>";
                        }

                        if (CGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + PurchaseCGSTAc + "' " +
                                                                  "UNIT_code='0' NARRATION='" + CreditNarration + "' AMOUNT='" + CGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurchaseCGSTid + "' vc='0' progid='5' tranid='0'/>";
                        }

                        if (SGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + PurchaseSGSTAc + "' " +
                                                                  "UNIT_code='0' NARRATION='" + CreditNarration + "' AMOUNT='" + SGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurchaseSGSTid + "' vc='0' progid='5' tranid='0'/>";
                        }

                        if (IGSTAmount > 0)
                        {
                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + PurchaseIGSTAc + "' " +
                                                                  "UNIT_code='0' NARRATION='" + CreditNarration + "' AMOUNT='" + IGSTAmount + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurchaseIGSTid + "' vc='0' progid='5' tranid='0'/>";
                        }

                        if (TCS_Amt > 0)
                        {

                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                                  "UNIT_code='0' NARRATION='" + TCSNarration + "' AMOUNT='" + TCS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='5' tranid='0'/>";

                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + PurchaseTCSAc + "' " +
                                                                  "UNIT_code='0' NARRATION='" + TCSNarration + "' AMOUNT='" + TCS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurchaseTCSAcid + "' vc='0' progid='5' tranid='0'/>";
                        }

                        if (TDS_Amt > 0)
                        {

                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + Ac_Code + "' " +
                                                                  "UNIT_code='0' NARRATION='" + TDSNarration + "' AMOUNT='" + TDS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='D' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + ac + "' vc='0' progid='5' tranid='0'/>";

                            Order_Code = Order_Code + 1;
                            purcledger = purcledger + "<Ledger TRAN_TYPE='PS' CASHCREDIT='' DOC_NO='" + MyDoc_No + "' DOC_DATE='" + MyDoc_Date + "' AC_CODE='" + PurchaseTDSAc + "' " +
                                                                  "UNIT_code='0' NARRATION='" + TDSNarration + "' AMOUNT='" + TDS_Amt + "' TENDER_ID='0' TENDER_ID_DETAIL='0' VOUCHER_ID='0' COMPANY_CODE='" + Company_Code + "' " +
                                                                  "YEAR_CODE='" + Year_Code + "' ORDER_CODE='" + Order_Code + "' DRCR='C' DRCR_HEAD='0' ADJUSTED_AMOUNT='0' Branch_Code='0' " +
                                                                  "SORT_TYPE='PS' SORT_NO='" + MyDoc_No + "' ac='" + PurchaseTDSacid + "' vc='0' progid='5' tranid='0'/>";
                        }


                        purcledger = purcledger + "</PurchaseHead>";

                        string XMLReport = purcledger.ToString();
                        XDocument xDoc = XDocument.Parse(XMLReport, LoadOptions.None);
                        XMLReport = xDoc.ToString(SaveOptions.DisableFormatting);
                        DataSet ds1 = new DataSet();
                        string spname = "PurchaseBill";
                        string xmlfile = XMLReport;
                        flag = 5;
                        #region [insert]
                        ds1 = clsDAL.xmlExecuteDMLQry(spname, xmlfile, ref op, flag, ref returnmaxno);
                        purcledger = "";
                        // lblcount.Text = dsupdate.Tables[0].Rows[i].ToString();
                        #endregion

                    }

                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Sucessfully Updated!!!!!');", true);
            return;

        }
        catch (Exception)
        {
            throw;
        }
    }
}