using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PATMAPCGIS.loadGisData;

namespace PATMAPCGIS.loadGisData
{
    public partial class Results : System.Web.UI.Page
    {
        //no error catching so far
        protected void Page_Load(object sender, EventArgs e)
        {
            string refreshRate = Page.Request.QueryString["refreshRate"];

            lbl_Results.Text = ThreadResults.getMsgs();

            //Check the status of any thread (any importing process)
            if (ThreadResults.isComplete())
            {
                //goes back to original page after import, passing current requestID
                Response.Redirect(Page.Request.QueryString["redirect"]); 
            }
            else
            {
                //uses separate label to display process taking place, never erasing the current status
                lbl_process.Text = "Processing, Please wait...";
                Response.AddHeader("Refresh", refreshRate);
            }
            
        }
    }
}
