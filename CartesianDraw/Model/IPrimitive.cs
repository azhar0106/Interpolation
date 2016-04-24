using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartesianDraw.Model
{
    public interface ICartesianPrimitive
    {
        double XCoordinate { get; set; }

        double YCoordinate { get; set; }
    }
}
