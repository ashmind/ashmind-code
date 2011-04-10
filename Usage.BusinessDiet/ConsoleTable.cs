// +-----------------------------------------------------------------------+ 
// | Copyright (c) 2002-2003 Richard Heyes                                 |
// | All rights reserved.                                                  |
// |                                                                       |
// | Redistribution and use in source and binary forms, with or without    | 
// | modification, are permitted provided that the following conditions    |
// | are met:                                                              | 
// |                                                                       |
// | o Redistributions of source code must retain the above copyright      | 
// |   notice, this list of conditions and the following disclaimer.       |
// | o Redistributions in binary form must reproduce the above copyright   | 
// |   notice, this list of conditions and the following disclaimer in the | 
// |   documentation and/or other materials provided with the distribution.| 
// | o The names of the authors may not be used to endorse or promote      |
// |   products derived from this software without specific prior written  | 
// |   permission.                                                         | 
// |                                                                       | 
// | THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS   |
// | "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT     | 
// | LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR | 
// | A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT  |
// | OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, | 
// | SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT      | 
// | LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, | 
// | DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY |
// | THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT   | 
// | (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE | 
// | OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  |
// |                                                                       | 
// +-----------------------------------------------------------------------+
// | Author: Richard Heyes <richard at phpguru dot org>                    |
// +-----------------------------------------------------------------------+

/**
 * Usage
 * =====
 * 
 * ConsoleTable myTable = new ConsoleTable();
 * 
 * myTable.Append(new string[] {"foo", "bar"}); // Could use a predefined string array
 * myTable.Append(new string[] {"One", "Two"}); // or an ArrayList here
 * myTable.Prepend(new string[] {});            // Inserts an empty row
 * 
 * myTable.SetHeaders(new string[] {"Column 1", "Column 2"});
 * 
 * Console.WriteLine(myTable.ToString());       // Prints the table
 */
using System;
using System.Collections;

namespace HLib {
    /// <summary>
    /// Handles building of Console Tables
    /// </summary>
    public class ConsoleTable {
        #region Fields
        private ArrayList headers;
        private ArrayList footers;
        private ArrayList data;
        private int maxCols;
        private int maxRows;
        private ArrayList cellLengths;
        private int padding;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Instanciates new copy of object
        /// </summary>
        public ConsoleTable() {
            this.headers = new ArrayList();
            this.footers = new ArrayList();
            this.data = new ArrayList();
            this.cellLengths = new ArrayList();
            this.padding = 1;
        }
        #endregion

        #region Public instance methods

        /// <summary>
        /// Adds headers to the table.
        /// </summary>
        /// <param name="headers">An IList collection of the headers</param>
        public void SetHeaders(IList headers) {
            this.headers = new ArrayList(headers);
            this.UpdateRowsCols(this.headers);
        }

        /// <summary>
        /// Adds footers to the table.
        /// </summary>
        /// <param name="footers">An IList collection of the headers</param>
        public void SetFooters(IList footers) {
            this.footers = new ArrayList(footers);
            this.UpdateRowsCols(this.footers);
        }

        /// <summary>
        /// Adds a row of data to the table.
        /// </summary>
        /// <param name="rowData">An IList of the row data</param>
        public void AppendRow(IList rowData) {
            this.data.Add(new ArrayList(rowData));
            this.UpdateRowsCols((ArrayList)this.data[this.data.Count - 1]);
        }

        /// <summary>
        /// Prepends a row of data to the table.
        /// </summary>
        /// <param name="rowData">An IList of the row data</param>
        public void PrependRow(IList rowData) {
            this.data.Insert(0, new ArrayList(rowData));
            this.UpdateRowsCols((ArrayList)this.data[0]);
        }

        /// <summary>
        /// Inser the given row data at the beginning of the table.
        /// Much the same as PrependRow().
        /// </summary>
        /// <param name="rowData">An IList of the row data</param>
        public void InsertRow(IList rowData) {
            this.InsertRow(rowData, 0);
        }

        /// <summary>
        /// Insert the given row data at the given row index.
        /// </summary>
        /// <param name="rowData">An IList of the row data</param>
        /// <param name="rowIndex">Row index to insert at</param>
        public void InsertRow(IList rowData, int rowIndex) {
            while (this.data.Count < rowIndex) {
                this.data.Add(new ArrayList());
            }

            this.data.Insert(rowIndex, new ArrayList(rowData));
            this.UpdateRowsCols((ArrayList)this.data[rowIndex]);
        }

        public override string ToString() {
            this.ValidateTable();
            return this.BuildTable();
        }
        #endregion

        #region Private instance methods
        private void ValidateTable() {
            for (int i = 0; i < this.maxRows; ++i) {
                for (int j = 0; j < this.maxCols; ++j) {
                    if (((ArrayList)this.data[i]).Count < this.maxCols) {
                        ((ArrayList)this.data[i]).Add("");
                    }

                    this.CalculateCellLengths((ArrayList)this.data[i]);
                }
            }
        }

        /// <summary>
        /// Creates the table
        /// </summary>
        /// <returns>The string representation of the table</returns>
        private string BuildTable() {
            string[] retVal = new string[this.maxRows];
            ArrayList rowData = this.data;

            // Go thru every row
            for (int i = 0; i < this.maxRows; ++i) {

                ArrayList currentRow = (ArrayList)rowData[i];

                // Pad this rows data
                for (int j = 0; j < currentRow.Count; ++j) {
                    if (currentRow[j].ToString().Length < (int)this.cellLengths[j]) {
                        currentRow[j] = currentRow[j].ToString().PadRight((int)this.cellLengths[j], ' ');
                    }
                }

                string rowBegin = "|" + " ".PadRight(this.padding, ' ');
                string rowEnd = " ".PadRight(this.padding, ' ') + "|";
                string implode = " ".PadRight(this.padding, ' ') + "|" + " ".PadRight(this.padding, ' ');

                retVal[i] = rowBegin + String.Join(implode, (string[])currentRow.ToArray(typeof(string))) + rowEnd;
            }

            string returnStr = this.GetSeparator() + "\r\n" + String.Join("\r\n", retVal) + "\r\n" + this.GetSeparator() + "\r\n";

            // Add headers
            if (this.headers.Count > 0) {
                returnStr = this.GetHeaderLine() + "\r\n" + returnStr;
            }

            // Add footers
            if (this.footers.Count > 0) {
                returnStr = returnStr + this.GetFooterLine() + "\r\n";
            }

            return returnStr;
        }

        /// <summary>
        /// Returns a string composed of a separator line
        /// </summary>
        /// <returns>The separator line</returns>
        private string GetSeparator() {
            ArrayList retVal = new ArrayList();
            foreach (int width in this.cellLengths) {
                retVal.Add("-".PadRight(width, '-'));
            }

            string rowBegin = "+" + "-".PadRight(this.padding, '-');
            string rowEnd = "-".PadRight(this.padding, '-') + "+";
            string implode = "-".PadRight(this.padding, '-') + "+" + "-".PadRight(this.padding, '-');

            return rowBegin + String.Join(implode, (string[])retVal.ToArray(typeof(string))) + rowEnd;
        }

        /// <summary>
        /// Returns a string composed of the headers for the table
        /// </summary>
        /// <returns>The headerline</returns>
        private string GetHeaderLine() {
            for (int i = 0; i < this.maxCols; ++i) {
                if (this.headers.Count <= i) {
                    this.headers.Add(""); ;
                }
            }

            for (int i = 0; i < this.headers.Count; ++i) {
                if (this.headers[i].ToString().Length < (int)this.cellLengths[i]) {
                    this.headers[i] = ((string)this.headers[i]).PadRight((int)this.cellLengths[i], ' ');
                }
            }

            string rowBegin = "|" + " ".PadRight(this.padding, ' ');
            string rowEnd = " ".PadRight(this.padding, ' ') + "|";
            string implode = " ".PadRight(this.padding, ' ') + "|" + " ".PadRight(this.padding, ' ');

            return this.GetSeparator() + "\r\n" + rowBegin + String.Join(implode, (string[])this.headers.ToArray(typeof(string))) + rowEnd;
        }

        /// <summary>
        /// Returns a string composed of the footers for the table
        /// </summary>
        /// <returns>The headerline</returns>
        private string GetFooterLine() {
            for (int i = 0; i < this.maxCols; ++i) {
                if (this.footers.Count <= i) {
                    this.footers.Add("");
                }
            }

            for (int i = 0; i < this.footers.Count; ++i) {
                if (this.footers[i].ToString().Length < (int)this.cellLengths[i]) {
                    this.footers[i] = ((string)this.footers[i]).PadRight((int)this.cellLengths[i], ' ');
                }
            }

            string rowBegin = "|" + " ".PadRight(this.padding, ' ');
            string rowEnd = " ".PadRight(this.padding, ' ') + "|";
            string implode = " ".PadRight(this.padding, ' ') + "|" + " ".PadRight(this.padding, ' ');

            return rowBegin + String.Join(implode, (string[])this.footers.ToArray(typeof(string))) + rowEnd + "\r\n" + this.GetSeparator();
        }


        /// <summary>
        /// Updates maxrows and maxcols values
        /// </summary>
        /// <param name="rowData">The row data to check</param>
        private void UpdateRowsCols(ArrayList rowData) {
            this.maxCols = Math.Max(this.maxCols, rowData.Count);
            this.maxRows = this.data.Count;
        }

        /// <summary>
        /// Calculates the maximum cell length for each column
        /// and stores it in the cellLengths array.
        /// </summary>
        private void CalculateCellLengths(ArrayList rowData) {
            for (int i = 0; i < rowData.Count; ++i) {

                if (this.cellLengths.Count < (i + 1)) {
                    this.cellLengths.Add(0);
                }

                // Compare row data length to current cell length
                this.cellLengths[i] = Math.Max((int)this.cellLengths[i], rowData[i].ToString().Length);

                // Compare cell length to header length
                if (this.headers.Count > i) {
                    this.cellLengths[i] = Math.Max(this.headers[i].ToString().Length, (int)this.cellLengths[i]);
                }
            }
        }
        #endregion
    }
}