using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrawBezier
{
    public class CartesianCanvas : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Canvas arranges children at their DesiredSize.
            //This means that Margin on children is actually respected and added
            //to the size of layout partition for a child. 
            //Therefore, is Margin is 10 and Left is 20, the child's ink will start at 30.

            foreach (UIElement child in InternalChildren)
            {
                if (child == null) { continue; }

                double x = 0;
                double y = 0;


                //Compute offset of the child:
                //If Left is specified, then Right is ignored
                //If Left is not specified, then Right is used
                //If both are not there, then 0
                double left = GetLeft(child);
                if (!Double.IsNaN(left))
                {
                    x = left;
                }
                else
                {
                    double right = GetRight(child);

                    if (!Double.IsNaN(right))
                    {
                        x = right;
                    }
                }

                double top = GetTop(child);
                if (!Double.IsNaN(top))
                {
                    y = top;
                }
                else
                {
                    double bottom = GetBottom(child);

                    if (!Double.IsNaN(bottom))
                    {
                        y = bottom;
                    }
                }

                // Transform for Cartesian coordinates
                x += arrangeSize.Width / 2;
                y = -y;
                y += arrangeSize.Height / 2;

                // Center child
                x -= child.DesiredSize.Width / 2;
                y -= child.DesiredSize.Height / 2;

                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
