using System.Collections.Generic;

namespace Frontend
{
    public static class ROMScoreCalculator
    {
        private static readonly Dictionary<ROMStandardCodes, float> standardCodesScores =
            new Dictionary<ROMStandardCodes, float>( /* todo: icomparer */)
            {
                {ROMStandardCodes.VerifiedGoodDump, 1000f},
                {ROMStandardCodes.CorrectedDump, 900f},
                {ROMStandardCodes.DelayedDump, 750f},
                // {ROMStandardCodes.None, 500f},
                {ROMStandardCodes.None, 25f},
                {ROMStandardCodes.AlternateVersion, -800f}, // todo: i don't think this should be this low
                {ROMStandardCodes.OverDump, -600f},
                {ROMStandardCodes.PiratedVersion, -500f},
                {ROMStandardCodes.VersionWithTrainer, -400f},
                {ROMStandardCodes.HackedROM, -500f},
                {ROMStandardCodes.BadDump, -1000f},
            };

        private static readonly Dictionary<ROMUniversalCode, float> universalCodesScores =
            new Dictionary<ROMUniversalCode, float>( /* todo: icomparer */)
            {
                {ROMUniversalCode.NewerTranslation, 100f},
                {ROMUniversalCode.RevisionSpecified, 50f},
                {ROMUniversalCode.Multilanguage, 50f},
                // {ROMUniversalCode.RevisionSpecified, -50f},
                {ROMUniversalCode.OlderTranslation, -100f},
                {ROMUniversalCode.Prototype, -100f},
                {ROMUniversalCode.Beta, -100f},
                {ROMUniversalCode.Sample, -100f},
            };

        private static readonly Dictionary<ROMCountry, float> romCountryScores =
            new Dictionary<ROMCountry, float>( /* todo: icomparer */)
            {
                {ROMCountry.USA, 1000f},
                {ROMCountry.Europe, 990f},
                {ROMCountry.USAEurope, 985f},
                {ROMCountry.World, 980f},
                {ROMCountry.UnitedKingdom, 970f},
                {ROMCountry.None, 960f},
                {ROMCountry.JapanUSA, 955f},
                {ROMCountry.Australia, 950f},
                {ROMCountry.Canada, 940f},
                {ROMCountry.Holland, 930f},
                {ROMCountry.Spain, 920f},
                {ROMCountry.Sweden, 910f},
                {ROMCountry.Asia, 900f},
                {ROMCountry.Brazil, 890f},
                {ROMCountry.China, 880f},
                {ROMCountry.Greece, 870f},
                {ROMCountry.Italy, 860f},
                {ROMCountry.Russia, 850f},
                {ROMCountry.Korea, 840f},
                {ROMCountry.Japan, 830f},
                {ROMCountry.Norway, 820f},
                {ROMCountry.Dutch, 810f},
                {ROMCountry.France, 800f},
            };

        public static float GenerateScore(ROMEntry rom)
        {
            var score = 0f;

            /// Dummy Rom
            if (rom.IsDummyROM)
            {
                score = -2000f;

                return score;
            }

            /// Standard Codes

            if (!rom.HasStandardCodes())
            {
                score += standardCodesScores[ROMStandardCodes.None];
            }
            else
            {
                if (rom.HasCode(ROMStandardCodes.VerifiedGoodDump))
                {
                    score += standardCodesScores[ROMStandardCodes.VerifiedGoodDump];
                }

                // TODO: need to take into consideration the score when no attributes are set

                if (rom.HasCode(ROMStandardCodes.CorrectedDump))
                {
                    score += standardCodesScores[ROMStandardCodes.CorrectedDump];
                }

                if (rom.HasCode(ROMStandardCodes.AlternateVersion))
                {
                    score += standardCodesScores[ROMStandardCodes.AlternateVersion];
                }

                if (rom.HasCode(ROMStandardCodes.DelayedDump))
                {
                    score += standardCodesScores[ROMStandardCodes.DelayedDump];
                }

                if (rom.HasCode(ROMStandardCodes.HackedROM))
                {
                    score += standardCodesScores[ROMStandardCodes.HackedROM];
                }

                if (rom.HasCode(ROMStandardCodes.OverDump))
                {
                    score += standardCodesScores[ROMStandardCodes.OverDump];
                }

                if (rom.HasCode(ROMStandardCodes.PiratedVersion))
                {
                    score += standardCodesScores[ROMStandardCodes.PiratedVersion];
                }

                if (rom.HasCode(ROMStandardCodes.VersionWithTrainer))
                {
                    score += standardCodesScores[ROMStandardCodes.VersionWithTrainer];
                }

                if (rom.HasCode(ROMStandardCodes.BadDump))
                {
                    score += standardCodesScores[ROMStandardCodes.BadDump];
                }
            }

            /// Universal Codes
            var romUniversalCodes = rom.UniversalCodes;
            foreach (var romUniversalCode in romUniversalCodes)
            {
                if (universalCodesScores.TryGetValue(romUniversalCode, out var universalCodeScore))
                {
                    score += universalCodeScore;
                }
            }

            /// Rom Country
            if (romCountryScores.TryGetValue(rom.Country, out var romCountryScore))
            {
                score += romCountryScore;
            }
            else
            {
                // TODO: Log what country we didn't find here
                // TODO: Log what country we didn't find here
                // TODO: Log what country we didn't find here
            }

            return score;
        }
    }
}