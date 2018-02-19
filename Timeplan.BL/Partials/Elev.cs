using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{

    public partial class Elev
    {
        private int ElevTilstedeDummyId = 0;

        public bool Update(
            string navn,
            decimal sfoProsent,
            int klasseId,
            int sfoId,
            int trinnId,
            int hovedPedagogId,
            int bemanningsNormSkoleId,
            int bemanningsNormSfoId,
            string tlfNr,
            ICollection<ElevTilstede> elevTilstedes
            )
        //ICollection<ElevTilstede> elevTilstedes
        //IList<int> elevTilstedeIds
        {
            if ((TlfNr ?? string.Empty) != (tlfNr ?? string.Empty))
            {
                TlfNr = tlfNr;
                IsChanged = true;
            }

            foreach (var elevTilstede in elevTilstedes)
            {
                var existingElevTilstede = ElevTilstedes.First(e => e.Id == elevTilstede.Id);

                existingElevTilstede.Update(
                        elevTilstede.MandagStart,
                        elevTilstede.MandagSlutt,
                        elevTilstede.TirsdagStart,
                        elevTilstede.TirsdagSlutt,
                        elevTilstede.OnsdagStart,
                        elevTilstede.OnsdagSlutt,
                        elevTilstede.TorsdagStart,
                        elevTilstede.TorsdagSlutt,
                        elevTilstede.FredagStart,
                        elevTilstede.FredagSlutt,
                        elevTilstede.Elev.Id,
                        elevTilstede.UkeType.Id
                        );
            }

            if (ElevTilstedes.Any(elevTilstede => elevTilstede.IsChanged))
            {
                IsChanged = true;
            }

            Update(navn, sfoProsent, klasseId, sfoId, trinnId, hovedPedagogId, bemanningsNormSkoleId, bemanningsNormSfoId);

            //var elevTilstedeIdsOld = ElevTilstedes.OrderBy(elevTilstede => elevTilstede.Id).Select(elevTilstede => elevTilstede.Id).ToList();

            //if (elevTilstedeIdsOld.Count != elevTilstedeIds.Count
            //    || elevTilstedeIdsOld.SequenceEqual(elevTilstedeIds.OrderBy(i => i)) == false)
            //{
            //    ElevTilstedes.Clear();
            //    foreach (var elevTilstede in elevTilstedeIds)
            //    {
            //        ElevTilstedes.Add(ElevTilstede.GetById(elevTilstede));
            //    }
            //    IsChanged = true;
            //}

            return IsChanged;
        }


        public bool Update(
            string navn,
            decimal sfoProsent,
            int klasseId,
            int sfoId,
            int trinnId,
            int hovedPedagogId,
            int bemanningsNormSkoleId,
            int bemanningsNormSfoId)
        {
            if (Navn != navn)
            {
                Navn = navn;
                IsChanged = true;
            }

            if (SfoProsent != sfoProsent)
            {
                SfoProsent = sfoProsent;
                IsChanged = true;
            }

            if (Klasse == null || Klasse.Id != klasseId)
            {
                Klasse = Klasse.GetById(klasseId);
                IsChanged = true;
            }

            if ((Sfo == null && sfoId > 0) ||
               (Sfo != null && Sfo.Id != sfoId))
            {
                Sfo = Sfo.GetById(sfoId);
                IsChanged = true;

                SetDefaultElevTilstede();
            }

            if (Trinn == null || Trinn.Id != trinnId)
            {
                Trinn = Trinn.GetById(trinnId);

                IsChanged = true;

                SetDefaultElevTilstede();
            }

            if (HovedPedagog == null || HovedPedagog.Id != hovedPedagogId)
            {
                HovedPedagog = Ansatt.GetById(hovedPedagogId);
                IsChanged = true;
            }

            if (BemanningsNormSkole == null || BemanningsNormSkole.Id != bemanningsNormSkoleId)
            {
                BemanningsNormSkole = BemanningsNorm.GetById(bemanningsNormSkoleId);
                IsChanged = true;
            }

            if ((BemanningsNormSfo == null && bemanningsNormSfoId > 0) ||
                (BemanningsNormSfo != null && BemanningsNormSfo.Id != bemanningsNormSfoId))
            {
                BemanningsNormSfo = BemanningsNorm.GetById(bemanningsNormSfoId);
                IsChanged = true;
            }

            return IsChanged;
        }

        public Elev Save()
        {
            var db = new TimeplanEntities();

            var elev = Id > 0 ? GetById(Id, db) : new Elev();

            if (elev != null) // or else it is deleted
            {
                elev.Copy(this, db);

                if (Id <= 0)
                {
                    db.Elevs.Add(elev);
                }

                db.SaveChanges();

                Id = elev.Id;

                SaveElevTilstedes();
            }

            IsChanged = false;

            return elev;
        }

        private void SaveElevTilstedes()
        {
            if (ElevTilstedes.Count == 0)
            {
                AddElevTilstede();
            }

            if (ElevTilstedes.Any(elevTilstede => elevTilstede.IsChanged))
            {
                foreach (var elevTilstede in ElevTilstedes)
                {
                    if (elevTilstede.IsChanged)
                    {
                        elevTilstede.Save();
                    }
                }
            }

            var elevTilstedesOld = ElevTilstede.GetAll().Where(elevTilstede => elevTilstede.Elev.Id == Id);

            foreach (var elevTilstede in elevTilstedesOld)
            {
                if (ElevTilstedes.Any(t => t.Id == elevTilstede.Id) == false)
                {
                    elevTilstede.Delete();
                }
            }
        }

        private ElevTilstede SetDefaultElevTilstede()
        {
            ElevTilstede defaultElevTilstede = null;

            var foundLikUke = false;

            if (Sfo == null)
            {
                foreach (ElevTilstede elevTilstede in ElevTilstedes.ToList())
                {
                    if (foundLikUke == false && elevTilstede.UkeType.Id == (int)UkeTypeEnum.LikUke)
                    {
                        foundLikUke = true;

                        if (Trinn.ElevTilstedeIsDefault(elevTilstede) == false)
                        {
                            defaultElevTilstede = Trinn.GetDefaultElevTilstede(elevTilstede);
                            defaultElevTilstede.IsChanged = true;
                        }
                        else
                        {
                            defaultElevTilstede = elevTilstede;
                        }
                    }
                    else
                    {
                        ElevTilstedes.Remove(elevTilstede);
                    }
                }

                if (defaultElevTilstede == null)
                {
                    defaultElevTilstede = new ElevTilstede
                    {
                        MandagStart = Trinn.MandagStart,
                        MandagSlutt = Trinn.MandagSlutt,
                        TirsdagStart = Trinn.TirsdagStart,
                        TirsdagSlutt = Trinn.TirsdagSlutt,
                        OnsdagStart = Trinn.OnsdagStart,
                        OnsdagSlutt = Trinn.OnsdagSlutt,
                        TorsdagStart = Trinn.TorsdagStart,
                        TorsdagSlutt = Trinn.TorsdagSlutt,
                        FredagStart = Trinn.FredagStart,
                        FredagSlutt = Trinn.FredagSlutt,
                        Elev = this,
                        UkeType = UkeType.GetById((int)UkeTypeEnum.LikUke),
                        IsChanged = true
                    };

                    ElevTilstedes.Add(defaultElevTilstede);
                }
            }

            return defaultElevTilstede;
        }

        public void Delete()
        {
            if (Id > 0)
            {
                foreach (ElevTilstede elevTilstede in ElevTilstedes)
                {
                    elevTilstede.Delete();
                }

                var db = new TimeplanEntities();

                var elev = GetById(Id, db);

                if (elev != null)
                {
                    db.Elevs.Remove(elev);

                    db.SaveChanges();
                }
            }
        }

        private void Copy(Elev copyFrom, TimeplanEntities db)
        {
            Navn = copyFrom.Navn;
            SfoProsent = copyFrom.SfoProsent;
            TlfNr = copyFrom.TlfNr;
            fk_KlasseId = copyFrom.Klasse.Id;
            fk_SfoId = copyFrom.Sfo != null ? copyFrom.Sfo.Id : (int?)null;
            fk_TrinnId = copyFrom.Trinn.Id;
            fk_HovedPedagogAnsattId = copyFrom.HovedPedagog.Id;
            fk_SkoleBemanningsNormId = copyFrom.BemanningsNormSkole.Id;
            fk_SfoBemanningsNormId = copyFrom.BemanningsNormSfo != null ? copyFrom.BemanningsNormSfo.Id : (int?)null;

            //ElevTilstedes.Clear();
            //foreach (var elevTilstede in copyFrom.ElevTilstedes)
            //{
            //    ElevTilstedes.Add(ElevTilstede.GetById(elevTilstede.Id, db));
            //}
        }


        public void AddElevTilstede()
        {
            ElevTilstede newElevTilstede;

            var existingElevTilstede = ElevTilstedes.Count > 0 ? ElevTilstedes.First() : null;

            if (existingElevTilstede != null)
            {
                newElevTilstede = new ElevTilstede
                {
                    Id = ElevTilstedeDummyId--,
                    IsChanged = true,
                    Elev = this,
                    MandagStart = existingElevTilstede.MandagStart,
                    MandagSlutt = existingElevTilstede.MandagSlutt,
                    TirsdagStart = existingElevTilstede.TirsdagStart,
                    TirsdagSlutt = existingElevTilstede.TirsdagSlutt,
                    OnsdagStart = existingElevTilstede.OnsdagStart,
                    OnsdagSlutt = existingElevTilstede.OnsdagSlutt,
                    TorsdagStart = existingElevTilstede.TorsdagStart,
                    TorsdagSlutt = existingElevTilstede.TorsdagSlutt,
                    FredagStart = existingElevTilstede.FredagStart,
                    FredagSlutt = existingElevTilstede.FredagSlutt,
                    UkeType = UkeType.GetById((int)UkeTypeEnum.UlikUke)
                };
            }
            else
            {
                newElevTilstede = new ElevTilstede
                {
                    Id = ElevTilstedeDummyId--,
                    IsChanged = true,
                    Elev = this,
                    MandagStart = Trinn.MandagStart,
                    MandagSlutt = Trinn.MandagSlutt,
                    TirsdagStart = Trinn.TirsdagStart,
                    TirsdagSlutt = Trinn.TirsdagSlutt,
                    OnsdagStart = Trinn.OnsdagStart,
                    OnsdagSlutt = Trinn.OnsdagSlutt,
                    TorsdagStart = Trinn.TorsdagStart,
                    TorsdagSlutt = Trinn.TorsdagSlutt,
                    FredagStart = Trinn.FredagStart,
                    FredagSlutt = Trinn.FredagSlutt,
                    UkeType = UkeType.GetById((int)UkeTypeEnum.LikUke)
                };
            }

            ElevTilstedes.Add(newElevTilstede);

            IsChanged = true;
        }

        public void DeleteElevTilstede(int id)
        {
            if (ElevTilstedes.Count > 0)
            {
                var existingElevTilstede = ElevTilstedes.Where(elevTilstede => elevTilstede.Id == id).First();
                ElevTilstedes.Remove(existingElevTilstede);

                IsChanged = true;
            }
        }

        public static IEnumerable<Elev> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Elevs;
        }

        public static Elev GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Elev GetById(int id, TimeplanEntities db)
        {
            return db.Elevs.FirstOrDefault(elev => elev.Id == id);
        }

        public bool IsChanged { get; set; }

        public Ansatt HovedPedagog
        {
            get { return Ansatt; }
            set { Ansatt = value; }
        }

        public BemanningsNorm BemanningsNormSkole
        {
            get { return BemanningsNorm1; }
            set { BemanningsNorm1 = value; }
        }

        public BemanningsNorm BemanningsNormSfo
        {
            get { return BemanningsNorm; }
            set { BemanningsNorm = value; }
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Elev return false.
            var elev = obj as Elev;
            if (elev == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == elev.Id;
        }

        public bool Equals(Elev elev)
        {
            // If parameter is null return false:
            if (elev == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == elev.Id;
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