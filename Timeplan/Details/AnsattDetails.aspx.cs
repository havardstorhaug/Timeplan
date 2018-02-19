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
    public partial class AnsattDetails : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                }

                var allAnsatte = (IList<Ansatt>)Session["allAnsatte"];

                var ansattId = Convert.ToInt32(Request.QueryString["Id"]);

                if (ansattId > 0)
                {
                    Session["AnsattDetails - CurrentIndex"] = allAnsatte.IndexOf(allAnsatte.First(ansatt => ansatt.Id == ansattId));
                }

                var currentAnsatt = ansattId > 0 ? Ansatt.GetById(ansattId) : new Ansatt();

                UpdateBinding(currentAnsatt);
            }
        }

        private void UpdateBinding(Ansatt ansatt)
        {
            Session["currentAnsatt"] = ansatt;

            var avdelinger = Avdeling.GetAll().OrderBy(a => a.Navn).ToList();
            var stillingsTyper = StillingsType.GetAll().OrderBy(s => s.Navn).ToList();
            var klasser = Klasse.GetAll().OrderBy(k => k.Navn).ToList();
            var sfos = Sfo.GetAll().OrderBy(s => s.Navn).ToList();

            IdTextBox.Text = ansatt.Id.ToString();
            NavnTextBox.Text = ansatt.Navn;
            StillingsStørrelseTextBox.Text = ansatt.Stillingsstørrelse.ToString();
            TlfNrTextBox.Text = ansatt.Tlfnr;

            TimerPrUkeTextBox.Text = ansatt.TimerPrUke().ToString("N2");

            DiffTimerTextBox.Text = ansatt.DiffTimer().ToString("N2");

            AvdelingDropDown.DataSource = avdelinger;
            AvdelingDropDown.DataValueField = Utilities.GetPropertyName(() => ansatt.Avdeling.Id);
            AvdelingDropDown.DataTextField = Utilities.GetPropertyName(() => ansatt.Avdeling.Navn);
            AvdelingDropDown.DataBind();

            if (ansatt.Avdeling != null)
                AvdelingDropDown.SelectedValue = ansatt.Avdeling.Id.ToString();

            StillingsTypeDropDown.DataSource = stillingsTyper;
            StillingsTypeDropDown.DataValueField = Utilities.GetPropertyName(() => ansatt.StillingsType.Id);
            StillingsTypeDropDown.DataTextField = Utilities.GetPropertyName(() => ansatt.StillingsType.Navn);
            StillingsTypeDropDown.DataBind();

            if (ansatt.StillingsType != null)
                StillingsTypeDropDown.SelectedValue = ansatt.StillingsType.Id.ToString();

            var klasseTeller = 0;
            var klasseSelectedTeller = 0;

            JobberIKlasserListBox.Items.Clear();

            foreach (var klasse in klasser)
            {
                var listItem = new ListItem(klasse.Navn, klasse.Id.ToString());

                if (ansatt.JobberIKlasser.Any(k => k.Id == klasse.Id))
                {
                    listItem.Selected = true;
                }

                if (listItem.Selected)
                {
                    JobberIKlasserListBox.Items.Insert(klasseSelectedTeller++, listItem);
                    klasseTeller++;
                }
                else
                {
                    JobberIKlasserListBox.Items.Insert(klasseTeller++, listItem);
                }
            }

            var sfoTeller = 0;
            var sfoSelectedTeller = 0;

            JobberISfosListBox.Items.Clear();

            foreach (var sfo in sfos)
            {
                var listItem = new ListItem(sfo.Navn, sfo.Id.ToString());

                if (ansatt.JobberISfos.Any(s => s.Id == sfo.Id))
                {
                    listItem.Selected = true;
                }

                if (listItem.Selected)
                {
                    JobberISfosListBox.Items.Insert(sfoSelectedTeller++, listItem);
                    sfoTeller++;
                }
                else
                {
                    JobberISfosListBox.Items.Insert(sfoTeller++, listItem);
                }
            }

            BindToListView(ansatt);
        }

        private void BindToListView(Ansatt ansatt)
        {
            var ukeTyper = UkeType.GetAll().OrderBy(u => u.Navn);

            var ansattTilstedeViewList = new List<object>();

            if (ansatt.AnsattTilstedes.Count == 0)
            {
                ansatt.AddAnsattTilstede();
            }

            var ansattTilstedes = ansatt.AnsattTilstedes;

            foreach (var ansattTilstede in ansattTilstedes)
            {
                ansattTilstedeViewList.Add(new
                {
                    ansattTilstede.Id,
                    MandagStart = TimeAsString(ansattTilstede.MandagStart),
                    MandagSlutt = TimeAsString(ansattTilstede.MandagSlutt),
                    ansattTilstede.MandagFri,
                    TirsdagStart = TimeAsString(ansattTilstede.TirsdagStart),
                    TirsdagSlutt = TimeAsString(ansattTilstede.TirsdagSlutt),
                    ansattTilstede.TirsdagFri,
                    OnsdagStart = TimeAsString(ansattTilstede.OnsdagStart),
                    OnsdagSlutt = TimeAsString(ansattTilstede.OnsdagSlutt),
                    ansattTilstede.OnsdagFri,
                    TorsdagStart = TimeAsString(ansattTilstede.TorsdagStart),
                    TorsdagSlutt = TimeAsString(ansattTilstede.TorsdagSlutt),
                    ansattTilstede.TorsdagFri,
                    FredagStart = TimeAsString(ansattTilstede.FredagStart),
                    FredagSlutt = TimeAsString(ansattTilstede.FredagSlutt),
                    ansattTilstede.FredagFri,
                    ansattTilstede.Skole
                });
            }

            AnsattTilstedeListView.DataSource = ansattTilstedeViewList;
            AnsattTilstedeListView.DataBind();

            foreach (var item in AnsattTilstedeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var ansattTilstede = ansattTilstedes.FirstOrDefault(e => e.Id.ToString() == idButton.Text);

                    var ukeTypeDropDown = (DropDownList)item.FindControl("UkeTypeDropDown");
                    ukeTypeDropDown.DataSource = ukeTyper;
                    ukeTypeDropDown.DataValueField = Utilities.GetPropertyName(() => ansattTilstede.UkeType.Id);
                    ukeTypeDropDown.DataTextField = Utilities.GetPropertyName(() => ansattTilstede.UkeType.Navn);
                    ukeTypeDropDown.DataBind();

                    if (ansattTilstede.UkeType != null)
                        ukeTypeDropDown.SelectedValue = ansattTilstede.UkeType.Id.ToString();
                }
            }
        }

        private string TimeAsString(TimeSpan? time)
        {
            return time != null ? time.ToString().Substring(0, 5) : string.Empty;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var ansatt = UpdateDataSource();

                if (ansatt.IsChanged)
                {
                    ansatt.Save();

                    ansatt = Ansatt.GetById(ansatt.Id);

                    UpdateBinding(ansatt);
                }
            }
        }


        private Ansatt UpdateDataSource()
        {
            var ansatt = (Ansatt)Session["currentAnsatt"];
            var ansattId = ansatt.Id;

            ICollection<AnsattTilstede> ansattTilstedes = new List<AnsattTilstede>();

            foreach (var item in AnsattTilstedeListView.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    var idButton = (Button)item.FindControl("IdButton");
                    var mandagStartTextBox = (TextBox)item.FindControl("MandagStartTextBox");
                    var mandagSluttTextBox = (TextBox)item.FindControl("MandagSluttTextBox");
                    var mandagFriCheckBox = (CheckBox)item.FindControl("MandagFriCheckBox");
                    var tirsdagStartTextBox = (TextBox)item.FindControl("TirsdagStartTextBox");
                    var tirsdagSluttTextBox = (TextBox)item.FindControl("TirsdagSluttTextBox");
                    var tirsdagFriCheckBox = (CheckBox)item.FindControl("TirsdagFriCheckBox");

                    var onsdagStartTextBox = (TextBox)item.FindControl("OnsdagStartTextBox");
                    var onsdagSluttTextBox = (TextBox)item.FindControl("OnsdagSluttTextBox");
                    var onsdagFriCheckBox = (CheckBox)item.FindControl("OnsdagFriCheckBox");

                    var torsdagStartTextBox = (TextBox)item.FindControl("TorsdagStartTextBox");
                    var torsdagSluttTextBox = (TextBox)item.FindControl("TorsdagSluttTextBox");
                    var torsdagFriCheckBox = (CheckBox)item.FindControl("TorsdagFriCheckBox");

                    var fredagStartTextBox = (TextBox)item.FindControl("FredagStartTextBox");
                    var fredagSluttTextBox = (TextBox)item.FindControl("FredagSluttTextBox");
                    var fredagFriCheckBox = (CheckBox)item.FindControl("FredagFriCheckBox");

                    var skoleCheckBox = (CheckBox)item.FindControl("SkoleCheckBox");

                    var ukeTypeDropDown = (DropDownList)item.FindControl("UkeTypeDropDown");

                    var ansattTilstedeId = Convert.ToInt32(idButton.Text);

                    var ansattTilstede = new AnsattTilstede
                    {
                        Id = ansattTilstedeId,
                        MandagStart = string.IsNullOrWhiteSpace(mandagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(mandagStartTextBox.Text),
                        MandagSlutt = string.IsNullOrWhiteSpace(mandagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(mandagSluttTextBox.Text),
                        MandagFri = mandagFriCheckBox.Checked,
                        TirsdagStart = string.IsNullOrWhiteSpace(tirsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(tirsdagStartTextBox.Text),
                        TirsdagSlutt = string.IsNullOrWhiteSpace(tirsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(tirsdagSluttTextBox.Text),
                        TirsdagFri = tirsdagFriCheckBox.Checked,
                        OnsdagStart = string.IsNullOrWhiteSpace(onsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(onsdagStartTextBox.Text),
                        OnsdagSlutt = string.IsNullOrWhiteSpace(onsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(onsdagSluttTextBox.Text),
                        OnsdagFri = onsdagFriCheckBox.Checked,
                        TorsdagStart = string.IsNullOrWhiteSpace(torsdagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(torsdagStartTextBox.Text),
                        TorsdagSlutt = string.IsNullOrWhiteSpace(torsdagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(torsdagSluttTextBox.Text),
                        TorsdagFri = torsdagFriCheckBox.Checked,
                        FredagStart = string.IsNullOrWhiteSpace(fredagStartTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(fredagStartTextBox.Text),
                        FredagSlutt = string.IsNullOrWhiteSpace(fredagSluttTextBox.Text) ? (TimeSpan?)null : TimeSpan.Parse(fredagSluttTextBox.Text),
                        FredagFri = fredagFriCheckBox.Checked,
                        Skole = skoleCheckBox.Checked,
                        Ansatt = ansatt,
                        UkeType = UkeType.GetById(Convert.ToInt32(ukeTypeDropDown.SelectedValue))
                    };

                    ansattTilstedes.Add(ansattTilstede);
                }
            }

            IList<int> jobberIKlasserIds =
                       (from ListItem listItem in JobberIKlasserListBox.Items
                        where listItem.Selected
                        select Convert.ToInt32(listItem.Value)).ToList();

            IList<int> jobberISfoIds =
                (from ListItem listItem in JobberISfosListBox.Items
                 where listItem.Selected
                 select Convert.ToInt32(listItem.Value)).ToList();

            ansatt.Update(
                NavnTextBox.Text,
                Convert.ToDecimal(StillingsStørrelseTextBox.Text),
                TlfNrTextBox.Text,
                Convert.ToInt32(AvdelingDropDown.SelectedValue),
                Convert.ToInt32(StillingsTypeDropDown.SelectedValue),
                jobberIKlasserIds,
                jobberISfoIds,
                ansattTilstedes);

            return ansatt;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);

            if (id > 0)
            {
                var ansatt = Ansatt.GetById(id);
                UpdateBinding(ansatt);
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
                var ansatt = UpdateDataSource();

                if (ansatt.IsChanged)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    Response.Redirect("AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode("0"));
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(IdTextBox.Text);
            var ansatt = id > 0 ? Ansatt.GetById(id) : null;

            if (ansatt != null)
            {
                ansatt.Delete();
                var allAnsatte = (IList<Ansatt>)Session["allAnsatte"];
                allAnsatte.Remove(ansatt);
            }

            GoTo(0);
        }

        protected void AddAnsattTilstedeButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var ansatt = UpdateDataSource();

                ansatt.AddAnsattTilstede();

                UpdateBinding(ansatt);
            }
        }

        protected void DeleteAnsattTilstedeButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var ansatt = UpdateDataSource();

                var button = (LinkButton)sender;
                var id = Convert.ToInt32(button.CommandArgument);

                ansatt.DeleteAnsattTilstede(id);

                UpdateBinding(ansatt);
            }
        }

        protected void GoBackButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var ansatt = UpdateDataSource();

                if (ansatt.IsChanged)
                {
                    WebUtilities.ShowMessageBoxPopUp(this, "Det er ulagrede endringer på siden. Lagre (eller avbryt) disse for å unngå å miste data.");
                }
                else
                {
                    Response.Redirect("~/Lists/AnsattList.aspx");
                }
            }
        }


        protected void PreviousButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var ansatt = UpdateDataSource();

                if (ansatt.IsChanged)
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
                var ansatt = UpdateDataSource();

                if (ansatt.IsChanged)
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
            var allAnsatte = (IList<Ansatt>)Session["allAnsatte"];
            var currentIndex = Convert.ToInt32(Session["AnsattDetails - CurrentIndex"]);
            var navigateToIndex = currentIndex + direction;
            if (navigateToIndex == allAnsatte.Count)
            {
                navigateToIndex = 0;
            }
            else if (navigateToIndex == -1)
            {
                navigateToIndex = allAnsatte.Count - 1;
            }

            var ansattId = allAnsatte[navigateToIndex].Id;
            Response.Redirect("AnsattDetails.aspx?Id=" + HttpUtility.UrlEncode(ansattId.ToString()));
        }

    }
}