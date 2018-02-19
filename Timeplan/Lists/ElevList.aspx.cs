using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class ElevList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdjustScrollPosition();

            if (IsPostBack == false)
            {
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();

                ViewState["dummyId"] = 0;

                if (Session["ElevList - hidedColumns"] == null)
                {
                    Session["ElevList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Elev> allElever = Elev.GetAll().ToList();

                Session["ElevList - SortDirection"] = Session["ElevList - SortDirection"] ?? SortDirection.Ascending;
                Session["ElevList - SortExpression"] = Session["ElevList - SortExpression"] ?? "Navn";

                UpdateBinding(allElever, sort: true);

                //stopwatch.Stop();
            }

        }

        private void AdjustScrollPosition()
        {
            if (IsPostBack == false)
            {
                //Default to 0
                ScrollPosition.Value = "0";

                //if ScrollPosition session variable is null, store the default
                if (Session["ElevList - ScrollPosition"] == null)
                {
                    Session.Add("ElevList - ScrollPosition", ScrollPosition.Value);
                }
                //else set the scroll position to the value stored in session.
                else
                {
                    ScrollPosition.Value = Session["ElevList - ScrollPosition"].ToString();
                }
            }
            else
            {
                //On subsequent postbacks store the new scroll position
                Session.Add("ElevList - ScrollPosition", ScrollPosition.Value);
            }
        }


        private void BindToListView(IList<Elev> allElever)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var klasser = Klasse.GetAll().OrderBy(k => k.Navn).ToList();
            var sfoes = Sfo.GetAll().OrderBy(s => s.Navn).ToList();
            var trinn = Trinn.GetAll().OrderBy(t => Utilities.DigitPart(t.Navn)).ToList();
            var ansatte = Ansatt.GetAll().Where(a => a.StillingsType.Id == (int)StillingsTypeEnum.Pedagog).OrderBy(a => a.Navn).ToList();
            var bemanningsNorm = BemanningsNorm.GetAll().OrderBy(b => b.Navn).ToList();

            stopwatch.Stop();

            var elevViewList = new List<object>();

            foreach (var elev in allElever)
            {
                var sfoProsent = (elev.SfoProsent % 1 == 0) ? elev.SfoProsent.ToString("N0") : elev.SfoProsent.ToString("N2");
                var skoleTimerPrUke = elev.Trinn != null ? elev.Trinn.UkeTimeTall : trinn.First().UkeTimeTall;
                elevViewList.Add(new
                {
                    elev.Id,
                    elev.Navn,
                    skoleTimerPrUke,
                    sfoProsent
                });
            }

            ElevListView.DataSource = elevViewList;
            ElevListView.DataBind();



            foreach (var item in ElevListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var elev = allElever.First(e => e.Id.ToString() == idButton.Text);

                    var klasseDropDown = (DropDownList)item.FindControl("KlasseDropDown");
                    klasseDropDown.DataSource = klasser;
                    klasseDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Klasse.Id);
                    klasseDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Klasse.Navn);
                    klasseDropDown.DataBind();

                    if (elev.Klasse != null)
                        klasseDropDown.SelectedValue = elev.Klasse.Id.ToString();

                    var sfoDropDown = (DropDownList)item.FindControl("SfoDropDown");
                    sfoDropDown.DataSource = sfoes;
                    sfoDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Sfo.Id);
                    sfoDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Sfo.Navn);
                    sfoDropDown.DataBind();

                    // Default value
                    sfoDropDown.Items.Insert(0, new ListItem("Velg sfo", "0"));
                    sfoDropDown.SelectedValue = elev.Sfo != null ? elev.Sfo.Id.ToString() : "0";

                    var trinnDropDown = (DropDownList)item.FindControl("TrinnDropDown");
                    trinnDropDown.DataSource = trinn;
                    trinnDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Trinn.Id);
                    trinnDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Trinn.Navn);
                    trinnDropDown.DataBind();

                    if (elev.Trinn != null)
                        trinnDropDown.SelectedValue = elev.Trinn.Id.ToString();

                    var hovedPedagogDropDown = (DropDownList)item.FindControl("HovedPedagogDropDown");
                    hovedPedagogDropDown.DataSource = ansatte;
                    hovedPedagogDropDown.DataValueField = Utilities.GetPropertyName(() => elev.HovedPedagog.Id);
                    hovedPedagogDropDown.DataTextField = Utilities.GetPropertyName(() => elev.HovedPedagog.Navn);
                    hovedPedagogDropDown.DataBind();

                    if (elev.HovedPedagog != null)
                        hovedPedagogDropDown.SelectedValue = elev.HovedPedagog.Id.ToString();

                    var bemanningsNormSkoleDropDown = (DropDownList)item.FindControl("BemanningsNormSkoleDropDown");
                    bemanningsNormSkoleDropDown.DataSource = bemanningsNorm;
                    bemanningsNormSkoleDropDown.DataValueField = Utilities.GetPropertyName(() => elev.BemanningsNormSkole.Id);
                    bemanningsNormSkoleDropDown.DataTextField = Utilities.GetPropertyName(() => elev.BemanningsNormSkole.Navn);
                    bemanningsNormSkoleDropDown.DataBind();
                    bemanningsNormSkoleDropDown.SelectedValue = elev.BemanningsNormSkole != null ? elev.BemanningsNormSkole.Id.ToString() : ((int)BemanningsNormEnum.OneToOne).ToString();

                    var bemanningsNormSfoDropDown = (DropDownList)item.FindControl("BemanningsNormSfoDropDown");
                    bemanningsNormSfoDropDown.DataSource = bemanningsNorm;
                    bemanningsNormSfoDropDown.DataValueField = Utilities.GetPropertyName(() => elev.BemanningsNormSfo.Id);
                    bemanningsNormSfoDropDown.DataTextField = Utilities.GetPropertyName(() => elev.BemanningsNormSfo.Navn);
                    bemanningsNormSfoDropDown.DataBind();

                    // Default value
                    bemanningsNormSfoDropDown.Items.Insert(0, new ListItem("Velg BN", "0"));
                    bemanningsNormSfoDropDown.SelectedValue = elev.BemanningsNormSfo != null ? elev.BemanningsNormSfo.Id.ToString() : "0";
                }
            }

            //stopwatch.Stop();

        }


        private void SaveAllElever()
        {
            var allElever = UpdateDataSource();

            if (allElever.Any(elev => elev.IsChanged))
            {
                foreach (var elev in allElever)
                {
                    if (elev.IsChanged)
                    {
                        elev.Save();
                    }
                }

                allElever = Elev.GetAll().ToList();

                UpdateBinding(allElever, sort: true);
            }
        }


        private IList<Elev> UpdateDataSource()
        {
            var allElever = (IList<Elev>)Session["allElever"];

            foreach (var item in ElevListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var sfoProsentTextBox = (TextBox)item.FindControl("SfoProsentTextBox");
                    var klasseDropDown = (DropDownList)item.FindControl("KlasseDropDown");
                    var sfoDropDown = (DropDownList)item.FindControl("SfoDropDown");
                    var trinnDropDown = (DropDownList)item.FindControl("TrinnDropDown");
                    var hovedPedagogDropDown = (DropDownList)item.FindControl("HovedPedagogDropDown");
                    var bemanningsNormSkoleDropDown = (DropDownList)item.FindControl("BemanningsNormSkoleDropDown");
                    var bemanningsNormSfoDropDown = (DropDownList)item.FindControl("BemanningsNormSfoDropDown");

                    var id = Convert.ToInt32(idButton.Text);

                    var elev = allElever.First(e => e.Id == id);

                    elev.Update(
                        navnTextBox.Text,
                        Convert.ToDecimal(sfoProsentTextBox.Text),
                        Convert.ToInt32(klasseDropDown.SelectedValue),
                        Convert.ToInt32(sfoDropDown.SelectedValue),
                        Convert.ToInt32(trinnDropDown.SelectedValue),
                        Convert.ToInt32(hovedPedagogDropDown.SelectedValue),
                        Convert.ToInt32(bemanningsNormSkoleDropDown.SelectedValue),
                        Convert.ToInt32(bemanningsNormSfoDropDown.SelectedValue)
                        );
                }
            }

            return allElever;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllElever();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var allElever = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                var elev = allElever.First(a => a.Id == id);
                if (elev.IsChanged)
                {
                    elev.Save();

                    //if (elev.ElevTilstedes.Count == 0)
                    //{
                    //    AddDefaultElevTilstede(elev);
                    //}
                }

                UpdateBinding(allElever, sort: false);
            }
        }

        //private void AddDefaultElevTilstede(Elev elev)
        //{
        //    var defaultElevTilstede = new ElevTilstede
        //    {
        //        MandagStart = elev.Trinn.MandagStart,
        //        MandagSlutt = elev.Trinn.MandagSlutt,
        //        TirsdagStart = elev.Trinn.TirsdagStart,
        //        TirsdagSlutt = elev.Trinn.TirsdagSlutt,
        //        OnsdagStart = elev.Trinn.OnsdagStart,
        //        OnsdagSlutt = elev.Trinn.OnsdagSlutt,
        //        TorsdagStart = elev.Trinn.TorsdagStart,
        //        TorsdagSlutt = elev.Trinn.TorsdagSlutt,
        //        FredagStart = elev.Trinn.FredagStart,
        //        FredagSlutt = elev.Trinn.FredagSlutt,
        //        Elev = elev,
        //        UkeType = UkeType.GetById((int)UkeTypeEnum.LikUke)
        //    };

        //    defaultElevTilstede.Save();
        //}

        private void UpdateBinding(IList<Elev> allElever, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["ElevList - SortDirection"];
                var sortExpression = Session["ElevList - SortExpression"];
                allElever = SortAllElever(allElever, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allElever"] = allElever;

            BindToListView(allElever);

            var hidedColumns = (Dictionary<string, string>)Session["ElevList - hidedColumns"];

            if (hidedColumns != null)
            {
                foreach (var column in hidedColumns)
                {
                    HideShow(column.Key, column.Value);
                }
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["ElevList - SortDirection"];
            var sortExpression = Session["ElevList - SortExpression"].ToString();

            if (sortExpression == e.SortExpression)
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
            else
            {
                sortDirection = SortDirection.Ascending;
            }

            Session["ElevList - SortDirection"] = sortDirection;
            Session["ElevList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }


        private IList<Elev> SortAllElever(IEnumerable<Elev> allElever, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Elev> result = new List<Elev>();

            var propertyInfo = typeof(Elev).GetProperty(sortExpression);

            if (sortExpression == "Klasse")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.Klasse != null ? elev.Klasse.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.Klasse != null ? elev.Klasse.Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "SkoleTimerPrUke")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.Trinn != null ? elev.Trinn.UkeTimeTall : 0).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.Trinn != null ? elev.Trinn.UkeTimeTall : 0).ToList();
                }
            }
            else if (sortExpression == "Sfo")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.Sfo != null ? elev.Sfo.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.Sfo != null ? elev.Sfo.Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "Trinn")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.Trinn != null ? Utilities.DigitPart(elev.Trinn.Navn) : 0).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.Trinn != null ? Utilities.DigitPart(elev.Trinn.Navn) : 0).ToList();
                }
            }
            else if (sortExpression == "HovedPedagog")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.HovedPedagog != null ? elev.HovedPedagog.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.HovedPedagog != null ? elev.HovedPedagog.Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "BemanningsNormSkole")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.BemanningsNormSkole != null ? elev.BemanningsNormSkole.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.BemanningsNormSkole != null ? elev.BemanningsNormSkole.Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "BemanningsNormSfo")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => elev.BemanningsNormSfo != null ? elev.BemanningsNormSfo.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => elev.BemanningsNormSfo != null ? elev.BemanningsNormSfo.Navn : string.Empty).ToList();
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allElever.OrderBy(elev => propertyInfo.GetValue(elev)).ToList();
                }
                else
                {
                    result = allElever.OrderByDescending(elev => propertyInfo.GetValue(elev)).ToList();
                }
            }

            return result.ToList();
        }


        protected void HideShowLinkButton_OnClick(object sender, EventArgs e)
        {
            var linkButton = (LinkButton)sender;

            var linkButtonId = linkButton.ID;

            var tableHeader = string.Empty;
            var tableData = string.Empty;

            if (linkButtonId == "HideShowIdLinkButton")
            {
                tableHeader = "IdLinkButton";
                tableData = "IdButton";
            }
            else if (linkButtonId == "HideShowNavnLinkButton")
            {
                tableHeader = "NavnLinkButton";
                tableData = "NavnTextBox";
            }
            else if (linkButtonId == "HideShowSkoleTimerPrUkeLinkButton")
            {
                tableHeader = "SkoleTimerPrUkeLinkButton";
                tableData = "SkoleTimerPrUkeTextBox";
            }
            else if (linkButtonId == "HideShowSfoProsentLinkButton")
            {
                tableHeader = "SfoProsentLinkButton";
                tableData = "SfoProsentTextBox";
            }
            //else if (linkButtonId == "HideShowTlfNrLinkButton")
            //{
            //    tableHeader = "TlfNrLinkButton";
            //    tableData = "TlfNrTextBox";
            //}
            else if (linkButtonId == "HideShowKlasseLinkButton")
            {
                tableHeader = "KlasseLinkButton";
                tableData = "KlasseDropDown";
            }
            else if (linkButtonId == "HideShowTrinnLinkButton")
            {
                tableHeader = "TrinnLinkButton";
                tableData = "TrinnDropDown";
            }
            else if (linkButtonId == "HideShowHovedPedagogLinkButton")
            {
                tableHeader = "HovedPedagogLinkButton";
                tableData = "HovedPedagogDropDown";
            }
            else if (linkButtonId == "HideShowBemanningsNormSkoleLinkButton")
            {
                tableHeader = "BemanningsNormSkoleLinkButton";
                tableData = "BemanningsNormSkoleDropDown";
            }
            else if (linkButtonId == "HideShowBemanningsNormSfoLinkButton")
            {
                tableHeader = "BemanningsNormSfoLinkButton";
                tableData = "BemanningsNormSfoDropDown";
            }
            else if (linkButtonId == "HideShowSfoLinkButton")
            {
                tableHeader = "SfoLinkButton";
                tableData = "SfoDropDown";
            }
            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["ElevList - hidedColumns"];

                if (hidedColumns.ContainsKey(tableHeader))
                {
                    hidedColumns.Remove(tableHeader);
                }
                else
                {
                    hidedColumns.Add(tableHeader, tableData);
                }
            }

        }


        private void HideShow(string tableHeader, string tableData)
        {
            var button = ElevListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in ElevListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var control = item.FindControl(tableData);
                    tableHeaderVisible = !control.Visible;
                    control.Visible = tableHeaderVisible;
                }
            }

            button.Visible = tableHeaderVisible;
        }


        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var allElever = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                var elev = allElever.First(a => a.Id == id);
                elev.Delete();

                allElever.Remove(elev);

                UpdateBinding(allElever, sort: false);
            }
        }


        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var allElever = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                var elevForEdit = allElever.First(a => a.Id == id);
                if (elevForEdit.IsChanged)
                {
                    var elev = elevForEdit.Save();
                    id = elev.Id;

                    //if (elevForEdit.ElevTilstedes.Count == 0)
                    //{
                    //    AddDefaultElevTilstede(elevForEdit);
                    //}
                }

                if (allElever.Any(elev => elev.IsChanged))
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else if (Elev.GetById(id) == null)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Elev '" + elevForEdit.Navn + "' finnes ikke lenger i systemet!");

                    allElever.Remove(elevForEdit);
                    UpdateBinding(allElever, sort: false);
                }
                else
                {
                    Response.Redirect("~/Details/ElevDetails.aspx?Id=" + HttpUtility.UrlEncode(elevForEdit.Id.ToString()), false);
                }
            }

        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allElever = Elev.GetAll().ToList();
            UpdateBinding(allElever, sort: true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var allElever = UpdateDataSource();

                var dummyId = (int)ViewState["dummyId"];

                var elev = new Elev { Id = dummyId, IsChanged = true };

                allElever.Add(elev);

                //Response.Redirect("AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode(dummyId.ToString()));

                ViewState["dummyId"] = --dummyId;

                UpdateBinding(allElever, sort: false);

                //MaintainScrollPositionOnPostBack = false;
                //ClientScript.RegisterStartupScript(GetType(), "ScrollScript", "window.scrollTo(0, document.body.scrollHeight);", true);

                WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
            }
        }

        //private void AdjustScrollPosition(int positionX, int positionY)
        //{
        //    var newLine = Environment.NewLine;

        //    if (!ClientScript.IsClientScriptBlockRegistered(this.GetType(), "CreateResetScrollPosition"))
        //    {
        //        //Create the ResetScrollPosition() function
        //        ClientScript.RegisterClientScriptBlock(GetType(), "CreateResetScrollPosition",
        //                         "function ResetScrollPosition() {" + newLine +
        //                         " var scrollX = document.getElementById('__SCROLLPOSITIONX');" + newLine +
        //                         " var scrollY = document.getElementById('__SCROLLPOSITIONY');" + newLine +
        //                         " if (scrollX && scrollY) {" + newLine +
        //                         "    scrollX.value = " + positionX + ";" + newLine +
        //                         "    scrollY.value = " + positionY + ";" + newLine +
        //                         " }" + newLine +
        //                         "}", true);

        //        //Add the call to the ResetScrollPosition() function
        //        ClientScript.RegisterStartupScript(GetType(), "CallResetScrollPosition", "ResetScrollPosition();", true);
        //    }
        //}

        #endregion

    }
}