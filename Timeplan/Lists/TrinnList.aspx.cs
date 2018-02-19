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
    public partial class TrinnList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                ViewState["dummyId"] = 0;

                if (Session["TrinnList - hidedColumns"] == null)
                {
                    Session["TrinnList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Trinn> allTrinn = Trinn.GetAll().ToList();

                Session["TrinnList - SortDirection"] = Session["TrinnList - SortDirection"] ?? SortDirection.Ascending;
                Session["TrinnList - SortExpression"] = Session["TrinnList - SortExpression"] ?? "Navn";

                UpdateBinding(allTrinn, sort: true);
            }
        }


        private void BindToListView(IList<Trinn> allTrinn)
        {
            var elever = Elev.GetAll().OrderBy(k => k.Navn).ToList();

            var trinnViewList = new List<object>();

            foreach (var trinn in allTrinn)
            {
                trinnViewList.Add(new
                {
                    trinn.Id,
                    trinn.Navn,
                    trinn.UkeTimeTall
                });
            }

            TrinnListView.DataSource = trinnViewList;
            TrinnListView.DataBind();

            foreach (var item in TrinnListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var trinn = allTrinn.First(t => t.Id.ToString() == idButton.Text);

                    var eleverListBox = (ListBox)item.FindControl("EleverListBox");
                    var elevTeller = 0;
                    var elevSelectedTeller = 0;
                    foreach (var elev in elever)
                    {
                        var listItem = new ListItem(elev.Navn, elev.Id.ToString());

                        if (trinn.Elevs.Any(e => e.Id == elev.Id))
                        {
                            listItem.Selected = true;
                            listItem.Attributes.Add("class", WebUtilities.CSS_CLASS_DISABLED);
                        }

                        if (listItem.Selected)
                        {
                            eleverListBox.Items.Insert(elevSelectedTeller++, listItem);
                            elevTeller++;
                        }
                        else
                        {
                            eleverListBox.Items.Insert(elevTeller++, listItem);
                        }
                    }

                    if (trinn.Elevs.Count > 0)
                    {
                        var deleteLinkButton = (LinkButton)item.FindControl("DeleteLinkButton");
                        WebUtilities.DisableLinkButton(deleteLinkButton, @"Alle elever må overføres til andre trinn før trinn '" + trinn.Navn + "' kan slettes.");
                    }

                }
            }
        }


        private void SaveAllTrinn()
        {
            var allTrinn = UpdateDataSource();

            if (allTrinn.Any(trinn => trinn.IsChanged))
            {
                foreach (var trinn in allTrinn)
                {
                    if (trinn.IsChanged)
                    {
                        trinn.Save();
                    }
                }

                allTrinn = Trinn.GetAll().ToList();

                UpdateBinding(allTrinn, sort: true);
            }
        }


        private IList<Trinn> UpdateDataSource()
        {
            var allTrinn = (IList<Trinn>)Session["allTrinn"];

            foreach (var item in TrinnListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var ukeTimeTallTextBox = (TextBox)item.FindControl("UkeTimeTallTextBox");
                    var eleverListBox = (ListBox)item.FindControl("EleverListBox");

                     IList<int> eleverIds =
                        (from ListItem listItem in eleverListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    var id = Convert.ToInt32(idButton.Text);

                    var trinn = allTrinn.First(t => t.Id == id);

                    trinn.Update(
                        navnTextBox.Text,
                        Convert.ToDecimal(ukeTimeTallTextBox.Text),
                        eleverIds
                        );
                }
            }

            return allTrinn;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllTrinn();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allTrinn = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var trinnToSave = allTrinn.First(t => t.Id == id);
            if (trinnToSave.IsChanged)
            {
                trinnToSave.Save();

                // for alle andre trinn enn det som nettopp ble lagret; 
                // sjekk om trinnet har noen av de samme elevene som på trinnet som ble lagret;
                // fjern i så tilfelle disse elevene fra dette trinnet

                foreach (var trinn in allTrinn)
                {
                    if (trinn.Id != id)
                    {
                       var duplicateElevs = trinn.Elevs.Intersect(trinnToSave.Elevs).ToList();
                       duplicateElevs.ForEach(elev => trinn.Elevs.Remove(elev));
                    }
                }
            }

            UpdateBinding(allTrinn, sort: false);
        }

        private void UpdateBinding(IList<Trinn> allTrinn, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["TrinnList - SortDirection"];
                var sortExpression = Session["TrinnList - SortExpression"];
                allTrinn = SortAllTrinn(allTrinn, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allTrinn"] = allTrinn;

            BindToListView(allTrinn);

            var hidedColumns = (Dictionary<string, string>)Session["TrinnList - hidedColumns"];

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
            var allTrinn = UpdateDataSource();

            var sortDirection = (SortDirection)Session["TrinnList - SortDirection"];
            var sortExpression = Session["TrinnList - SortExpression"].ToString();

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

            Session["TrinnList - SortDirection"] = sortDirection;
            Session["TrinnList - SortExpression"] = e.SortExpression;

            UpdateBinding(allTrinn, sort: true);
        }


        private IList<Trinn> SortAllTrinn(IEnumerable<Trinn> allTrinn, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Trinn> result = new List<Trinn>();

            var propertyInfo = typeof(Trinn).GetProperty(sortExpression);

            if (sortExpression == "Navn")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allTrinn.OrderBy(trinn => Utilities.DigitPart(trinn.Navn));
                }
                else
                {
                    result = allTrinn.OrderByDescending(trinn => Utilities.DigitPart(trinn.Navn));
                }
            }
            else if (sortExpression == "Elever")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allTrinn.OrderBy(trinn => trinn.Elevs.Count > 0 ? trinn.Elevs.First().Navn : string.Empty);
                }
                else
                {
                    result = allTrinn.OrderByDescending(trinn => trinn.Elevs.Count > 0 ? trinn.Elevs.First().Navn : string.Empty);
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allTrinn.OrderBy(trinn => propertyInfo.GetValue(trinn));
                }
                else
                {
                    result = allTrinn.OrderByDescending(trinn => propertyInfo.GetValue(trinn));
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
            else if (linkButtonId == "HideShowUkeTimeTallLinkButton")
            {
                tableHeader = "UkeTimeTallLinkButton";
                tableData = "UkeTimeTallTextBox";
            }
            else if (linkButtonId == "HideShowEleverLinkButton")
            {
                tableHeader = "EleverLinkButton";
                tableData = "EleverListBox";
            }
            
            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["TrinnList - hidedColumns"];

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
            var button = TrinnListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in TrinnListView.Items)
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
                var allTrinn = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                var trinn = allTrinn.First(t => t.Id == id);
                trinn.Delete();

                allTrinn.Remove(trinn);

                UpdateBinding(allTrinn, sort: false);
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var allTrinn = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                var trinnForEdit = allTrinn.First(a => a.Id == id);
                if (trinnForEdit.IsChanged)
                {
                    var trinn = trinnForEdit.Save();
                    id = trinn.Id;
                }

                if (allTrinn.Any(trinn => trinn.IsChanged))
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else if (Trinn.GetById(id) == null)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Trinn '" + trinnForEdit.Navn + "' finnes ikke lenger i systemet!");

                    allTrinn.Remove(trinnForEdit);
                    UpdateBinding(allTrinn, sort: false);
                }
                else
                {
                    Response.Redirect("~/Details/TrinnDetails.aspx?Id=" + HttpUtility.UrlEncode(trinnForEdit.Id.ToString()), false);
                }
            }

        }

        //protected void EditButton_Click(object sender, EventArgs e)
        //{
        //    var allTrinn = UpdateDataSource();
        //    if (allTrinn.Any(trinn => trinn.IsChanged))
        //    {
        //        WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
        //    }
        //    else
        //    {
        //        var button = (LinkButton)sender;
        //        var id = Convert.ToInt32(button.CommandArgument);

        //        Response.Redirect("../Details/ElevDetails.aspx?Id=" + HttpUtility.UrlEncode(id.ToString()));
        //    }

        //}


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allTrinn = Trinn.GetAll().ToList();
            UpdateBinding(allTrinn, sort: true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allTrinn = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var trinn = new Trinn { Id = dummyId, IsChanged = true };

            allTrinn.Add(trinn);

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allTrinn, sort: false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }

        #endregion
    }
}