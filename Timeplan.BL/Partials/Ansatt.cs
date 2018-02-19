using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class Ansatt
    {
        private int AnsattTilstedeDummyId = 0;

        public void Update(string navn, decimal stillingsStørrelse, string tlfNr, int avdelingId, int stillingsTypeId, IList<int> jobberIKlasserIds, IList<int> jobberISfosIds, ICollection<AnsattTilstede> ansattTilstedes)
        {
            Update(navn, stillingsStørrelse, tlfNr, avdelingId, stillingsTypeId, jobberIKlasserIds, jobberISfosIds);

            foreach (var ansattTilstede in ansattTilstedes)
            {
                var existingAnsattTilstede = AnsattTilstedes.First(e => e.Id == ansattTilstede.Id);

                existingAnsattTilstede.Update(
                    ansattTilstede.MandagStart,
                        ansattTilstede.MandagSlutt,
                        ansattTilstede.MandagFri,
                        ansattTilstede.TirsdagStart,
                        ansattTilstede.TirsdagSlutt,
                        ansattTilstede.TirsdagFri,
                        ansattTilstede.OnsdagStart,
                        ansattTilstede.OnsdagSlutt,
                        ansattTilstede.OnsdagFri,
                        ansattTilstede.TorsdagStart,
                        ansattTilstede.TorsdagSlutt,
                        ansattTilstede.TorsdagFri,
                        ansattTilstede.FredagStart,
                        ansattTilstede.FredagSlutt,
                        ansattTilstede.FredagFri,
                        ansattTilstede.Skole,
                        ansattTilstede.Ansatt.Id,
                        ansattTilstede.UkeType.Id
                        );
            }

            if (AnsattTilstedes.Any(ansattTilstede => ansattTilstede.IsChanged))
            {
                IsChanged = true;
            }

        }


        public void Update(string navn, decimal stillingsStørrelse, string tlfNr, int avdelingId, int stillingsTypeId, IList<int> jobberIKlasserIds, IList<int> jobberISfosIds /*, IList<int> avdelingsLederIAvdelingIds , int varslesAvAnsattId, IList<int> varslerTilAnsatteIds*/)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (Stillingsstørrelse != stillingsStørrelse)
            {
                Stillingsstørrelse = stillingsStørrelse;
                IsChanged = true;
            }

            if (Tlfnr != tlfNr)
            {
                Tlfnr = tlfNr;
                IsChanged = true;
            }

            if (Avdeling == null || Avdeling.Id != avdelingId)
            {
                Avdeling = Avdeling.GetById(avdelingId);
                IsChanged = true;
            }

            if (StillingsType == null || StillingsType.Id != stillingsTypeId)
            {
                StillingsType = StillingsType.GetById(stillingsTypeId);
                IsChanged = true;
            }

            var jobberIKlasserIdsOld = JobberIKlasser.OrderBy(klasse => klasse.Id).Select(klasse => klasse.Id).ToList();

            if (jobberIKlasserIdsOld.Count != jobberIKlasserIds.Count
                || jobberIKlasserIdsOld.SequenceEqual(jobberIKlasserIds.OrderBy(i => i)) == false)
            {
                JobberIKlasser.Clear();
                foreach (var klasseId in jobberIKlasserIds)
                {
                    JobberIKlasser.Add(Klasse.GetById(klasseId));
                }
                IsChanged = true;
            }

            var jobberISfosIdsOld = JobberISfos.OrderBy(sfo => sfo.Id).Select(sfo => sfo.Id).ToList();

            if (jobberISfosIdsOld.Count != jobberISfosIds.Count
                || jobberISfosIdsOld.SequenceEqual(jobberISfosIds.OrderBy(i => i)) == false)
            {
                JobberISfos.Clear();
                foreach (var sfoId in jobberISfosIds)
                {
                    JobberISfos.Add(Sfo.GetById(sfoId));
                }
                IsChanged = true;
            }

            //var avdelingsLederIAvdelingsIdsOld = AvdelingsLederIAvdelinger.OrderBy(avdeling => avdeling.Id).Select(avdeling => avdeling.Id).ToList();

            //if (avdelingsLederIAvdelingsIdsOld.Count != avdelingsLederIAvdelingIds.Count
            //    || avdelingsLederIAvdelingsIdsOld.SequenceEqual(avdelingsLederIAvdelingIds.OrderBy(i => i)) == false)
            //{
            //    AvdelingsLederIAvdelinger.Clear();
            //    foreach (var id in avdelingsLederIAvdelingIds)
            //    {
            //        AvdelingsLederIAvdelinger.Add(Avdeling.GetById(id));
            //    }
            //    IsChanged = true;
            //}

            //if ((fk_VarslesAvAnsattId != null && fk_VarslesAvAnsattId != varslesAvAnsattId)
            //    || (fk_VarslesAvAnsattId == null && varslesAvAnsattId != 0))
            //{
            //    VarslesAvAnsatt = varslesAvAnsattId > 0 ? GetById(varslesAvAnsattId) : null;
            //    IsChanged = true;
            //}

            //var varslerTilAnsatteIdsOld = VarslerTilAnsatte.Select(ansatt => ansatt.Id).ToList();

            //if (varslerTilAnsatteIdsOld.Count != varslerTilAnsatteIds.Count
            //    || varslerTilAnsatteIdsOld.SequenceEqual(varslerTilAnsatteIds) == false)
            //{
            //    VarslerTilAnsatte.Clear();
            //    foreach (var ansattId in varslerTilAnsatteIds)
            //    {
            //        VarslerTilAnsatte.Add(GetById(ansattId));
            //    }
            //    IsChanged = true;
            //}

        }

        public Ansatt Save()
        {
            var db = new TimeplanEntities();

            var ansatt = Id > 0 ? GetById(Id, db) : new Ansatt();

            if (ansatt != null) // or else it is deleted
            {
                ansatt.Copy(this, db);

                if (Id <= 0)
                {
                    db.Ansatts.Add(ansatt);
                }

                db.SaveChanges();

                Id = ansatt.Id;

                SaveAnsattTilstedes();
            }

            IsChanged = false;

            return ansatt;
        }

        private void SaveAnsattTilstedes()
        {
            if (AnsattTilstedes.Count == 0)
            {
                var newAnsattTilstede = new AnsattTilstede { IsChanged = true, Ansatt = this, UkeType = UkeType.GetById((int)UkeTypeEnum.LikUke) };
                AnsattTilstedes.Add(newAnsattTilstede);
            }

            if (AnsattTilstedes.Any(ansattTilstede => ansattTilstede.IsChanged))
            {
                foreach (var ansattTilstede in AnsattTilstedes)
                {
                    if (ansattTilstede.IsChanged)
                    {
                        ansattTilstede.Save();
                    }
                }
            }

            var ansattTilstedesOld = AnsattTilstede.GetAll().Where(ansattTilstede => ansattTilstede.Ansatt.Id == Id);

            foreach (var ansattTilstede in ansattTilstedesOld)
            {
                if (AnsattTilstedes.Any(t => t.Id == ansattTilstede.Id) == false)
                {
                    ansattTilstede.Delete();
                }
            }
        }

        public void Delete()
        {
            if (Id > 0)
            {
                foreach (AnsattTilstede ansattTilstede in AnsattTilstedes)
                {
                    ansattTilstede.Delete();
                }

                var db = new TimeplanEntities();

                var ansatt = GetById(Id, db);

                ansatt.JobberISfos.Clear();
                ansatt.JobberIKlasser.Clear();

                db.Ansatts.Remove(ansatt);

                db.SaveChanges();
            }
        }

        private void Copy(Ansatt copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            Stillingsstørrelse = copyFrom.Stillingsstørrelse;
            Tlfnr = copyFrom.Tlfnr;
            fk_AvdelingId = copyFrom.Avdeling.Id;
            fk_StillingsTypeId = copyFrom.StillingsType.Id;

            JobberIKlasser.Clear();
            foreach (var klasse in copyFrom.JobberIKlasser)
            {
                JobberIKlasser.Add(Klasse.GetById(klasse.Id, db));
            }

            JobberISfos.Clear();
            foreach (var sfo in copyFrom.JobberISfos)
            {
                JobberISfos.Add(Sfo.GetById(sfo.Id, db));
            }

            //AvdelingsLederIAvdelinger.Clear();
            //foreach (var avdeling in copyFrom.AvdelingsLederIAvdelinger)
            //{
            //    AvdelingsLederIAvdelinger.Add(Avdeling.GetById(avdeling.Id, db));
            //}

            //fk_VarslesAvAnsattId = copyFrom.fk_VarslesAvAnsattId;
            //VarslerTilAnsatte.Clear();
            //foreach (var ansatt in copyFrom.VarslerTilAnsatte)
            //{
            //    VarslerTilAnsatte.Add(GetById(ansatt.Id, db));
            //}

            //AvdelingsLederIAvdelinger = copyFrom.AvdelingsLederIAvdelinger; // TODO:
            //Sfoes = copyFrom.Sfoes; // TODO:
        }

        public void AddAnsattTilstede()
        {
            AnsattTilstede newAnsattTilstede;

            var existingAnsattTilstede = AnsattTilstedes.Count > 0 ? AnsattTilstedes.First() : null;

            if (existingAnsattTilstede != null)
            {
                newAnsattTilstede = new AnsattTilstede
                {
                    Id = AnsattTilstedeDummyId--,
                    IsChanged = true,
                    Ansatt = this,
                    UkeType = UkeType.GetById((int)UkeTypeEnum.UlikUke),
                    MandagStart = existingAnsattTilstede.MandagStart,
                    MandagSlutt = existingAnsattTilstede.MandagSlutt,
                    MandagFri = existingAnsattTilstede.MandagFri,
                    TirsdagStart = existingAnsattTilstede.TirsdagStart,
                    TirsdagSlutt = existingAnsattTilstede.TirsdagSlutt,
                    TirsdagFri = existingAnsattTilstede.TirsdagFri,
                    OnsdagStart = existingAnsattTilstede.OnsdagStart,
                    OnsdagSlutt = existingAnsattTilstede.OnsdagSlutt,
                    OnsdagFri = existingAnsattTilstede.OnsdagFri,
                    TorsdagStart = existingAnsattTilstede.TorsdagStart,
                    TorsdagSlutt = existingAnsattTilstede.TorsdagSlutt,
                    TorsdagFri = existingAnsattTilstede.TorsdagFri,
                    FredagStart = existingAnsattTilstede.FredagStart,
                    FredagSlutt = existingAnsattTilstede.FredagSlutt,
                    FredagFri = existingAnsattTilstede.FredagFri
                };
            }
            else
            {
                newAnsattTilstede = new AnsattTilstede { Id = AnsattTilstedeDummyId--, IsChanged = true, Ansatt = this, UkeType = UkeType.GetById((int)UkeTypeEnum.LikUke) };
            }

            AnsattTilstedes.Add(newAnsattTilstede);

            IsChanged = true;
        }

        public void DeleteAnsattTilstede(int id)
        {
            if (AnsattTilstedes.Count > 0)
            {
                var existingAnsattTilstede = AnsattTilstedes.Where(ansattTilstede => ansattTilstede.Id == id).First();
                AnsattTilstedes.Remove(existingAnsattTilstede);

                IsChanged = true;
            }
        }

        public static IEnumerable<Ansatt> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Ansatts;
        }

        public static Ansatt GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Ansatt GetById(int id, TimeplanEntities db)
        {
            return db.Ansatts.FirstOrDefault(ansatt => ansatt.Id == id);
        }

        public ICollection<Avdeling> AvdelingsLederIAvdelinger
        {
            get { return Avdelings; }
        }

        public ICollection<Elev> HovedPedagogForElever
        {
            get { return Elevs; }
        }

        public ICollection<Klasse> JobberIKlasser
        {
            get { return Klasses; }
        }

        public ICollection<Sfo> JobberISfos
        {
            get { return Sfoes; }
        }

        //public ICollection<Ansatt> VarslerTilAnsatte
        //{
        //    get { return Ansatt1; }
        //}

        //public Ansatt VarslesAvAnsatt
        //{
        //    get { return Ansatt2; }
        //    set { Ansatt2 = value; }
        //}

        public bool IsChanged { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Elev return false.
            var ansatt = obj as Ansatt;
            if (ansatt == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == ansatt.Id;
        }

        public bool Equals(Ansatt ansatt)
        {
            // If parameter is null return false:
            if (ansatt == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == ansatt.Id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();

                return hash;
            }
        }

        public decimal TimerPrUke()
        {
            return StillingsType != null ? ((StillingsType.TimerElevarbeid + StillingsType.TimerSamarbeid) * (Stillingsstørrelse / 100)) : 0;
        }

        public double DiffTimer()
        {
            var timerPrUke = TimerPrUke();

            return AnsattTilstedes.Count > 0 ? (AnsattTilstedes.Average(a => a.TotalHours()) - Convert.ToDouble(timerPrUke)) : Convert.ToDouble(-timerPrUke);
        }


        // TODO: ------------------------------------------------------------------------------------------

        //public void Save()
        //{
        //    var db = new TimeplanEntities();
        //    var ansatt = GetById(db, Id);

        //    if (ansatt == null)
        //    {
        //        ansatt = this;
        //        db.Ansatts.Add(ansatt);
        //    }
        //    else
        //    {
        //        ansatt.Copy(this);
        //    }

        //    db.SaveChanges();
        //}

        //private void Copy(Ansatt copyFrom)
        //{
        //    Avdeling = copyFrom.Avdeling;
        //    //AvdelingsLederIAvdelinger = copyFrom.AvdelingsLederIAvdelinger; // TODO:
        //    //Klasses = copyFrom.Klasses; // TODO:
        //    //JobberIKlasser = copyFrom.JobberIKlasser;
        //    Navn = copyFrom.Navn;
        //    //Sfoes = copyFrom.Sfoes; // TODO:
        //    //StillingsType = copyFrom.StillingsType; 
        //    Stillingsstørrelse = copyFrom.Stillingsstørrelse;
        //    Tlfnr = copyFrom.Tlfnr;
        //    //VarslerTilAnsatte = copyFrom.VarslerTilAnsatte; // TODO:
        //    //VarslesAvAnsatt = copyFrom.VarslesAvAnsatt; 
        //    fk_AvdelingId = copyFrom.fk_AvdelingId;
        //    fk_StillingsTypeId = copyFrom.fk_StillingsTypeId;
        //    fk_VarslesAvAnsattId = copyFrom.fk_VarslesAvAnsattId;
        //}
    }
}
