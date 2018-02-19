using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class AnsattTilstede
    {
        public void Update(
            TimeSpan? mandagStart,
            TimeSpan? mandagSlutt,
            bool mandagFri,
            TimeSpan? tirsdagStart,
            TimeSpan? tirsdagSlutt,
            bool tirsdagFri,
            TimeSpan? onsdagStart,
            TimeSpan? onsdagSlutt,
            bool onsdagFri,
            TimeSpan? torsdagStart,
            TimeSpan? torsdagSlutt,
            bool torsdagFri,
            TimeSpan? fredagStart,
            TimeSpan? fredagSlutt,
            bool fredagFri,
            bool skole, 
            int ansattId,
            int ukeTypeId)
        {

            if (MandagStart != mandagStart)
            {
                MandagStart = mandagStart;
                IsChanged = true;
            }

            if (MandagSlutt != mandagSlutt)
            {
                MandagSlutt = mandagSlutt;
                IsChanged = true;
            }

            if (MandagFri != mandagFri)
            {
                MandagFri = mandagFri;
                IsChanged = true;
            }

            if (TirsdagStart != tirsdagStart)
            {
                TirsdagStart = tirsdagStart;
                IsChanged = true;
            }

            if (TirsdagSlutt != tirsdagSlutt)
            {
                TirsdagSlutt = tirsdagSlutt;
                IsChanged = true;
            }

            if (TirsdagFri != tirsdagFri)
            {
                TirsdagFri = tirsdagFri;
                IsChanged = true;
            }

            if (OnsdagStart != onsdagStart)
            {
                OnsdagStart = onsdagStart;
                IsChanged = true;
            }

            if (OnsdagSlutt != onsdagSlutt)
            {
                OnsdagSlutt = onsdagSlutt;
                IsChanged = true;
            }

            if (OnsdagFri != onsdagFri)
            {
                OnsdagFri = onsdagFri;
                IsChanged = true;
            }

            if (TorsdagStart != torsdagStart)
            {
                TorsdagStart = torsdagStart;
                IsChanged = true;
            }

            if (TorsdagSlutt != torsdagSlutt)
            {
                TorsdagSlutt = torsdagSlutt;
                IsChanged = true;
            }

            if (TorsdagFri != torsdagFri)
            {
                TorsdagFri = torsdagFri;
                IsChanged = true;
            }

            if (FredagStart != fredagStart)
            {
                FredagStart = fredagStart;
                IsChanged = true;
            }

            if (FredagSlutt != fredagSlutt)
            {
                FredagSlutt = fredagSlutt;
                IsChanged = true;
            }

            if (FredagFri != fredagFri)
            {
                FredagFri = fredagFri;
                IsChanged = true;
            }

            if (Skole != skole)
            {
                Skole = skole;
                IsChanged = true;
            }

            if (Ansatt == null)
            {
                Ansatt = Ansatt.GetById(ansattId);
            }

            if (UkeType == null || 
                UkeType != null && UkeType.Id != ukeTypeId)
            {
                UkeType = UkeType.GetById(ukeTypeId);
                IsChanged = true;
            }

        }

        public AnsattTilstede Save()
        {
            var db = new TimeplanEntities();

            var ansattTilstede = Id > 0 ? GetById(Id, db) : new AnsattTilstede();

            if (ansattTilstede != null) // or else it is deleted
            {
                ansattTilstede.Copy(this);

                if (Id <= 0)
                {
                    db.AnsattTilstedes.Add(ansattTilstede);
                }

                db.SaveChanges();

                Id = ansattTilstede.Id;
            }

            IsChanged = false;

            return ansattTilstede;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var ansattTilstede = GetById(Id, db);

                if (ansattTilstede != null)
                {
                    db.AnsattTilstedes.Remove(ansattTilstede);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(AnsattTilstede copyFrom)
        {
            MandagStart = copyFrom.MandagStart;
            MandagSlutt = copyFrom.MandagSlutt;
            MandagFri = copyFrom.MandagFri;
            TirsdagStart = copyFrom.TirsdagStart;
            TirsdagSlutt = copyFrom.TirsdagSlutt;
            TirsdagFri = copyFrom.TirsdagFri;
            OnsdagStart = copyFrom.OnsdagStart;
            OnsdagSlutt = copyFrom.OnsdagSlutt;
            OnsdagFri = copyFrom.OnsdagFri;
            TorsdagStart = copyFrom.TorsdagStart;
            TorsdagSlutt = copyFrom.TorsdagSlutt;
            TorsdagFri = copyFrom.TorsdagFri;
            FredagStart = copyFrom.FredagStart;
            FredagSlutt = copyFrom.FredagSlutt;
            FredagFri = copyFrom.FredagFri;
            Skole = copyFrom.Skole;

            fk_AnsattId = copyFrom.Ansatt.Id;
            fk_UkeTypeId = copyFrom.UkeType.Id;
        }

        public static IEnumerable<AnsattTilstede> GetAll()
        {
            var db = new TimeplanEntities();
            return db.AnsattTilstedes;
        }

        public static AnsattTilstede GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static AnsattTilstede GetById(int id, TimeplanEntities db)
        {
            return db.AnsattTilstedes.FirstOrDefault(ansattTilstede => ansattTilstede.Id == id);
        }

        public bool IsChanged { get; set; }

        public double TotalHours()
        {
            var totalHours =
                (MandagSlutt.HasValue && MandagStart.HasValue ? (MandagSlutt - MandagStart).Value.TotalHours : 0) +
                (TirsdagSlutt.HasValue && TirsdagStart.HasValue ? (TirsdagSlutt - TirsdagStart).Value.TotalHours : 0) +
                (OnsdagSlutt.HasValue && OnsdagStart.HasValue ? (OnsdagSlutt - OnsdagStart).Value.TotalHours : 0) +
                (TorsdagSlutt.HasValue && TorsdagStart.HasValue ? (TorsdagSlutt - TorsdagStart).Value.TotalHours : 0) +
                (FredagSlutt.HasValue && FredagStart.HasValue ? (FredagSlutt - FredagStart).Value.TotalHours : 0);

            return totalHours;
        }

    }
}
