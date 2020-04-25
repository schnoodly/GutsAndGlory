using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using MBOptionScreen.Attributes;
using MBOptionScreen.Settings;
using TaleWorlds.Localization;
using MBOptionScreen.Attributes.v1;

namespace GutsAndGlory
{
    public class ModOptions : AttributeSettings<ModOptions>
    {
        public override string ModName => "Guts And Glory";
        public override string ModuleFolderName => "GutsAndGlory";

        public override string Id { get; set; } = "GutsAndGlory_v1";

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
    }
}
