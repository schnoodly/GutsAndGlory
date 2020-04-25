using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;
using TaleWorlds.Localization;

namespace GutsAndGlory
{
    public class ModOptions : AttributeSettings<ModOptions>
    {
        public override string ModName => "Guts And Glory";
        public override string ModuleFolderName => "GutsAndGlory";

        public override string Id { get; set; } = "GutsAndGlory_v1";

        //private static Random random = new Random();
        //private static string configFile = "../../Modules/GutsAndGlory/settings.xml"; // Path of Settings XML relative to Current Directory
        //private static string currentDirectory = Directory.GetCurrentDirectory(); // Current Directory
        //private static string settingsFilepath = System.IO.Path.Combine(currentDirectory, configFile); // Combine the parts

        [SettingProperty("Enabled", true, "")]
        [SettingPropertyGroup("General")]
        public bool Enabled { get; set; } = true;
        [SettingProperty("Enable Player Death", true, "")]
        [SettingPropertyGroup("General")]
        public bool playerEnabled { get; set; } = true;
        [SettingProperty("Dismemberment Damage Overkill Threshold", true, "")]
        [SettingPropertyGroup("General")]
        public float damageThreshold { get; set; } = 50;
        [SettingProperty("Overall Chance", true, "The max probability for all gore.")]
        [SettingPropertyGroup("Probabilities")]
        public int probability { get; set; } = 25;
        [SettingProperty("Beheading Chance", true, "Likelihood that character loses their head")]
        [SettingPropertyGroup("Probabilities")]
        public int headProbability { get; set; } = 25;
        [SettingProperty("Arm Dismemberment Chance", true, "Likelihood that character loses their arm")]
        [SettingPropertyGroup("Probabilities")]
        public int armProbability { get; set; } = 25;
        [SettingProperty("Leg Dismemberment Chance", true, "Likelihood that character loses their leg")]
        [SettingPropertyGroup("Probabilities")]
        public int legProbability { get; set; } = 25;
        [SettingProperty("Chest Bisect Chance", true, "Likelihood that character gets cut in half through the torso")]
        [SettingPropertyGroup("Probabilities")]
        public int chestProbability { get; set; } = 25;


        /*
        private void GetConfigValues()
        {
            XElement settings = XElement.Load(settingsFilepath); // Load the XML file using LINQ to XML

            // find settings elements and get their values
            this.probability = Int32.Parse(settings.Elements("GoreProbability").First().Attribute("value").Value);
            this.playerDeath = Boolean.Parse(settings.Elements("PlayerDeath").First().Attribute("value").Value);
            this.debug = Boolean.Parse(settings.Elements("Debug").First().Attribute("value").Value);
            this.loaded = true;
        }
        */
    }
}
