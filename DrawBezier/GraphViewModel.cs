using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;

namespace DrawBezier
{
    public class GraphViewModel : ViewModelBase
    {
        public GraphViewModel()
        {
            m_bezierViewModels = new ObservableCollection<BezierViewModel>();

            BezierModel.Beziers.CollectionChanged += Beziers_CollectionChanged;
        }

        public IBezierModel BezierModel
        {
            get { return (Application.Current as App).BezierModel; }
        }

        private ObservableCollection<BezierViewModel> m_bezierViewModels;
        public ObservableCollection<BezierViewModel> BezierViewModels
        {
            get { return m_bezierViewModels; }
            set { m_bezierViewModels = value; }
        }


        private void Beziers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                Bezier b = (Bezier)e.NewItems[0];
                BezierViewModel bvm = new BezierViewModel(b, BezierModel.InterpolantResolutionToUse);
                m_bezierViewModels.Add(bvm);
            }
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                Bezier b = (Bezier)e.OldItems[0];
                BezierViewModel bvm = m_bezierViewModels.Single((bv) => bv.Bezier == b);
                m_bezierViewModels.Remove(bvm);
            }
        }
    }
}
