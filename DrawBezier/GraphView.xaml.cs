using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawBezier
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        List<Primitives> m_prims;
        DependencyPropertyDescriptor m_leftDescriptor = DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Ellipse));
        DependencyPropertyDescriptor m_topDescriptor = DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Ellipse));
        bool m_circlesNotDragged;


        public GraphView()
        {
            InitializeComponent();
            m_prims = new List<Primitives>();
            BezierModel = (Application.Current as App).BezierModel;
            GraphViewModel = new GraphViewModel();
            this.DataContext = GraphViewModel;
            GraphViewModel.BezierViewModels.CollectionChanged += BezierViewModels_CollectionChanged;
            DrawingCanvas.SizeChanged += DrawingCanvas_SizeChanged;
            
        }

        
        public IBezierModel BezierModel { get; set; }
        public GraphViewModel GraphViewModel { get; set; }


        private void DrawingCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            m_circlesNotDragged = true;
            RedrawBeziers();
            m_circlesNotDragged = false;
        }
        private void BezierViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                BezierViewModel bvm = (BezierViewModel)e.NewItems[0];
                bvm.PropertyChanged += Bvm_PropertyChanged;
                AddBezier(bvm);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                BezierViewModel bvm = (BezierViewModel)e.OldItems[0];
                bvm.PropertyChanged -= Bvm_PropertyChanged;
                RemoveBezier(bvm);
            }
        }
        private void Bvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName==nameof(BezierViewModel.InterpolantPoints))
            {
                BezierViewModel bvm = (BezierViewModel)sender;
                Primitives prim = m_prims.Single((p) => p.BVM == bvm);
                RedrawPolyline(prim);
            }
        }
        private void Ellipse_PositionChanged(object sender, EventArgs e)
        {
            if (m_circlesNotDragged) return;

            Ellipse point = (Ellipse)sender;
            double newLeft = Canvas.GetLeft(point);
            double newTop = Canvas.GetTop(point);
            Point newPoint = new Point(newLeft, newTop);
            Point cartPoint = InverseTransformPoint(newPoint, point.RenderSize, true);

            Primitives prim = null;
            int pointIndex = -1;

            bool _primFound = false;
            foreach(var _prim in m_prims)
            {
                int count = _prim.Points.Count;
                for (int _i = 0; _i < count; _i++)
                {
                    Ellipse _e = _prim.Points[_i];
                    if (_e == point)
                    {
                        _primFound = true;
                        prim = _prim;
                        pointIndex = _i;
                        break;
                    }
                }
                if (_primFound) break;
            }

            prim.BVM.Bezier.ControlPoints[pointIndex] = cartPoint;
        }

        void AddBezier(BezierViewModel bvm)
        {
            Primitives prim = new Primitives();
            prim.BVM = bvm;

            //Draw points
            m_circlesNotDragged = true;
            for (int i = 0; i < bvm.Bezier.ControlPoints.Count; i++)
            {
                Ellipse e = new Ellipse();
                e.Stroke = new SolidColorBrush(Colors.Black);
                e.StrokeThickness = 5;
                e.Fill = new SolidColorBrush(Colors.Gray);
                e.Width = 40;
                e.Height = 40;
                m_leftDescriptor.AddValueChanged(e, Ellipse_PositionChanged);
                m_topDescriptor.AddValueChanged(e, Ellipse_PositionChanged);

                Point p = TransformPoint(bvm.Bezier.ControlPoints[i], new Size(e.Width, e.Height), true);
                Canvas.SetLeft(e, p.X);
                Canvas.SetTop(e, p.Y);

                prim.Points.Add(e);

                DrawingCanvas.Children.Add(e);
            }
            m_circlesNotDragged = false;

            //Draw bezier
            Polyline lines = new Polyline();
            lines.Stroke = new SolidColorBrush(Colors.Black);
            lines.StrokeThickness = 1;
            
            for (int ip = 0; ip < bvm.InterpolantPoints.Length; ip++)
            {
                Point lp = TransformPoint(bvm.InterpolantPoints[ip], lines.RenderSize, false);
                lines.Points.Add(lp);
            }

            prim.Interpolant = lines;
            DrawingCanvas.Children.Add(lines);

            m_prims.Add(prim);
        }
        void RemoveBezier(BezierViewModel bvm)
        {
            Primitives prim = m_prims.Single((p) => p.BVM == bvm);
            foreach(var point in prim.Points)
            {
                m_leftDescriptor.RemoveValueChanged(point, Ellipse_PositionChanged);
                m_topDescriptor.RemoveValueChanged(point, Ellipse_PositionChanged);
                DrawingCanvas.Children.Remove(point);
            }

            DrawingCanvas.Children.Remove(prim.Interpolant);
        }
        void RedrawBeziers()
        {
            foreach (var prim in m_prims)
            {
                RedrawBezier(prim);
            }
        }
        void RedrawBezier(Primitives prim)
        {
            //Draw points
            RedrawCircles(prim);

            //Draw bezier
            RedrawPolyline(prim);
        }
        void RedrawCircles(Primitives prim)
        {
            BezierViewModel bvm = prim.BVM;

            for (int i = 0; i < bvm.Bezier.ControlPoints.Count; i++)
            {
                Ellipse e = prim.Points[i];

                Point p = TransformPoint(bvm.Bezier.ControlPoints[i], e.RenderSize, true);
                Canvas.SetLeft(e, p.X);
                Canvas.SetTop(e, p.Y);
            }
        }
        void RedrawPolyline(Primitives prim)
        {
            BezierViewModel bvm = prim.BVM;
            Polyline lines = prim.Interpolant;

            for (int ip = 0; ip < bvm.InterpolantPoints.Length; ip++)
            {
                Point lp = TransformPoint(bvm.InterpolantPoints[ip], new Size(), false);
                lines.Points[ip] = lp;
            }
        }
        Point TransformPoint(Point p, Size s, bool center)
        {
            p.X = p.X + DrawingCanvas.ActualWidth / 2;
            p.Y = -p.Y + DrawingCanvas.ActualHeight / 2;

            if (center)
            {
                p.X -= s.Width / 2;
                p.Y -= s.Height / 2;
            }

            return p;
        }
        Point InverseTransformPoint(Point p, Size s, bool center)
        {
            p.X = p.X - DrawingCanvas.ActualWidth / 2;
            p.Y = -(p.Y - DrawingCanvas.ActualHeight / 2);

            if (center)
            {
                p.X += s.Width / 2;
                p.Y -= s.Height / 2;
            }

            return p;
        }


        class Primitives
        {
            public BezierViewModel BVM { get; set; }
            public List<Ellipse> Points { get; set; }
            public Polyline Interpolant { get; set; }

            public Primitives()
            {
                Points = new List<Ellipse>();
            }
        }
    }
}
