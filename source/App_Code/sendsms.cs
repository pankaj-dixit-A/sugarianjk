using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Data;

/// <summary>
/// Summary description for sendsms
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class sendsms : System.Web.Services.WebService
{
    string respString = string.Empty;
    string Moblie_Number = string.Empty;
    string mobileNo = string.Empty;
    public sendsms()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public int SendSMS(string msg, string mobile, string msgAPI, string senderid, string accusage)
    {
        //string msgAPI = clsCommon.getString("select smsApi from eway_bill where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //string senderid = clsCommon.getString("select Sender_id from eway_bill where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //string accusage = clsCommon.getString("select Accusage from eway_bill where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

       // string msgAPI = clsGV.msgAPI;
        //string URL = msgAPI + "mobile=" + mobile + "&message=" + msg + "&senderid=NAVKAR&accusage=1";
        string URL = msgAPI + "mobile=" + mobile + "&message=" + msg + "&senderid=" + senderid + "&accusage=" + accusage + "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string read = reader.ReadToEnd();
        reader.Close();
        response.Close();
        return 1;
    }
    [WebMethod]
    public void SendWhatsUP(string msg, string mobile, string instanceid, string accesstoken)
    {


        mobileNo = mobile.Trim();
        DataTable table = new DataTable();
        table.Columns.Add("mobno", typeof(string));
        string[] names = mobileNo.Split(',');
        for (int i = 0; i < names.Length; i++)
            table.Rows.Add(new object[] { names[i] });
        if (table.Rows.Count > 0)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Moblie_Number = table.Rows[i]["mobno"].ToString();
                string Url = "https://wawatext.com/api/send.php?number=91" + Moblie_Number + "&type=text&message=" + msg + "&instance_id=" + instanceid + "&access_token=" + accesstoken + "";
                //string Url = "https://wawatext.com/api/send.php?number=91";
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
                HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
                StreamReader reder = new StreamReader(resp.GetResponseStream());
                respString = reder.ReadToEnd();
                reder.Close();
                resp.Close();
                //return 1;
            }
        }
    }
     
}
