using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class Sfo
    {
        public void Update(string navn, IList<int> eleverIds, IList<int> ansatteIds, int åpningsTiderId, int tidligvaktTiderId, int seinvaktTiderId)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            var eleverIdsOld = Elevs.OrderBy(elev => elev.Id).Select(elev => elev.Id).ToList();

            if (eleverIdsOld.Count != eleverIds.Count
                || eleverIdsOld.SequenceEqual(eleverIds.OrderBy(i => i)) == false)
            {
                Elevs.Clear();
                foreach (var elevId in eleverIds)
                {
                    Elevs.Add(Elev.GetById(elevId));
                }
                IsChanged = true;
            }

            var ansattIdsOld = Ansatts.OrderBy(ansatt => ansatt.Id).Select(ansatt => ansatt.Id).ToList();

            if (ansattIdsOld.Count != ansatteIds.Count
                || ansattIdsOld.SequenceEqual(ansatteIds.OrderBy(i => i)) == false)
            {
                Ansatts.Clear();
                foreach (var ansattId in ansatteIds)
                {
                    Ansatts.Add(Ansatt.GetById(ansattId));
                }
                IsChanged = true;
            }

            if (ÅpningsTider == null || ÅpningsTider.Id != åpningsTiderId)
            {
                ÅpningsTider = TidsInndeling.GetById(åpningsTiderId);
                IsChanged = true;
            }

            if (TidligvaktTider == null || TidligvaktTider.Id != tidligvaktTiderId)
            {
                TidligvaktTider = TidsInndeling.GetById(tidligvaktTiderId);
                IsChanged = true;
            }

            if (SeinvaktTider == null || SeinvaktTider.Id != seinvaktTiderId)
            {
                SeinvaktTider = TidsInndeling.GetById(seinvaktTiderId);
                IsChanged = true;
            }
        }

        public Sfo Save()
        {
            var db = new TimeplanEntities();

            var sfo = Id > 0 ? GetById(Id, db) : new Sfo();

            if (sfo != null)
            {
                sfo.Copy(this, db);

                if (Id <= 0)
                {
                    db.Sfoes.Add(sfo);
                }

                db.SaveChanges();

                Id = sfo.Id;
            }

            IsChanged = false;

            return sfo;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var sfo = GetById(Id, db);

                if (sfo != null)
                {
                    sfo.Elevs.Clear();
                    sfo.Ansatts.Clear();

                    db.Sfoes.Remove(sfo);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(Sfo copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;

            Elevs.Clear();
            foreach (var elev in copyFrom.Elevs)
            {
                Elevs.Add(Elev.GetById(elev.Id, db));
            }

            Ansatts.Clear();
            foreach (var ansatt in copyFrom.Ansatts)
            {
                Ansatts.Add(Ansatt.GetById(ansatt.Id, db));
            }

            fk_TidsInndelingId = copyFrom.ÅpningsTider.Id;
            fk_TidligvaktTidsInndelingId = copyFrom.TidligvaktTider.Id;
            fk_SeinvaktTidsInndelingId = copyFrom.SeinvaktTider.Id;
        }

        public static IEnumerable<Sfo> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Sfoes;
        }

        public static Sfo GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Sfo GetById(int id, TimeplanEntities db)
        {
            return db.Sfoes.FirstOrDefault(sfo => sfo.Id == id);
        }

        public TidsInndeling ÅpningsTider
        {
            get
            {
                return TidsInndeling;
            }

            set
            {
                TidsInndeling = value;
            }
        }

        public TidsInndeling TidligvaktTider
        {
            get
            {
                return TidsInndeling1;
            }

            set
            {
                TidsInndeling1 = value;
            }
        }

        public TidsInndeling SeinvaktTider
        {
            get
            {
                return TidsInndeling2;
            }

            set
            {
                TidsInndeling2 = value;
            }
        }

        public bool IsChanged { get; set; }
    }
}