using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace PATMAPCGIS.loadGisData
{
    public sealed class ThreadResults
    {
        private static string msgs = string.Empty;
        private static bool _complete = true;
        private static SqlCommand currentCommand = null;
        private static TimeSpan _runtime = TimeSpan.Zero;
        //flag to indicate whether an operation is complete
        public static void setComplete(bool complete, TimeSpan runtime)
        {
            _complete = complete;
            _runtime = runtime;
        }
        public static bool isComplete()
        {
            return _complete;
        }

        public static string getMsgs()
        {
            return msgs;
        }
        public static TimeSpan getRuntime()
        {
            return _runtime;
        }
        public static void addMsg(string message)
        {
            msgs = msgs + message + Environment.NewLine;
        }
        public static void clear()
        {
            msgs = string.Empty;
        }
        public static void stop()
        {
            if (currentCommand != null)
            {
                currentCommand.Cancel();
            }
        }
    }
}
