using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathTreeKata.Tree
{
    public class ValueNode : INode
    {
        private double value;

        public ValueNode(double value)
        {
            this.value = value;

        }

        #region INode Members

        public double Evaluate()
        {

            return value;
        }

        public bool IsLeaf() { return true; }

        #endregion
    }
}
