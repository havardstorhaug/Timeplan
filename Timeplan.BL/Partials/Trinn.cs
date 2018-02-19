using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class Trinn
    {
        public void Update(string navn, decimal ukeTimeTall, IList<int> eleverIds)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (UkeTimeTall != ukeTimeTall)
            {
                UkeTimeTall = ukeTimeTall;
                IsChanged = true;
            }

            var eleverIdsOld = Elevs.OrderBy(elev => elev.Id).Select(elev => elev.Id).ToList();

            if (eleverIdsOld.Count != eleverIds.Count
                || eleverIdsOld.SequenceEqual(eleverIds.OrderBy(i => i)) == false)
            {
                Elevs.Clear();
                foreach (var elev in eleverIds)
                {
                    Elevs.Add(Elev.GetById(elev));
                }
                IsChanged = true;
            }
        }


        public void Update(string navn, decimal ukeTimeTall, IList<int> eleverIds,
            TimeSpan? mandagStart,
            TimeSpan? mandagSlutt,
            TimeSpan? tirsdagStart,
            TimeSpan? tirsdagSlutt,
            TimeSpan? onsdagStart,
            TimeSpan? onsdagSlutt,
            TimeSpan? torsdagStart,
            TimeSpan? torsdagSlutt,
            TimeSpan? fredagStart,
            TimeSpan? fredagSlutt)
        {
            Update(navn, ukeTimeTall, eleverIds);

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
        }

        public Trinn Save()
        {
            var db = new TimeplanEntities();

            var trinn = Id > 0 ? GetById(Id, db) : new Trinn();

            if (trinn != null)
            {
                trinn.Copy(this, db);

                if (Id <= 0)
                {
                    db.Trinns.Add(trinn);
                }

                db.SaveChanges();

                Id = trinn.Id;
            }

            IsChanged = false;

            return trinn;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var trinn = GetById(Id, db);

                if (trinn != null)
                {
                    db.Trinns.Remove(trinn);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(Trinn copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            UkeTimeTall = copyFrom.UkeTimeTall;

            Elevs.Clear();
            foreach (var elev in copyFrom.Elevs)
            {
                Elevs.Add(Elev.GetById(elev.Id, db));
            }

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
        }

        public static IEnumerable<Trinn> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Trinns;
        }

        public static Trinn GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Trinn GetById(int id, TimeplanEntities db)
        {
            return db.Trinns.FirstOrDefault(trinn => trinn.Id == id);
        }

        public bool IsChanged { get; set; }


        public bool ElevTilstedeIsDefault(ElevTilstede elevTilstede)
        {
            return elevTilstede.MandagStart == MandagStart &&
                elevTilstede.MandagSlutt == MandagSlutt &&
                elevTilstede.TirsdagStart == TirsdagStart &&
                elevTilstede.TirsdagSlutt == TirsdagSlutt &&
                elevTilstede.OnsdagStart == OnsdagStart &&
                elevTilstede.OnsdagSlutt == OnsdagSlutt &&
                elevTilstede.TorsdagStart == TorsdagStart &&
                elevTilstede.TorsdagSlutt == TorsdagSlutt &&
                elevTilstede.FredagStart == FredagStart &&
                elevTilstede.FredagSlutt == FredagSlutt;
        }

        public ElevTilstede GetDefaultElevTilstede(ElevTilstede elevTilstede)
        {
            elevTilstede.MandagStart = MandagStart;
            elevTilstede.MandagSlutt = MandagSlutt;
            elevTilstede.TirsdagStart = TirsdagStart;
            elevTilstede.TirsdagSlutt = TirsdagSlutt;
            elevTilstede.OnsdagStart = OnsdagStart;
            elevTilstede.OnsdagSlutt = OnsdagSlutt;
            elevTilstede.TorsdagStart = TorsdagStart;
            elevTilstede.TorsdagSlutt = TorsdagSlutt;
            elevTilstede.FredagStart = FredagStart;
            elevTilstede.FredagSlutt = FredagSlutt;

            return elevTilstede;
        }
    }
}
