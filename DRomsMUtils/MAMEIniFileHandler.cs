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
        public MAMEIniFileSectionLine SectionLine { get; set; }

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

    public class MAMEIniFileSectionLine : MAMEIniFileLine
    {
        public string SectionName { get; }

        public MAMEIniFileSectionLine(int lineNumber, string fullLine, string sectionName) : base(lineNumber, fullLine)
        {
            SectionName = sectionName;
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
                    var lastLine = AllLines[AllLines.Count - 1];
                    var newLineNumber = lastLine.LineNumber + 1;
                    var newLine = new MAMEIniFileDataLine(newLineNumber, otherFileValueLineKey, otherFileValueLine.Value, otherFileValueLine.Spacer, otherFileValueLine.FullLine)
                    {
                        // SectionLine = otherFileValueLine.SectionLine
                        SectionLine = lastLine.SectionLine // Use the section from the previous line
                    };

                    AllLines.Add(newLine);
                    ValueLines.Add(otherFileValueLineKey, newLine);
                }
                else
                {
                    var valueLine = ValueLines[otherFileValueLineKey];
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
                sb.AppendLine(line.FullLine);
            }

            return sb.ToString();
        }
    }

    public class MAMEIniFileHandler
    {
        private const char CommentCharacterHash = '#';
        private const char CommentCharacterSemicolon = ';';
        private const char EqualityCharacter = '=';
        private const char SectionStartCharacter = '[';
        private const char SectionEndCharacter = ']';

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
            // var fieldsDictionary_threaded = new ConcurrentDictionary<string, string>();
            // var totalFileCharacters = 0;
            var allLines_threaded = new ConcurrentQueue<MAMEIniFileLine>();

            Parallel.ForEach(fileLinesEnumerable, (line, parallelLoopState, ln) =>
            {
                var lineNumber = (int) ln;

                // Check if this is an empty line
                if (string.IsNullOrEmpty(line))
                {
                    allLines_threaded.Enqueue(new MAMEIniFileEmptyLine(lineNumber, line));

                    // Interlocked.Increment(ref totalFileCharacters);
                    return;
                }

                var trimmedLine = line.Trim();
                var firstLetter = trimmedLine[0];

                var isComment =
                    firstLetter == CommentCharacterHash ||
                    firstLetter == CommentCharacterSemicolon;

                // Check if this line is a comment
                if (isComment)
                {
                    allLines_threaded.Enqueue(new MAMEIniFileCommentLine(lineNumber, line));

                    // Interlocked.Add(ref totalFileCharacters, line.Length);
                    return;
                }

                // Check if this line is a [Section]
                var isSection = firstLetter == SectionStartCharacter;
                if (isSection)
                {
                    var sectionName = trimmedLine.Substring(1, trimmedLine.IndexOf(SectionEndCharacter) - 1);
                    var sectionLine = new MAMEIniFileSectionLine(lineNumber, line, sectionName);
                    allLines_threaded.Enqueue(sectionLine);

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
                    var isCharWhiteSpace =
                        char.IsWhiteSpace(lineChar) ||
                        lineChar == EqualityCharacter;

                    // If we haven't yet fully found the key, continue searching for it
                    if (!lineKeyFound)
                    {
                        // If this character is a whitespace, then it means we've just fully read the key
                        if (isCharWhiteSpace)
                        {
                            lineSpacer += lineChar;
                            lineKeyFound = true;
                            continue;
                        }

                        // Since this character is not whitespace, then this character is part of the key
                        lineKey += lineChar;
                        continue;
                    }

                    // If this character is whitespace, then we're still reading the spacer
                    if (isCharWhiteSpace)
                    {
                        lineSpacer += lineChar;
                        continue;
                    }

                    // If this character is not whitespace, then we've found the first character of the value so read the rest of the line as the value
                    lineValue = trimmedLine.Substring(i);
                    break;
                }

                var dataLine = new MAMEIniFileDataLine(lineNumber, lineKey, lineValue, lineSpacer, line);
                allLines_threaded.Enqueue(dataLine);
                // Interlocked.Add(ref totalFileCharacters, line.Length);

                // Save the field data
                // fieldsDictionary_threaded[lineKey] = lineValue;
            });

            var allLines = new List<MAMEIniFileLine>(allLines_threaded.Count);
            var valueLines = new Dictionary<string, MAMEIniFileDataLine>(allLines_threaded.Count, EqualityComparer<string>.Default);

            // Go through the unsorted collection of lines and start adding them to our list which will be sorted after
            while (allLines_threaded.TryDequeue(out MAMEIniFileLine line))
            {
                allLines.Add(line);

                if (line is MAMEIniFileDataLine valueLine)
                {
                    var key = valueLine.Key;

                    // Make sure we don't have duplicate keys
                    if (valueLines.TryGetValue(key, out var existingValueLine))
                    {
                        var errorMessage = $"Duplicate keys are not supported.{Environment.NewLine}" +
                                           $"{Environment.NewLine}" +
                                           $"Key '{key}' found on lines {existingValueLine.LineNumber} and {valueLine.LineNumber}";
                        MessageBoxOperations.ShowError(errorMessage, "Keys need to be unique");
                        return null;
                    }

                    valueLines[key] = valueLine;
                }
            }

            // Sort the lines according to their line number
            allLines.Sort(new MAMEIniFileLineComparer_LineNumber());

            // Set the sections to the lines
            MAMEIniFileSectionLine currentSectionLine = null;
            foreach (var line in allLines)
            {
                if (line is MAMEIniFileSectionLine sectionLine)
                {
                    currentSectionLine = sectionLine;
                    continue;
                }

                line.SectionLine = currentSectionLine;
            }

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