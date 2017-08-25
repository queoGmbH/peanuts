using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Web.Infrastructure.Security {
    [TestFixture]
    public class PreAuthorizeAttributeTest {

        [Test]
        public void TestFindMethodName() {
            string actionExpression = "HasRightInInstitute(#user, 'parameterwert')";
            string expectedMethodName = "HasRightInInstitute";

            string methodeName = PreAuthorizeAttribute.ParseMethodeName(actionExpression);
            Assert.AreEqual(expectedMethodName, methodeName);
        }

        [Test]
        public void TestToShortMethodName() {
            string actionExpression = "a(#uhuhuhuhuh, #sdsdsd)";
            Assert.Throws<InvalidOperationException>(()=>PreAuthorizeAttribute.ParseMethodeName(actionExpression));
        }

        [Test]
        public void TestParseMethodNameWithoutParams() {
            string actionExpression = "hasRight";
            Assert.Throws<InvalidOperationException>(()=>PreAuthorizeAttribute.ParseMethodeName(actionExpression));
        }

        [Test]
        public void TestGetParameterBlock() {
            string actionExpression = "HasRightInInstitute(#user, 'parameterwert')";
            string expectedParameterBlock = "#user, 'parameterwert'";
            string parameterBlock = PreAuthorizeAttribute.GetParameterBlock(actionExpression);

            Assert.AreEqual(expectedParameterBlock, parameterBlock);
        }

        [Test]
        public void TestGetMethodeParameters() {
            string actionExpression = "HasRightInInstitute(#user, 'parameterwert')";
            string parameterBlock = PreAuthorizeAttribute.GetParameterBlock(actionExpression);
            TestClass expectedMethodParameter = new TestClass("objekt1");
            Dictionary<string, object> callingParameters = new Dictionary<string, object>();
            callingParameters.Add("user", expectedMethodParameter);
            callingParameters.Add("otherParam", new TestClass("wrongObject"));

            List<MethodParameter> expectedMethodParameters = new List<MethodParameter>();
            expectedMethodParameters.Add(new MethodParameter() { ParameterName = "user", ParameterType = typeof(TestClass), ParameterValue = expectedMethodParameter });
            expectedMethodParameters.Add(new MethodParameter() { ParameterType = typeof(string), ParameterValue = "parameterwert" });


            IList<MethodParameter> methodeParameters = PreAuthorizeAttribute.GetMethodParameters(parameterBlock, callingParameters);
            CollectionAssert.AreEqual(expectedMethodParameters, methodeParameters.ToList());
        }

        [Test]
        public void TestGetMethodeParametersFailingParameter() {
            string actionExpression = "HasRightInInstitute(#user, #institute, 'parameterwert')";
            string parameterBlock = PreAuthorizeAttribute.GetParameterBlock(actionExpression);
            TestClass expectedMethodParameter = new TestClass("objekt1");
            Dictionary<string, object> callingParameters = new Dictionary<string, object>();
            callingParameters.Add("user", expectedMethodParameter);
            callingParameters.Add("otherParam", new TestClass("wrongObject"));

            Assert.Throws<KeyNotFoundException>(()=>PreAuthorizeAttribute.GetMethodParameters(parameterBlock, callingParameters));
        }

        [Test]
        public void TestPropertyPath() {
            string actionExpression = "HasRightInInstitute(#user.Name)";
            string parameterBlock = PreAuthorizeAttribute.GetParameterBlock(actionExpression);
            TestClass expectedMethodParameter = new TestClass("objekt1");
            Dictionary<string, object> callingParameters = new Dictionary<string, object>();
            callingParameters.Add("user", expectedMethodParameter);
            callingParameters.Add("otherParam", new TestClass("wrongObject"));

            List<MethodParameter> expectedMethodParameters = new List<MethodParameter>();
            expectedMethodParameters.Add(new MethodParameter() { ParameterName = "user.Name", ParameterType = typeof(string), ParameterValue = "objekt1" });

            IList<MethodParameter> methodeParameters = PreAuthorizeAttribute.GetMethodParameters(parameterBlock, callingParameters);
            CollectionAssert.AreEqual(expectedMethodParameters, methodeParameters.ToList());
        }

        internal class TestClass {
            private readonly string _name;

            public TestClass(string name) {
                _name = name;
            }

            public string Name {
                get { return _name; }
            }
        }
    }
}