using System;
using System.Globalization;

using NUnit.Framework;

namespace Com.QueoFlow.Peanuts.Net.Core.Infrastructure.ResourceManagement {
    [TestFixture]
    public class TemplaterTest {
        /// <summary>
        /// Testet das Ersetzen eines simplen Templates
        /// </summary>
        [Test]
        public void TestSimpleTemplate() {

            /* Given: Ein einfacher String mit einem Platzhalter */
            const string TEMPLATE = "Das ist ein {value} Template";
            const string PLACE_HOLDER_VALUE = "einfaches";
            string expectedString = TEMPLATE.Replace("{value}", PLACE_HOLDER_VALUE);
            ModelMap model = new ModelMap();
            model.Add("value", PLACE_HOLDER_VALUE);
            /* When: Das Template mit dem Platzhalter gefüllt werden soll */
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Muss der Platzhalter korrekt ersetzt werden. */
            Assert.AreEqual(expectedString, result);
        }

        /// <summary>
        /// Testet das Ersetzen in einem Template, bei dem ein Platzhalter auf ein Child-Property eines Child-Property eines Child-Property ... zugreift.
        /// </summary>
        [Test]
        public void TestDeepPathTemplate() {

            /* Given: Ein Template mit einem 3-teiligen Platzhalter-Pfad */
            const string TEMPLATE = "Das ist ein {user.name.firstname} Template";
            const string PLACE_HOLDER_VALUE = "Tobias";
            string expectedString = TEMPLATE.Replace("{user.name.firstname}", PLACE_HOLDER_VALUE);
            
            /* When: Das Template gefüllt werden soll */
            var name = new { firstname = PLACE_HOLDER_VALUE };
            // ReSharper disable once RedundantAnonymousTypePropertyName : Readability
            var user = new { name = name };

            ModelMap model = new ModelMap();
            model.Add("user", user);
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Müssen die Platzhalter korrekt ersetzt werden. */
            Assert.AreEqual(expectedString, result);

        }

        /// <summary>
        /// Testet das Ersetzen in einem Template, bei dem mehrere Platzhalter auf verschiedene ebenen des Models zugreifen.
        /// </summary>
        [Test]
        public void TestMultiplePlaceholdersOfDifferentDepth() {

            /* Given: Ein Template mit einem 3-teiligen Platzhalter-Pfad */
            const string TEMPLATE = "Mein Name ist {user.name.firstname} {user.name.lastname}. Ich habe im Jahr {application.year} eine tolle Software namens {application.name} entwickelt.";

            const string FIRST_NAME_VALUE = "Tobias";
            const string LAST_NAME_VALUE = "Jäkel";

            const string APPLICATION_YEAR_VALUE = "2000";
            const string APPLICATION_NAME_VALUE = "Bubble Sort";

            string expectedString = TEMPLATE;
            expectedString = expectedString.Replace("{user.name.firstname}", FIRST_NAME_VALUE);
            expectedString = expectedString.Replace("{user.name.lastname}", LAST_NAME_VALUE);
            expectedString = expectedString.Replace("{application.year}", APPLICATION_YEAR_VALUE);
            expectedString = expectedString.Replace("{application.name}", APPLICATION_NAME_VALUE);
            
            /* When: Das Template gefüllt werden soll */
            var name = new {
                firstname = FIRST_NAME_VALUE,
                lastname = LAST_NAME_VALUE
            };
            var user = new {
                // ReSharper disable once RedundantAnonymousTypePropertyName : Readability
                name = name
            };

            var app = new { name = APPLICATION_NAME_VALUE, year = APPLICATION_YEAR_VALUE };

            ModelMap model = new ModelMap();
            model.Add("user", user);
            model.Add("application", app);
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Müssen die Platzhalter korrekt ersetzt werden. */
            Assert.AreEqual(expectedString, result);

        }

        /// <summary>
        /// Testet das ersetzen eines mehrfach vorkommenden Platzhalters.
        /// </summary>
        [Test]
        public void TestReplaceTwice() {

            /* Given: Ein Template in dem ein Platzhalter mehrfach vorkommt. */
            const string TEMPLATE = "{hello}, {hello}, {hello}. Wie geht's?";
            const string HELLO_VALUE = "Hi";

            string expected = TEMPLATE.Replace("{hello}", HELLO_VALUE);
            ModelMap model = new ModelMap();
            model.Add("hello", HELLO_VALUE);
            /* When: Die Platzhalter ersetzt werden sollen */
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Müssen alle Platzhalter ersetzt werden. */
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Testet das formatierte Ersetzen eines POlatzhalters
        /// </summary>
        [Test]
        public void TestFormat() {

            /* Given: Ein Template, in welches ein Datum eingefügt werden soll. */
            const string TEMPLATE = "Heute ist der {today:d}";
            DateTime now = DateTime.Now;
            string expected = TEMPLATE.Replace("{today:d}", now.ToString("d"));
            ModelMap model = new ModelMap();
            model.Add("today", now);
            /* When: Die Platzhalter ersetzt werden sollen */
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Muss das korrekte Datum im richtigen Format eingefügt wurden sein. */
            Assert.AreEqual(expected, result);

        }

        /// <summary>
        /// Testet das kulturabhängige formatierte Ersetzen eines Platzhalters. 
        /// </summary>
        [Test]
        public void TestFormatCulture() {

            /* Given: Eine Zeichenfolge, in welches ein Datum im englischen Format eingefügt werden soll. */
            const string TEMPLATE = "Heute ist der {today:d}";
            DateTime now = DateTime.Now;
            CultureInfo englishCulture = CultureInfo.GetCultureInfo("en");
            string expected = TEMPLATE.Replace("{today:d}", now.ToString("d", englishCulture));
            ModelMap model = new ModelMap();
            model.Add("today", now);
            /* When: Die Platzhalter ersetzt werden sollen */
            string result = new Templater(englishCulture).FormatTemplate(TEMPLATE, model);

            /* Then: Muss das Datum korrekt im englischen Datumsformat eingefügt sein. */
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Testet das formatieren des Templates, wenn der Pfad im Model nicht gefunden wird.
        /// </summary>
        [Test]
        public void TestFormatPathNotFoundDefaultNull() {

            /* Given: Ein Template mit zwei Platzhaltern, von denen im Model nur ein Pfad gefunden werden kann. */
            const string TEMPLATE = "Heute ist der {today:d}. Weltuntergang ist am {doomsday:d}";
            DateTime now = DateTime.Now;
            string expected = TEMPLATE.Replace("{today:d}", now.ToString("d"));
            ModelMap model = new ModelMap();
            model.Add("today", now);
            /* When: Das Template ersetzt werden soll */
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Muss das gefundene ersetzt werden und das andere so belassen werden. */
            Assert.AreEqual(expected, result);

        }

        /// <summary>
        /// Testet das formatieren des Templates, wenn der Pfad im Model nicht gefunden wird.
        /// </summary>
        [Test]
        public void TestFormatPathNotFoundDefaultStringEmpty() {

            /* Given: Ein Template mit zwei Platzhaltern, von denen im Model nur ein Pfad gefunden werden kann. */
            const string TEMPLATE = "Heute ist der {today:d}. Weltuntergang ist am {doomsday:d}";
            DateTime now = DateTime.Now;
            string defaultValue = string.Empty;
            string expected = TEMPLATE.Replace("{today:d}", now.ToString("d")).Replace("{doomsday:d}", defaultValue);
            ModelMap model = new ModelMap();
            model.Add("today", now);
            /* When: Das Template ersetzt werden soll */
            string result = new Templater(defaultValue).FormatTemplate(TEMPLATE, model);

            /* Then: Muss das gefundene mit dem Datum und das andere mit string.Empty ersetzt werden . */
            Assert.AreEqual(expected, result);

        }

        /// <summary>
        /// Testet das formatieren des Templates, wenn der Pfad im Model nicht gefunden wird und in diesem Fall ein beliebiger String dafür eingesetzt werden soll.
        /// </summary>
        [Test]
        public void TestFormatPathNotFoundDefaultCustomString() {

            /* Given: Ein Template mit zwei Platzhaltern, von denen im Model nur ein Pfad gefunden werden kann. */
            const string TEMPLATE = "Heute ist der {today:d}. Weltuntergang ist am {doomsday:d}";
            DateTime now = DateTime.Now;
            const string DEFAULT_VALUE = "!!!Nicht gefunden!!!";
            string expected = TEMPLATE.Replace("{today:d}", now.ToString("d")).Replace("{doomsday:d}", DEFAULT_VALUE);
            ModelMap model = new ModelMap();
            model.Add("today", now);
            /* When: Das Template ersetzt werden soll */
            string result = new Templater(DEFAULT_VALUE).FormatTemplate(TEMPLATE, model);

            /* Then: Muss das gefundene mit dem Datum und das andere mit dem Wert in DEFAULT_VALUE ersetzt werden . */
            Assert.AreEqual(expected, result);
        }

                /// <summary>
        /// Testet das Ersetzen eines simplen Templates
        /// </summary>
        [Test]
        public void TestSimpleTemplateWithModelMap() {

            /* Given: Ein einfacher String mit einem Platzhalter */
            const string TEMPLATE = "Das ist ein {value} Template";
            const string PLACE_HOLDER_VALUE = "einfaches";
            string expectedString = TEMPLATE.Replace("{value}", PLACE_HOLDER_VALUE);

            /* When: Das Template mit dem Platzhalter gefüllt werden soll */
            ModelMap model = new ModelMap();
            model.Add("value", PLACE_HOLDER_VALUE);
            string result = new Templater().FormatTemplate(TEMPLATE, model);

            /* Then: Muss der Platzhalter korrekt ersetzt werden. */
            Assert.AreEqual(expectedString, result);
        }
    }
}