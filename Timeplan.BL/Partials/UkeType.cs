using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{
    public partial class UkeType
    {
        public void Update(string navn, IList<int> elevTilstedeIds, IList<int> ansattTilstedeIds)
        {
            // TODO:

            //if (Navn != navn)
            //{
            //    Navn = navn;
            //    IsChanged = true;
            //}

            //var elevTilstedeIdsOld = ElevTilstedes.OrderBy(elevTilstede => elevTilstede.Id).Select(elevTilstede => elevTilstede.Id).ToList();

            //if (elevTilstedeIdsOld.Count != elevTilstedeIds.Count
            //    || elevTilstedeIdsOld.SequenceEqual(elevTilstedeIds.OrderBy(i => i)) == false)
            //{
            //    ElevTilstedes.Clear();
            //    foreach (var elevTilstedeId in elevTilstedeIds)
            //    {
            //        ElevTilstedes.Add(ElevTilstede.GetById(elevTilstedeId));
            //    }
            //    IsChanged = true;
            //}

            //var ansattTilstedeIdsOld = AnsattTilstedes.OrderBy(ansattTilstede => ansattTilstede.Id).Select(ansattTilstede => ansattTilstede.Id).ToList();

            //if (ansattTilstedeIdsOld.Count != ansattTilstedeIds.Count
            //    || ansattTilstedeIdsOld.SequenceEqual(ansattTilstedeIds.OrderBy(i => i)) == false)
            //{
            //    AnsattTilstedes.Clear();
            //    foreach (var ansattTilstedeId in ansattTilstedeIds)
            //    {
            //        AnsattTilstedes.Add(AnsattTilstede.GetById(ansattTilstedeId));
            //    }
            //    IsChanged = true;
            //}
        }

        public UkeType Save()
        {
            var db = new TimeplanEntities();

            var ukeType = Id > 0 ? GetById(Id, db) : new UkeType();

            if (ukeType != null) // or else it is deleted
            {

                ukeType.Copy(this, db);

                if (Id <= 0)
                {
                    db.UkeTypes.Add(ukeType);
                }

                db.SaveChanges();

                Id = ukeType.Id;
            }

            IsChanged = false;

            return ukeType;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                var db = new TimeplanEntities();

                var ukeType = GetById(Id, db);

                if (ukeType != null) // or else it is already deleted
                {
                    db.UkeTypes.Remove(ukeType);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(UkeType copyFrom, TimeplanEntities db)
        {
            //Navn = copyFrom.Navn;
            //TimerElevarbeid = copyFrom.TimerElevarbeid;
            //TimerSamarbeid = copyFrom.TimerSamarbeid;

            //ElevTilstedes.Clear();
            //foreach (var elevTilstede in copyFrom.ElevTilstedes)
            //{
            //    ElevTilstedes.Add(ElevTilstede.GetById(elevTilstede.Id, db));
            //}

            //AnsattTilstedes.Clear();
            //foreach (var ansattTilstede in copyFrom.AnsattTilstedes)
            //{
            //    AnsattTilstedes.Add(AnsattTilstede.GetById(ansattTilstede.Id, db));
            //}
        }

        public static IEnumerable<UkeType> GetAll()
        {
            var db = new TimeplanEntities();
            return db.UkeTypes;
        }

        public static UkeType GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static UkeType GetById(int id, TimeplanEntities db)
        {
            return db.UkeTypes.FirstOrDefault(ukeType => ukeType.Id == id);
        }

        public bool IsChanged { get; set; }

    }
}
