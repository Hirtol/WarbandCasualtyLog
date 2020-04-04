using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;

namespace WarbandCasualtyLog
{
    public class SubModule : MBSubModuleBase
    {
        public static bool invalidConfigFlag = false;
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            WarbandConfig.Initialize();
            var harmony = new Harmony("top.hirtol.warbandcasualty.patch");
            harmony.PatchAll();
            LogPatch.loadConfigValues();
        }

        protected override void OnSubModuleUnloaded()
        {
            WarbandConfig.Save();
        }

        protected override void OnApplicationTick(float dt)
        {
            if (invalidConfigFlag)
            {
                InformationManager.DisplayMessage(new InformationMessage("Invalid value(s) in Warband Casualty Log Config, please reconfigure!"));
            }
        }
    }

    
}
