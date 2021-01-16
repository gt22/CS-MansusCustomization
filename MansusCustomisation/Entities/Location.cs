using Assets.Core.Fucine;
using Assets.Core.Fucine.DataImport;

namespace MansusCustomisation.Entities
{
    public class Location : AbstractEntity<Location>
    {
        [FucineValue] 
        public int X { get; set; }

        [FucineValue]
        public int Y { get; set; }
        
        public Location(EntityData importDataForEntity, ContentImportLog log)
            : base(importDataForEntity, log)
        {
        }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected override void OnPostImportForSpecificEntity(ContentImportLog log, ICompendium populatedCompendium)
        {
        }
    }
}