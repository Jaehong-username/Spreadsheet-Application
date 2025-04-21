// <copyright file="ExpressionTreeTest.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// Tests public methods in Spreadsheet class.
    /// </summary>
    public class ExpressionTreeTest
    {
        /// <summary>
        /// This set up will be made before testing functions, if needed.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tests BuldTree method with a normal test case.
        /// </summary>
        [Test]
        public void BuildTreeTestNormal()
        {
            ExpressionTree tree = new ExpressionTree("Hello+World");
            tree.ParsingInput();
            tree.BuildTree();
            Assert.That(tree.Root, Is.Not.Null);
            Assert.That(tree.Root, Is.InstanceOf<BinaryOperatorNode>());

            BinaryOperatorNode testNode = (BinaryOperatorNode)tree.Root; // safe casting after ensuging the type. so it won't cause a run time error.
            Assert.That(testNode, Is.Not.Null);  // Ensure casting was successful, a good practice.
            Assert.That(testNode.Content, Is.EqualTo("+"));  // Access methods of BinaryOperatorNode

            Assert.That(testNode.Left, Is.Not.Null);
            Assert.That(testNode.Left, Is.InstanceOf<VariableNode>());

            var leftNode = testNode.Left as VariableNode;
            Assert.That(leftNode, Is.Not.Null);  // Ensure the cast succeeded
            Assert.That(leftNode.Content, Is.EqualTo("Hello"));
            Assert.That(testNode.Right, Is.Not.Null);
            Assert.That(testNode.Right, Is.InstanceOf<VariableNode>());

            var rightNode = testNode.Right as VariableNode;
            Assert.That(rightNode, Is.Not.Null);  // Ensure the cast succeeded
            Assert.That(rightNode.Content, Is.EqualTo("World"));
        }

        /// <summary>
        /// Tests ParsingInput from ExpressionTree class with a nornal test case.
        /// </summary>
        [Test]
        public void ParsingInputTestNormal()
        {
            ExpressionTree tree = new ExpressionTree("Hello+World");
            tree.ParsingInput();
            Assert.That(tree.Tokens[0], Is.EqualTo("Hello"));
            Assert.That(tree.Tokens[1], Is.EqualTo("+"));
            Assert.That(tree.Tokens[2], Is.EqualTo("World"));

            Assert.That(tree.Variables.ContainsKey("Hello"), Is.True);
            Assert.That(tree.Variables["Hello"], Is.EqualTo(0.0.ToString()));

            Assert.That(tree.Variables.ContainsKey("World"), Is.True);
            Assert.That(tree.Variables["World"], Is.EqualTo(0.0.ToString()));
        }

        /// <summary>
        /// This tests SetVariable method with normal cases.
        /// </summary>
        [Test]
        public void SetVariableTestNormal()
        {
            ExpressionTree tree = new ExpressionTree("Hello+World");
            tree.ParsingInput();
            tree.SetVariable("Hello", 20.0.ToString());
            tree.SetVariable("World", 50.0.ToString());

            Assert.That(tree.Variables.ContainsKey("Hello"), Is.True); // check if the key exists.
            Assert.That(tree.Variables["Hello"], Is.EqualTo(20.0.ToString()));

            Assert.That(tree.Variables.ContainsKey("World"), Is.True); // check if the key exists.
            Assert.That(tree.Variables["World"], Is.EqualTo(50.0.ToString()));
        }

        /// <summary>
        /// This tests SetVariable method with an exceptional case.
        /// </summary>
        [Test]
        public void SetVariableTestExceptional()
        {
            ExpressionTree tree = new ExpressionTree("Hello+World");
            tree.ParsingInput();
            Assert.Throws<KeyNotFoundException>(() => tree.SetVariable("NotExist!", 10.0.ToString()));
        }

        /// <summary>
        /// This tests if Evaluate method from ExpressionTree class works properly with a normal variable test case.
        /// </summary>
        [Test]
        public void TreeEvaluateTestNormalVariable()
        {
            ExpressionTree tree = new ExpressionTree("Hello+World");
            tree.ParsingInput();
            tree.BuildTree();
            tree.SetVariable("Hello", 20.0.ToString());
            tree.SetVariable("World", 50.0.ToString());

            double testResult = tree.Evaluate();
            Assert.That(testResult, Is.EqualTo(70.0));
        }

        /// <summary>
        /// This tests if Evaluate method from ExpressionTree class works properly with a normal numeric test case.
        /// </summary>
        [Test]
        public void TreeEvaluateTestNormalNumeric()
        {
            ExpressionTree tree = new ExpressionTree("1.0+2.0");
            tree.ParsingInput();
            tree.BuildTree();
            double testResult = tree.Evaluate();
            Assert.That(testResult, Is.EqualTo(3.0));
        }

        /// <summary>
        /// Tests if it's the left partenthesis.
        /// </summary>
        [Test]
        public void IsLeftParenthesisTest()
        {
            Assert.That(OperatorNodeFactory.IsLeftParenthesis("("), Is.True);
            Assert.That(OperatorNodeFactory.IsLeftParenthesis("+"), Is.False);
        }

        /// <summary>
        /// Tests if it's the right partenthesis.
        /// </summary>
        [Test]
        public void IsRightParenthesisTest()
        {
            Assert.That(OperatorNodeFactory.IsRightParenthesis(")"), Is.True);
            Assert.That(OperatorNodeFactory.IsRightParenthesis("+"), Is.False);
        }

        /// <summary>
        /// Tests ConvertInfixToPostfix method.
        /// </summary>
        [Test]
        public void ConvertInfixToPostfixTestNormal()
        {
            List<string> tokens = new List<string> { "2", "+", "3", "+", "5" };
            Assert.That(ShuntingYard.ConvertInfixToPostfix(tokens), Is.EqualTo("2 3 + 5 + "));
        }

        /// <summary>
        /// Tests ConvertInfixToPostfix method with parenthesis.
        /// </summary>
        [Test]
        public void ConvertInfixToPostfixTestParenthesis()
        {
            List<string> tokens = new List<string> { "2", "*", "(", "3", "+", "5", ")" };
            Assert.That(ShuntingYard.ConvertInfixToPostfix(tokens), Is.EqualTo("2 3 5 + * "));
        }

        /// <summary>
        /// Tests ConvertInfixToPostfix method with exceptional cass.
        /// </summary>
        [Test]
        public void ConvertInfixToPostfixTestExceptional()
        {
            List<string> tokens = new List<string> { "(", "(" };
            Assert.Throws<Exception>(() => ShuntingYard.ConvertInfixToPostfix(tokens));

            List<string> anotherTokens = new List<string> { ")", ")" };
            Assert.Throws<Exception>(() => ShuntingYard.ConvertInfixToPostfix(anotherTokens));
        }

        /// <summary>
        /// Tests if it's a binary operator or not for Factory class.
        /// </summary>
        [Test]
        public void IsOperatorSymbolTest()
        {
            Assert.That(OperatorNodeFactory.IsOperatorSymbol("+"), Is.True);
            Assert.That(OperatorNodeFactory.IsOperatorSymbol("-"), Is.True);
            Assert.That(OperatorNodeFactory.IsOperatorSymbol("*"), Is.True);
            Assert.That(OperatorNodeFactory.IsOperatorSymbol("/"), Is.True);
            Assert.That(OperatorNodeFactory.IsOperatorSymbol("("), Is.True);
            Assert.That(OperatorNodeFactory.IsOperatorSymbol(")"), Is.True);
        }

        /// <summary>
        /// Tests if it's numeric or not.
        /// </summary>
        [Test]
        public void IsNumericTest()
        {
            Assert.That(NodeFactory.IsNumeric("12.23"), Is.True);
            Assert.That(NodeFactory.IsNumeric("-12.23"), Is.True);
            Assert.That(NodeFactory.IsNumeric("0"), Is.True);
            Assert.That(NodeFactory.IsNumeric("A1"), Is.False);
            Assert.That(NodeFactory.IsNumeric("1A"), Is.False);
            Assert.That(NodeFactory.IsNumeric("Hello"), Is.False);
        }

        /// <summary>
        /// Tests if it's a variable or not.
        /// </summary>
        [Test]
        public void IsVariableTest()
        {
            Assert.That(NodeFactory.IsVariable("A1"), Is.True);
            Assert.That(NodeFactory.IsVariable("1A"), Is.True);
            Assert.That(NodeFactory.IsVariable("Hello"), Is.True);
            Assert.That(NodeFactory.IsVariable("12.23"), Is.False);
            Assert.That(NodeFactory.IsVariable("-12.23"), Is.False);
            Assert.That(NodeFactory.IsVariable("0"), Is.False);
            Assert.That(NodeFactory.IsVariable("1.0"), Is.False);
            Assert.That(NodeFactory.IsVariable("-1.12"), Is.False);
        }

        /// <summary>
        /// Tests if it creates the right type of node.
        /// </summary>
        [Test]
        public void CreateNodeTest()
        {
            Assert.That(NodeFactory.CreateNode("A1"), Is.InstanceOf<VariableNode>());
            Assert.That(NodeFactory.CreateNode("1.23"), Is.InstanceOf<ConstNumericNode>());
            Assert.That(NodeFactory.CreateNode("+"), Is.InstanceOf<BinaryOperatorNode>());
        }
    }
}