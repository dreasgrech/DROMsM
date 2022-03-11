using System;

namespace Frontend
{
    public enum ROMStandardCodes
    {
        None,
        VerifiedGoodDump,
        AlternateVersion,
        BadDump,
        CorrectedDump,
        HackedROM,
        OverDump,
        PiratedVersion,
        VersionWithTrainer,
        DelayedDump,
    }

    public enum ROMCountry
    {
        None,
        Australia,
        Asia,
        Brazil,
        Canada,
        China,
        Dutch,
        Europe,
        France,
        FrenchCanadian,
        Germany,
        Greece,
        HongKong,
        Italy,
        Japan,
        JapanKorea,
        JapanUSA,
        Korea,
        Holland,
        Norway,
        Russia,
        Spain,
        Sweden,
        USA,
        USABrazilNTSC,
        USAEurope,
        UnitedKingdom,
        World,
        Unknown
    }

    public enum ROMLicense
    {
        None,
        Unlicensed,
        PublicDomain
    }

    public enum ROMUniversalCode
    {
        NewerTranslation,
        OlderTranslation,
        NESMapper,
        NESSachen,
        MultiplayerMenu,
        NESVSVersion,
        NESAladdinCartridge,
        Multilanguage,
        Prototype,
        Beta,
        Sample,
        RevisionSpecified,
        TrackSpecified,
        HackedMapper,
        TimeHack,
        NeoDemiforceHack,
    }
}