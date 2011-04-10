using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

using AshMind.Code.Analysis.Collections;
using AshMind.Code.Smells.Sniffer.Model.Annotations;

namespace AshMind.Code.Smells.Sniffer.Model.Nodes {
    public abstract class CodeTreeNode : INotifyPropertyChanged, IAnnotable {
        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        private readonly AnnotationSupport annotations = new AnnotationSupport();
        protected LazyReadOnlyCollection<CodeTreeNode> childNodes;

        private bool visible;
        private bool visibleOnlyBecauseOfChildren;

        protected CodeTreeNode(string text, ImageSource icon, IEnumerable<CodeTreeNode> childNodesSource) {
            this.Text = text;
            this.Icon = icon;
            this.childNodes = new LazyReadOnlyCollection<CodeTreeNode>(childNodesSource);

            this.visible = true;
        }

        public bool Visible {
            get { return visible; }
            set {
                if (value == visible)
                    return;

                visible = value;
                this.RaisePropertyChanged("Visible");
            }
        }

        public bool VisibleOnlyBecauseOfChildren {
            get { return visibleOnlyBecauseOfChildren; }
            set {
                if (value == visibleOnlyBecauseOfChildren)
                    return;

                visibleOnlyBecauseOfChildren = value;
                this.RaisePropertyChanged("VisibleOnlyBecauseOfChildren");
            }            
        }

        public ImageSource Icon { get; set; }
        public string Text { get; set; }

        public LazyReadOnlyCollection<CodeTreeNode> ChildNodes {
            get { return childNodes; }
        }

        private void RaisePropertyChanged(string propertyName) {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IAnnotable Members

        public void Annotate<T>(T annotation) 
            where T : class 
        {
            this.annotations.Annotate(annotation);
        }

        public void Deannotate<T>() 
            where T : class 
        {
            this.annotations.Deannotate<T>();
        }

        public T Annotation<T>() 
            where T : class 
        {
            return this.annotations.Annotation<T>();
        }

        #endregion
    }
}