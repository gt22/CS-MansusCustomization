using Assets.Core.Entities;
using Assets.Core.Enums;
using Assets.CS.TabletopUI;
using Assets.TabletopUi;
using Assets.TabletopUi.Scripts.Infrastructure;
using GreatWork;
using GreatWork.Patches;
using GreatWork.Utils;
using MansusCustomisation.Events;
using UnityEngine;

namespace MansusCustomisation.Patches
{
    public static class MansusPatch
    {
        public static void PatchAll()
        {
            HarmonyHolder.Harmony.Patch(
                typeof(DoorSlot).Method("GetDeckName"),
                postfix: HarmonyHolder.Wrap("DoorDeckPostfix")
            );
            HarmonyHolder.Harmony.Patch(
                typeof(TabletopManager).Method("ShowMansusMap"),
                HarmonyHolder.Wrap("MansusEntryPrefix"),
                HarmonyHolder.Wrap("MansusEntryPostfix")
            );
            HarmonyHolder.Harmony.Patch(
                typeof(TabletopManager).Method("ReturnFromMansus"),
                HarmonyHolder.Wrap("MansusExitPrefix"),
                HarmonyHolder.Wrap("MansusExitPostfix")
            );
            HarmonyHolder.Harmony.Patch(
                typeof(MapController).Method("SetupMap"),
                HarmonyHolder.Wrap("MansusSetupPrefix"),
                HarmonyHolder.Wrap("MansusSetupPostfix")
            );
        }

        private static void DoorDeckPostfix(int cardPosition, ref string __result, DoorSlot __instance)
        {
            var mansus = MansusManager.CurrentMansus;
            if (mansus == null)
            {
                Debug.Log("Null mansus!");
                return;
            }
            var decks = mansus.Doors[__instance.portalType.ToString().ToLower()]?.Decks;
            if (decks != null && decks.Count > cardPosition)
            {
                __result = decks[cardPosition];
                Debug.Log("Deck: " + __result + " - " + Registry.Get<Compendium>().GetEntityById<DeckSpec>(__result));
            }
        }

        private static void MansusEntryPrefix(SituationController situation, Transform origin, PortalEffect effect)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Entry.Pre(situation, origin, effect));
        }
        
        private static void MansusEntryPostfix(SituationController situation, Transform origin, PortalEffect effect)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Entry.Post(situation, origin, effect));
        }
        
        private static void MansusExitPrefix(Transform origin, ElementStackToken mansusCard)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Exit.Pre(origin, mansusCard));
        }
        
        private static void MansusExitPostfix(Transform origin, ElementStackToken mansusCard)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Exit.Post(origin, mansusCard));
        }

        private static void MansusSetupPrefix(PortalEffect effect)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Setup.Pre(effect));
        }
        
        private static void MansusSetupPostfix(PortalEffect effect)
        {
            GreatWorkAPI.Events.FireEvent(new MansusEvent.Setup.Pre(effect));
        }
    }
}