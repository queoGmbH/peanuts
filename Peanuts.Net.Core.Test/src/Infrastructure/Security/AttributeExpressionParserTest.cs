using System;

using FluentAssertions;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Security {
    [TestFixture]
    public class AttributeExpressionParserTest {

        [Test]
        public void TestStandardText() {
            string textToParse = "test_{12345.test}_update";
            // when:
            string actualExpression = AttributeExpressionParser.Parse(textToParse);
            // then:
            actualExpression.ShouldBeEquivalentTo("12345.test");
        }

        [Test]
        public void TestOnlyExpressionTest() {
            string textToParse = "{test.property1}";
            // when:
            string actualExpression = AttributeExpressionParser.Parse(textToParse);
            // then:
            actualExpression.ShouldBeEquivalentTo("test.property1");
        }

        [Test]
        public void TestParseOnlyOpeningBracket() {
            string textToParse = "user_{test.property1_read";
            // when:then:
            Assert.Throws<InvalidOperationException>(()=>AttributeExpressionParser.Parse(textToParse));
        }

        [Test]
        public void TestParseOnlyClosingBracket() {
            string textToParse = "test.property}_update";
            // when, then:
            Assert.Throws<InvalidOperationException>(() => AttributeExpressionParser.Parse(textToParse));
        }

        [Test]
        public void TestIsExpressionTrue() {
            string testText = "eins{zwei}drei";
            // when:
            bool isExpression = AttributeExpressionParser.IsExpression(testText);
            // then 
            isExpression.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void TestIsExpressionOnlyOpeningBracket() {
            string testText = "eins{zweidrei";
            // when:
            bool isExpression = AttributeExpressionParser.IsExpression(testText);
            // then:
            isExpression.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void TestIsExpressionOnlyClosingBracket() {
            string testText = "einszwei}drei";
            // when:
            bool isExpression = AttributeExpressionParser.IsExpression(testText);
            // then:
            isExpression.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void TestIsExpressionBracketsPermuted() {
            string testText = "eins}zwei{drei";
            // when:
            bool isExpression = AttributeExpressionParser.IsExpression(testText);
            // then:
            isExpression.ShouldBeEquivalentTo(false);
        }
    }
}