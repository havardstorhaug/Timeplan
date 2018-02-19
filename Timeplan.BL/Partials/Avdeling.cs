using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class Avdeling
    {
        public void Update(string navn, int avdelingsLederId, IList<int> ansatteIds, IList<int> klasserIds)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (AvdelingsLeder == null || AvdelingsLeder.Id != avdelingsLederId)
            {
                AvdelingsLeder = Ansatt.GetById(avdelingsLederId);
                IsChanged = true;
            }

            var klasserIdsOld = Klasses.OrderBy(klasse => klasse.Id).Select(klasse => klasse.Id).ToList();

            if (klasserIdsOld.Count != klasserIds.Count
                || klasserIdsOld.SequenceEqual(klasserIds.OrderBy(i => i)) == false)
            {
                Klasses.Clear();
                foreach (var klasseId in klasserIds)
                {
                    Klasses.Add(Klasse.GetById(klasseId));
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

        public Avdeling Save()
        {
            var db = new TimeplanEntities();

            var avdeling = Id > 0 ? GetById(Id, db) : new Avdeling();

            if (avdeling != null)
            {
                avdeling.Copy(this, db);

                if (Id <= 0)
                {
                    db.Avdelings.Add(avdeling);
                }

                db.SaveChanges();

                Id = avdeling.Id;
            }

            IsChanged = false;

            return avdeling;
        }

        public void Delete()
        {
            // TODO: probably the simplest/easiest way, but exception or error code enum might be better...
            // could also split method and use bool CanDelete()...
            //var message = string.Empty;

            //var db = new TimeplanEntities();

            //var avdeling = Id > 0 ? GetById(Id, db) : this;

            //if (Id > 0 &&
            //    (avdeling.Ansatts.Count > 0 ||
            //    avdeling.Klasses.Count > 0))
            //{
            //    message = @"Ansatte og klasser må overføres til andre avdelinger før avdeling '" + avdeling.Navn + "' kan slettes.";
            //}
            //else
            //{
            //    if (Id > 0)
            //    {
            //        db.Avdelings.Remove(avdeling);
            //    }

            //    db.SaveChanges();
            //}

            //return message;

            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var avdeling = GetById(Id, db);

                if (avdeling != null)
                {
                    db.Avdelings.Remove(avdeling);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(Avdeling copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            fk_AvdelingslederAnsattId = copyFrom.AvdelingsLeder.Id;

            Klasses.Clear();
            foreach (var klasse in copyFrom.Klasses)
            {
                Klasses.Add(Klasse.GetById(klasse.Id, db));
            }

            Ansatts.Clear();
            foreach (var ansatt in copyFrom.Ansatts)
            {
                Ansatts.Add(Ansatt.GetById(ansatt.Id, db));
            }
        }

        public static IEnumerable<Avdeling> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Avdelings;
        }

        public static Avdeling GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Avdeling GetById(int id, TimeplanEntities db)
        {
            return db.Avdelings.FirstOrDefault(avdeling => avdeling.Id == id);
        }

        public Ansatt AvdelingsLeder
        {
            get { return Ansatt; }
            set { Ansatt = value; }
        }

        public bool IsChanged { get; set; }

    }
}
