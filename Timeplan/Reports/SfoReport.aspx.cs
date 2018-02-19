using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Timeplan.BL;
using System.Reflection;

namespace Timeplan.Reports
{
    public partial class SfoReport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                var sfos = Sfo.GetAll().OrderBy(s => s.Navn).ToList();
                var defaultSfo = sfos.First();

                SfoAvdelingDropDown.DataSource = sfos;
                SfoAvdelingDropDown.DataValueField = Utilities.GetPropertyName(() => defaultSfo.Id);
                SfoAvdelingDropDown.DataTextField = Utilities.GetPropertyName(() => defaultSfo.Navn);
                SfoAvdelingDropDown.DataBind();
                SfoAvdelingDropDown.SelectedValue = defaultSfo.Id.ToString();

                var ukeTyper = UkeType.GetAll().OrderBy(u => u.Navn).ToList();
                var defaultUkeType = ukeTyper.First();

                UkeTypeDropDown.DataSource = ukeTyper;
                UkeTypeDropDown.DataValueField = Utilities.GetPropertyName(() => defaultUkeType.Id);
                UkeTypeDropDown.DataTextField = Utilities.GetPropertyName(() => defaultUkeType.Navn);
                UkeTypeDropDown.DataBind();
                UkeTypeDropDown.SelectedValue = defaultUkeType.Id.ToString();

                BindToListView();
            }
        }


        private void BindToListView()
        {
            var selectedSfo = Sfo.GetById(Convert.ToInt32(SfoAvdelingDropDown.SelectedValue));

            var sfoReportViewList = new List<object>();

            var allElever = selectedSfo.Elevs.ToList();
            var allAnsatte = selectedSfo.Ansatts.ToList();

            if (allElever.Count() > 0)
            {
                var skole = Skole.GetAll().First();

                var startTime = selectedSfo.TidligvaktTider.StartTid;
                var endTime = selectedSfo.TidligvaktTider.SluttTid;
                var description = selectedSfo.TidligvaktTider.Navn;
                var interval = selectedSfo.TidligvaktTider.TidsIntervall;

                PopulateEleverToViewList(sfoReportViewList, allElever, startTime, endTime, interval, description, startTid: true);
                PopulateAnsatteToViewList(sfoReportViewList, allAnsatte, startTime, endTime, interval, description, startTid: true);

                interval = selectedSfo.TidsInndeling.TidsIntervall;
                startTime = skole.TidsInndeling.SluttTid.Add(interval);
                endTime = selectedSfo.SeinvaktTider.SluttTid.Add(interval);

                var klasser = Klasse.GetAll().OrderBy(k => k.Navn).ToList();
                foreach (var klasse in klasser)
                {
                    var eleverIKlasse = allElever.Where(e => e.Klasse.Navn == klasse.Navn);
                    if (eleverIKlasse.Count() > 0)
                    {
                        PopulateEleverToViewList(sfoReportViewList, eleverIKlasse, startTime, endTime, interval, klasse.Navn, startTid: false);
                    }

                    var ansatteIKlasse = allAnsatte.Where(a => a.JobberIKlasser.Any(k => k.Navn == klasse.Navn));
                    if (ansatteIKlasse.Count() > 0)
                    {
                        PopulateAnsatteToViewList(sfoReportViewList, ansatteIKlasse, startTime, endTime, interval, klasse.Navn, startTid: false);
                    }
                }

                interval = selectedSfo.SeinvaktTider.TidsIntervall;
                startTime = selectedSfo.SeinvaktTider.StartTid;
                endTime = selectedSfo.SeinvaktTider.SluttTid.Add(interval);
                description = selectedSfo.SeinvaktTider.Navn;

                PopulateEleverToViewList(sfoReportViewList, allElever, startTime, endTime, interval, description, startTid: false);
                PopulateAnsatteToViewList(sfoReportViewList, allAnsatte, startTime, endTime, interval, description, startTid: false);
            }

            SfoReportListView.DataSource = sfoReportViewList;
            SfoReportListView.DataBind();
        }


        private List<object> PopulateEleverToViewList(List<object> sfoReportViewList, IEnumerable<Elev> elever, TimeSpan time, TimeSpan endTime, TimeSpan interval, string description, bool startTid)
        {
            IEnumerable<Elev> eleverMandag;
            IEnumerable<Elev> eleverTirsdag;
            IEnumerable<Elev> eleverOnsdag;
            IEnumerable<Elev> eleverTorsdag;
            IEnumerable<Elev> eleverFredag;

            var numberOfAddedListViewRows = 0;

            while (time < endTime)
            {
                // INFO: used to make propertyname typesafe/statically typed
                var elevTilstede = new ElevTilstede();

                if (startTid)
                {
                    eleverMandag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.MandagStart))));
                    eleverTirsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.TirsdagStart))));
                    eleverOnsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.OnsdagStart))));
                    eleverTorsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.TorsdagStart))));
                    eleverFredag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.FredagStart))));
                }
                else
                {
                    eleverMandag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.MandagSlutt))));
                    eleverTirsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.TirsdagSlutt))));
                    eleverOnsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.OnsdagSlutt))));
                    eleverTorsdag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.TorsdagSlutt))));
                    eleverFredag = elever.Where(elev => ElevTilstedePåTid(elev, time, typeof(ElevTilstede).GetProperty(Utilities.GetPropertyName(() => elevTilstede.FredagSlutt))));
                }

                if (eleverMandag.Count() > 0 ||
                    eleverTirsdag.Count() > 0 ||
                    eleverOnsdag.Count() > 0 ||
                    eleverTorsdag.Count() > 0 ||
                    eleverFredag.Count() > 0)
                {
                    var formattedTime = time.ToString(@"hh\:mm");

                    sfoReportViewList.Add(new
                    {
                        Description = description,
                        MandagTid = FormatTime(eleverMandag, formattedTime),
                        MandagElever = FormatElever(eleverMandag),
                        TirsdagTid = FormatTime(eleverTirsdag, formattedTime),
                        TirsdagElever = FormatElever(eleverTirsdag),
                        OnsdagTid = FormatTime(eleverOnsdag, formattedTime),
                        OnsdagElever = FormatElever(eleverOnsdag),
                        TorsdagTid = FormatTime(eleverTorsdag, formattedTime),
                        TorsdagElever = FormatElever(eleverTorsdag),
                        FredagTid = FormatTime(eleverFredag, formattedTime),
                        FredagElever = FormatElever(eleverFredag),
                        CSSClass = numberOfAddedListViewRows == 0 ? "row-standard line-solid" : "row-standard"
                    });

                    numberOfAddedListViewRows++;

                    // INFO: to make sure only distinct descriptions are added to list
                    description = string.Empty;
                }

                time = time.Add(interval);
            }

            return sfoReportViewList;
        }

        private List<object> PopulateAnsatteToViewList(List<object> sfoReportViewList, IEnumerable<Ansatt> ansatte, TimeSpan time, TimeSpan endTime, TimeSpan interval, string description, bool startTid)
        {
            description = string.Empty; // INFO: not in use at the moment...

            IEnumerable<Ansatt> ansatteMandag;
            IEnumerable<Ansatt> ansatteTirsdag;
            IEnumerable<Ansatt> ansatteOnsdag;
            IEnumerable<Ansatt> ansatteTorsdag;
            IEnumerable<Ansatt> ansatteFredag;

            var numberOfAddedListViewRows = 0;

            while (time < endTime)
            {
                // INFO: used to make propertyname typesafe/statically typed
                var ansattTilstede = new AnsattTilstede();

                if (startTid)
                {
                    ansatteMandag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.MandagStart))));
                    ansatteTirsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.TirsdagStart))));
                    ansatteOnsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.OnsdagStart))));
                    ansatteTorsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.TorsdagStart))));
                    ansatteFredag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.FredagStart))));
                }
                else
                {
                    ansatteMandag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.MandagSlutt))));
                    ansatteTirsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.TirsdagSlutt))));
                    ansatteOnsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.OnsdagSlutt))));
                    ansatteTorsdag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.TorsdagSlutt))));
                    ansatteFredag = ansatte.Where(ansatt => AnsattTilstedePåTid(ansatt, time, typeof(AnsattTilstede).GetProperty(Utilities.GetPropertyName(() => ansattTilstede.FredagSlutt))));
                }

                if (ansatteMandag.Count() > 0 ||
                    ansatteTirsdag.Count() > 0 ||
                    ansatteOnsdag.Count() > 0 ||
                    ansatteTorsdag.Count() > 0 ||
                    ansatteFredag.Count() > 0)
                {
                    var formattedTime = time.ToString(@"hh\:mm");

                    sfoReportViewList.Add(new
                    {
                        Description = description,
                        MandagTid = FormatTime(ansatteMandag, formattedTime),
                        MandagElever = FormatAnsatte(ansatteMandag),
                        TirsdagTid = FormatTime(ansatteTirsdag, formattedTime),
                        TirsdagElever = FormatAnsatte(ansatteTirsdag),
                        OnsdagTid = FormatTime(ansatteOnsdag, formattedTime),
                        OnsdagElever = FormatAnsatte(ansatteOnsdag),
                        TorsdagTid = FormatTime(ansatteTorsdag, formattedTime),
                        TorsdagElever = FormatAnsatte(ansatteTorsdag),
                        FredagTid = FormatTime(ansatteFredag, formattedTime),
                        FredagElever = FormatAnsatte(ansatteFredag),
                        CSSClass = numberOfAddedListViewRows == 0 ? "row-standard line-dashed" : "row-standard"
                    });

                    numberOfAddedListViewRows++;

                    // INFO: to make sure only distinct descriptions are added to list
                    description = string.Empty;
                }

                time = time.Add(interval);
            }

            return sfoReportViewList;
        }

        private static string FormatTime(IEnumerable<Elev> elever, string formattedTime)
        {
            return elever.Count() > 0 ? formattedTime : string.Empty;
        }

        private static string FormatTime(IEnumerable<Ansatt> ansatte, string formattedTime)
        {
            return ansatte.Count() > 0 ? formattedTime : string.Empty;
        }

        private bool ElevTilstedePåTid(Elev elev, TimeSpan time, PropertyInfo propertyInfo)
        {
            var ukeTypeId = Convert.ToInt32(UkeTypeDropDown.SelectedValue);

            return (ukeTypeId == (int)UkeTypeEnum.LikUke && elev.ElevTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time && t.UkeType.Id == ukeTypeId) > 0) ||
                   (ukeTypeId == (int)UkeTypeEnum.UlikUke &&
                        (elev.ElevTilstedes.Count(t => t.UkeType.Id == ukeTypeId) > 0 && elev.ElevTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time && t.UkeType.Id == ukeTypeId) > 0) ||
                        (elev.ElevTilstedes.Count(t => t.UkeType.Id == ukeTypeId) == 0 && elev.ElevTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time) > 0));
        }

        private bool AnsattTilstedePåTid(Ansatt ansatt, TimeSpan time, PropertyInfo propertyInfo)
        {
            var ukeTypeId = Convert.ToInt32(UkeTypeDropDown.SelectedValue);

            return (ukeTypeId == (int)UkeTypeEnum.LikUke && ansatt.AnsattTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time && t.UkeType.Id == ukeTypeId) > 0) ||
                   (ukeTypeId == (int)UkeTypeEnum.UlikUke &&
                        (ansatt.AnsattTilstedes.Count(t => t.UkeType.Id == ukeTypeId) > 0 && ansatt.AnsattTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time && t.UkeType.Id == ukeTypeId) > 0) ||
                        (ansatt.AnsattTilstedes.Count(t => t.UkeType.Id == ukeTypeId) == 0 && ansatt.AnsattTilstedes.Count(t => propertyInfo.GetValue(t) != null && (TimeSpan)propertyInfo.GetValue(t) == time) > 0));
        }

        private static string FormatElever(IEnumerable<Elev> elever)
        {
            return elever.Count() > 0 ? elever.Select(e => e.Navn).Aggregate((current, c) => current + ", " + c) : string.Empty;
        }

        private static string FormatAnsatte(IEnumerable<Ansatt> ansatte)
        {
            return ansatte.Count() > 0 ? ansatte.Select(e => e.Navn).Aggregate((current, c) => current + ", " + c) : string.Empty;
        }

        protected void SfoAvdelingDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindToListView();
        }

        protected void UkeTypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindToListView(); 
        }

    }
}



