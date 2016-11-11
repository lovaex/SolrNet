using System;
using System.Collections.Generic;
using NUnit.Framework;
using SolrNet.Impl.FieldParsers;

namespace SolrNet.Tests {
    [TestFixture]
    public class LocationTests {
        private static IEnumerable<TestCaseData> Locations
        {
            get
            {
                yield return new TestCaseData(new {location = new Location(12, 23), toString = "12,23"});
                yield return new TestCaseData(new {location = new Location(-4.3, 0.20), toString = "-4.3,0.2"});
            }
        }

        private static IEnumerable<TestCaseData> InvalidLatitudes
        {
            get
            {
                yield return new TestCaseData(new {latitude = -100});
                yield return new TestCaseData(new {latitude = 120});
            }
        }

        private static IEnumerable<TestCaseData> InvalidLongitudes
        {
            get
            {
                yield return new TestCaseData(new {longitude = -200});
                yield return new TestCaseData(new {longitude = 999});
            }
        }

        private static IEnumerable<dynamic> LatitudeLongitude
        {
            get
            {
                yield return new TestCaseData(new {latitude = -100, longitude = -200});
                yield return new TestCaseData(new {latitude = -100, longitude = 999});
                yield return new TestCaseData(new {latitude = 120, longitude = -200});
                yield return new TestCaseData(new {latitude = 120, longitude = 999});
            }
        }

        [TestCaseSource(nameof(Locations))]
        public void ParseLocationTests(dynamic location) {
            var parsedLocation = LocationFieldParser.Parse(location.toString);
            Assert.AreEqual(location.toString, location.location.ToString());
            Assert.AreEqual(location.location, parsedLocation);
            Assert.AreEqual(location.location.Latitude, parsedLocation.Latitude);
            Assert.AreEqual(location.location.Longitude, parsedLocation.Longitude);
        }

        [TestCaseSource(nameof(InvalidLatitudes))]
        public void InvalidLatitudeTests(dynamic latitude) {
            Assert.IsFalse(Location.IsValidLatitude(latitude.latitude));
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Location(latitude.latitude, 0);
            });
        }

        [TestCaseSource(nameof(InvalidLongitudes))]
        public void InvalidLongitudeTests(dynamic longitude) {
            Assert.IsFalse(Location.IsValidLongitude(longitude.longitude));
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new Location(0, longitude.longitude);
            });
        }

        [TestCaseSource(nameof(LatitudeLongitude))]
        public void TryCreateReturnsNullWithInvalidLatLong(dynamic latitudeLongitude) {
            var lat = latitudeLongitude.latitude;
            var lng = latitudeLongitude.longitude;
            var loc = Location.TryCreate(lat, lng);
            Assert.IsNull(loc);
        }

        [TestCaseSource(nameof(Locations))]
        public void TryCreateReturnsNotNullWithValidLatLong(dynamic location) {
            var loc = location.location;
            var loc2 = Location.TryCreate(loc.Latitude, loc.Longitude);
            Assert.IsNotNull(loc2);
            Assert.AreEqual(loc, loc2);
        }
    }
}