using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace AlwaysLeadSieges.DefaultEncounterPatches
{
    [HarmonyPatch(typeof(DefaultEncounterModel), "GetLeaderOfSiegeEvent")]
    public class GetLeaderOfSiegeEventPatch
    {
        private static bool playerIsInvolved = false;

        private static void Prefix(out Hero __state, SiegeEvent siegeEvent, BattleSideEnum side, DefaultEncounterModel __instance)
        {
            __state = null;
            playerIsInvolved = false;

            //Attacking side
            if (side == BattleSideEnum.Attacker)
            {
                List<PartyBase> parties = siegeEvent.GetSiegeEventSide(side).GetInvolvedPartiesForEventType((MapEvent.BattleTypes)5).ToList<PartyBase>();

                foreach (PartyBase party in parties)
                    if (party.LeaderHero.Id == Hero.MainHero.Id)
                        playerIsInvolved = true;

                //Save current siege leader to __state so it is not null.
                if (siegeEvent.BesiegerCamp.LeaderParty != null)
                    __state = siegeEvent.BesiegerCamp.LeaderParty.LeaderHero;
            }
        }

        private static void Postfix(Hero __state, ref Hero __result)
        {
            //__state should always be null unless we're on the attacking side and have a saved leader from prefix
            //so this should not change the leader of the defenders
            //tldr: player is involved in siege and GetLeaderOfSiegeEvent was called on the attacking side so set leader to player
            if (playerIsInvolved && __state != null)
            {
                __result = Hero.MainHero;
            }
        }
    }
}
