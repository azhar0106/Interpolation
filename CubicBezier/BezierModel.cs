using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DrawBezier
{
    public interface IBezierModel
    {
        ObservableCollection<Bezier> Beziers { get; }
        int InterpolantResolutionToUse { get; set; }
        void AddBezier(Bezier bezier);
        void RemoveBezier(Bezier bezier);
    }


    public class BezierModel : IBezierModel
    {
        public BezierModel()
        {
            m_beziers = new ObservableCollection<Bezier>();
        }


        private ObservableCollection<Bezier> m_beziers;
        public ObservableCollection<Bezier> Beziers
        {
            get { return m_beziers; }
            private set { m_beziers = value; }
        }

        private int m_interpolantResolutionToUse;
        public int InterpolantResolutionToUse
        {
            get { return m_interpolantResolutionToUse; }
            set { m_interpolantResolutionToUse = value; }
        }


        public void AddBezier(Bezier bezier)
        {
            m_beziers.Add(bezier);
        }
        public void RemoveBezier(Bezier bezier)
        {
            m_beziers.Remove(bezier);
        }
    }
}
