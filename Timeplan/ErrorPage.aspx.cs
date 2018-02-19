using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO: change aspx and this file to reasonable design :)

            // Create safe error messages.
            string generalErrorMsg = "Hva har du gjort??? Nei, nei, nei... nå har det gått fryktelig galt i systemet!!! Vær så vennlig å ikke gjøre dette til en vane!!!";
            string httpErrorMsg = "Siden ble ikke funnet, vennligst prøv igjen";
            string unhandledErrorMsg = "Denne feilen ble ikke håndtert av definerte feilsjekker! Følg opp!";

            // Display safe error message.
            FriendlyErrorMsg.Text = generalErrorMsg;

            // Determine where error was handled.
            string errorHandler = Request.QueryString["handler"];
            if (errorHandler == null)
            {
                errorHandler = "Feilside";
            }

            // Get the last error from the server.
            Exception ex = Server.GetLastError();

            // Get the error number passed as a querystring value.
            string errorMsg = Request.QueryString["msg"];
            if (errorMsg == "404")
            {
                ex = new HttpException(404, httpErrorMsg, ex);
                FriendlyErrorMsg.Text = ex.Message;
            }

            // If the exception no longer exists, create a generic exception.
            if (ex == null)
            {
                ex = new Exception(unhandledErrorMsg);
            }

            // Show error details to only you (developer). LOCAL ACCESS ONLY.
            if (Request.IsLocal)
            {
                // Detailed Error Message.
                ErrorDetailedMsg.Text = ex.Message;

                // Show where the error was handled.
                ErrorHandler.Text = errorHandler;

                // Show local access details.
                DetailedErrorPanel.Visible = true;

                if (ex.InnerException != null)
                {
                    InnerMessage.Text = ex.GetType().ToString() + "<br/>" +
                        ex.InnerException.Message;
                    InnerTrace.Text = ex.InnerException.ToString();
                }
                else
                {
                    InnerMessage.Text = ex.GetType().ToString() + "<br/>" +
                        ex.Message;
                    if (ex.StackTrace != null)
                    {
                        InnerTrace.Text = ex.StackTrace.ToString().TrimStart();
                    }
                }
            }

            // Log the exception, automatically logged to Windows Application Eventlog if web.config customerrors is on!
            Utilities.LogException("Timeplan", ex.ToString(), EventLogEntryType.Error);

            // Clear the error from the server.
            Server.ClearError();
        }
    }
}