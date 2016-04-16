using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DrawBezier
{
    public class Bezier
    {
        public Bezier(int order)
        {
            if (order < 2)
            {
                throw new ArgumentException("Order cannot be less than 2.", nameof(order));
            }

            m_order = order;
            m_controlPoints = new Points(m_order);
        }


        private int m_order;
        public int Order
        {
            get { return m_order; }
            private set
            {
                m_order = value;
            }
        }

        private Points m_controlPoints;
        public Points ControlPoints
        {
            get { return m_controlPoints; }
            private set
            {
                m_controlPoints = value;
            }
        }

                
        public Point GetInterpolantValueAt(double distance)
        {
            Point[] points = new Point[m_order];
            for (int i = 0; i < m_order; i++)
            {
                points[i] = ControlPoints[i];
            }

            int pointsLeft = m_order;

            while (pointsLeft > 1)
            {
                for (int p = 0; p < pointsLeft - 1; p++)
                {
                    points[p].X = getDistantPoint(points[p].X, points[p + 1].X, distance);
                    points[p].Y = getDistantPoint(points[p].Y, points[p + 1].Y, distance);
                }

                pointsLeft--;
            }

            return points[0];
        }

        private static double getDistantPoint(double x1, double x2, double distance)
        {
            double diff = 0.0;
            double travel = 0.0;

            diff = x2 - x1;
            travel = x1 + diff * distance;

            return travel;
        }


        public class Points
        {
            Point[] m_points;
            public event Action<Points, int> PointChanged;

            public Points(int size)
            {
                m_points = new Point[size];
            }

            public Point this[int index]
            {
                get { return m_points[index]; }
                set
                {
                    m_points[index] = value;
                    if (PointChanged != null)
                    {
                        PointChanged(this, index);
                    }
                }
            }
            
            public int Count
            {
                get { return m_points.Length; }
            }

        }
    }
}
