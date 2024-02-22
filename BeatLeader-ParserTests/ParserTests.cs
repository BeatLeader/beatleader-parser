using Microsoft.VisualStudio.TestTools.UnitTesting;
using beatleader_parser;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace beatleader_parser.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void LoadPathTest()
        {
            Parser parser = new Parser();

            var result = parser.LoadPath("C:\\SteamLibrary\\steamapps\\common\\Beat Saber\\Beat Saber_Data\\CustomWIPLevels\\HERO");

            Console.WriteLine(JsonConvert.SerializeObject(result));
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadInfoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoadDifficultiesFromPathTest()
        {
            Assert.Fail();
        }
    }
}