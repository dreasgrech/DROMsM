using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Frontend;

namespace DRomsMUtils
{
    public static class DATFileCSVWriter
    {
        public static bool WriteToFile(string filePath, DATFile datFile)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    //Mode = CsvMode.Escape
                    TrimOptions = TrimOptions.None,
                    HasHeaderRecord = true
                };

                using (var writer = new StreamWriter(filePath))
                // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteHeader<DATFileMachine>();
                    csv.NextRecord();
                    using (var machinesEnumerator = datFile.GetMachinesEnumerator())
                    {
                        while (machinesEnumerator.MoveNext())
                        {
                            var datFileMachine = machinesEnumerator.Current;
                            csv.WriteRecord(datFileMachine);
                            csv.NextRecord();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }

            return true;
        }
    }
}
