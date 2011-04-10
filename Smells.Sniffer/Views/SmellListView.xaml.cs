using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

using AshMind.Code.Smells.Sniffer.Model;

using UserControl=System.Windows.Controls.UserControl;

namespace AshMind.Code.Smells.Sniffer.Views {
    /// <summary>
    /// Interaction logic for SmellListView.xaml
    /// </summary>
    public partial class SmellListView : UserControl {
        // Using a DependencyProperty as the backing store for SelectedSmells.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSmellsProperty =
            DependencyProperty.Register("SelectedSmell", typeof(SmellInfo), typeof(SmellListView));

        public event EventHandler SelectedSmellChanged = delegate { };

        public SmellListView() {
            InitializeComponent();
        }

        void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var smell = e.NewValue as SmellInfo;
            if (smell == null && e.NewValue != null)
                return;

            this.SelectedSmell = smell;
            this.SelectedSmellChanged(this, EventArgs.Empty);
        }

        private void properyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e) {
            this.SelectedSmellChanged(this, EventArgs.Empty);
        }

        public SmellInfo SelectedSmell {
            get { return (SmellInfo)GetValue(SelectedSmellsProperty); }
            set { SetValue(SelectedSmellsProperty, value); }
        }

    }
}
