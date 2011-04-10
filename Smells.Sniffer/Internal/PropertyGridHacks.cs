using System;
using System.Reflection;
using System.Windows.Forms;

namespace AshMind.Code.Smells.Sniffer.Internal {
    internal static class PropertyGridHacks {
        private static readonly FieldInfo gridViewField = GetField(typeof(PropertyGrid), "gridView");

        private static FieldInfo GetField(Type type, string name) {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        public static void SetLabelWidthRatio(this PropertyGrid grid, double value) {
            var gridView = gridViewField.GetValue(grid);

            var labelWidthField = GetField(gridView.GetType(), "labelWidth");
            labelWidthField.SetValue(gridView, (int)(grid.Width * value));

            var labelRatioField = GetField(gridView.GetType(), "labelRatio");
            labelRatioField.SetValue(gridView, 1/value);
        }
    }
}
