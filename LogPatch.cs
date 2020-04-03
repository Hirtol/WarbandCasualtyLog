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

namespace WarbandCasualtyLog
{
    [HarmonyPatch(typeof(SPKillFeedVM), "OnAgentRemoved")]
    internal class LogPatch
    {

        public static Color CYAN = Color.White;
        public static Color ORANGE;
        public static Color RED;

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

            if (CYAN == Color.White)
            {
                CYAN = Color.ConvertStringToColor(WarbandConfig.FriendlyKill);
                ORANGE = Color.ConvertStringToColor(WarbandConfig.FriendlyUnconscious);
                RED = Color.ConvertStringToColor(WarbandConfig.FriendlyKilled);
            }

            if (killed.Team != null)
            {
                if (killed.Team.IsPlayerAlly)
                {
                    return isUnconscious ? ORANGE : RED;
                }

                return CYAN;
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("ERROR: Warband log exception!"));
                return Color.FromUint(4294967295u);
            }
        }
    }
}
