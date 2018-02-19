using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;

namespace Timeplan.Details
{
    public partial class TrinnDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                }

                ViewState["dummyId"] = 0;

                var allTrinn = (IList<Trinn>)Session["allTrinn"];

                var trinnId = Convert.ToInt32(Request.QueryString["Id"]);

                if (trinnId > 0)
                {
                    Session["TrinnDetails - CurrentIndex"] = allTrinn.IndexOf(allTrinn.First(trinn => trinn.Id == trinnId));
                }

                BindData(trinnId);
            }

        }

        private void BindData(int id)
        {
            var trinn = id > 0 ? Trinn.GetById(id) : new Trinn();

            var elever = Elev.GetAll().OrderBy(e => e.Navn);

            IdTextBox.Text = trinn.Id.ToString();

            NavnTextBox.Text = trinn.Navn;
            UkeTimeTallTextBox.Text = trinn.UkeTimeTall.ToString();

            var elevTeller = 0;
            var elevSelectedTeller = 0;

            EleverListBox.Items.Clear();

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
                    EleverListBox.Items.Insert(elevSelectedTeller++, listItem);
                    elevTeller++;
                }
                else
                {
                    EleverListBox.Items.Insert(elevTeller++, listItem);
                }
            }

            BindToTable(trinn);
        }

        private void BindToTable(Trinn trinn)
        {
            MandagStartTextBox.Text = TimeAsString(trinn.MandagStart);
            MandagSluttTextBox.Text = TimeAsString(trinn.MandagSlutt);
            TirsdagStartTextBox.Text = TimeAsString(trinn.TirsdagStart);
            TirsdagSluttTextBox.Text = TimeAsString(trinn.TirsdagSlutt);
            OnsdagStartTextBox.Text = TimeAsString(trinn.OnsdagStart);
            OnsdagSluttTextBox.Text = TimeAsString(trinn.OnsdagSlutt);
            TorsdagStartTextBox.Text = TimeAsString(trinn.TorsdagStart);
            TorsdagSluttTextBox.Text = TimeAsString(trinn.TorsdagSlutt);
            FredagStartTextBox.Text = TimeAsString(trinn.FredagStart);
            FredagSluttTextBox.Text = TimeAsString(trinn.FredagSlutt);
        }

        private string TimeAsString(TimeSpan? time)
        {
            return time != null ? time.ToString().Substring(0, 5) : string.Empty;
        }


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var id = Convert.ToInt32(IdTextBox.Text);
                var trinn = id > 0 ? Trinn.GetById(Convert.ToInt32(IdTextBox.Text)) : new Trinn { IsChanged = true };

                IList<int> eleverIds =
                        (from ListItem listItem in EleverListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

                trinn.Update(
                    NavnTextBox.Text,
                    Convert.ToDecimal(UkeTimeTallTextBox.Text),
                    eleverIds,
                    string.IsNullOrWhiteSpace(MandagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(MandagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(MandagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(MandagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(TirsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TirsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(TirsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TirsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(OnsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(OnsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(OnsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(OnsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(TorsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TorsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(TorsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TorsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(FredagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(FredagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(FredagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(FredagSluttTextBox.Text)
                    );

                if (trinn.IsChanged)
                {
                    trinn.Save();

                    if (id == 0)
                        IdTextBox.Text = trinn.Id.ToString();
                }

                BindData(trinn.Id);
            }
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);

            if (id > 0)
            {
                BindData(id);
            }
            else
            {
                object refUrl = ViewState["RefUrl"];
                if (refUrl != null)
                {
                    Response.Redirect((string)refUrl);
                }
            }
        }

        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PageChanged())
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    Response.Redirect("TrinnDetails.aspx?Id=" + HttpUtility.UrlEncode("0"));
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);
            var elev = id > 0 ? Trinn.GetById(Convert.ToInt32(IdTextBox.Text)) : null;

            if (elev != null)
            {
                elev.Delete();
            }

            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }


        protected void GoBackButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PageChanged())
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    //object refUrl = ViewState["RefUrl"];
                    //if (refUrl != null)
                    //{
                    //    Response.Redirect((string)refUrl);
                    //}

                    Response.Redirect("~/Lists/TrinnList.aspx");
                }
            }
        }

        protected void PreviousButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PageChanged())
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    GoTo(-1);
                }
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PageChanged())
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    GoTo(1);
                }
            }
        }

        private void GoTo(int direction)
        {
            var allTrinn = (IList<Trinn>)Session["allTrinn"];
            var currentIndex = Convert.ToInt32(Session["TrinnDetails - CurrentIndex"]);
            var navigateToIndex = currentIndex + direction;
            if (navigateToIndex == allTrinn.Count)
            {
                navigateToIndex = 0;
            }
            else if (navigateToIndex == -1)
            {
                navigateToIndex = allTrinn.Count - 1;
            }

            var elevId = allTrinn[navigateToIndex].Id;
            Response.Redirect("TrinnDetails.aspx?Id=" + HttpUtility.UrlEncode(elevId.ToString()));
        }


        private bool PageChanged()
        {
            var changed = false;

            var id = Convert.ToInt32(IdTextBox.Text);
            var trinn = id > 0 ? Trinn.GetById(Convert.ToInt32(IdTextBox.Text)) : new Trinn { IsChanged = true };

            IList<int> eleverIds =
                        (from ListItem listItem in EleverListBox.Items
                         where listItem.Selected
                         select Convert.ToInt32(listItem.Value)).ToList();

            trinn.Update(
                NavnTextBox.Text,
                    Convert.ToDecimal(UkeTimeTallTextBox.Text),
                    eleverIds,
                    string.IsNullOrWhiteSpace(MandagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(MandagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(MandagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(MandagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(TirsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TirsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(TirsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TirsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(OnsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(OnsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(OnsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(OnsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(TorsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TorsdagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(TorsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(TorsdagSluttTextBox.Text),
                    string.IsNullOrWhiteSpace(FredagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(FredagStartTextBox.Text),
                    string.IsNullOrWhiteSpace(FredagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(FredagSluttTextBox.Text)
                );

            if (trinn.IsChanged)
            {
                changed = true;
            }


            return changed;
        }

    }
}