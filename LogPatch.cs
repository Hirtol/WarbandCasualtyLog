using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed;

namespace WarbandCasualtyList
{
    [HarmonyPatch(typeof(SPKillFeedVM), "OnAgentRemoved")]
    internal class LogPatch
    {

        public const string CYAN = "#00997AB7";
        public const string ORANGE = "#CC6600E5";
        public const string RED = "#AF6353FF";

        /**
         * Cancel main method call by returning false, don't want the normal log to display.
         * In future would be best to add a separate option from "Report Casualties", but haven't quite figured that out yet.
         */
        public static bool Prefix(ref Agent affectedAgent, ref Agent affectorAgent)
        {
            var itemVM = new SPMissionKillNotificationItemVM(affectedAgent, affectorAgent, null, null);

            var builder = new StringBuilder();
            builder.Append(itemVM.VictimName);

            builder.Append(itemVM.IsUnconscious ? " knocked unconscious by " : " killed by ");

            builder.Append(itemVM.MurdererName);
            InformationManager.DisplayMessage(new InformationMessage(builder.ToString(), GetColor(affectedAgent, itemVM.IsUnconscious)));

            return false;
        }

        private static Color GetColor(Agent killed, bool isUnconscious)
        {
            Color result;
            if (killed.Team != null)
            {
                if (killed.Team.IsPlayerAlly)
                {
                    result = isUnconscious ? Color.ConvertStringToColor(ORANGE) : Color.ConvertStringToColor(RED);
                }
                else
                {
                    result = Color.ConvertStringToColor(CYAN);
                }
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("ERROR: Warband log exception!"));
                result = Color.FromUint(4294967295u);
            }

            return result;
        }
    }
}
