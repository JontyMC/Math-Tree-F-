using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathTreeKata.Tree
{
    public class NodeFactory
    {

        public INode CreateExpressionTree(string expression)
        {
            Queue<INode> nodes = ParseExpressionString(expression);
            INode nd = ParseQueue(nodes);
            return nd;
        }

        private INode ParseQueue(Queue<INode> nodes)
        {
            Stack<INode> operands = new Stack<INode>();
            Stack<OperatorNode> operators = new Stack<OperatorNode>();

            do
            {
                INode n = nodes.Dequeue();

                if (n is ValueNode)
                {
                    operands.Push(n);
                }
                else
                {
                    OperatorNode curr = (OperatorNode)n;
                    if (operators.Count > 0 && operators.Peek().Precidence >= curr.Precidence)
                    {
                            OperatorNode v = operators.Pop();
                            v.AppendChild(operands.Pop());
                            v.AppendChild(operands.Pop());
                            operands.Push(v);
                    }

                    if (operators.Count > 0 && curr.Precidence == operators.Peek().Precidence)
                    {
                        OperatorNode v = operators.Pop();
                        v.AppendChild(operands.Pop());
                        v.AppendChild(operands.Pop());
                        operands.Push(v);
                    }

                    operators.Push(curr);
                }

            } while (nodes.Count > 0);

            while (operators.Count > 0)
            {
                OperatorNode n = operators.Pop();
                n.AppendChild(operands.Pop());
                n.AppendChild(operands.Pop());
                operands.Push(n);
            }

            return operands.Pop();
        }

        private Queue<INode> ParseExpressionString(string expression)
        {
            Queue<INode> nodes = new Queue<INode>();
            foreach (char c in expression)
            {
                if (Char.IsDigit(c) == true)
                {
                    nodes.Enqueue(CreateValueNode(c.ToString()));
                }
                else
                {
                    nodes.Enqueue(CreateOperatorNode(c.ToString()));
                }
            }
            return nodes;
        }

        private ValueNode CreateValueNode(string bit)
        {
            ValueNode nd = null;

            double result = 0;
            if (Double.TryParse(bit, out result) == true)
            {
                nd = new ValueNode(result);
            }

            return nd;
        }
 
        private OperatorNode CreateOperatorNode(string op)
        {
            OperatorNode node = null;
            Func<double, double, double> f = null;
            switch (op)
            {
                case "+":
                    f = (x, y) => x + y;
                    node = new OperatorNode(f, 1);
                    break;
                case "-":
                    f = (x, y) => x - y;
                    node = new OperatorNode(f, 1);
                    break;
                case "*":
                    f = (x, y) => x * y;
                    node = new OperatorNode(f, 2);
                    break;
                case "/":
                    f = (x, y) => x / y;
                    node = new OperatorNode(f, 2);
                    break;
            }
            return node;
        }

    }
}
