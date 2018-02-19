using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class StillingsType
    {
        public void Update(string navn, decimal timerElevarbeid, decimal timerSamarbeid, IList<int> ansatteIds)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (TimerElevarbeid != timerElevarbeid)
            {
                TimerElevarbeid = timerElevarbeid;
                IsChanged = true;
            }

            if (TimerSamarbeid != timerSamarbeid)
            {
                TimerSamarbeid = timerSamarbeid;
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

        public StillingsType Save()
        {
            var db = new TimeplanEntities();

            var stillingsType = Id > 0 ? GetById(Id, db) : new StillingsType();

            if (stillingsType != null)
            {
                stillingsType.Copy(this, db);

                if (Id <= 0)
                {
                    db.StillingsTypes.Add(stillingsType);
                }

                db.SaveChanges();

                Id = stillingsType.Id;
            }

            IsChanged = false;

            return stillingsType;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var stillingsType = GetById(Id, db);

                if (stillingsType != null)
                {
                    db.StillingsTypes.Remove(stillingsType);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(StillingsType copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            TimerElevarbeid = copyFrom.TimerElevarbeid;
            TimerSamarbeid = copyFrom.TimerSamarbeid;

            Ansatts.Clear();
            foreach (var ansatt in copyFrom.Ansatts)
            {
                Ansatts.Add(Ansatt.GetById(ansatt.Id, db));
            }
        }

        public static IEnumerable<StillingsType> GetAll()
        {
            var db = new TimeplanEntities();
            return db.StillingsTypes;
        }

        public static StillingsType GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static StillingsType GetById(int id, TimeplanEntities db)
        {
            return db.StillingsTypes.FirstOrDefault(stillingsType => stillingsType.Id == id);
        }


        public bool IsChanged { get; set; }

    }
}
