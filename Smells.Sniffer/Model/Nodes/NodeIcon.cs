using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using AshMind.Code.Analysis;
using AshMind.Constructs;
using AshMind.Extensions;

namespace AshMind.Code.Smells.Sniffer.Model.Nodes {
    public static class NodeIcon {
        private const string IconPrefix = "AshMind.Code.Smells.Sniffer.Images.";

        private class IconInfo {
            public IconInfo(IconType iconType) {
                this.IconType = iconType;
            }

            public IconInfo(IconType iconType, IconAccess iconAccess) : this(iconType) {
                this.IconAccess = iconAccess;
            }

            public IconType IconType { get; private set; }
            public IconAccess? IconAccess { get; private set; }

            public string GetIconName() {
                if (this.IconAccess == null)
                    return this.IconType.ToString();

                return string.Format("{0}.{1}", this.IconType, this.IconAccess);
            }
        }

        private enum IconType {
            Assembly,
            Class,
            Interface,
            Struct,
            Enum,
            Method,
            Property,
            Event
        }

        private enum IconAccess {
            Private,
            Protected,
            Internal,
            Public,
        }

        private const string FailedIconName = "Failed";
        private static readonly IDictionary<string, ImageSource> icons = new Dictionary<string, ImageSource>();
 
        static NodeIcon() {
            var iconTypesNotSupportingAccess = new[] {IconType.Assembly.ToString()};
            iconTypesNotSupportingAccess.ForEach(LoadIcon);

            var accessNames = Enum.GetNames(typeof (IconAccess));

            foreach (var typeName in Enum.GetNames(typeof(IconType)).Except(iconTypesNotSupportingAccess)) {
                foreach (var accessName in accessNames) {
                    LoadIcon(typeName + "." + accessName);
                }
            }

            LoadIcon(FailedIconName);
        }

        private static readonly Func<IAnalysisData, IconInfo> selectInfo =
            Switch.Type<IAnalysisData>().To<IconInfo>()
                .Case<IAssemblyData>(a => new IconInfo(IconType.Assembly))
                .Case<TypeData>(type => new IconInfo(GetIconType(type), GetAccess(type)))
                .Case<MethodData>(m => new IconInfo(IconType.Method, GetAccess(m)))
                .Case<PropertyData>(p => new IconInfo(IconType.Property, GetAccess(p)))
                .Case<EventData>(e => new IconInfo(IconType.Event, GetAccess(e)))
                .Compile();
        
        private static IconType GetIconType(TypeData type) {
            if (type.Inner.IsEnum)
                return IconType.Enum;

            if (type.Inner.IsInterface)
                return IconType.Interface;

            return type.Inner.IsClass ? IconType.Class : IconType.Struct;
        }

        private static IconAccess GetAccess(IWithAccessors propertyOrEvent) {
            return propertyOrEvent.Accessors.Max(m => GetAccess(m));
        }

        private static IconAccess GetAccess(MethodData method) {
            if (method.Inner.IsPrivate)
                return IconAccess.Private;

            if (method.Inner.IsFamily)
                return IconAccess.Protected;

            if (method.Inner.IsAssembly || method.Inner.IsFamilyOrAssembly)
                return IconAccess.Internal;

            return IconAccess.Public;
        }

        private static IconAccess GetAccess(TypeData type) {
            if (type.Inner.IsNestedPrivate)
                return IconAccess.Private;

            if (type.Inner.IsNestedFamily )
                return IconAccess.Protected;

            if (type.Inner.IsNestedAssembly || type.Inner.IsNestedFamORAssem || type.Inner.IsNotPublic)
                return IconAccess.Internal;

            return IconAccess.Public;
        }

        public static ImageSource SelectFor(IAnalysisData data) {
            var info = selectInfo(data);

            return icons[info.GetIconName()];
        }

        public static ImageSource Failed {
            get { return icons[FailedIconName]; }
        }

        private static void LoadIcon(string name) {
            var fullName = IconPrefix + name + ".png";
            using (var stream = typeof(NodeIcon).Assembly.GetManifestResourceStream(fullName)) {
                icons[name] = BitmapFrame.Create(stream);
            }
        }
    }
}