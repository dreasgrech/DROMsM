using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DRomsMUtils;
using Frontend;

namespace DROMsMUtils
{
    public class MAMEIniFileLine
    {
        public int LineNumber { get; }
        public string FullLine { get; protected set; }

        public MAMEIniFileLine(int lineNumber, string fullLine)
        {
            LineNumber = lineNumber;
            FullLine = fullLine;
        }

        public override string ToString()
        {
            return FullLine;
        }
    }

    public class MAMEIniFileDataLine : MAMEIniFileLine
    {
        public string Key { get; }
        public string Value { get; private set; }
        public string Spacer { get; }

        public MAMEIniFileDataLine(int lineNumber, string key, string value, string spacer, string fullLine) : base(lineNumber, fullLine)
        {
            Key = key;
            Value = value;
            Spacer = spacer;
        }

        public void UpdateValue(string newValue)
        {
            Value = newValue;
            RefreshFullLine();
        }

        private void RefreshFullLine()
        {
            FullLine = $"{Key}{Spacer}{Value}";

        }
    }

    public class MAMEIniFileCommentLine : MAMEIniFileLine
    {
        public MAMEIniFileCommentLine(int lineNumber, string fullLine) : base(lineNumber, fullLine)
        {
        }
    }

    public class MAMEIniFileEmptyLine : MAMEIniFileLine
    {
        public MAMEIniFileEmptyLine(int lineNumber, string fullLine) : base(lineNumber, fullLine)
        {
        }
    }

    public class MAMEIniFile
    {
        public List<MAMEIniFileLine> AllLines; // { get; set; }
        public Dictionary<string, MAMEIniFileDataLine> ValueLines; // { get; set; }

        public MAMEIniFile(List<MAMEIniFileLine> allLines, Dictionary<string, MAMEIniFileDataLine> valueLines)
        {
            AllLines = allLines;
            ValueLines = valueLines;
        }

        /// <summary>
        /// Applies the data from the inputted ini file to our data
        /// </summary>
        public void ApplyChanges(MAMEIniFile otherMAMEIniFile)
        {
            var otherFileValueLines = otherMAMEIniFile.ValueLines;
            foreach (var otherFileValueLineElement in otherFileValueLines)
            {
                var otherFileValueLine = otherFileValueLineElement.Value;
                var otherFileValueLineKey = otherFileValueLineElement.Key;

                if (!ValueLines.ContainsKey(otherFileValueLineKey))
                {
                    var lineNumber = AllLines[AllLines.Count - 1].LineNumber + 1;
                    var newLine = new MAMEIniFileDataLine(lineNumber, otherFileValueLineKey, otherFileValueLine.Value, otherFileValueLine.Spacer, otherFileValueLine.FullLine);
                    AllLines.Add(newLine);
                    ValueLines.Add(otherFileValueLineKey, newLine);
                }
                else
                {
                    var valueLine = ValueLines[otherFileValueLineKey];
                    // valueLine.Value = otherFileValueLine.Value;
                    valueLine.UpdateValue(otherFileValueLine.Value);
                    ValueLines[otherFileValueLineKey] = valueLine;
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var line in AllLines)
            {
                /*
                if (line is MAMEIniFileEmptyLine)
                {
                    sb.AppendLine();
                    continue;
                }

                if (line is MAMEIniFileCommentLine commentLine)
                {
                    sb.AppendLine(commentLine.FullLine);
                    continue;
                }

                if (line is MAMEIniFileDataLine dataLine)
                {
                    var key = dataLine.Key;
                    var spacer = dataLine.Spacer;
                    // var value = dataLine.Value;
                    // var value = Values[key]; // Make sure to get the value from the Values collection since that contains the fresh values
                    var value = dataLine.Value;
                    sb.AppendLine($"{key}{spacer}{value}");
                }
                */

                sb.AppendLine(line.FullLine);
            }

            return sb.ToString();
        }
    }

    public class MAMEIniFileHandler
    {
        private const char CommentCharacter = '#';

        public MAMEIniFile ParseMAMEIniText(string text)
        {
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return ParseMAMEIniLines(lines);
        }

        public MAMEIniFile ParseMAMEIniFile(string filePath)
        {
            if (!FileUtilities.FileExists(filePath))
            {
                return null;
            }

            var fileLinesEnumerable = FileUtilities.ReadLines(filePath);
            return ParseMAMEIniLines(fileLinesEnumerable);
        }

        public MAMEIniFile ParseMAMEIniLines(IEnumerable<string> fileLinesEnumerable)
        {
            var fieldsDictionary_threaded = new ConcurrentDictionary<string, string>();
            var allLines_threaded = new ConcurrentQueue<MAMEIniFileLine>();
            // var totalFileCharacters = 0;

            Parallel.ForEach(fileLinesEnumerable, (line, _, ln) =>
            {
                var lineNumber = (int) ln;
                if (string.IsNullOrEmpty(line))
                {
                    allLines_threaded.Enqueue(new MAMEIniFileEmptyLine(lineNumber, line));
                    // Interlocked.Increment(ref totalFileCharacters);
                    return;
                }

                var trimmedLine = line.Trim();
                var firstLetter = trimmedLine[0];

                // Skip comment lines
                var isComment = firstLetter == CommentCharacter;
                if (isComment)
                {
                    allLines_threaded.Enqueue(new MAMEIniFileCommentLine(lineNumber, line));
                    // Interlocked.Add(ref totalFileCharacters, line.Length);
                    return;
                }

                var lineKeyFound = false;
                string lineKey = string.Empty;
                string lineSpacer = string.Empty;
                string lineValue = string.Empty;

                for (int i = 0; i < trimmedLine.Length; i++)
                {
                    var lineChar = trimmedLine[i];
                    var isCharWhiteSpace = char.IsWhiteSpace(lineChar);
                    if (!lineKeyFound)
                    {
                        if (isCharWhiteSpace)
                        {
                            lineSpacer += lineChar;
                            lineKeyFound = true;
                            continue;
                        }

                        lineKey += lineChar;
                        continue;
                    }

                    // If this character is whitespace, then we're still reading the spacer
                    if (isCharWhiteSpace)
                    {
                        lineSpacer += lineChar;
                        continue;
                    }

                    // If this character is not whitespace, then we've found the first character of the key
                    lineValue = trimmedLine.Substring(i);
                    break;
                }

                var dataLine = new MAMEIniFileDataLine(lineNumber, lineKey, lineValue, lineSpacer, line);
                allLines_threaded.Enqueue(dataLine);
                // Interlocked.Add(ref totalFileCharacters, line.Length);

                // Save the field data
                fieldsDictionary_threaded[lineKey] = lineValue;
            });


            // var allLines = allLines_threaded.ToList();

            var allLines = new List<MAMEIniFileLine>(allLines_threaded.Count);
            var valueLines = new Dictionary<string, MAMEIniFileDataLine>(allLines_threaded.Count, EqualityComparer<string>.Default);

            while (allLines_threaded.TryDequeue(out MAMEIniFileLine line))
            {
                allLines.Add(line);

                if (line is MAMEIniFileDataLine valueLine)
                {
                    valueLines[valueLine.Key] = valueLine;
                }
            }

            allLines.Sort(new MAMEIniFileLineComparer_LineNumber());

            var mameIniFile = new MAMEIniFile(allLines, valueLines);

            return mameIniFile;
        }

        /// <summary>
        /// Write the MAME ini file to disk
        /// </summary>
        public bool SaveMAMEIniFileToDisk(MAMEIniFile mameIniFile, string saveToFilePath)
        {
            try
            {
                var fileText = mameIniFile.ToString();
                FileUtilities.WriteAllText(saveToFilePath, fileText);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}