using Assets.Core.Enums;
using Assets.CS.TabletopUI;
using Assets.TabletopUi;
using UnityEngine;
using Event = GreatWork.Events.EventTypes.Event;

namespace MansusCustomisation.Events
{
    public class MansusEvent : Event
    {

        public class Setup : MansusEvent
        {

            public readonly PortalEffect Effect;

            public Setup(PortalEffect effect)
            {
                Effect = effect;
            }

            public class Pre : Setup
            {
                public Pre(PortalEffect effect) : base(effect)
                {
                }
            }

            public class Post : Setup
            {
                public Post(PortalEffect effect) : base(effect)
                {
                }
            }
        }

        public class Entry : MansusEvent
        {
            public readonly SituationController Situation;
            public readonly Transform Origin;
            public readonly PortalEffect Effect;

            public Entry(SituationController situation, Transform origin, PortalEffect effect)
            {
                Situation = situation;
                Origin = origin;
                Effect = effect;
            }

            public class Pre : Entry
            {
                public Pre(SituationController situation, Transform origin, PortalEffect effect) : base(situation, origin, effect)
                {
                }
            }

            public class Post : Entry
            {
                public Post(SituationController situation, Transform origin, PortalEffect effect) : base(situation, origin, effect)
                {
                }
            }
        }

        public class Exit : MansusEvent
        {
            public readonly Transform Origin;
            public readonly ElementStackToken MansusCard;

            public Exit(Transform origin, ElementStackToken mansusCard)
            {
                Origin = origin;
                MansusCard = mansusCard;
            }

            public class Pre : Exit
            {
                public Pre(Transform origin, ElementStackToken mansusCard) : base(origin, mansusCard)
                {
                }
            }
            
            public class Post : Exit
            {
                public Post(Transform origin, ElementStackToken mansusCard) : base(origin, mansusCard)
                {
                }
            }
        }
        
    }
}