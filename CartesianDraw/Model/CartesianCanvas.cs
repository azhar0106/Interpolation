using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartesianDraw.Model
{
    public class CartesianCanvas
    {
        List<ICartesianPrimitive> m_primitives;

        public event EventHandler<PrimitiveCollectionChangedEventArgs> PrimitiveCollectionChanged = (s, e) => { };
        

        public CartesianCanvas()
        {
            m_primitives = new List<ICartesianPrimitive>();
        }


        public void AddPrimitive(ICartesianPrimitive primitive)
        {
            if (!m_primitives.Contains(primitive))
            {
                m_primitives.Add(primitive);

                RaisePrimitiveCollectionChanged(primitive, CollectionChangeType.Add);
            }
        }

        public void RemovePrimitive(ICartesianPrimitive primitive)
        {
            if (m_primitives.Contains(primitive))
            {
                m_primitives.Remove(primitive);

                RaisePrimitiveCollectionChanged(primitive, CollectionChangeType.Remove);
            }
        }
        
        public void RaisePrimitiveCollectionChanged(ICartesianPrimitive primitive, CollectionChangeType changeType)
        {
            PrimitiveCollectionChanged(this,
                new PrimitiveCollectionChangedEventArgs()
                {
                    Primitive = primitive,
                    IsAdded = changeType
                });
        }
    }


    public class PrimitiveCollectionChangedEventArgs : EventArgs
    {
        public ICartesianPrimitive Primitive { get; set; }
        public CollectionChangeType IsAdded { get; set; }
    }


    public enum CollectionChangeType
    {
        Add,
        Remove
    }
}
