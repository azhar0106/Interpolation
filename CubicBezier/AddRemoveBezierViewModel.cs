using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DrawBezier
{
    public class AddRemoveBezierViewModel : ViewModelBase
    {
        public AddRemoveBezierViewModel()
        {
            InitCommands();
            InitBezierResolution();
            
            m_controlPointsCountList = new ObservableCollection<int>();
            m_controlPoints = new ObservableCollection<PointViewModel>();

            for(int i=2; i<=5; i++)
            {
                m_controlPointsCountList.Add(i);
            }

            NumberOfControlPoints = 4;
        }

        
        public IBezierModel BezierModel
        {
            get { return (Application.Current as App).BezierModel; }
        }

        private ObservableCollection<int> m_bezierResolutionList;
        public ObservableCollection<int> BezierResolutionList
        {
            get { return m_bezierResolutionList; }
            set { m_bezierResolutionList = value; }
        }

        private int m_bezierResolution;
        public int BezierResolution
        {
            get { return m_bezierResolution; }
            set
            {
                if (value == m_bezierResolution)
                    return;

                m_bezierResolution = value;
                BezierModel.InterpolantResolutionToUse = m_bezierResolution;

                NotifyPropertyChanged(nameof(BezierResolution));
            }
        }

        private ObservableCollection<int> m_controlPointsCountList;
        public ObservableCollection<int> ControlPointsCountList
        {
            get { return m_controlPointsCountList; }
            set { m_controlPointsCountList = value; }
        }
        
        private int m_numberOfControlPoints;
        public int NumberOfControlPoints
        {
            get { return m_numberOfControlPoints; }
            set
            {
                if (value == m_numberOfControlPoints)
                    return;

                m_numberOfControlPoints = value;
                UpdateControlPoints();

                NotifyPropertyChanged(nameof(NumberOfControlPoints));
            }
        }
        
        private ObservableCollection<PointViewModel> m_controlPoints;
        public ObservableCollection<PointViewModel> ControlPoints
        {
            get { return m_controlPoints; }
            set { m_controlPoints = value; }
        }

        private Bezier m_selectedBezier;
        public Bezier SelectedBezier
        {
            get { return m_selectedBezier; }
            set { m_selectedBezier = value; }
        }

        private ICommand m_addCommand;
        public ICommand AddCommand
        {
            get { return m_addCommand; }
            private set { m_addCommand = value; }
        }

        private ICommand m_removeCommand;
        public ICommand RemoveCommand
        {
            get { return m_removeCommand; }
            private set { m_removeCommand = value; }
        }


        private void InitCommands()
        {
            m_addCommand = new DelegateCommand(AddBezier, () => true);
            m_removeCommand = new DelegateCommand(RemoveBezier, () => SelectedBezier != null);
        }
        private void InitBezierResolution()
        {
            m_bezierResolutionList = new ObservableCollection<int>();
            for (int i = 1; i <= 100; i++)
            {
                m_bezierResolutionList.Add(i);
            }

            BezierResolution = 10;
        }
        void AddBezier()
        {
            Bezier bezier = new Bezier(m_numberOfControlPoints);

            for (int i = 0; i < m_numberOfControlPoints; i++)
            {
                double x = double.Parse(ControlPoints[i].X);
                double y = double.Parse(ControlPoints[i].Y);
                Point p = new Point(x, y);
                bezier.ControlPoints[i] = p;
            }

            BezierModel.AddBezier(bezier);
        }
        void RemoveBezier()
        {
            if(SelectedBezier!=null)
            {
                BezierModel.RemoveBezier(SelectedBezier);
            }
        }
        void UpdateControlPoints()
        {
            if (m_numberOfControlPoints > ControlPoints.Count)
            {
                int diff = m_numberOfControlPoints - m_controlPoints.Count;

                for (int i = 0; i < diff; i++)
                {
                    m_controlPoints.Add(new PointViewModel() { X="-100", Y="-100" });
                }
            }
            else if (m_numberOfControlPoints < ControlPoints.Count)
            {
                int diff = m_controlPoints.Count - m_numberOfControlPoints;

                for(int i=0; i< diff; i++)
                {
                    m_controlPoints.RemoveAt(m_controlPoints.Count - 1);
                }
            }
        }
    }


    public class PointViewModel : ViewModelBase
    {
        private string m_x;
        public string X
        {
            get { return m_x; }
            set
            {
                if (value == m_x) return;

                m_x = value;
                NotifyPropertyChanged(nameof(X));
            }
        }

        private string m_y;
        public string Y
        {
            get { return m_y; }
            set
            {
                if (value == m_y) return;

                m_y = value;
                NotifyPropertyChanged(nameof(Y));
            }
        }

    }
}
