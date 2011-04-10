using System;
using System.Linq;
using System.Windows;

using AshMind.Code.Smells.Sniffer.Internal;

namespace AshMind.Code.Smells.Sniffer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            ((BoundNameScope)this.Resources["windowNameScope"]).Target = this;
        }

        protected override void OnDragOver(DragEventArgs e) {
            var fileDrop = e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effects = fileDrop ? DragDropEffects.Copy : DragDropEffects.None;

            base.OnDragOver(e);
        }

        protected override void OnDrop(DragEventArgs e) {
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            this.controller.LoadAssemblies(paths);

            base.OnDrop(e);
        }
    }
}
