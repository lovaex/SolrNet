using System.Collections.Generic;
using NUnit.Framework;
using SolrNet.Impl.FieldParsers;

namespace SolrNet.Tests
{
    [TestFixture]
    public class MoneyTests
    {
        [TestCase(12, null, "12")]
        [TestCase(12.45, "USD", "12.45,USD")]
        [TestCase(52.66, "EUR", "52.66,EUR")]
        public void ParseMoneyTest(decimal value, string currency, string toString)
        {
            var money = new Money(value, currency);
            var parsedMoney = MoneyFieldParser.Parse(toString);
            Assert.AreEqual(toString, money.ToString());
            Assert.AreEqual(money.Currency, parsedMoney.Currency);
            Assert.AreEqual(money.Value, parsedMoney.Value);
        }
    }
}