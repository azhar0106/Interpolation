using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartesianDraw.Model
{
    public class Line : PrimitiveBase
    {
        private double m_x1;
        private double m_y1;
        private double m_x2;
        private double m_y2;


        public Line()
        {
            m_x1 = 0.0;
            m_x2 = 0.0;
            m_y1 = 0.0;
            m_y2 = 0.0;
        }


        public override double XCoordinate
        {
            get
            {
                return base.XCoordinate;
            }

            set
            {
                base.XCoordinate = value;
                X1 = value;
            }
        }

        public override double YCoordinate
        {
            get
            {
                return base.YCoordinate;
            }

            set
            {
                base.YCoordinate = value;
                Y1 = value;
            }
        }
        
        public double X1
        {
            get { return m_x1; }
            set
            {
                if (value == m_x1)
                    return;

                m_x1 = value;

                RaisePropertyChanged(nameof(X1));

                XCoordinate = value;
            }
        }
        
        public double Y1
        {
            get { return m_y1; }
            set
            {
                if (value == m_y1)
                    return;

                m_y1 = value;

                RaisePropertyChanged(nameof(Y1));

                YCoordinate = value;
            }
        }
        
        public double X2
        {
            get { return m_x2; }
            set
            {
                if (value == m_x2)
                    return;

                m_x2 = value;

                RaisePropertyChanged(nameof(X2));
            }
        }
        
        public double Y2
        {
            get { return m_y2; }
            set
            {
                if (value == m_y2)
                    return;

                m_y2 = value;

                RaisePropertyChanged(nameof(Y2));
            }
        }
    }
}
