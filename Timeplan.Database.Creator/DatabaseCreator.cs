using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeplan.BL;

namespace Timeplan.Database.Creator
{
    public class DatabaseCreator
    {
        private readonly TimeplanEntities _db = new TimeplanEntities();

        public void CreateProdDb()
        {
            if (DatabaseExists() == false)
            {
                CreateDb();
            }
        }

        public void CreateTestDevDb()
        {
            if (DatabaseExists() == false)
            {
                CreateDb();
            }
        }

        public void CreateUnitTestDb()
        {
            if (DatabaseExists()) _db.Database.Delete();
            CreateDb();
        }

        private void CreateDb()
        {
            _db.Database.Create();
            CreateMetaData();
            _db.SaveChanges();
        }

        private void CreateMetaData()
        {
            _db.UkeTypes.Add(new UkeType() { Id = 1, Navn = "Ulik uke" });
            _db.UkeTypes.Add(new UkeType() { Id = 2, Navn = "Lik uke" });

            for (int i = 1; i <= 6; i++)
            {
                _db.BemanningsNorms.Add(new BemanningsNorm()
                {
                    Id = i,
                    Navn = "1:" + i,
                    Antall = i
                });
            }

            for (int i = 1; i <= 10; i++)
            {
                _db.Trinns.Add(new Trinn()
                {
                    Id = i,
                    Navn = i + ".trinn",
                    UkeTimeTall = 20 + i,
                    MandagStart = new TimeSpan(i, i, 00),
                    MandagSlutt = new TimeSpan(i, i, 00),
                    TirsdagStart = new TimeSpan(i, i, 00),
                    TirsdagSlutt = new TimeSpan(i, i, 00),
                    OnsdagStart = new TimeSpan(i, i, 00),
                    OnsdagSlutt = new TimeSpan(i, i, 00),
                    TorsdagStart = new TimeSpan(i, i, 00),
                    TorsdagSlutt = new TimeSpan(i, i, 00),
                    FredagStart = new TimeSpan(i, i, 00),
                    FredagSlutt = new TimeSpan(i, i, 00),
                });
            }

            _db.TidsInndelings.Add(new TidsInndeling() { Id = 1, Navn = "Tidsinndeling1", StartTid = new TimeSpan(7, 0, 0), SluttTid = new TimeSpan(17, 0, 0), TidsIntervall = new TimeSpan(0, 15, 0) });
            _db.TidsInndelings.Add(new TidsInndeling() { Id = 2, Navn = "Tidsinndeling2", StartTid = new TimeSpan(8, 45, 0), SluttTid = new TimeSpan(14, 30, 0), TidsIntervall = new TimeSpan(0, 15, 0) });
            _db.TidsInndelings.Add(new TidsInndeling() { Id = 3, Navn = "Tidsinndeling3", StartTid = new TimeSpan(7, 0, 0), SluttTid = new TimeSpan(8, 45, 0), TidsIntervall = new TimeSpan(0, 15, 0) });
            _db.TidsInndelings.Add(new TidsInndeling() { Id = 4, Navn = "Tidsinndeling4", StartTid = new TimeSpan(16, 15, 0), SluttTid = new TimeSpan(17, 0, 0), TidsIntervall = new TimeSpan(0, 15, 0) });

            for (int i = 1; i <= 8; i++)
            {
                _db.StillingsTypes.Add(new StillingsType()
                {
                    Id = i,
                    Navn = "Stillingstype" + i,
                    TimerElevarbeid = 10 + i,
                    TimerSamarbeid = i
                });
            }

            _db.Skoles.Add(new Skole()
            {
                Id = 1,
                Navn = "Skole",
                fk_TidsInndelingId = 2
            });


            for (int i = 1; i <= 3; i++)
            {
                _db.Sfoes.Add(new Sfo()
                {
                    Id = i,
                    Navn = "Sfo" + i,
                    fk_TidsInndelingId = 1,
                    fk_TidligvaktTidsInndelingId = 3,
                    fk_SeinvaktTidsInndelingId = 4
                });
            }

            for (int i = 1; i <= 4; i++)
            {
                _db.Avdelings.Add(new Avdeling()
                {
                    Id = i,
                    Navn = "Avdeling" + i
                });
            }

            for (int i = 1; i <= 110; i++)
            {
                _db.Ansatts.Add(new Ansatt()
                {
                    Id = i,
                    Navn = "Ansatt" + i,
                    Stillingsstørrelse = Math.Abs(100 - i),
                    fk_AvdelingId = (i % 4) + 1,
                    fk_StillingsTypeId = (i % 8) + 1,
                    Tlfnr = string.Empty
                });
            }

            _db.SaveChanges();

            for (int i = 1; i <= 4; i++)
            {
                var avdeling = Avdeling.GetById(i);
                avdeling.AvdelingsLeder = Ansatt.GetById(i);
                avdeling.Save();
            }

            for (int i = 1; i <= 15; i++)
            {
                _db.Klasses.Add(new Klasse()
                {
                    Id = i,
                    Navn = "Klasse" + i,
                    MaksAntallElever = i,
                    fk_AvdelingId = (i % 4) + 1
                });
            }

            var hovedpedagoger = Ansatt.GetAll().Where(a => a.StillingsType.Id == (int)StillingsTypeEnum.Pedagog).OrderBy(a => a.Navn).ToList();

            for (int i = 1; i <= 90; i++)
            {
                _db.Elevs.Add(new Elev()
                {
                    Id = i,
                    Navn = "Elev" + i,
                    SfoProsent = Math.Abs(100 - i),
                    fk_KlasseId = (i % 4) + 1,
                    fk_SkoleBemanningsNormId = (i % 6) + 1,
                    fk_TrinnId = (i % 10) + 1,
                    fk_HovedPedagogAnsattId = hovedpedagoger.ElementAt(i % 14).Id,
                    TlfNr = string.Empty
                });
            }

            _db.SaveChanges();

            var elever = Elev.GetAll().ToList();

            foreach(var elev in elever)
            {
                _db.ElevTilstedes.Add(new ElevTilstede()
                {
                    MandagStart = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    MandagSlutt = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    TirsdagStart = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    TirsdagSlutt = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    OnsdagStart = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    OnsdagSlutt = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    TorsdagStart = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    TorsdagSlutt = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    FredagStart = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    FredagSlutt = new TimeSpan(elev.Id % 12, elev.Id % 12, 00),
                    fk_ElevId = elev.Id,
                    fk_UkeTypeId = (int) UkeTypeEnum.LikUke
                });
            }

            var ansatte = Ansatt.GetAll().ToList();

            foreach (var ansatt in ansatte)
            {
                _db.AnsattTilstedes.Add(new AnsattTilstede()
                {
                    MandagStart = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    MandagSlutt = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    MandagFri = false,
                    TirsdagStart = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    TirsdagSlutt = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    TirsdagFri = false,
                    OnsdagStart = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    OnsdagSlutt = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    OnsdagFri = false,
                    TorsdagStart = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    TorsdagSlutt = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    TorsdagFri = false,
                    FredagStart = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    FredagSlutt = new TimeSpan(ansatt.Id % 12, ansatt.Id % 12, 00),
                    FredagFri = false,
                    Skole = true,
                    fk_AnsattId = ansatt.Id,
                    fk_UkeTypeId = (int)UkeTypeEnum.LikUke
                });
            }

            _db.SaveChanges();
        }

        public bool DatabaseExists()
        {
            return _db.Database.Exists();
        }

    }

}
