using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartesianDraw.Model
{
    public class PrimitiveBase : ICartesianPrimitive
    {
        protected double m_xCoordinate;
        protected double m_yCoordinate;

        public event PropertyChangedEventHandler PropertyChanged;


        public PrimitiveBase()
        {
            m_xCoordinate = 0.0;
            m_yCoordinate = 0.0;
            PropertyChanged = (s, e) => { };
        }


        public virtual double XCoordinate
        {
            get
            {
                return m_xCoordinate;
            }

            set
            {
                if (value == m_xCoordinate)
                    return;

                m_xCoordinate = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(XCoordinate)));
            }
        }

        public virtual double YCoordinate
        {
            get
            {
                return m_yCoordinate;
            }

            set
            {
                if (value == m_yCoordinate)
                    return;

                m_yCoordinate = value;
            }
        }


        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
