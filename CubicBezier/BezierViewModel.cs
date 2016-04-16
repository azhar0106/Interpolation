using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DrawBezier
{
    public class BezierViewModel : ViewModelBase
    {
        public BezierViewModel(Bezier bezier, int resolution)
        {
            m_bezier = bezier;
            m_resolution = resolution;
            m_interpolantPoints = new Point[m_resolution];

            UpdatePoints();

            m_bezier.ControlPoints.PointChanged += ControlPoints_PointChanged;
        }


        private Bezier m_bezier;
        public Bezier Bezier
        {
            get { return m_bezier; }
            private set { m_bezier = value; }
        }

        private Point[] m_interpolantPoints;
        public Point[] InterpolantPoints
        {
            get { return m_interpolantPoints; }
            private set
            {
                m_interpolantPoints = value;
                NotifyPropertyChanged(nameof(InterpolantPoints));
            }
        }

        private int m_resolution;
        public int Resolution
        {
            get { return m_resolution; }
            set
            {
                if (value == m_resolution)
                    return;

                m_resolution = value;
                UpdatePoints();

                NotifyPropertyChanged(nameof(Resolution));
            }
        }


        private void ControlPoints_PointChanged(Bezier.Points arg1, int arg2)
        {
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            Point[] newPoints = new Point[m_resolution + 1];
            for (int i = 0; i <= m_resolution; i++)
            {
                newPoints[i] = m_bezier.GetInterpolantValueAt((double)i / m_resolution);
            }
            InterpolantPoints = newPoints;
        }

    }
}
