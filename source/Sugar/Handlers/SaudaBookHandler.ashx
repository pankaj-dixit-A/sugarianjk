<%@ WebHandler Language="C#" Class="SaudaBookHandler" %>  
using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using System.IO;

public class SaudaBookHandler : IHttpHandler {

    string Moblie_Number = string.Empty;
    DataTable table = new DataTable();
    string respString = string.Empty;
     
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";

        string WhatsappNo = context.Request.QueryString["Moblie_Number"].ToString();
        //string message = context.Request.QueryString["message"].ToString();
        string instanceid = context.Request.QueryString["instanceid"].ToString();
        string accesstoken = context.Request.QueryString["accesstoken"].ToString();
        string SALE_ID_NO = context.Request.QueryString["SALE_ID_NO"].ToString();
        string Mill_Name = context.Request.QueryString["Mill_Name"].ToString();
        string QUNTIL = context.Request.QueryString["QUNTIL"].ToString();
        string GRADE = context.Request.QueryString["GRADE"].ToString();
        string SALE_RATE = context.Request.QueryString["SALE_RATE"].ToString();
        string COMMSSION = context.Request.QueryString["COMMSSION"].ToString();
        string SALE_DATE = context.Request.QueryString["SALE_DATE"].ToString();
        string WaTitle = context.Request.QueryString["WaTitle"].ToString();
        string Mobile_No = context.Request.QueryString["Mobile_No"].ToString();


        string message = " Hello, " + Environment.NewLine + " Greetings from *" + WaTitle + "*" + Environment.NewLine + "" + Environment.NewLine + " *SALE ORDER CONFIRMATION:*" + Environment.NewLine +
                 " *SALE ID NO:  " + SALE_ID_NO + "* " + Environment.NewLine + " *MILL NAME:* " + Mill_Name + "" + Environment.NewLine +
                 " *QUINTAL:* " + QUNTIL + "  " + Environment.NewLine + " *GRADE:* " + GRADE + "  " + Environment.NewLine +
                 " *SALE RATE:* " + SALE_RATE + "  " + Environment.NewLine + " *COMMISSION:* " + COMMSSION + "  " + Environment.NewLine + " *SALE DATE:* " + SALE_DATE + " " + Environment.NewLine +
                 " *TRUCK NO:* " + Environment.NewLine + " *QUANTITY:* " + Environment.NewLine +
                 " *BILL TO NAME:* " + Environment.NewLine + " *CITY:* " + Environment.NewLine + " *GST:* " + Environment.NewLine +
                 " *UTR NO:* " + Environment.NewLine + " *AMOUNT:* " + Environment.NewLine + " *DATE:* " + Environment.NewLine +
                 " *TCS AMOUNT:* " + Environment.NewLine + " *TDS AMOUNT:* ";
      
        string mobileNumber = WhatsappNo.Trim();
        table.Columns.Add("mobno", typeof(string));
        table.Columns.Add("filename", typeof(string));
        string[] names = mobileNumber.Split(',');
        for (int i = 0; i < names.Length; i++)
            table.Rows.Add(new object[] { names[i] });
         
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    Moblie_Number = table.Rows[i]["mobno"].ToString();
                    //string Url = "https://wawatext.com/api/send.php?number=91" + Moblie_Number + "&type=text&message=" + message + "&instance_id=" + instanceid + "&access_token=" + accesstoken + "";
                    string Url = "https://wawatext.com/api/send.php?number=91";
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                    HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
                    StreamReader reder = new StreamReader(resp.GetResponseStream());
                    respString = reder.ReadToEnd();
                    reder.Close();
                    resp.Close();
                }

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
                    string data = "Yes";
                    context.Response.Write(data);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Successfully Sent!');", true);
                }
                else
                {
                    string data = "No";
                    context.Response.Write(data);
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Cloud Not Sent!');", true);
                }
            }
             
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}