﻿namespace Caribou.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Caribou.Data;
    using Caribou.Processing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestsWayParsingSimple : SimpleCase
    {
        const int allCrafts = 2;
        const int allAmenities = 0;
        const int allBuildings = 1;

        const int allAmenitiesRestaurants = 0;
        const int allCraftJewellers = 1;
        const int allBuildingsRetail = 0;

        private static int CountWaysFoundForKey(RequestHandler matches, string key)
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
            var matches = ParseViaXMLDocument.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountWaysFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        //[TestMethod]
        public void ParseWaysGivenKeyViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountWaysFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(mainFeatures, simpleFile);
            Assert.AreEqual(allCrafts, CountWaysFoundForKey(matches, "craft"));
            Assert.AreEqual(allAmenities, CountWaysFoundForKey(matches, "amenity"));
            Assert.AreEqual(allBuildings, CountWaysFoundForKey(matches, "building"));
        }

        [TestMethod]
        public void ParseWaysGivenKeyValueViaXMLReader()
        {
            var matches = ParseViaXMLReader.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allCraftJewellers, matches.Ways["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);
        }

        //[TestMethod]
        public void ParseWaysGivenKeyValueViaXMLDocument()
        {
            var matches = ParseViaXMLDocument.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allCraftJewellers, matches.Ways["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);
        }

        //[TestMethod]
        public void ParseWaysGivenKeyValueViaLinq()
        {
            var matches = ParseViaLinq.FindByFeatures(miscSubFeatures, simpleFile);
            Assert.AreEqual(allAmenitiesRestaurants, matches.Ways["amenity"]["restaurant"].Count);
            Assert.AreEqual(allCraftJewellers, matches.Ways["craft"]["jeweller"].Count);
            Assert.AreEqual(allBuildingsRetail, matches.Ways["building"]["retail"].Count);
        }
    }
}
