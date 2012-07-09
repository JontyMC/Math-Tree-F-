using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathTreeKata.Tree
{
    public class OperatorNode : INode
    {
        private Func<double, double, double> func = null;
        public int Precidence { get; private set; }
        public INode Left { get; set; }
        public INode Right { get; set; }

        public OperatorNode(Func<double, double, double> func, int precidence)
        {
            this.func = func;
            this.Precidence = precidence;
            
        }

        #region INode Members

        public double Evaluate()
        {
           return func(Left.Evaluate(), Right.Evaluate());
        }


        public bool IsLeaf() { return false; }

        #endregion

        public void AppendChild(INode node)
        {
            if (this.Right == null)
            {
                this.Right = node;
            }
            else
            {
                this.Left = node;
            }
        }

    }
}
