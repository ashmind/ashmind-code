using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Xml;

namespace AshMind.Code.Smells.Sniffer.Storage {
    public class XamlStorage : IStorage {
        public void Save(string key, object data) {
            var fileName = key + ".xaml";

            var settings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(fileName, settings)) {
                XamlWriter.Save(data, writer);
            }
        }
    }
}
