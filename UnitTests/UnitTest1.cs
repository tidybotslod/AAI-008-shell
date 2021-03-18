using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;

using AAI;

namespace Personalizer
{
    [TestClass]
    public class Tests
    {

#if TestService

        [TestMethod]
        public void TestService()
        {
            IConfiguration config; // Load configuration data found in appsettings.json, need Azure authoring key and resource name to build URL to azure.
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            PersonalizerService personalizer = new PersonalizerService(
                GetConfigString(config, "PersonalizerEndpointKey"),
                GetConfigString(config, "PersonalizerResourceName"));
            RunTasteTest(personalizer);
        }
#endif
#if TestCreate

        [TestMethod]
        public void TestCreate()
        {
            Program program = new Program();
            RunTasteTest(program.Personalizer);
        }
#endif
#if TestFeatures
        [TestMethod]
        public void TestFeatures()
        {
            Program program = new Program();
            program.LoadFeatures(@"D:\LabFiles\AAI-008\Data\Features.json");
            Assert.AreEqual(5, program.Personalizer.Features.Length);
        }
#endif
#if TestActions
        [TestMethod]
        public void TestActions()
        {
            Program program = new Program();
            program.LoadActions(@"D:\LabFiles\AAI-008\Data\Actions.json");
            Assert.AreEqual(4, program.Personalizer.Actions.Count);

        }
#endif
#if TestTraining
        [TestMethod]
        public void TestTraining()
        {
            TrainingCase[] simple = new TrainingCase[]
            {
                new TrainingCase
                {
                    Name = "SimpleCase",
                    Features = new object[] { new { Location = "Bedroom", Color = "Pastel"} },
                    Exclude = new string[] { "Comfortable Sample" },
                    Expected = "Sleepy Sample"
                }
            };

            Program program = new Program();
            program.Personalizer.Train(simple);
            // Ensures no exceptions are thrown.
        }
#endif
#if TestTrainingFile
        [TestMethod]
        public void TestTrainingFile()
        {
            Program program = new Program();
            program.TrainingFile(@"D:\LabFiles\AAI-008\Data\Training.json");
            // Ensures no exceptions are thrown.
        }
#endif
        private static string GetConfigString(IConfiguration config, string key)
        {
            string result = config[key];
            if (result != null && result.Length == 0)
            {
                result = null;
            }
            return result;
        }

        private static void RunTasteTest(PersonalizerService personalizer)
        {
            IList<RankableAction> actions = new List<RankableAction>
            {
                new RankableAction
                {
                    Id = "pasta",
                    Features =
                    new List<object>() { new { taste = "salty", spiceLevel = "medium" }, new { nutritionLevel = 5, cuisine = "italian" } }
                },

                new RankableAction
                {
                    Id = "salad",
                    Features =
                    new List<object>() { new { taste = "salty", spiceLevel = "low" }, new { nutritionLevel = 8 } }
                }
            };

            string id = Guid.NewGuid().ToString();
            IList<object> currentContext = new List<object>() {
                new { time = "morning" },
                new { taste = "salty" }
            };

            IList<string> exclude = new List<string> { "pasta" };
            var request = new RankRequest(actions, currentContext, exclude, id);
            RankResponse resp = personalizer.Client.Rank(request);
            Assert.AreEqual("salad", resp.RewardActionId);
        }
    }
}
