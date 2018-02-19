using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class AnsattList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AdjustScrollPosition();

            if (IsPostBack == false)
            {
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();

                ViewState["dummyId"] = 0;

                if (Session["AnsattList - hidedColumns"] == null)
                {
                    Session["AnsattList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Ansatt> allAnsatte = Ansatt.GetAll().ToList();

                Session["AnsattList - SortDirection"] = Session["AnsattList - SortDirection"] ?? SortDirection.Ascending;
                Session["AnsattList - SortExpression"] = Session["AnsattList - SortExpression"] ?? "Navn";

                UpdateBinding(allAnsatte, sort: true);

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
                if (Session["AnsattList - ScrollPosition"] == null)
                {
                    Session.Add("AnsattList - ScrollPosition", ScrollPosition.Value);
                }
                //else set the scroll position to the value stored in session.
                else
                {
                    ScrollPosition.Value = Session["AnsattList - ScrollPosition"].ToString();
                }
            }
            else
            {
                //On subsequent postbacks store the new scroll position
                Session.Add("AnsattList - ScrollPosition", ScrollPosition.Value);
            }
        }

        private void BindToListView(IList<Ansatt> allAnsatte)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            var avdelinger = Avdeling.GetAll().OrderBy(a => a.Navn).ToList();
            var stillingsTyper = StillingsType.GetAll().OrderBy(s => s.Navn).ToList();
            var klasser = Klasse.GetAll().OrderBy(k => k.Navn).ToList();
            var sfos = Sfo.GetAll().OrderBy(s => s.Navn).ToList();

            var ansattViewList = new List<object>();

            foreach (var ansatt in allAnsatte)
            {
                ansattViewList.Add(new
                {
                    ansatt.Id,
                    ansatt.Navn,
                    ansatt.Stillingsstørrelse,
                    ansatt.Tlfnr
                });
            }

            AnsattListView.DataSource = ansattViewList;
            AnsattListView.DataBind();

            foreach (var item in AnsattListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");

                    var avdelingDropDown = (DropDownList)item.FindControl("AvdelingDropDown");

                    var ansatt = allAnsatte.First(a => a.Id.ToString() == idButton.Text);

                    avdelingDropDown.DataSource = avdelinger;
                    avdelingDropDown.DataValueField = Utilities.GetPropertyName(() => ansatt.Avdeling.Id);
                    avdelingDropDown.DataTextField = Utilities.GetPropertyName(() => ansatt.Avdeling.Navn);
                    avdelingDropDown.DataBind();

                    if (ansatt.Avdeling != null)
                        avdelingDropDown.SelectedValue = ansatt.Avdeling.Id.ToString();

                    var stillingsTypeDropDown = (DropDownList)item.FindControl("StillingsTypeDropDown");
                    stillingsTypeDropDown.DataSource = stillingsTyper;
                    stillingsTypeDropDown.DataValueField = Utilities.GetPropertyName(() => ansatt.StillingsType.Id);
                    stillingsTypeDropDown.DataTextField = Utilities.GetPropertyName(() => ansatt.StillingsType.Navn);
                    stillingsTypeDropDown.DataBind();

                    if (ansatt.StillingsType != null)
                        stillingsTypeDropDown.SelectedValue = ansatt.StillingsType.Id.ToString();

                    var jobberIKlasserListBox = (ListControl)item.FindControl("JobberIKlasserListBox");
                    jobberIKlasserListBox.Items.Clear();

                    var klasseTeller = 0;
                    var klasseSelectedTeller = 0;
                    foreach (var klasse in klasser)
                    {
                        var listItem = new ListItem(klasse.Navn, klasse.Id.ToString());

                        if (ansatt.JobberIKlasser.Any(k => k.Id == klasse.Id))
                        {
                            listItem.Selected = true;
                        }

                        if (listItem.Selected)
                        {
                            jobberIKlasserListBox.Items.Insert(klasseSelectedTeller++, listItem);
                            klasseTeller++;
                        }
                        else
                        {
                            jobberIKlasserListBox.Items.Insert(klasseTeller++, listItem);
                        }
                    }

                    var jobberISfosListBox = (ListControl)item.FindControl("JobberISfosListBox");
                    jobberISfosListBox.Items.Clear();

                    var sfoTeller = 0;
                    var sfoSelectedTeller = 0;
                    foreach (var sfo in sfos)
                    {
                        var listItem = new ListItem(sfo.Navn, sfo.Id.ToString());

                        if (ansatt.JobberISfos.Any(s => s.Id == sfo.Id))
                        {
                            listItem.Selected = true;
                        }

                        if (listItem.Selected)
                        {
                            jobberISfosListBox.Items.Insert(sfoSelectedTeller++, listItem);
                            sfoTeller++;
                        }
                        else
                        {
                            jobberISfosListBox.Items.Insert(sfoTeller++, listItem);
                        }
                    }

                    var avdelingsLederIAvdelinger = ansatt.AvdelingsLederIAvdelinger.Count > 0;
                    var hovedPedagogForElever = ansatt.HovedPedagogForElever.Count > 0;

                    if (avdelingsLederIAvdelinger || hovedPedagogForElever)
                    {
                        string message;

                        if (avdelingsLederIAvdelinger)
                        {
                            message = @"Avdelinger hvor '" + ansatt.Navn + "' er avdelingsleder må tilordnes annen avdelingsleder før '" + ansatt.Navn + "' kan slettes.";
                        }
                        else
                        {
                            message = @"Elever som har '" + ansatt.Navn + "' som hovedpedagog må tilordnes annen hovedpedagog før '" + ansatt.Navn + "' kan slettes.";
                        }

                        var deleteLinkButton = (LinkButton)item.FindControl("DeleteLinkButton");
                        WebUtilities.DisableLinkButton(deleteLinkButton, message);
                    }

                    //var avdelingsLederIAvdelingerListBox = (ListBox)item.FindControl("AvdelingsLederIAvdelingerListBox");
                    //var avdelingTeller = 0;
                    //foreach (var avdeling in avdelinger)
                    //{
                    //    var listItem = new ListItem(avdeling.Navn, avdeling.Id.ToString());

                    //    if (ansatt.AvdelingsLederIAvdelinger.Any(a => a.Id == avdeling.Id))
                    //    {
                    //        listItem.Selected = true;
                    //    }

                    //    avdelingsLederIAvdelingerListBox.Items.Insert(avdelingTeller++, listItem);
                    //}

                    //var yearsDropDownCheckBoxes = (DropDownCheckBoxes)item.FindControl("DropDownCheckBoxes1");
                    //var klasseTeller1 = 0;
                    //foreach (var klasse in klasser)
                    //{
                    //    var listItem = new ListItem(klasse.Navn, klasse.Id.ToString());

                    //    if (ansatt.JobberIKlasser.Any(k => k.Id == klasse.Id))
                    //    {
                    //        listItem.Selected = true;
                    //    }

                    //    yearsDropDownCheckBoxes.Items.Insert(klasseTeller1++, listItem);
                    //}

                    //yearsDropDownCheckBoxes.CssClass = "form-control";

                    //var varslesAvAnsattDropDown = (DropDownList)item.FindControl("VarslesAvAnsattDropDown");
                    //varslesAvAnsattDropDown.DataSource = ansatte;
                    //varslesAvAnsattDropDown.DataValueField = Utilities.GetPropertyName(() => ansatt.VarslesAvAnsatt.Id);
                    //varslesAvAnsattDropDown.DataTextField = Utilities.GetPropertyName(() => ansatt.VarslesAvAnsatt.Navn);
                    //varslesAvAnsattDropDown.DataBind();

                    //// Default value
                    //varslesAvAnsattDropDown.Items.Insert(0, new ListItem("Velg varsling", "0"));
                    //varslesAvAnsattDropDown.SelectedValue = ansatt.VarslesAvAnsatt != null ? ansatt.VarslesAvAnsatt.Id.ToString() : "0";
                }
            }
            //stopwatch.Stop();
        }


        private void SaveAllAnsatte()
        {
            var allAnsatte = UpdateDataSource();

            if (allAnsatte.Any(ansatt => ansatt.IsChanged))
            {
                foreach (var ansatt in allAnsatte)
                {
                    if (ansatt.IsChanged)
                    {
                        ansatt.Save();
                    }
                }

                allAnsatte = Ansatt.GetAll().ToList();

                UpdateBinding(allAnsatte, sort: true);
            }
        }


        private IList<Ansatt> UpdateDataSource()
        {
            var allAnsatte = (IList<Ansatt>)Session["allAnsatte"];

            foreach (var item in AnsattListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var stillingsStørrelseTextBox = (TextBox)item.FindControl("StillingsStørrelseTextBox");
                    var tlfNrTextBox = (TextBox)item.FindControl("TlfNrTextBox");
                    var avdelingDropDown = (DropDownList)item.FindControl("AvdelingDropDown");
                    var stillingsTypeDropDown = (DropDownList)item.FindControl("StillingsTypeDropDown");
                    var jobberIKlasserListBox = (ListControl)item.FindControl("JobberIKlasserListBox");
                    var jobberISfosListBox = (ListControl)item.FindControl("JobberISfosListBox");
                    //var avdelingsLederIAvdelingerListBox = (ListBox)item.FindControl("AvdelingsLederIAvdelingerListBox");
                    //var varslesAvAnsattDropDown = (DropDownList)item.FindControl("VarslesAvAnsattDropDown");

                    IList<int> jobberIKlasserIds =
                        (from ListItem listItem in jobberIKlasserListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    IList<int> jobberISfoIds =
                        (from ListItem listItem in jobberISfosListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    //IList<int> avdelingsLederIAvdelingIds =
                    //    (from ListItem listItem in avdelingsLederIAvdelingerListBox.Items
                    //     where listItem.Selected
                    //     select Convert.ToInt32(listItem.Value)).ToList();

                    var id = Convert.ToInt32(idButton.Text);

                    var ansatt = allAnsatte.First(a => a.Id == id);

                    ansatt.Update(
                        navnTextBox.Text,
                        Convert.ToDecimal(stillingsStørrelseTextBox.Text),
                        tlfNrTextBox.Text,
                        Convert.ToInt32(avdelingDropDown.SelectedValue),
                        Convert.ToInt32(stillingsTypeDropDown.SelectedValue),
                        jobberIKlasserIds,
                        jobberISfoIds
                        //avdelingsLederIAvdelingIds
                        //Convert.ToInt32(varslesAvAnsattDropDown.SelectedValue),
                        );
                }
            }

            return allAnsatte;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllAnsatte();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allAnsatte = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var ansatt = allAnsatte.First(a => a.Id == id);
            if (ansatt.IsChanged)
            {
                ansatt.Save();
            }

            UpdateBinding(allAnsatte, sort:false);
        }

        private void UpdateBinding(IList<Ansatt> allAnsatte, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["AnsattList - SortDirection"];
                var sortExpression = Session["AnsattList - SortExpression"];
                allAnsatte = SortAllAnsatte(allAnsatte, (SortDirection)sortDirection, sortExpression.ToString());
            }
        
            Session["allAnsatte"] = allAnsatte;

            BindToListView(allAnsatte);

            var hidedColumns = (Dictionary<string, string>)Session["AnsattList - hidedColumns"];

            foreach (var column in hidedColumns)
            {
                HideShow(column.Key, column.Value);
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["AnsattList - SortDirection"];
            var sortExpression = Session["AnsattList - SortExpression"].ToString();

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

            Session["AnsattList - SortDirection"] = sortDirection;
            Session["AnsattList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }

        private IList<Ansatt> SortAllAnsatte(IEnumerable<Ansatt> allAnsatte, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Ansatt> result = new List<Ansatt>();

            var propertyInfo = typeof(Ansatt).GetProperty(sortExpression);

            if (sortExpression == "Avdeling")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAnsatte.OrderBy(ansatt => ansatt.Avdeling != null ? ansatt.Avdeling.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allAnsatte.OrderByDescending(ansatt => ansatt.Avdeling != null ? ansatt.Avdeling.Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "StillingsType")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAnsatte.OrderBy(ansatt => ansatt.StillingsType != null ? ansatt.StillingsType.Navn : string.Empty).ToList();
                }
                else
                {
                    result = allAnsatte.OrderByDescending(ansatt => ansatt.StillingsType != null ? ansatt.StillingsType.Navn : string.Empty).ToList();
                }
            }
            //else if (sortExpression == "VarslesAvansatt")
            //{
            //    if (sortDirection == SortDirection.Ascending)
            //    {
            //        result = allAnsatte.OrderBy(ansatt => ansatt.VarslesAvAnsatt != null ? ansatt.VarslesAvAnsatt.Navn : string.Empty);
            //    }
            //    else
            //    {
            //        result = allAnsatte.OrderByDescending(ansatt => ansatt.VarslesAvAnsatt != null ? ansatt.VarslesAvAnsatt.Navn : string.Empty);
            //    }
            //}
            else if (sortExpression == "JobberIKlasser")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAnsatte.OrderBy(ansatt => ansatt.JobberIKlasser.Count > 0 ? ansatt.JobberIKlasser.First().Navn : string.Empty).ToList();
                }
                else
                {
                    result = allAnsatte.OrderByDescending(ansatt => ansatt.JobberIKlasser.Count > 0 ? ansatt.JobberIKlasser.First().Navn : string.Empty).ToList();
                }
            }
            else if (sortExpression == "JobberISfos")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAnsatte.OrderBy(ansatt => ansatt.JobberISfos.Count > 0 ? ansatt.JobberISfos.First().Navn : string.Empty).ToList();
                }
                else
                {
                    result = allAnsatte.OrderByDescending(ansatt => ansatt.JobberISfos.Count > 0 ? ansatt.JobberISfos.First().Navn : string.Empty).ToList();
                }
            }
            //else if (sortExpression == "AvdelingsLederIAvdelinger")
            //{
            //    if (sortDirection == SortDirection.Ascending)
            //    {
            //        result = allAnsatte.OrderBy(ansatt => ansatt.AvdelingsLederIAvdelinger.Count > 0 ? ansatt.AvdelingsLederIAvdelinger.First().Navn : string.Empty);
            //    }
            //    else
            //    {
            //        result = allAnsatte.OrderByDescending(ansatt => ansatt.AvdelingsLederIAvdelinger.Count > 0 ? ansatt.AvdelingsLederIAvdelinger.First().Navn : string.Empty);
            //    }
            //}
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAnsatte.OrderBy(ansatt => propertyInfo.GetValue(ansatt)).ToList();
                }
                else
                {
                    result = allAnsatte.OrderByDescending(ansatt => propertyInfo.GetValue(ansatt)).ToList();
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
            else if (linkButtonId == "HideShowStillingsStørrelseLinkButton")
            {
                tableHeader = "StillingsStørrelseLinkButton";
                tableData = "StillingsStørrelseTextBox";
            }
            else if (linkButtonId == "HideShowTlfnrLinkButton")
            {
                tableHeader = "TlfnrLinkButton";
                tableData = "TlfNrTextBox";
            }
            else if (linkButtonId == "HideShowAvdelingLinkButton")
            {
                tableHeader = "AvdelingLinkButton";
                tableData = "AvdelingDropDown";
            }
            else if (linkButtonId == "HideShowStillingsTypeLinkButton")
            {
                tableHeader = "StillingsTypeLinkButton";
                tableData = "StillingsTypeDropDown";
            }
            else if (linkButtonId == "HideShowJobberIKlasserLinkButton")
            {
                tableHeader = "JobberIKlasserLinkButton";
                tableData = "JobberIKlasserListBox";
            }
            else if (linkButtonId == "HideShowJobberISfosLinkButton")
            {
                tableHeader = "JobberISfosLinkButton";
                tableData = "JobberISfosListBox";
            }
            //else if (linkButtonId == "HideShowAvdelingsLederIAvdelingerLinkButton")
            //{
            //    tableHeader = "AvdelingsLederIAvdelingerLinkButton";
            //    tableData = "AvdelingsLederIAvdelingerListBox";
            //}

            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["AnsattList - hidedColumns"];

                if (hidedColumns.ContainsKey(tableHeader))
                {
                    hidedColumns.Remove(tableHeader);
                    //linkButton.ToolTip = "Not hided";
                }
                else
                {
                    hidedColumns.Add(tableHeader, tableData);
                    //linkButton.ToolTip = "Hided";
                }
            }
            
        }


        private void HideShow(string tableHeader, string tableData)
        {
            var button = AnsattListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in AnsattListView.Items)
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
            var allAnsatte = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var ansatt = allAnsatte.First(a => a.Id == id);
            ansatt.Delete();

            allAnsatte.Remove(ansatt);
            
            UpdateBinding(allAnsatte, sort:false);
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            var allAnsatte = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var ansattForEdit = allAnsatte.First(a => a.Id == id);
            if (ansattForEdit.IsChanged)
            {
                var ansatt = ansattForEdit.Save();
                id = ansatt.Id;
            }

            if (allAnsatte.Any(ansatt => ansatt.IsChanged))
            {
                WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
            }
            else if (Ansatt.GetById(id) == null)
            {
                WebUtilities.ShowMessageBoxPopUp(this, "Elev '" + ansattForEdit.Navn + "' finnes ikke lenger i systemet!");

                allAnsatte.Remove(ansattForEdit);
                UpdateBinding(allAnsatte, sort: false);
            }
            else
            {
                Response.Redirect("../Details/AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode(ansattForEdit.Id.ToString()));
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allAnsatte = Ansatt.GetAll().ToList();
            UpdateBinding(allAnsatte, sort:true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allAnsatte = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var ansatt = new Ansatt {Id = dummyId};

            allAnsatte.Add(ansatt);

            //Response.Redirect("AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode(dummyId.ToString()));

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allAnsatte, sort:false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }


        //INFO: fix of paging for ListView when imperative binding
        //protected void ListViewEvents_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    //set current page startindex, max rows and rebind to false
        //    AnsattItemDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

        //    var allAnsatte = (IList<Ansatt>)Session["allAnsatte"];

        //    UpdateDataSource(allAnsatte);

        //    BindToListView(allAnsatte);
        //}

        #endregion


        //private void MessageBox(string message, string title = "title")
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), title, "alert('" + message + "');", true);
        //}

        //protected string TrackChangedRow(object dataItem)
        //{
        //    var listViewDataItem = (ListViewDataItem)dataItem;

        //    var IdButton = (Label)listViewDataItem.FindControl("IdButton");

        //    var id = Convert.ToInt32(IdButton.Text);

        //    List<Int32> list = null;

        //    if (Session["List"] == null)
        //    {
        //        list = new List<int>();

        //        if (list.Contains(id) == false)
        //        {
        //            list.Add(id);
        //            Session["List"] = list;
        //        }
        //    }
        //    else
        //    {
        //        list = Session["List"] as List<Int32>;

        //        if (list.Contains(id) == false)
        //        {
        //            list.Add(id);
        //        }
        //    }

        //    return "dummyFunction()"; 
        //}
    }
}


// TODO: ---------------------------------------------------------------------------------------------

// oninput="<%# TrackChangedRow(Container) %>"

//var ansattCopy = ansatt; // INFO: fix for: access to foreach variable in closure...


//private class AnsattView
//{
//    public int Id { get; set; }
//    public string Navn { get; set; }
//    //public DropDownList AvdelingDropDown { get; set; }
//    //public int Stillingsstørrelse { get; set; }
//    //public int Id { get; set; }
//    //public int Id { get; set; }
//}


