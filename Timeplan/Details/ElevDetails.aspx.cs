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
    public partial class ElevDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                }

                var allElever = (IList<Elev>)Session["allElever"];

                var elevId = Convert.ToInt32(Request.QueryString["Id"]);

                if (elevId > 0)
                {
                    Session["ElevDetails - CurrentIndex"] = allElever.IndexOf(allElever.First(elev => elev.Id == elevId));
                }

                var currentElev = elevId > 0 ? Elev.GetById(elevId) : new Elev();

                UpdateBinding(currentElev);
            }

        }

        private void UpdateBinding(Elev elev)
        {
            Session["currentElev"] = elev;

            var klasser = Klasse.GetAll().OrderBy(k => k.Navn);
            var sfoes = Sfo.GetAll().OrderBy(s => s.Navn);
            var trinn = Trinn.GetAll().OrderBy(t => Utilities.DigitPart(t.Navn));
            var ansatte = Ansatt.GetAll().Where(a => a.StillingsType.Id == (int)StillingsTypeEnum.Pedagog).OrderBy(a => a.Navn);
            var bemanningsNorm = BemanningsNorm.GetAll().OrderBy(b => b.Navn);

            IdTextBox.Text = elev.Id.ToString();

            //TextBox1.Attributes["type"] = "time";

            NavnTextBox.Text = elev.Navn;
            SkoleTimerPrUkeTextBox.Text = elev.Trinn != null ? elev.Trinn.UkeTimeTall.ToString() : trinn.First().UkeTimeTall.ToString();
            SfoProsentTextBox.Text = (elev.SfoProsent % 1 == 0) ? elev.SfoProsent.ToString("N0") : elev.SfoProsent.ToString("N2");
            TlfnrTextBox.Text = elev.TlfNr;
            

            KlasseDropDown.DataSource = klasser;
            KlasseDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Klasse.Id);
            KlasseDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Klasse.Navn);
            KlasseDropDown.DataBind();

            if (elev.Klasse != null)
                KlasseDropDown.SelectedValue = elev.Klasse.Id.ToString();

            SfoDropDown.DataSource = sfoes;
            SfoDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Sfo.Id);
            SfoDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Sfo.Navn);
            SfoDropDown.DataBind();

            // Default value
            SfoDropDown.Items.Insert(0, new ListItem("Velg sfo", "0"));
            SfoDropDown.SelectedValue = elev.Sfo != null ? elev.Sfo.Id.ToString() : "0";

            TrinnDropDown.DataSource = trinn;
            TrinnDropDown.DataValueField = Utilities.GetPropertyName(() => elev.Trinn.Id);
            TrinnDropDown.DataTextField = Utilities.GetPropertyName(() => elev.Trinn.Navn);
            TrinnDropDown.DataBind();

            if (elev.Trinn == null)
                elev.Trinn = trinn.First();

            TrinnDropDown.SelectedValue = elev.Trinn.Id.ToString();
            
            HovedPedagogDropDown.DataSource = ansatte;
            HovedPedagogDropDown.DataValueField = Utilities.GetPropertyName(() => elev.HovedPedagog.Id);
            HovedPedagogDropDown.DataTextField = Utilities.GetPropertyName(() => elev.HovedPedagog.Navn);
            HovedPedagogDropDown.DataBind();

            if (elev.HovedPedagog != null)
                HovedPedagogDropDown.SelectedValue = elev.HovedPedagog.Id.ToString();

            BemanningsNormSkoleDropDown.DataSource = bemanningsNorm;
            BemanningsNormSkoleDropDown.DataValueField = Utilities.GetPropertyName(() => elev.BemanningsNormSkole.Id);
            BemanningsNormSkoleDropDown.DataTextField = Utilities.GetPropertyName(() => elev.BemanningsNormSkole.Navn);
            BemanningsNormSkoleDropDown.DataBind();
            BemanningsNormSkoleDropDown.SelectedValue = elev.BemanningsNormSkole != null ? elev.BemanningsNormSkole.Id.ToString() : ((int)BemanningsNormEnum.OneToOne).ToString();

            BemanningsNormSfoDropDown.DataSource = bemanningsNorm;
            BemanningsNormSfoDropDown.DataValueField = Utilities.GetPropertyName(() => elev.BemanningsNormSfo.Id);
            BemanningsNormSfoDropDown.DataTextField = Utilities.GetPropertyName(() => elev.BemanningsNormSfo.Navn);
            BemanningsNormSfoDropDown.DataBind();

            // Default value
            BemanningsNormSfoDropDown.Items.Insert(0, new ListItem("Velg BN", "0"));
            BemanningsNormSfoDropDown.SelectedValue = elev.BemanningsNormSfo != null ? elev.BemanningsNormSfo.Id.ToString() : "0";

            BindToListView(elev);
        }

        private void BindToListView(Elev elev)
        {
            var ukeTyper = UkeType.GetAll().OrderBy(u => u.Navn);

            var elevTilstedeViewList = new List<object>();

            if (elev.ElevTilstedes.Count == 0)
            {
                elev.AddElevTilstede();
            }

            var elevTilstedes = elev.ElevTilstedes;

            foreach (var elevTilstede in elevTilstedes)
            {
                elevTilstedeViewList.Add(new
                {
                    elevTilstede.Id,
                    MandagStart = TimeAsString(elevTilstede.MandagStart),
                    MandagSlutt = TimeAsString(elevTilstede.MandagSlutt),
                    TirsdagStart = TimeAsString(elevTilstede.TirsdagStart),
                    TirsdagSlutt = TimeAsString(elevTilstede.TirsdagSlutt),
                    OnsdagStart = TimeAsString(elevTilstede.OnsdagStart),
                    OnsdagSlutt = TimeAsString(elevTilstede.OnsdagSlutt),
                    TorsdagStart = TimeAsString(elevTilstede.TorsdagStart),
                    TorsdagSlutt = TimeAsString(elevTilstede.TorsdagSlutt),
                    FredagStart = TimeAsString(elevTilstede.FredagStart),
                    FredagSlutt = TimeAsString(elevTilstede.FredagSlutt)
                });
            }

            ElevTilstedeListView.DataSource = elevTilstedeViewList;
            ElevTilstedeListView.DataBind();

            foreach (var item in ElevTilstedeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var elevTilstede = elevTilstedes.FirstOrDefault(e => e.Id.ToString() == idButton.Text);

                    var ukeTypeDropDown = (DropDownList)item.FindControl("UkeTypeDropDown");
                    ukeTypeDropDown.DataSource = ukeTyper;
                    ukeTypeDropDown.DataValueField = Utilities.GetPropertyName(() => elevTilstede.UkeType.Id);
                    ukeTypeDropDown.DataTextField = Utilities.GetPropertyName(() => elevTilstede.UkeType.Navn);
                    ukeTypeDropDown.DataBind();

                    if (elevTilstede.UkeType != null)
                        ukeTypeDropDown.SelectedValue = elevTilstede.UkeType.Id.ToString();
                }
            }
        }

        private string TimeAsString(TimeSpan? time)
        {
            return time != null ? time.ToString().Substring(0, 5) : string.Empty;
        }


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var elev = UpdateDataSource();

                if (elev.IsChanged)
                {
                    elev.Save();

                    elev = Elev.GetById(elev.Id);

                    UpdateBinding(elev);
                }
            }
        }


        private Elev UpdateDataSource()
        {
            var elev = (Elev)Session["currentElev"];
            var elevId = elev.Id;

            ICollection<ElevTilstede> elevTilstedes = new List<ElevTilstede>();

            foreach (var item in ElevTilstedeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var mandagStartTextBox = (TextBox)item.FindControl("MandagStartTextBox");
                    var mandagSluttTextBox = (TextBox)item.FindControl("MandagSluttTextBox");
                    var tirsdagStartTextBox = (TextBox)item.FindControl("TirsdagStartTextBox");
                    var tirsdagSluttTextBox = (TextBox)item.FindControl("TirsdagSluttTextBox");
                    var onsdagStartTextBox = (TextBox)item.FindControl("OnsdagStartTextBox");
                    var onsdagSluttTextBox = (TextBox)item.FindControl("OnsdagSluttTextBox");
                    var torsdagStartTextBox = (TextBox)item.FindControl("TorsdagStartTextBox");
                    var torsdagSluttTextBox = (TextBox)item.FindControl("TorsdagSluttTextBox");
                    var fredagStartTextBox = (TextBox)item.FindControl("FredagStartTextBox");
                    var fredagSluttTextBox = (TextBox)item.FindControl("FredagSluttTextBox");

                    var ukeTypeDropDown = (DropDownList)item.FindControl("UkeTypeDropDown");

                    var elevTilstedeId = Convert.ToInt32(idButton.Text);

                    var elevTilstede = new ElevTilstede
                    {
                        Id = elevTilstedeId,
                        MandagStart = string.IsNullOrWhiteSpace(mandagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(mandagStartTextBox.Text),
                        MandagSlutt = string.IsNullOrWhiteSpace(mandagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(mandagSluttTextBox.Text),
                        TirsdagStart = string.IsNullOrWhiteSpace(tirsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(tirsdagStartTextBox.Text),
                        TirsdagSlutt = string.IsNullOrWhiteSpace(tirsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(tirsdagSluttTextBox.Text),
                        OnsdagStart = string.IsNullOrWhiteSpace(onsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(onsdagStartTextBox.Text),
                        OnsdagSlutt = string.IsNullOrWhiteSpace(onsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(onsdagSluttTextBox.Text),
                        TorsdagStart = string.IsNullOrWhiteSpace(torsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(torsdagStartTextBox.Text),
                        TorsdagSlutt = string.IsNullOrWhiteSpace(torsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(torsdagSluttTextBox.Text),
                        FredagStart = string.IsNullOrWhiteSpace(fredagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(fredagStartTextBox.Text),
                        FredagSlutt = string.IsNullOrWhiteSpace(fredagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(fredagSluttTextBox.Text),
                        Elev = elev,
                        UkeType = UkeType.GetById(Convert.ToInt32(ukeTypeDropDown.SelectedValue))
                    };

                    elevTilstedes.Add(elevTilstede);
                }
            }

            elev.Update(
                    NavnTextBox.Text,
                    Convert.ToDecimal(SfoProsentTextBox.Text),
                    Convert.ToInt32(KlasseDropDown.SelectedValue),
                    Convert.ToInt32(SfoDropDown.SelectedValue),
                    Convert.ToInt32(TrinnDropDown.SelectedValue),
                    Convert.ToInt32(HovedPedagogDropDown.SelectedValue),
                    Convert.ToInt32(BemanningsNormSkoleDropDown.SelectedValue),
                    Convert.ToInt32(BemanningsNormSfoDropDown.SelectedValue),
                    TlfnrTextBox.Text,
                    elevTilstedes
                    );

            return elev;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);

            if (id > 0)
            {
                var elev = Elev.GetById(id);
                UpdateBinding(elev);
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
                var elev = UpdateDataSource();

                if (elev.IsChanged)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    Response.Redirect("ElevDetails.aspx?Id=" + HttpUtility.UrlEncode("0"));
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);
            var elev = id > 0 ? Elev.GetById(Convert.ToInt32(IdTextBox.Text)) : null;

            if (elev != null)
            {
                elev.Delete();
                var allElever = (IList<Elev>)Session["allElever"];
                allElever.Remove(elev);
            }

            GoTo(0);
        }

        protected void AddElevTilstedeButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var elev = UpdateDataSource();

                elev.AddElevTilstede();

                UpdateBinding(elev);
            }
        }

        protected void DeleteElevTilstedeButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var elev = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                elev.DeleteElevTilstede(id);

                UpdateBinding(elev);
            }
        }

        protected void GoBackButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var elev = UpdateDataSource();

                if (elev.IsChanged)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    Response.Redirect("~/Lists/ElevList.aspx");
                }
            }
        }

        protected void PreviousButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var elev = UpdateDataSource();

                if (elev.IsChanged)
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
                var elev = UpdateDataSource();

                if (elev.IsChanged)
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
            var allElever = (IList<Elev>)Session["allElever"];
            var currentIndex = Convert.ToInt32(Session["ElevDetails - CurrentIndex"]);
            var navigateToIndex = currentIndex + direction;
            if (navigateToIndex == allElever.Count)
            {
                navigateToIndex = 0;
            }
            else if (navigateToIndex == -1)
            {
                navigateToIndex = allElever.Count - 1;
            }

            var elevId = allElever[navigateToIndex].Id;
            Response.Redirect("ElevDetails.aspx?Id=" + HttpUtility.UrlEncode(elevId.ToString()));
        }

        protected void TrinnDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var elev = UpdateDataSource();
            UpdateBinding(elev);
        }

        protected void SfoDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var elev = UpdateDataSource();
            UpdateBinding(elev);
        }

    }
}