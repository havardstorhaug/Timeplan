using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeplan.BL
{
    public enum StillingsTypeEnum
    {
        Rektor = 1,
        AvdelingsLeder = 2,
        Pedagog = 3,
        Faglærer = 4,
        Miljøterapeut = 5,
        PedagogiskMedarbeider = 6,
        Vikar = 7,
        Merkantil = 8
    };

    public enum BemanningsNormEnum
    {
        OneToOne = 1,
        OneAndAHalfToOne = 2,
        TwoToOne = 3,
        Point75ToOne = 4,
        HalfToOne = 5,
        Point25ToOne = 6
    };

    public enum UkeTypeEnum
    {
        UlikUke = 1,
        LikUke = 2
    };

    public enum TidsInndelingEnum
    {
        Sfo = 1,
        Skole = 2,
        Tidlig = 3,
        Seint = 4
    };
}
