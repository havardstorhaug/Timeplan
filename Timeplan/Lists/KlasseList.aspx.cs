using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class KlasseList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                ViewState["dummyId"] = 0;

                if (Session["KlasseList - hidedColumns"] == null)
                {
                    Session["KlasseList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Klasse> allKlasser = Klasse.GetAll().ToList();

                Session["KlasseList - SortDirection"] = Session["KlasseList - SortDirection"] ?? SortDirection.Ascending;
                Session["KlasseList - SortExpression"] = Session["KlasseList - SortExpression"] ?? "Navn";

                UpdateBinding(allKlasser, sort: true);
            }
        }


        private void BindToListView(IList<Klasse> allKlasser)
        {
            var avdelinger = Avdeling.GetAll().OrderBy(a => a.Navn).ToList();
            var elever = Elev.GetAll().OrderBy(e => e.Navn).ToList();
            var ansatte = Ansatt.GetAll().OrderBy(a => a.Navn).ToList();

            var klasseViewList = new List<object>();

            foreach (var klasse in allKlasser)
            {
                klasseViewList.Add(new
                {
                    klasse.Id,
                    klasse.Navn,
                    klasse.MaksAntallElever
                });
            }

            KlasseListView.DataSource = klasseViewList;
            KlasseListView.DataBind();

            foreach (var item in KlasseListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");

                    var avdelingDropDown = (DropDownList)item.FindControl("AvdelingDropDown");

                    var klasse = allKlasser.First(k => k.Id.ToString() == idButton.Text);

                    avdelingDropDown.DataSource = avdelinger;
                    avdelingDropDown.DataValueField = Utilities.GetPropertyName(() => klasse.Avdeling.Id);
                    avdelingDropDown.DataTextField = Utilities.GetPropertyName(() => klasse.Avdeling.Navn);
                    avdelingDropDown.DataBind();

                    if (klasse.Avdeling != null)
                        avdelingDropDown.SelectedValue = klasse.Avdeling.Id.ToString();

                    var eleverListBox = (ListControl)item.FindControl("EleverListBox");
                    var elevTeller = 0;
                    var elevSelectedTeller = 0;
                    foreach (var elev in elever)
                    {
                        var listItem = new ListItem(elev.Navn, elev.Id.ToString());

                        if (klasse.Elevs.Any(e => e.Id == elev.Id))
                        {
                            listItem.Selected = true;
                            listItem.Enabled = false;
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

                    var ansatteListBox = (ListControl)item.FindControl("AnsatteListBox");
                    var ansattTeller = 0;
                    var ansattSelectedTeller = 0;
                    foreach (var ansatt in ansatte)
                    {
                        var listItem = new ListItem(ansatt.Navn, ansatt.Id.ToString());

                        if (klasse.Ansatts.Any(a => a.Id == ansatt.Id))
                        {
                            listItem.Selected = true;
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

                    if (klasse.Elevs.Count > 0)
                    {
                        var deleteLinkButton = (LinkButton)item.FindControl("DeleteLinkButton");
                        WebUtilities.DisableLinkButton(deleteLinkButton, @"Alle elever må overføres til andre klasser før klasse '" + klasse.Navn + "' kan slettes.");
                    }
                }
            }
        }


        private void SaveAllKlasser()
        {
            var allKlasser = UpdateDataSource();

            if (allKlasser.Any(klasse => klasse.IsChanged))
            {
                foreach (var klasse in allKlasser)
                {
                    if (klasse.IsChanged)
                    {
                        klasse.Save();
                    }
                }

                allKlasser = Klasse.GetAll().ToList();

                UpdateBinding(allKlasser, sort: true);
            }
        }


        private IList<Klasse> UpdateDataSource()
        {
            var allKlasser = (IList<Klasse>)Session["allKlasser"];

            foreach (var item in KlasseListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
                    var maksAntallElever = (TextBox)item.FindControl("MaksAntallEleverTextBox");
                    var avdelingDropDown = (DropDownList)item.FindControl("AvdelingDropDown");
                    var eleverListBox = (ListControl)item.FindControl("EleverListBox");
                    var ansatteListBox = (ListControl)item.FindControl("AnsatteListBox");

                    IList<int> eleverIds =
                        (from ListItem listItem in eleverListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    IList<int> ansatteIds =
                        (from ListItem listItem in ansatteListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                    var id = Convert.ToInt32(idButton.Text);

                    var klasse = allKlasser.First(k => k.Id == id);

                    klasse.Update(
                        navnTextBox.Text,
                        Convert.ToInt32(maksAntallElever.Text),
                        Convert.ToInt32(avdelingDropDown.SelectedValue),
                        eleverIds,
                        ansatteIds
                        );
                }
            }

            return allKlasser;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllKlasser();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allKlasser = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var klasseToSave = allKlasser.First(k => k.Id == id);
            if (klasseToSave.IsChanged)
            {
                klasseToSave.Save();

                // for alle andre klasser enn den som nettopp ble lagret; 
                // sjekk om klassen har noen av de samme elevene som på klassen som ble lagret;
                // fjern i så tilfelle disse elevene fra denne klassen

                foreach (var klasse in allKlasser)
                {
                    if (klasse.Id != id)
                    {
                        var duplicateElevs = klasse.Elevs.Intersect(klasseToSave.Elevs).ToList();
                        duplicateElevs.ForEach(elev => klasse.Elevs.Remove(elev));
                    }
                }
            }

            UpdateBinding(allKlasser, sort: false);
        }

        private void UpdateBinding(IList<Klasse> allKlasser, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["KlasseList - SortDirection"];
                var sortExpression = Session["KlasseList - SortExpression"];
                allKlasser = SortAllKlasser(allKlasser, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allKlasser"] = allKlasser;
            BindToListView(allKlasser);

            var hidedColumns = (Dictionary<string, string>)Session["KlasseList - hidedColumns"];

            foreach (var column in hidedColumns)
            {
                HideShow(column.Key, column.Value);
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["KlasseList - SortDirection"];
            var sortExpression = Session["KlasseList - SortExpression"].ToString();

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

            Session["KlasseList - SortDirection"] = sortDirection;
            Session["KlasseList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }

        private IList<Klasse> SortAllKlasser(IEnumerable<Klasse> allKlasser, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Klasse> result = new List<Klasse>();

            var propertyInfo = typeof(Klasse).GetProperty(sortExpression);

            if (sortExpression == "Avdeling")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allKlasser.OrderBy(klasse => klasse.Avdeling != null ? klasse.Avdeling.Navn : string.Empty);
                }
                else
                {
                    result = allKlasser.OrderByDescending(klasse => klasse.Avdeling != null ? klasse.Avdeling.Navn : string.Empty);
                }
            }
            else if (sortExpression == "Elever")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allKlasser.OrderBy(klasse => klasse.Elevs.Count > 0 ? klasse.Elevs.First().Navn : string.Empty);
                }
                else
                {
                    result = allKlasser.OrderByDescending(klasse => klasse.Elevs.Count > 0 ? klasse.Elevs.First().Navn : string.Empty);
                }
            }
            else if (sortExpression == "Ansatte")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allKlasser.OrderBy(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
                else
                {
                    result = allKlasser.OrderByDescending(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allKlasser.OrderBy(klasse => propertyInfo.GetValue(klasse));
                }
                else
                {
                    result = allKlasser.OrderByDescending(klasse => propertyInfo.GetValue(klasse));
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
            else if (linkButtonId == "HideShowMaksAntallEleverLinkButton")
            {
                tableHeader = "MaksAntallEleverLinkButton";
                tableData = "MaksAntallEleverTextBox";
            }
            else if (linkButtonId == "HideShowAvdelingLinkButton")
            {
                tableHeader = "AvdelingLinkButton";
                tableData = "AvdelingDropDown";
            }
            else if (linkButtonId == "HideShowEleverLinkButton")
            {
                tableHeader = "EleverLinkButton";
                tableData = "EleverListBox";
            }
            else if (linkButtonId == "HideShowAnsatteLinkButton")
            {
                tableHeader = "AnsatteLinkButton";
                tableData = "AnsatteListBox";
            }

            if (tableHeader != string.Empty && tableData != string.Empty)
            {
                HideShow(tableHeader, tableData);

                var hidedColumns = (Dictionary<string, string>)Session["KlasseList - hidedColumns"];

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
            var button = KlasseListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in KlasseListView.Items)
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
            var allKlasser = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var klasse = allKlasser.First(a => a.Id == id);
            klasse.Delete();

            allKlasser.Remove(klasse);

            UpdateBinding(allKlasser, sort: false);
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allKlasser = Klasse.GetAll().ToList();
            UpdateBinding(allKlasser, sort: true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allKlasser = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var klasse = new Klasse { Id = dummyId };

            allKlasser.Add(klasse);

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allKlasser, sort: false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }

        #endregion

    }
}
