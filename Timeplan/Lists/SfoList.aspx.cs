using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Saplin.Controls;
using Timeplan.BL;

namespace Timeplan.Lists
{
    public partial class SfoList : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                ViewState["dummyId"] = 0;

                if (Session["SfoList - hidedColumns"] == null)
                {
                    Session["SfoList - hidedColumns"] = new Dictionary<string, string>();
                }

                IList<Sfo> allSfos = Sfo.GetAll().ToList();

                Session["SfoList - SortDirection"] = Session["SfoList - SortDirection"] ?? SortDirection.Ascending;
                Session["SfoList - SortExpression"] = Session["SfoList - SortExpression"] ?? "Navn";

                UpdateBinding(allSfos, sort: true);
            }

        }


        private void BindToListView(IList<Sfo> allSfos)
        {
            var elever = Elev.GetAll().Where(e => e.Sfo != null).OrderBy(e => e.Navn).ToList();
            var ansatte = Ansatt.GetAll().Where(a => (a.StillingsType.Id == (int)StillingsTypeEnum.Miljøterapeut) || (a.StillingsType.Id == (int)StillingsTypeEnum.PedagogiskMedarbeider)).OrderBy(a => a.Navn).ToList();

            var sfoViewList = new List<object>();

            foreach (var sfo in allSfos)
            {
                sfoViewList.Add(new
                {
                    sfo.Id,
                    sfo.Navn
                });
            }

            SfoListView.DataSource = sfoViewList;
            SfoListView.DataBind();

            foreach (var item in SfoListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");

                    var sfo = allSfos.First(s => s.Id.ToString() == idButton.Text);

                    var eleverListBox = (ListControl)item.FindControl("EleverListBox");
                    var elevTeller = 0;
                    var elevSelectedTeller = 0;
                    foreach (var elev in elever)
                    {
                        var listItem = new ListItem(elev.Navn, elev.Id.ToString());

                        if (sfo.Elevs.Any(e => e.Id == elev.Id))
                        {
                            listItem.Selected = true;
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

                        if (sfo.Ansatts.Any(a => a.Id == ansatt.Id))
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

                    //var tidsInndelingDropDown = (DropDownList)item.FindControl("TidsInndelingDropDown");
                    //tidsInndelingDropDown.DataSource = tidsInndeling;
                    //tidsInndelingDropDown.DataValueField = Utilities.GetPropertyName(() => sfo.ÅpningsTider.Id);
                    //tidsInndelingDropDown.DataTextField = Utilities.GetPropertyName(() => sfo.ÅpningsTider.Navn);
                    //tidsInndelingDropDown.DataBind();
                    //tidsInndelingDropDown.SelectedValue = sfo.Id > 0 ? sfo.ÅpningsTider.Id.ToString() : ((int)TidsInndelingEnum.Sfo).ToString();

                    //var tidligvaktTidsInndelingDropDown = (DropDownList)item.FindControl("TidligvaktTidsInndelingDropDown");
                    //tidligvaktTidsInndelingDropDown.DataSource = tidsInndeling;
                    //tidligvaktTidsInndelingDropDown.DataValueField = Utilities.GetPropertyName(() => sfo.TidligvaktTider.Id);
                    //tidligvaktTidsInndelingDropDown.DataTextField = Utilities.GetPropertyName(() => sfo.TidligvaktTider.Navn);
                    //tidligvaktTidsInndelingDropDown.DataBind();
                    //tidligvaktTidsInndelingDropDown.SelectedValue = sfo.Id > 0 ? sfo.TidligvaktTider.Id.ToString() : ((int)TidsInndelingEnum.Tidlig).ToString();

                    //var seinvaktTidsInndelingDropDown = (DropDownList)item.FindControl("SeinvaktTidsInndelingDropDown");
                    //seinvaktTidsInndelingDropDown.DataSource = tidsInndeling;
                    //seinvaktTidsInndelingDropDown.DataValueField = Utilities.GetPropertyName(() => sfo.SeinvaktTider.Id);
                    //seinvaktTidsInndelingDropDown.DataTextField = Utilities.GetPropertyName(() => sfo.SeinvaktTider.Navn);
                    //seinvaktTidsInndelingDropDown.DataBind();
                    //seinvaktTidsInndelingDropDown.SelectedValue = sfo.Id > 0 ? sfo.SeinvaktTider.Id.ToString() : ((int)TidsInndelingEnum.Seint).ToString();
                }
            }
        }


        private void SaveAllSfos()
        {
            var allSfos = UpdateDataSource();

            if (allSfos.Any(sfo => sfo.IsChanged))
            {
                foreach (var sfo in allSfos)
                {
                    if (sfo.IsChanged)
                    {
                        sfo.Save();
                    }
                }

                allSfos = Sfo.GetAll().ToList();

                UpdateBinding(allSfos, sort: true);
            }
        }


        private IList<Sfo> UpdateDataSource()
        {
            var allSfos = (IList<Sfo>)Session["allSfos"];

            foreach (var item in SfoListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var navnTextBox = (TextBox)item.FindControl("NavnTextBox");
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

                    var sfo = allSfos.First(k => k.Id == id);

                    sfo.Update(
                        navnTextBox.Text,
                        eleverIds,
                        ansatteIds,
                        (int)TidsInndelingEnum.Sfo,
                        (int)TidsInndelingEnum.Tidlig,
                        (int)TidsInndelingEnum.Seint
                        );
                }
            }

            return allSfos;
        }

        #region Eventhandling

        protected void SaveAllButton_Click(object sender, EventArgs e)
        {
            SaveAllSfos();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var allSfos = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var sfo = allSfos.First(k => k.Id == id);
            if (sfo.IsChanged)
            {
                sfo.Save();
            }

            UpdateBinding(allSfos, sort: false);
        }

        private void UpdateBinding(IList<Sfo> allSfos, bool sort)
        {
            if (sort)
            {
                var sortDirection = Session["SfoList - SortDirection"];
                var sortExpression = Session["SfoList - SortExpression"];
                allSfos = SortallSfos(allSfos, (SortDirection)sortDirection, sortExpression.ToString());
            }

            Session["allSfos"] = allSfos;
            BindToListView(allSfos);

            var hidedColumns = (Dictionary<string, string>)Session["SfoList - hidedColumns"];

            foreach (var column in hidedColumns)
            {
                HideShow(column.Key, column.Value);
            }
        }

        protected void ListViewEvents_Sorting(object sender, ListViewSortEventArgs e)
        {
            var allElever = UpdateDataSource();

            var sortDirection = (SortDirection)Session["SfoList - SortDirection"];
            var sortExpression = Session["SfoList - SortExpression"].ToString();

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

            Session["SfoList - SortDirection"] = sortDirection;
            Session["SfoList - SortExpression"] = e.SortExpression;

            UpdateBinding(allElever, sort: true);
        }


        private IList<Sfo> SortallSfos(IEnumerable<Sfo> allSfos, SortDirection sortDirection, string sortExpression)
        {
            IEnumerable<Sfo> result = new List<Sfo>();

            var propertyInfo = typeof(Sfo).GetProperty(sortExpression);

            if (sortExpression == "Elever")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allSfos.OrderBy(klasse => klasse.Elevs.Count > 0 ? klasse.Elevs.First().Navn : string.Empty);
                }
                else
                {
                    result = allSfos.OrderByDescending(klasse => klasse.Elevs.Count > 0 ? klasse.Elevs.First().Navn : string.Empty);
                }
            }
            else if (sortExpression == "Ansatte")
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allSfos.OrderBy(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
                else
                {
                    result = allSfos.OrderByDescending(klasse => klasse.Ansatts.Count > 0 ? klasse.Ansatts.First().Navn : string.Empty);
                }
            }
            else
            {
                if (sortDirection == SortDirection.Ascending)
                {
                    result = allSfos.OrderBy(klasse => propertyInfo.GetValue(klasse));
                }
                else
                {
                    result = allSfos.OrderByDescending(klasse => propertyInfo.GetValue(klasse));
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

                var hidedColumns = (Dictionary<string, string>)Session["SfoList - hidedColumns"];

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
            var button = SfoListView.FindControl(tableHeader);
            var tableHeaderVisible = true;

            foreach (var item in SfoListView.Items)
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
            var allSfos = UpdateDataSource();

            var button = (LinkButton)sender;
            var id = Convert.ToInt32(button.CommandArgument);

            var sfo = allSfos.First(a => a.Id == id);
            sfo.Delete();

            allSfos.Remove(sfo);

            UpdateBinding(allSfos, sort: false);
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var allSfos = Sfo.GetAll().ToList();
            UpdateBinding(allSfos, sort: true);
        }


        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            var allSfos = UpdateDataSource();

            var dummyId = (int)ViewState["dummyId"];

            var sfo = new Sfo { Id = dummyId };

            allSfos.Add(sfo);

            ViewState["dummyId"] = --dummyId;

            UpdateBinding(allSfos, sort: false);

            WebUtilities.AdjustScrollPosition(this, 0, Int32.MaxValue);
        }

        #endregion

    }
}
