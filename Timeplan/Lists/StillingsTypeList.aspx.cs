using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class StillingsTypeList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                ViewState["dummyId"] = 0;

                if (Session["StillingsTypeList - hidedColumns"] == null)
                {
                    Session["StillingsTypeList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<StillingsType> allStillingsTyper = StillingsType.GetAll().ToList();

                Session["StillingsTypeList - SortDirection"] = Session["StillingsTypeList - SortDirection"] ?? SortDirection.Ascending;
                Session["StillingsTypeList - SortExpression"] = Session["StillingsTypeList - SortExpression"] ?? "Navn";

                UpdateBinding(allStillingsTyper, sort: true);
            }
        }


        private void BindToListView(IList<StillingsType> allStillingsTyper)
        {
            var ansatte = Ansatt.GetAll().OrderBy(a => a.Navn).ToList();

            var stillingsTypeViewList = new List<object>();

            foreach (var klasse in allStillingsTyper)
            {
                stillingsTypeViewList.Add(new
                {
                    klasse.Id,
                    klasse.Navn,
                    klasse.TimerElevarbeid,
                    klasse.TimerSamarbeid
                });
            }

            StillingsTypeListView.DataSource = stillingsTypeViewList;
            StillingsTypeListView.DataBind();

            foreach (var item in StillingsTypeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");

                    var stillingsType = allStillingsTyper.First(s => s.Id.ToString() == idButton.Text);

                    var ansatteListBox = (ListBox)item.FindControl("AnsatteListBox");
                    var ansattTeller = 0;
                    var ansattSelectedTeller = 0;
                    foreach (var ansatt in ansatte)
                    {
                        var listItem = new ListItem(ansatt.Navn, ansatt.Id.ToString());

                        if (stillingsType.Ansatts.Any(a => a.Id == ansatt.Id))
                        {
                            listItem.Selected = true;
                            listItem.Attributes.Add("class", WebUtilities.CSS_CLASS_DISABLED);
                        }

                        if (listItem.Selected)
                        {
                            ansatteListBox.Items.Insert(ansattSelectedTeller++, listItem);
                            ansattTeller++;
                        }
                        else
                        {
                            ansatteListBox.Items.Insert(ansattTeller++, listItem);
                        }
                    }

                    if (stillingsType.Ansatts.Count > 0)
                    {
                        var deleteLinkButton = (LinkButton)item.FindControl("DeleteLinkButton");
                        WebUtilities.DisableLinkButton(deleteLinkButton, @"Alle ansatte må overføres til andre stillingstyper før stillingstype '" + stillingsType.Navn + "' kan slettes.");
                    }

                }
            }
        }


        private void SaveAllStillingsTyper()
        {
            var allStillingsTyper = UpdateDataSource();

            if (allStillingsTyper.Any(stillingsType => stillingsType.IsChanged))
            {
                foreach (var stillingsType in allStillingsTyper)
                {
                    if (stillingsType.IsChanged)
                    {
                        stillingsType.Save();
                    }
                }

                allStillingsTyper = StillingsType.GetAll().ToList();

                UpdateBinding(allStillingsTyper, sort: true);
            }
        }

        private IList<StillingsType> UpdateDataSource()
        {
            var allStillingsTyper = (IList<StillingsType>)Session["allStillingsTyper"];

            foreach (var item in StillingsTypeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var timerElevarbeid = (TextBox)item.FindControl("TimerElevarbeidTextBox");
                    var timerSamarbeid = (TextBox)item.FindControl("TimerSamarbeidTextBox");
                    var ansatteListBox = (ListBox)item.FindControl("AnsatteListBox");

                    IList<int> ansatteIds =
                        (from ListItem listItem in ansatteListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    var id = Convert.ToInt32(idButton.Text);

                    var stillingsType = allStillingsTyper.First(s => s.Id == id);

                    stillingsType.Update(
                        navnTextBox.Text,
                        Convert.ToDecimal(timerElevarbeid.Text),
                        Convert.ToDecimal(timerSamarbeid.Text),
                        ansatteIds
                        );
                }
            }

            return allStillingsTyper;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllStillingsTyper();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allStillingsTyper = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var stillingsTypeToSave = allStillingsTyper.First(s => s.Id == id);
            if (stillingsTypeToSave.IsChanged)
            {
                stillingsTypeToSave.Save();

                // for alle andre stillingstyper enn den som nettopp ble lagret; 
                // sjekk om stillingstypen har noen av de samme ansatte som på stillingstypen som ble lagret;
                // fjern i så tilfelle disse ansatte fra denne stillingstypen

                foreach (var stillingstype in allStillingsTyper)
                {
                    if (stillingstype.Id != id)
                    {
                        var duplicateAnsatts = stillingstype.Ansatts.Intersect(stillingsTypeToSave.Ansatts).ToList();
                        duplicateAnsatts.ForEach(ansatt => stillingstype.Ansatts.Remove(ansatt));
                    }
                }
            }

            UpdateBinding(allStillingsTyper, sort:false);
        }

        private void UpdateBinding(IList<StillingsType> allStillingsTyper, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["StillingsTypeList - SortDirection"];
                var sortExpression = Session["StillingsTypeList - SortExpression"];
                allStillingsTyper = SortAllStillingsTyper(allStillingsTyper, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allStillingsTyper"] = allStillingsTyper;
            BindToListView(allStillingsTyper);

            var hidedColumns = (Dictionary<string, string>)Session["StillingsTypeList - hidedColumns"];

            foreach (var column in hidedColumns)
            {
                HideShow(column.Key, column.Value);
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["StillingsTypeList - SortDirection"];
            var sortExpression = Session["StillingsTypeList - SortExpression"].ToString();

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

            Session["StillingsTypeList - SortDirection"] = sortDirection;
            Session["StillingsTypeList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }


        private IList<StillingsType> SortAllStillingsTyper(IEnumerable<StillingsType> allStillingsTyper, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<StillingsType> result = new List<StillingsType>();

            var propertyInfo = typeof(StillingsType).GetProperty(sortExpression);

            if (sortExpression == "Ansatte")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allStillingsTyper.OrderBy(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
                else
                {
                    result = allStillingsTyper.OrderByDescending(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allStillingsTyper.OrderBy(klasse => propertyInfo.GetValue(klasse));
                }
                else
                {
                    result = allStillingsTyper.OrderByDescending(klasse => propertyInfo.GetValue(klasse));
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
            else if (linkButtonId == "HideShowTimerElevarbeidLinkButton")
            {
                tableHeader = "TimerElevarbeidLinkButton";
                tableData = "TimerElevarbeidTextBox";
            }
            else if (linkButtonId == "HideShowTimerSamarbeidLinkButton")
            {
                tableHeader = "TimerSamarbeidLinkButton";
                tableData = "TimerSamarbeidTextBox";
            }
            else if (linkButtonId == "HideShowAnsatteLinkButton")
            {
                tableHeader = "AnsatteLinkButton";
                tableData = "AnsatteListBox";
            }

            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["StillingsTypeList - hidedColumns"];

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
            var button = StillingsTypeListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in StillingsTypeListView.Items)
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
            var allStillingsTyper = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var stillingsType = allStillingsTyper.First(a => a.Id == id);
            stillingsType.Delete();

            allStillingsTyper.Remove(stillingsType);

            UpdateBinding(allStillingsTyper, sort:false);
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allStillingsTyper = StillingsType.GetAll().ToList();
            UpdateBinding(allStillingsTyper, sort:true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allStillingsTyper = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var stillingsType = new StillingsType { Id = dummyId };

            allStillingsTyper.Add(stillingsType);

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allStillingsTyper, sort:false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }

        #endregion

    }
}
