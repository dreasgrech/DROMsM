using CsvHelper;
using CsvHelper.Configuration;
using Frontend;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace DRomsMUtils
{
    public static class DATFileCSVWriter
    {
        // public static bool WriteToFile(string filePath, ArrayList datFileMachinesArrayList, DATFileMachineCSVClassMap classMap)
        public static bool WriteToFile(string filePath, IList datFileMachinesList, DATFileMachineCSVClassMap classMap)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    //Mode = CsvMode.Escape
                    TrimOptions = TrimOptions.None,
                    HasHeaderRecord = true,
                    Encoding = Encoding.UTF8
                };

                using (var writer = new StreamWriter(filePath))
                // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.Context.RegisterClassMap(classMap);

                    csv.WriteHeader<DATFileMachine>();
                    csv.NextRecord();

                    foreach (var datFileMachineElement in datFileMachinesList)
                    {
                        var datFileMachine = (DATFileMachine) datFileMachineElement;
                        csv.WriteRecord(datFileMachine);
                        csv.NextRecord();
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
