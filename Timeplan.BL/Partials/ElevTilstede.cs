using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class ElevTilstede
    {
        public void Update(
            TimeSpan? mandagStart,
            TimeSpan? mandagSlutt,
            TimeSpan? tirsdagStart,
            TimeSpan? tirsdagSlutt,
            TimeSpan? onsdagStart,
            TimeSpan? onsdagSlutt,
            TimeSpan? torsdagStart,
            TimeSpan? torsdagSlutt,
            TimeSpan? fredagStart,
            TimeSpan? fredagSlutt,
            int elevId,
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

            if (Elev == null && IsChanged)
            {
                Elev = Elev.GetById(elevId);
            }

            if (UkeType == null && IsChanged || 
                UkeType != null && UkeType.Id != ukeTypeId)
            {
                UkeType = UkeType.GetById(ukeTypeId);
                IsChanged = true;
            }
        }

        public ElevTilstede Save()
        {
            var db = new TimeplanEntities();

            var elevTilstede = Id > 0 ? GetById(Id, db) : new ElevTilstede();

            if (elevTilstede != null)
            {
                elevTilstede.Copy(this);

                if (Id <= 0)
                {
                    db.ElevTilstedes.Add(elevTilstede);
                }

                db.SaveChanges();

                Id = elevTilstede.Id;
            }

            IsChanged = false;

            return elevTilstede;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var elevTilstede = GetById(Id, db);

                if (elevTilstede != null)
                {
                    db.ElevTilstedes.Remove(elevTilstede);

                    db.SaveChanges();
                }
            }
        }

        public void Copy(ElevTilstede copyFrom)
        {
            MandagStart = copyFrom.MandagStart;
            MandagSlutt = copyFrom.MandagSlutt;
            TirsdagStart = copyFrom.TirsdagStart;
            TirsdagSlutt = copyFrom.TirsdagSlutt;
            OnsdagStart = copyFrom.OnsdagStart;
            OnsdagSlutt = copyFrom.OnsdagSlutt;
            TorsdagStart = copyFrom.TorsdagStart;
            TorsdagSlutt = copyFrom.TorsdagSlutt;
            FredagStart = copyFrom.FredagStart;
            FredagSlutt = copyFrom.FredagSlutt;
            
            fk_ElevId = copyFrom.Elev.Id;
            fk_UkeTypeId = copyFrom.UkeType.Id;
        }

        public static IEnumerable<ElevTilstede> GetAll()
        {
            var db = new TimeplanEntities();
            return db.ElevTilstedes;
        }

        public static ElevTilstede GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static ElevTilstede GetById(int id, TimeplanEntities db)
        {
            return db.ElevTilstedes.FirstOrDefault(elevTilstede => elevTilstede.Id == id);
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
