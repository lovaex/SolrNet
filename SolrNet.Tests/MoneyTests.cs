using System.Collections.Generic;
using NUnit.Framework;
using SolrNet.Impl.FieldParsers;

namespace SolrNet.Tests {
    [TestFixture]
    public class MoneyTests {
        private static IEnumerable<TestCaseData> Moneys
        {
            get
            {
                yield return new TestCaseData(new {money = new Money(12, null), toString = "12",});
                yield return new TestCaseData(new {money = new Money(12.45m, "USD"), toString = "12.45,USD",});
                yield return new TestCaseData(new {money = new Money(52.66m, "EUR"), toString = "52.66,EUR",});
            }
        }

        [TestCaseSource(nameof(Moneys))]
        public void ParseMoneyTest(dynamic money) {
            var parsedMoney = MoneyFieldParser.Parse(money.toString);
            Assert.AreEqual(money.toString, money.money.ToString());
            Assert.AreEqual(money.money.Currency, parsedMoney.Currency);
            Assert.AreEqual(money.money.Value, parsedMoney.Value);
        }
    }
}