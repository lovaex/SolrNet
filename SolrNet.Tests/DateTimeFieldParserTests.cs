#region license

// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SolrNet.Impl.FieldParsers;
using SolrNet.Impl.FieldSerializers;
using SolrNet.Utils;

namespace SolrNet.Tests
{
    public class DateTimeFieldParserTests
    {

        [TestCase(1, 1, 1, 0, 0, 0)]
        [TestCase(2004, 11, 1, 0, 0, 0)]
        [TestCase(2012, 5, 10, 14, 17, 23, 684)]
        [TestCase(2012, 5, 10, 14, 17, 23, 680)]
        [TestCase(2012, 5, 10, 14, 17, 23, 600)]
        public void ParseYears(int year, int month, int day, int hour, int minute, int second, int milliseconds = 0)
        {
            var date = new DateTime(year, month, day, hour, minute, second, milliseconds, DateTimeKind.Utc);
            var dateTime = DateTimeFieldParser.ParseDate(date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            Assert.AreEqual(date, dateTime);
            Assert.AreEqual(date.Kind, dateTime.Kind);
        }

        [TestCase(1, 1, 1, 0, 0, 0)]
        [TestCase(2004, 11, 1, 15, 41, 23)]
        [TestCase(2012, 5, 10, 14, 17, 23, 684)]
        public void RoundTripUtcDateTime(int year, int month, int day, int hour, int minute, int second, int milliseconds = 0)
        {
            var date = new DateTime(year, month, day, hour, minute, second, milliseconds, DateTimeKind.Utc);
            var value = DateTimeFieldParser.ParseDate(DateTimeFieldSerializer.SerializeDate(date));
            Assert.AreEqual(date, value);
            Assert.AreEqual(date.Kind, value.Kind);
        }

        [TestCase(1, 1, 1, 0, 0, 0)]
        [TestCase(2004, 11, 1, 15, 41, 23)]
        [TestCase(2012, 5, 10, 14, 17, 23, 684)]
        public void NullableRoundTrips(int year, int month, int day, int hour, int minute, int second, int milliseconds = 0)
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second, milliseconds, DateTimeKind.Utc);
            var xml = new XDocument();
            xml.Add(new XElement("date", new NullableFieldSerializer(new DateTimeFieldSerializer()).Serialize(dateTime).First().FieldValue));
            var value = (DateTime?)new NullableFieldParser(new DateTimeFieldParser()).Parse(xml.Root, typeof(DateTime?));
            Assert.AreEqual(dateTime, value);
            Assert.AreEqual(dateTime.Kind, value.Value.Kind);
        }
    }
}