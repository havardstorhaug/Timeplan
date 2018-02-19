using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class AvdelingList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                ViewState["dummyId"] = 0;

                if (Session["AvdelingList - hidedColumns"] == null)
                {
                    Session["AvdelingList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Avdeling> allAvdelinger = Avdeling.GetAll().ToList();

                Session["AvdelingList - SortDirection"] = Session["AvdelingList - SortDirection"] ?? SortDirection.Ascending;
                Session["AvdelingList - SortExpression"] = Session["AvdelingList - SortExpression"] ?? "Navn";

                UpdateBinding(allAvdelinger, sort: true);
            }
        }


        private void BindToListView(IList<Avdeling> allAvdelinger)
        {
            var ansatte = Ansatt.GetAll().OrderBy(a => a.Navn).ToList();
            var klasser = Klasse.GetAll().OrderBy(k => k.Navn).ToList();

            var avdelingViewList = new List<object>();

            foreach (var avdeling in allAvdelinger)
            {
                avdelingViewList.Add(new
                {
                    avdeling.Id,
                    avdeling.Navn,
                });
            }

            AvdelingListView.DataSource = avdelingViewList;
            AvdelingListView.DataBind();

            foreach (var item in AvdelingListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");

                    var avdelingsLederDropDown = (DropDownList)item.FindControl("AvdelingsLederDropDown");

                    var avdeling = allAvdelinger.First(a => a.Id.ToString() == idButton.Text);

                    avdelingsLederDropDown.DataSource = ansatte;
                    avdelingsLederDropDown.DataValueField = Utilities.GetPropertyName(() => avdeling.Id);
                    avdelingsLederDropDown.DataTextField = Utilities.GetPropertyName(() => avdeling.Navn);
                    avdelingsLederDropDown.DataBind();

                    if (avdeling.AvdelingsLeder != null)
                        avdelingsLederDropDown.SelectedValue = avdeling.AvdelingsLeder.Id.ToString();

                    var ansatteListBox = (ListBox)item.FindControl("AnsatteListBox");
                    var ansatteTeller = 0;
                    var ansattSelectedTeller = 0;
                    foreach (var ansatt in ansatte)
                    {
                        var listItem = new ListItem(ansatt.Navn, ansatt.Id.ToString());

                        if (avdeling.Ansatts.Any(a => a.Id == ansatt.Id))
                        {
                            listItem.Selected = true;
                            listItem.Attributes.Add("class", WebUtilities.CSS_CLASS_DISABLED);
                        }

                        if (listItem.Selected)
                        {
                            ansatteListBox.Items.Insert(ansattSelectedTeller++, listItem);
                            ansatteTeller++;
                        }
                        else
                        {
                            ansatteListBox.Items.Insert(ansatteTeller++, listItem);
                        }
                    }

                    var klasserListBox = (ListBox)item.FindControl("KlasserListBox");

                    var klasseTeller = 0;
                    var klasseSelectedTeller = 0;
                    foreach (var klasse in klasser)
                    {
                        var listItem = new ListItem(klasse.Navn, klasse.Id.ToString());

                        if (avdeling.Klasses.Any(k => k.Id == klasse.Id))
                        {
                            listItem.Selected = true;
                            listItem.Attributes.Add("class", WebUtilities.CSS_CLASS_DISABLED);
                        }

                        if (listItem.Selected)
                        {
                            klasserListBox.Items.Insert(klasseSelectedTeller++, listItem);
                            klasseTeller++;
                        }
                        else
                        {
                            klasserListBox.Items.Insert(klasseTeller++, listItem);
                        }
                    }

                    if (avdeling.Ansatts.Count > 0 || avdeling.Klasses.Count > 0)
                    {
                        var deleteLinkButton = (LinkButton)item.FindControl("DeleteLinkButton");
                        WebUtilities.DisableLinkButton(deleteLinkButton, @"Alle ansatte og klasser må overføres til andre avdelinger før avdeling '" + avdeling.Navn + "' kan slettes.");
                    }
                }
            }

        }

        

        private void SaveAllAvdelinger()
        {
            var allAvdelinger = UpdateDataSource();

            if (allAvdelinger.Any(avdeling => avdeling.IsChanged))
            {
                foreach (var avdeling in allAvdelinger)
                {
                    if (avdeling.IsChanged)
                    {
                        avdeling.Save();
                    }
                }

                allAvdelinger = Avdeling.GetAll().ToList();

                UpdateBinding(allAvdelinger, sort: true);
            }
        }


        private IList<Avdeling> UpdateDataSource()
        {
            var allAvdelinger = (IList<Avdeling>)Session["allAvdelinger"];

            foreach (var item in AvdelingListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var avdelingsLederDropDown = (DropDownList)item.FindControl("AvdelingsLederDropDown");
                    var ansatteListBox = (ListBox)item.FindControl("AnsatteListBox");
                    var klasserListBox = (ListBox)item.FindControl("KlasserListBox");

                    IList<int> ansatteIds =
                        (from ListItem listItem in ansatteListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    IList<int> klasserIds =
                       (from ListItem listItem in klasserListBox.Items
                        where listItem.Selected
                        select Convert.ToInt32(listItem.Value)).ToList();


                    var id = Convert.ToInt32(idButton.Text);

                    var avdeling = allAvdelinger.First(a => a.Id == id);

                    avdeling.Update(
                        navnTextBox.Text,
                        Convert.ToInt32(avdelingsLederDropDown.SelectedValue),
                        ansatteIds,
                        klasserIds
                        );
                }
            }

            return allAvdelinger;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllAvdelinger();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allAvdelinger = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var avdelingToSave = allAvdelinger.First(a => a.Id == id);
            if (avdelingToSave.IsChanged)
            {
                avdelingToSave.Save();

                foreach (var avdeling in allAvdelinger)
                {
                    if (avdeling.Id != id)
                    {
                        var duplicateKlasses = avdeling.Klasses.Intersect(avdelingToSave.Klasses).ToList();
                        duplicateKlasses.ForEach(klasse => avdeling.Klasses.Remove(klasse));

                        var duplicateAnsatts = avdeling.Ansatts.Intersect(avdelingToSave.Ansatts).ToList();
                        duplicateAnsatts.ForEach(ansatt => avdeling.Ansatts.Remove(ansatt));
                    }
                }
            }

            UpdateBinding(allAvdelinger, sort:false);
        }

        private void UpdateBinding(IList<Avdeling> allAvdelinger, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["AvdelingList - SortDirection"];
                var sortExpression = Session["AvdelingList - SortExpression"];
                allAvdelinger = SortAllAvdelinger(allAvdelinger, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allAvdelinger"] = allAvdelinger;
            
            BindToListView(allAvdelinger);

            var hidedColumns = (Dictionary<string, string>)Session["AvdelingList - hidedColumns"];

            foreach (var column in hidedColumns)
            {
                HideShow(column.Key, column.Value);
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["AvdelingList - SortDirection"];
            var sortExpression = Session["AvdelingList - SortExpression"].ToString();

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

            Session["AvdelingList - SortDirection"] = sortDirection;
            Session["AvdelingList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }

        private IList<Avdeling> SortAllAvdelinger(IEnumerable<Avdeling> allAvdelinger, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Avdeling> result = new List<Avdeling>();

            var propertyInfo = typeof(Avdeling).GetProperty(sortExpression);

            if (sortExpression == "AvdelingsLeder")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAvdelinger.OrderBy(avdeling => avdeling.AvdelingsLeder != null ? avdeling.AvdelingsLeder.Navn : string.Empty);
                }
                else
                {
                    result = allAvdelinger.OrderByDescending(avdeling => avdeling.AvdelingsLeder != null ? avdeling.AvdelingsLeder.Navn : string.Empty);
                }
            }
           else if (sortExpression == "Ansatte")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAvdelinger.OrderBy(avdeling => avdeling.Ansatts.Count > 0 ? avdeling.Ansatts.First().Navn : string.Empty);
                }
                else
                {
                    result = allAvdelinger.OrderByDescending(avdeling => avdeling.Ansatts.Count > 0 ? avdeling.Ansatts.First().Navn : string.Empty);
                }
            }
            else if (sortExpression == "Klasser")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAvdelinger.OrderBy(avdeling => avdeling.Klasses.Count > 0 ? avdeling.Klasses.First().Navn : string.Empty);
                }
                else
                {
                    result = allAvdelinger.OrderByDescending(avdeling => avdeling.Klasses.Count > 0 ? avdeling.Klasses.First().Navn : string.Empty);
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allAvdelinger.OrderBy(ansatt => propertyInfo.GetValue(ansatt));
                }
                else
                {
                    result = allAvdelinger.OrderByDescending(ansatt => propertyInfo.GetValue(ansatt));
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
            else if (linkButtonId == "HideShowAvdelingsLederLinkButton")
            {
                tableHeader = "AvdelingsLederLinkButton";
                tableData = "AvdelingsLederDropDown";
            }
            else if (linkButtonId == "HideShowAnsatteLinkButton")
            {
                tableHeader = "AnsattLinkButton";
                tableData = "AnsatteListBox";
            }
            else if (linkButtonId == "HideShowKlasserLinkButton")
            {
                tableHeader = "KlasseLinkButton";
                tableData = "KlasserListBox";
            }

            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["AvdelingList - hidedColumns"];

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
            var button = AvdelingListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in AvdelingListView.Items)
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
            var allAvdelinger = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var avdeling = allAvdelinger.First(a => a.Id == id);
            avdeling.Delete();
            
            allAvdelinger.Remove(avdeling);
            UpdateBinding(allAvdelinger, sort: false);
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allAvdelinger = Avdeling.GetAll().ToList();
            UpdateBinding(allAvdelinger, sort:true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allAvdelinger = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var avdeling = new Avdeling { Id = dummyId };

            allAvdelinger.Add(avdeling);

            //Response.Redirect("AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode(dummyId.ToString()));

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allAvdelinger, sort:false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }

        #endregion
        
    }
}



