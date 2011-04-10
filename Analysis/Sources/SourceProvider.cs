using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AshMind.Code.Analysis.Internal;
using Microsoft.Samples.Debugging.CorSymbolStore;

namespace AshMind.Code.Analysis.Sources {
    internal class SourceProvider {
        private struct SequencePoint : ISourceRange {
            public SourceDocument Document { get; set; }
            public SourcePosition Start { get; set; }
            public SourcePosition End { get; set; }
        }

        private static readonly ISourceRange[] NoSource = {};

        private const int SequencePointHiddenLine = 0xFeeFee;
        
        private readonly Cache<Assembly, ISymbolReader> readerCache = new Cache<Assembly, ISymbolReader>();
        private readonly Cache<string, SourceDocument> documentCache = new Cache<string, SourceDocument>();

        public ISourceRange[] GetSourceRanges(MemberInfo member) {
            var assembly = member.Module.Assembly;
            var reader = readerCache.Get(assembly, () => TryGetSymbolReader(assembly));
            if (reader == null)
                return NoSource;

            var methodSymbol = reader.GetMethod(new SymbolToken(member.MetadataToken));
            if (methodSymbol == null)
                return NoSource;

            var sequencePoints = this.GetSequencePoints(methodSymbol);

            return sequencePoints.Cast<ISourceRange>().ToArray();
        }

        private ISymbolReader TryGetSymbolReader(Assembly assembly) {
            const int E_PDB_NO_DEBUG_INFO = unchecked((int)0x806D0014);
            try {
                return SymbolAccess.GetReaderForFile(assembly.Location);
            }
            catch (COMException ex) {
                if (ex.ErrorCode == E_PDB_NO_DEBUG_INFO)
                    return null;

                throw;
            }
        }

        private SourceDocument GetOrLoadContent(ISymbolDocument symbols) {
            var location = symbols.URL;
            if (!File.Exists(location))
                return null;

            return documentCache.Get(
                location, () => new SourceDocument(File.ReadAllLines(location), location, symbols)
            );
        }

        //private string[] GetCombinedSource(IEnumerable<SequencePoint> points) {
        //    var lines = new List<string>();
        //    foreach (var point in points) {
        //        var contentLines = documentCache.Get(point.Document, () => File.ReadAllLines(point.Document.URL));
        //        var start = point.Start.Row - 1;
        //        var end = point.End.Row - 1;

        //        if (start >= contentLines.Length || end >= contentLines.Length)
        //            continue;

        //        for (var i = start; i <= end; i++) {
        //            var line = contentLines[i];

        //            if (i == start)
        //                line = line.Substring(point.Start.Column - 1);
        //            else if (i == end)
        //                line = line.Substring(0, point.End.Column - 1);

        //            lines.Add(line);
        //        }
        //    }

        //    return lines.ToArray();
        //}

        private IEnumerable<SequencePoint> GetSequencePoints(ISymbolMethod method) {
            var count = method.SequencePointCount;

            var offsets = new int[count];
            var docs = new ISymbolDocument[count];
            var startColumns = new int[count];
            var endColumns = new int[count];
            var startRows = new int[count];
            var endRows = new int[count];

            method.GetSequencePoints(offsets, docs, startRows, startColumns, endRows, endColumns);

            for (int i = 0; i < count; i++) {
                if (startRows[i] == SequencePointHiddenLine || endRows[i] == SequencePointHiddenLine)
                    continue;

                yield return new SequencePoint {
                    Document = this.GetOrLoadContent(docs[i]),
                    Start = new SourcePosition(startRows[i], startColumns[i]),
                    End = new SourcePosition(endRows[i], endColumns[i])
                };
            }
        }
    }
}
