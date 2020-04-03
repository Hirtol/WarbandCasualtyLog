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
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            var harmony = new Harmony("top.hirtol.warbandcasualty.patch");
            harmony.PatchAll();
        }
    }

    
}
