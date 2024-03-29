﻿using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Forms;
using CsvHelper.Configuration;

namespace DRomsMUtils
{
    /// <summary>
    /// This class is used for the CSVHelper to ignore certain properties
    /// https://joshclose.github.io/CsvHelper/examples/configuration/class-maps/ignoring-properties/
    /// </summary>
    public sealed class DATFileMachineCSVClassMap : ClassMap<DATFileMachine>
    {
        public DATFileMachineCSVClassMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.MAMESortingIndex).Ignore();
            Map(m => m.IsDevice).Ignore();
            Map(m => m.XMLValue).Ignore();
        }

        public void ToggleColumn<TMember>(Expression<Func<DATFileMachine, TMember>> expression, bool include, int columnIndex)
        {
            var mapped = Map(expression);
            mapped = mapped.Index(columnIndex);
            if (!include)
            {
                mapped.Ignore();
            }
        }
    }
}