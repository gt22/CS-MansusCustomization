using System.Collections.Generic;
using Assets.Core.Fucine;
using Assets.Core.Fucine.DataImport;
using Assets.Core.Interfaces;
using GreatWork.Fucine;
using UnityEngine;

namespace MansusCustomisation.Entities
{
    public class Door : AbstractEntity<Door>, IQuickSpecEntity
    {

        [FucineSprite("ui")]
        public Sprite Icon { get; set; }
        
        [FucineValue("")]
        public string Color { get; set; }
        
        [FucineEntityDict]
        public Dictionary<string, Location> Locations { get; set; }
        
        [FucineList]
        public List<string> Decks { get; set; }
        
        public Door(EntityData importDataForEntity, ContentImportLog log)
            : base(importDataForEntity, log)
        {
        }

        public Door(string value)
        {
            QuickSpec(value);
        }

        public Door()
        {
            
        }
        
        public void QuickSpec(string value)
        {
            Icon = ResourcesManager.GetSpriteForUI(value);
            Color = "";
            Locations = new Dictionary<string, Location>();
            Decks = new List<string>();
        }
        
        protected override void OnPostImportForSpecificEntity(ContentImportLog log, ICompendium populatedCompendium)
        {
        }
    }
}