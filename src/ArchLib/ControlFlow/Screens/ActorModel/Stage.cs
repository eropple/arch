using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens.ActorModel.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.ControlFlow.Screens.ActorModel
{
    public sealed class Stage
    {
        private readonly HashSet<Actor> _topLevelActors = new HashSet<Actor>();

        private readonly Dictionary<String, Actor> _nameTable =
            new Dictionary<String, Actor>(200);

        public Stage()
        {
        }

        public void Update(Double delta)
        {
            foreach (Actor a in _topLevelActors)
                a.DoUpdate(delta);
        }

        public void Draw(Double delta, SpriteBatch batch)
        {
            foreach(Actor a in _topLevelActors)
                a.DoDraw(delta, batch, Vector2.Zero);
        }

        public void Enstage(Actor a)
        {
            if (a.Stage != null)
            {
                throw new InvalidOperationException("Actors can only " +
                                                    "be staged if they have" +
                                                    "no current stage.");
            }

            a.Stage = this;
        }

        public void Register(Actor a)
        {
            if (_nameTable.ContainsKey(a.Name))
            {
                throw new InvalidOperationException("Duplicate actor '" +
                                                    a.Name + "' on stage.");
            }
            _nameTable.Add(a.Name, a);
        }

        public void Destage(Actor a)
        {
            if (a.Stage != this)
            {
                throw new InvalidOperationException("This actor is not " +
                                                    "staged to this stage.");
            }

            a.Stage = null;
        }

        public void Deregister(Actor a)
        {
            _nameTable.Remove(a.Name);
        }
    }
}
