using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Timeplan
{
    public static class WebUtilities
    {
        public static readonly string CHANGE_FOR_GIT_COMMIT = "disabled-dropdown-element";

        public static readonly string CSS_CLASS_DISABLED = "disabled-dropdown-element";

        public static void AdjustScrollPosition(Page page, int positionX, int positionY)
        {
            var newLine = Environment.NewLine;

            if (page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "CreateResetScrollPosition") == false)
            {
                //Create the ResetScrollPosition() function
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CreateResetScrollPosition",
                                 "function ResetScrollPosition() {" + newLine +
                                 " var scrollX = document.getElementById('__SCROLLPOSITIONX');" + newLine +
                                 " var scrollY = document.getElementById('__SCROLLPOSITIONY');" + newLine +
                                 " if (scrollX && scrollY) {" + newLine +
                                 "    scrollX.value = " + positionX + ";" + newLine +
                                 "    scrollY.value = " + positionY + ";" + newLine +
                                 " }" + newLine +
                                 "}", true);

                //Add the call to the ResetScrollPosition() function
                page.ClientScript.RegisterStartupScript(page.GetType(), "CallResetScrollPosition", "ResetScrollPosition();", true);
            }
        }

        public static void ShowMessageBoxPopUp(Page page, string message, string title = "title")
        {
            // TODO: use bootstrap modal instead!
            page.ClientScript.RegisterStartupScript(page.GetType(), title, "alert(\"" + message + "\");", true);
        }

        public static void DisableLinkButton(LinkButton linkButton, string toolTip)
        {
            //LinkButton.DisabledCssClass = string.Empty;

            linkButton.Attributes.Add("readonly", "true");
            linkButton.ToolTip = toolTip;

            //linkButton.Attributes.Add("disabled", "disabled");
           
            //linkButton.Attributes.Remove("href");

            linkButton.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "white";
            linkButton.Attributes.CssStyle[HtmlTextWriterStyle.BackgroundColor] = "#D4D4CA";

            //linkButton.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";

            if (linkButton.Enabled != false)
            {
                linkButton.Enabled = false;
            }

            if (linkButton.OnClientClick != null)
            {
                linkButton.OnClientClick = null;
            }
        }

    }
}
