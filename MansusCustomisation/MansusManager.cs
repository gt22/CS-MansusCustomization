using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Assets.Core.Entities;
using Assets.CS.TabletopUI;
using GreatWork.Entities;
using GreatWork.Events;
using GreatWork.Events.EventTypes;
using GreatWork.Utils;
using MansusCustomisation.Entities;
using MansusCustomisation.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MansusCustomisation
{
    [GwEventHandler]
    public static class MansusManager
    {
        public static Mansus DefaultMansus { get; private set; }
        public static Mansus CurrentMansus { get; private set; }
        public static Mansus LegacyMansus { get; private set; }
        
        private static readonly FieldInfo MansusAnim = typeof(TabletopManager).Field("mapAnimation");
        private static readonly FieldInfo DoorSlots = typeof(MapTokenContainer).Field("allSlots");

        [SubscribeEvent]
        private static void OnLegacyLoad(ServiceInitializationEvent<TabletopManager>.PostInit e)
        {
            BackupDefaultMansus(e.Service);
            var legacy = Registry.Get<Character>().ActiveLegacy.Extra();
            if (legacy.ContainsKey("mansus") && legacy["mansus"].ToString() != "")
            {
                LegacyMansus = Registry.Get<Compendium>().GetEntityById<Mansus>(legacy["mansus"].ToString());
            }
            else
            {
                LegacyMansus = DefaultMansus;
            }
            SetCurrentMansus(LegacyMansus);
        }

        [SubscribeEvent]
        private static void OnCompendiumReload(CompendiumEvent.End e)
        {
            if (DefaultMansus != null)
            {
                e.Compendium.TryAddEntity(DefaultMansus);
            }
        }

        [SubscribeEvent]
        private static void OnMansusEntry(MansusEvent.Entry.Pre e)
        {
            var recipe = Registry.Get<Compendium>().GetEntityById<Recipe>(e.Situation.SituationClock.RecipeId);
            if (recipe.Extra().ContainsKey("mansus"))
            {
                SetCurrentMansus(Registry.Get<Compendium>().GetEntityById<Mansus>(recipe.Extra()["mansus"].ToString()));
            }
        }

        [SubscribeEvent]
        private static void OnMansusExit(MansusEvent.Exit.Post e)
        {
            SetCurrentMansus(LegacyMansus);
        }
        
        private static void BackupDefaultMansus(TabletopManager tabletop)
        {
            if (DefaultMansus != null) return;
            var mapAnimation =
                MansusAnim.GetValue(tabletop) as MapAnimation;
            var doorSlots =
                DoorSlots.GetValue(tabletop.mapTokenContainer) as DoorSlot[];
            DefaultMansus = new Mansus("default")
            {
                Map = mapAnimation.GetComponent<Image>().sprite,
                Doors = new Dictionary<string, Door>()
            };
            foreach (var door in doorSlots)
            {
                var doorName = door.portalType.ToString().ToLower();
                DefaultMansus.Doors[doorName] = BackupDoor(door);
            }

            Registry.Get<Compendium>().TryAddEntity(DefaultMansus);
        }
        
        private static Door BackupDoor(DoorSlot door)
        {
            var ret = new Door
            {
                Icon = door.GetComponentsInChildren<Image>()[1].sprite,
                Color = RGBFromColor(door.defaultBackgroundColor).ToString("X6"),
                Locations = new Dictionary<string, Location>
                {
                    ["door"] = FromV3(door.transform.position),
                    ["card_door"] = FromV3(door.cardPositions[0].position),
                    ["card_1"] = FromV3(door.cardPositions[1].position),
                    ["card_2"] = FromV3(door.cardPositions[2].position)
                }
            };
            return ret;
        }


        private static Sprite GetSprite(string name)
        {
            var s1 = ResourcesManager.GetSpriteForUI(name);
            return s1 != null ? s1 : ResourcesManager.GetSprite("tech", name, false);
        }

        public static void SetCurrentMansus(Mansus mansus)
        {
            if (mansus == CurrentMansus) return;
            if (mansus != DefaultMansus)
            {
                SetCurrentMansus(DefaultMansus); // Reset to default, and then apply modification
            }

            CurrentMansus = mansus;
            var tabletop = Registry.Get<TabletopManager>();
            if (mansus.Map != null)
            {
                var mapAnimation =
                    MansusAnim.GetValue(tabletop) as MapAnimation;
                mapAnimation.GetComponent<Image>().sprite = mansus.Map;
            }

            var doorSlots =
                DoorSlots.GetValue(tabletop.mapTokenContainer) as DoorSlot[];
            foreach (var door in doorSlots)
            {
                var doorName = door.portalType.ToString().ToLower();
                if (mansus.Doors.ContainsKey(doorName)) ModifyDoor(door, mansus.Doors[doorName]);
            }
        }

        private static void ModifyDoor(DoorSlot door, Door d)
        {
            if (d.Icon != null)
            {
                var img = door.GetComponentsInChildren<Image>()[1];
                img.sprite = d.Icon;
            }

            if (!string.IsNullOrEmpty(d.Color))
            {
                var c = int.Parse(d.Color, NumberStyles.HexNumber);
                door.defaultBackgroundColor = ColorFromRGB(c);
            }

            if (d.Locations != null)
            {
                TrySetLocation(d.Locations, door.transform, "door");
                TrySetLocation(d.Locations, door.cardPositions[0], "card_door");
                TrySetLocation(d.Locations, door.cardPositions[1], "card_1");
                TrySetLocation(d.Locations, door.cardPositions[2], "card_2");
            }
        }

        private static void TrySetLocation(Dictionary<string, Location> locations, Transform t, string name)
        {
            if (locations.ContainsKey(name))
            {
                SetLocation(t, locations[name]);
            }
        }

        private static void SetLocation(Transform t, Location loc)
        {
            t.SetPositionAndRotation(new Vector3(loc.X, loc.Y, 0), Quaternion.identity);
        }

        private static Color ColorFromRGB(int rgb)
        {
            return new Color(
                ((rgb >> 16) & 0xFF) / 255f,
                ((rgb >> 8) & 0xFF) / 255f,
                (rgb & 0xFF) / 255f);
        }

        private static int RGBFromColor(Color c)
        {
            var r = (int) (c.r * 255);
            var g = (int) (c.g * 255);
            var b = (int) (c.b * 255);
            return (r << 16) | (g << 8) | b;
        }

        private static Location FromV3(Vector3 v)
        {
            return new Location(Convert.ToInt32(v.x), Convert.ToInt32(v.y));
        }
    }
}