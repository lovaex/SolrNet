using System;
using System.Collections.Generic;
using NUnit.Framework;
using SolrNet.Impl.FieldParsers;

namespace SolrNet.Tests
{
    [TestFixture]
    public class LocationTests
    {
        [TestCase(12, 23, "12,23")]
        [TestCase(-4.3, 0.20, "-4.3,0.2")]
        public void ParseLocationTests(double latitude, double longitude, string toString)
        {
            var location = new Location(latitude, longitude);
            var parsedLocation = LocationFieldParser.Parse(toString);
            Assert.AreEqual(toString, parsedLocation.ToString());
            Assert.AreEqual(location, parsedLocation);
            Assert.AreEqual(location.Latitude, parsedLocation.Latitude);
            Assert.AreEqual(location.Longitude, parsedLocation.Longitude);
        }


        [TestCase(-100)]
        [TestCase(120)]
        public void InvalidLatitudeTests(double latitude)
        {
            Assert.IsFalse(Location.IsValidLatitude(latitude));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Location(latitude, 0);
            });
        }


        [TestCase(-200)]
        [TestCase(999)]
        public void InvalidLongitudeTests(double longitude)
        {
            Assert.IsFalse(Location.IsValidLongitude(longitude));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Location(0, longitude);
            });
        }

        [TestCase(-100, -200)]
        [TestCase(-100, 999)]
        [TestCase(120, -200)]
        [TestCase(120, 999)]
        public void TryCreateReturnsNullWithInvalidLatLong(double latitude, double longitude)
        {
            Assert.IsNull(Location.TryCreate(latitude, longitude));
        }

        [TestCase(12, 23)]
        [TestCase(-4.3, 0.20)]
        public void TryCreateReturnsNotNullWithValidLatLong(double latitude, double longitude)
        {
            var loc = new Location(latitude, longitude);
            var loc2 = Location.TryCreate(loc.Latitude, loc.Longitude);
            Assert.IsNotNull(loc2);
            Assert.AreEqual(loc, loc2);
        }
    }
}