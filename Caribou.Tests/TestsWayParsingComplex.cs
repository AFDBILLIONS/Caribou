﻿namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsingComplex : MelbourneCase
    {
        const int allHighways = 612;
        const int allAmenities = 45;
        const int allBuildings = 466;
        const int allAmenitiesRestaurants = 0;
        const int allAmenitiesWorship = 6;
        const int allHighwaysResidential = 5;
        const int allBuildingsRetail = 19;
        private double firstAmenityWorshipFirstNodeLat = -37.8164641;
        private double firstAmenityWorksipLastNodeLat = -37.8165976; // Actually 2nd to last as closed
        private double firstHighwayResidentialFirstNodeLon = 144.9735701;
        private double firstHighwayResidentialLastNodeLon = 144.9659697; // Actually last as not closed

        private int CountWaysFoundForKey(RequestResults matches, string key)
        {
            var matchesCount = 0;
            foreach (var matchedSubFeature in matches.Ways[key].Keys)
            {
                matchesCount += matches.Ways[key][matchedSubFeature].Count;
            }
            return matchesCount;
        }

        //[TestMethod]
        public void ParseWaysGivenKeyViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        //[TestMethod]
        public void ParseWaysGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(mainFeatures, melbourneFile);
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allHighways, CountWaysFoundForKey(matches, "highway"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);

            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            var itemCountA = matches.Ways["amenity"]["place_of_worship"][0].Length;
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 2].Latitude);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            var itemCountB = matches.Ways["highway"]["residential"][0].Length;
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][0].Longitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 1].Longitude);

        }

        //[TestMethod]
        public void ParseWaysGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);

            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            var itemCountA = matches.Ways["amenity"]["place_of_worship"][0].Length;
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 2].Latitude);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            var itemCountB = matches.Ways["highway"]["residential"][0].Length;
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][1].Latitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 2].Latitude);
        }

        //[TestMethod]
        public void ParseWaysGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, melbourneFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allAmenitiesWorship, matches.Ways["amenity"]["place_of_worship"].Count);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);

            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][0].Latitude);
            var itemCountA = matches.Ways["amenity"]["place_of_worship"][0].Length;
            Assert.AreEqual(firstAmenityWorshipFirstNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 1].Latitude);
            Assert.AreEqual(firstAmenityWorksipLastNodeLat, matches.Ways["amenity"]["place_of_worship"][0][itemCountA - 2].Latitude);
            Assert.AreEqual(allHighwaysResidential, matches.Ways["highway"]["residential"].Count);
            var itemCountB = matches.Ways["highway"]["residential"][0].Length;
            Assert.AreEqual(firstHighwayResidentialFirstNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 1].Latitude);
            Assert.AreEqual(firstHighwayResidentialLastNodeLon, matches.Ways["highway"]["residential"][0][itemCountB - 2].Latitude);
        }
    }
}
