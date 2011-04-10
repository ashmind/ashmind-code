using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

using Caliburn.Metadata;

using AshMind.Extensions;

using AshMind.Code.Smells.Sniffer.Internal;
using AshMind.Code.Smells.Sniffer.Model;
using AshMind.Code.Smells.Sniffer.Storage;

namespace AshMind.Code.Smells.Sniffer.Controllers {
    public class SmellController {
        private readonly ISmell[] smells;
        private readonly CollectionPair<SmellGroup> smellGroups = new CollectionPair<SmellGroup>();

        public IStorage Storage { get; set; }

        public SmellController() {
            this.Storage = new NullStorage();

            this.smells = LoadSmells().ToArray();

            var groups = GroupSmells(smells);
            this.smellGroups.Inner.AddRange(groups);
        }

        [AsyncAction]
        public void ProcessSmellChange() {
            this.Storage.Save("Smells", this.smells);
        }
        
        private static IEnumerable<Type> LoadSmellTypes() {
            return from type in typeof(ISmell).Assembly.GetExportedTypes()
                   where type.IsClass && !type.IsAbstract
                      && type.GetInterfaces().Contains(typeof(ISmell))
                   select type;
        }

        private static IEnumerable<ISmell> LoadSmells() {
            return from type in LoadSmellTypes()
                   select (ISmell)Activator.CreateInstance(type);
        }

        private static IEnumerable<SmellGroup> GroupSmells(IEnumerable<ISmell> smells) {
            return from smell in smells
                   let groupName = Regex.Match(smell.GetType().Namespace, "[^.]+$").Groups[0].Value
                   let info = new SmellInfo(smell)
                   orderby info.Name
                   group info by groupName into smellGroup
                   orderby smellGroup.Key
                   select new SmellGroup(smellGroup.Key, smellGroup);
        }

        public ReadOnlyObservableCollection<SmellGroup> SmellGroups {
            get { return this.smellGroups.ReadOnly; }
        }
    }
}
