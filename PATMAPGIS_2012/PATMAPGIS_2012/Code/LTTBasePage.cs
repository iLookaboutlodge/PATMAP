using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;

/// <summary>
/// BasePage controls the security of every page within the application.
/// All pages inherit from BasePage
/// </summary>
public class LTTBasePage : System.Web.UI.Page
{
    public LTTBasePage()
    {

        this.Init += new EventHandler(BasePage_Init);
        this.Error += new EventHandler(BasePage_Error);
    }

    void BasePage_Init(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Session["levelID"] == null)
        {
            //this.ClientScript.RegisterStartupScript(this.GetType(), "logout", "top.document.location = 'index.aspx';", true);
            Response.Write("<SCRIPT>top.document.location = 'index.aspx';</SCRIPT>");
            this.Response.Flush();
            this.Response.End();
        }
        else
        {
            //this.Page.ErrorPage = "MapError.aspx";
            try
            {
                string errorCode = PATMAP.common.HasAccess(int.Parse(Session["levelID"].ToString()), "TaxTools/Map.aspx");

                if (errorCode != "")
                {
                    Session["responseCode"] = errorCode;
                    Response.Redirect("~/MapError.aspx");
                }
            }
            catch (ThreadAbortException)
            {
                //continue
            }
            catch (Exception ex)
            {
                Session["responseCode"] = ex.Message;
                Response.Redirect("~/MapError.aspx");
            }
        }
    }

    void BasePage_Error(object sender, EventArgs e)
    {
        Exception error = Server.GetLastError();
        Session.Add("responseCode", error.ToString());
        Response.Redirect("/error.aspx");
    }
}
