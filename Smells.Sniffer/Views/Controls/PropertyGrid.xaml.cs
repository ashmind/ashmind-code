using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using UserControl=System.Windows.Controls.UserControl;

using AshMind.Code.Smells.Sniffer.Internal;

namespace AshMind.Code.Smells.Sniffer.Views.Controls {
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl {
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid));

        public static readonly DependencyProperty LabelWidthRatioProperty =
            DependencyProperty.Register("LabelWidthRatio", typeof(double), typeof(PropertyGrid));

        public PropertyGrid() {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (e.Property == LabelWidthRatioProperty)
                this.properyGrid.SetLabelWidthRatio((double)e.NewValue);

            if (e.Property == SelectedObjectProperty)
                this.properyGrid.SelectedObject = e.NewValue;

            base.OnPropertyChanged(e);
        }

        public object SelectedObject {
            get { return GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        public double LabelWidthRatio {
            get { return (double)GetValue(LabelWidthRatioProperty); }
            set { SetValue(LabelWidthRatioProperty, value); }
        }

        private void properyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) {
            var handler = this.PropertyValueChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}
