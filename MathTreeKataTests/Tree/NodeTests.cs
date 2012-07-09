using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using MathTreeKata.Tree;

namespace MathTreeKataTests.Tree
{
    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void INode_Evaluates_ReturnsDouble()
        {
            double d = 1d;
            var mock = new Mock<INode>();
            mock.Setup(x => x.Evaluate()).Returns(1);

            Assert.AreEqual(d, mock.Object.Evaluate());
        }

        [Test]
        public void ValueNode_CreateNew_Exists()
        {
            INode node = new ValueNode(0);
            Assert.IsInstanceOf(typeof(ValueNode), node);
        }

        [Test]
        public void ValueNode_Evaluate_ReturnsInitialisationParameterValue()
        {
            double d = 15d;
            INode node = new ValueNode(d);
            Assert.AreEqual(d, node.Evaluate());
        }

        [Test]
        public void OperatorNode_CreateNew_Exists()
        {
            Func<double, double, double> func = (x, y) => x + y;
            INode node = new OperatorNode(func, 0);
            Assert.IsInstanceOf(typeof(OperatorNode), node);
        }

        [Test]
        public void OperatorNode_AppendChild_NoChildren_RightChildUpdated()
        {
            OperatorNode node = new OperatorNode(null, 0);
            node.AppendChild(new ValueNode(0));
            Assert.IsNotNull(node.Right);
        }

        [Test]
        public void OperatorNode_AppendChild_LeftFull_RightChildUpdated()
        {
            OperatorNode node = new OperatorNode(null, 0);
            node.Left = new ValueNode(0);
            node.AppendChild(new ValueNode(0));
            Assert.IsNotNull(node.Right);
        }

        [Test]
        public void OperatorNode_AdditionFunction_EvaluatesSumOfChildNodes()
        {
            Func<double, double, double> add = (x, y) => x + y;

            OperatorNode node = new OperatorNode(add, 0);
            node.Left = new ValueNode(5);
            node.Right = new ValueNode(2);

            Assert.AreEqual(7, node.Evaluate());
        }

        [TestCase("2+4+3", 9)]
        [TestCase("2+4*3", 14)]
        [TestCase("2/4*6", 3)]
        [TestCase("2+4*3+5", 19)]
        [TestCase("2+4*3-5", 9)]
        [TestCase("3+4*5+7*3*5-9", 119)]
        [TestCase("3+4*5+7*3*5-9/6*2+7", 132)]
        public void NodeFactory_ExpressionTree_Evaluate(string expression, int value)
        {
            NodeFactory nf = new NodeFactory();
            INode node = nf.CreateExpressionTree(expression);

            Assert.AreEqual(value, node.Evaluate());
        }

    }
}
