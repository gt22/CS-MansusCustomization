using System.Collections.Generic;
using Assets.Core.Fucine;
using Assets.Core.Fucine.DataImport;
using GreatWork.Fucine;
using UnityEngine;

namespace MansusCustomisation.Entities
{
    [FucineImportable("manses")]
    public class Mansus : AbstractEntity<Mansus>
    {

        [FucineSprite("ui")]
        public Sprite Map { get; set; }

        [FucineEntityDict]
        public Dictionary<string, Door> Doors { get; set; }
        
        public Mansus(EntityData importDataForEntity, ContentImportLog log)
            : base(importDataForEntity, log)
        {

        }

        public Mansus(string id = null)
        {
            _id = id;
        }
        
        protected override void OnPostImportForSpecificEntity(ContentImportLog log, ICompendium populatedCompendium)
        { }
    }
}