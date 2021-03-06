﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Timeplan.BL
{

    public partial class Skole
    {
        //public bool Update(
        //    string navn,
        //    decimal sfoProsent,
        //    int klasseId,
        //    int sfoId,
        //    int trinnId,
        //    int hovedPedagogId,
        //    int bemanningsNormSkoleId,
        //    int bemanningsNormSfoId,
        //    string tlfNr)
        ////IList<int> elevTilstedeIds
        //{

        //    Update(navn, sfoProsent, klasseId, sfoId, trinnId, hovedPedagogId, bemanningsNormSkoleId, bemanningsNormSfoId);

        //    if (TlfNr != tlfNr)
        //    {
        //        TlfNr = tlfNr;
        //        IsChanged = true;
        //    }

        //    //var elevTilstedeIdsOld = ElevTilstedes.OrderBy(elevTilstede => elevTilstede.Id).Select(elevTilstede => elevTilstede.Id).ToList();

        //    //if (elevTilstedeIdsOld.Count != elevTilstedeIds.Count
        //    //    || elevTilstedeIdsOld.SequenceEqual(elevTilstedeIds.OrderBy(i => i)) == false)
        //    //{
        //    //    ElevTilstedes.Clear();
        //    //    foreach (var elevTilstede in elevTilstedeIds)
        //    //    {
        //    //        ElevTilstedes.Add(ElevTilstede.GetById(elevTilstede));
        //    //    }
        //    //    IsChanged = true;
        //    //}

        //    return IsChanged;
        //}


        //public bool Update(
        //    string navn,
        //    decimal sfoProsent,
        //    int klasseId,
        //    int sfoId,
        //    int trinnId,
        //    int hovedPedagogId,
        //    int bemanningsNormSkoleId,
        //    int bemanningsNormSfoId)
        //{
        //    if (Navn != navn)
        //    {
        //        Navn = navn;
        //        IsChanged = true;
        //    }

        //    if (SfoProsent != sfoProsent)
        //    {
        //        SfoProsent = sfoProsent;
        //        IsChanged = true;
        //    }

        //    if (Klasse == null || Klasse.Id != klasseId)
        //    {
        //        Klasse = Klasse.GetById(klasseId);
        //        IsChanged = true;
        //    }

        //    if ((Sfo == null && sfoId > 0) ||
        //       (Sfo != null && Sfo.Id != sfoId))
        //    {
        //        Sfo = Sfo.GetById(sfoId);
        //        IsChanged = true;
        //    }

        //    if (Trinn == null || Trinn.Id != trinnId)
        //    {
        //        Trinn = Trinn.GetById(trinnId);
        //        IsChanged = true;
        //    }

        //    if (HovedPedagog == null || HovedPedagog.Id != hovedPedagogId)
        //    {
        //        HovedPedagog = Ansatt.GetById(hovedPedagogId);
        //        IsChanged = true;
        //    }

        //    if (BemanningsNormSkole == null || BemanningsNormSkole.Id != bemanningsNormSkoleId)
        //    {
        //        BemanningsNormSkole = BemanningsNorm.GetById(bemanningsNormSkoleId);
        //        IsChanged = true;
        //    }

        //    if ((BemanningsNormSfo == null && bemanningsNormSfoId > 0) ||
        //        (BemanningsNormSfo != null && BemanningsNormSfo.Id != bemanningsNormSfoId))
        //    {
        //        BemanningsNormSfo = BemanningsNorm.GetById(bemanningsNormSfoId);
        //        IsChanged = true;
        //    }

        //    return IsChanged;
        //}

        //public Elev Save()
        //{
        //    var db = new TimeplanEntities();

        //    var elev = Id > 0 ? GetById(Id, db) : new Elev();
        //    elev.Copy(this, db);

        //    if (Id <= 0)
        //    {
        //        db.Elevs.Add(elev);
        //    }

        //    db.SaveChanges();

        //    Id = elev.Id;

        //    IsChanged = false;

        //    return elev;
        //}

        //public void Delete()
        //{
        //    // TODO: feilsjekk og feilmeldinger!

        //    var db = new TimeplanEntities();

        //    var elev = Id > 0 ? GetById(Id, db) : this;

        //    if (Id > 0)
        //    {
        //        db.Elevs.Remove(elev);
        //    }

        //    db.SaveChanges();
        //}

        //private void Copy(Elev copyFrom, TimeplanEntities db)
        //{
        //    Navn = copyFrom.Navn;
        //    SfoProsent = copyFrom.SfoProsent;
        //    TlfNr = copyFrom.TlfNr;
        //    fk_KlasseId = copyFrom.Klasse.Id;
        //    fk_SfoId = copyFrom.Sfo != null ? copyFrom.Sfo.Id : (int?)null;
        //    fk_TrinnId = copyFrom.Trinn.Id;
        //    fk_HovedPedagogAnsattId = copyFrom.HovedPedagog.Id;
        //    fk_SkoleBemanningsNormId = copyFrom.BemanningsNormSkole.Id;
        //    fk_SfoBemanningsNormId = copyFrom.BemanningsNormSfo != null ? copyFrom.BemanningsNormSfo.Id : (int?)null;

        //    //ElevTilstedes.Clear();
        //    //foreach (var elevTilstede in copyFrom.ElevTilstedes)
        //    //{
        //    //    ElevTilstedes.Add(ElevTilstede.GetById(elevTilstede.Id, db));
        //    //}
        //}

        public static IEnumerable<Skole> GetAll()
        {
            var db = new TimeplanEntities();
            return db.Skoles;
        }

        public static Skole GetById(int id)
        {
            var db = new TimeplanEntities();
            return GetById(id, db);
        }

        public static Skole GetById(int id, TimeplanEntities db)
        {
            return db.Skoles.FirstOrDefault(skole => skole.Id == id);
        }

        public TidsInndeling ÅpningsTider()
        {
            return TidsInndeling;
        }

        //public bool IsChanged { get; set; }

        //public Ansatt HovedPedagog
        //{
        //    get { return Ansatt; }
        //    set { Ansatt = value; }
        //}

        //public BemanningsNorm BemanningsNormSkole
        //{
        //    get { return BemanningsNorm1; }
        //    set { BemanningsNorm1 = value; }
        //}

        //public BemanningsNorm BemanningsNormSfo
        //{
        //    get { return BemanningsNorm; }
        //    set { BemanningsNorm = value; }
        //}

    }
}