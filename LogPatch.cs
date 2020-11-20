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

        public static Color CYAN;
        public static Color ORANGE;
        public static Color RED;
        public static Color PURPLE;
        public static Color LIGHT_PURPLE;

        /**
         * Cancel main method call by returning false, don't want the normal log to display.
         * In future would be best to add a separate option from "Report Casualties", but haven't quite figured that out yet.
         */
        public static bool Prefix(ref Agent affectedAgent, ref Agent affectorAgent)
        {
            var itemVM = new SPMissionKillNotificationItemVM(affectedAgent, affectorAgent, null, null);

            var builder = new StringBuilder();
            builder.Append(affectedAgent.Name);

            builder.Append(affectedAgent.State == AgentState.Unconscious ? " knocked unconscious by " : " killed by ");

            builder.Append(affectorAgent.Name);
            InformationManager.DisplayMessage(new InformationMessage(builder.ToString(), GetColor(affectedAgent, affectedAgent.State == AgentState.Unconscious)));
            return false;
        }

        public static void loadConfigValues()
        {
            CYAN = Color.ConvertStringToColor(WarbandConfig.FriendlyKill);
            ORANGE = Color.ConvertStringToColor(WarbandConfig.FriendlyUnconscious);
            RED = Color.ConvertStringToColor(WarbandConfig.FriendlyKilled);
            PURPLE = Color.ConvertStringToColor(WarbandConfig.AllyKilled);
            LIGHT_PURPLE = Color.ConvertStringToColor(WarbandConfig.AllyUnconscious);
        }

        private static Color GetColor(Agent killed, bool isUnconscious)
        {
            
            if (killed.Team != null)
            {
                if (killed.Team.IsPlayerTeam)
                {
                    return isUnconscious ? ORANGE : RED;
                }
                else if (killed.Team.IsPlayerAlly)
                {
                    return isUnconscious ? LIGHT_PURPLE : PURPLE;
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
