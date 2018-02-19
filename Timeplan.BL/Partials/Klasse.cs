using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class Klasse
    {
        public void Update(string navn, int maksAntallElever, int avdelingId, IList<int> eleverIds, IList<int> ansatteIds)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (MaksAntallElever != maksAntallElever)
            {
                MaksAntallElever = maksAntallElever;
                IsChanged = true;
            }

            if (Avdeling == null || Avdeling.Id != avdelingId)
            {
                Avdeling = Avdeling.GetById(avdelingId);
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
        }

        public Klasse Save()
        {
            var db = new TimeplanEntities();

            var klasse = Id > 0 ? GetById(Id, db) : new Klasse();

            if (klasse != null)
            {
                klasse.Copy(this, db);

                if (Id <= 0)
                {
                    db.Klasses.Add(klasse);
                }

                db.SaveChanges();

                Id = klasse.Id;
            }

            IsChanged = false;

            return klasse;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var klasse = GetById(Id, db);

                if (klasse != null)
                {
                    db.Klasses.Remove(klasse);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(Klasse copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            MaksAntallElever = copyFrom.MaksAntallElever;
            fk_AvdelingId = copyFrom.Avdeling.Id;

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
        }

        public static IEnumerable<Klasse> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Klasses;
        }

        public static Klasse GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Klasse GetById(int id, TimeplanEntities db)
        {
            return db.Klasses.FirstOrDefault(klasse => klasse.Id == id);
        }


        public bool IsChanged { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Elev return false.
            var klasse = obj as Klasse;
            if (klasse == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == klasse.Id;
        }

        public bool Equals(Klasse klasse)
        {
            // If parameter is null return false:
            if (klasse == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == klasse.Id;
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

    }
}
