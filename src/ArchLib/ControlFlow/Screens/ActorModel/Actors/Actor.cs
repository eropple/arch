using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArchLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.ControlFlow.Screens.ActorModel.Actors
{
    public abstract class Actor
    {
        private Stage _stage;
        private RectangleF _bounds;

        private readonly List<Actor> _actors;

        public readonly ReadOnlyCollection<Actor> Children;
        public readonly String Name;

        public Vector2 Position
        {
            get { return new Vector2(_bounds.X, _bounds.Y); }
            set
            {
                _bounds.X = value.X;
                _bounds.Y = value.Y;
            }
        }
        public Vector2 Size
        {
            get { return new Vector2(_bounds.Width, _bounds.Height);}
            set
            {
                _bounds.Width = value.X;
                _bounds.Height = value.Y;
            }
        }
        public RectangleF Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        public Actor Parent { get; private set; }

        protected Actor(String name = null)
        {
            _actors = new List<Actor>();
            Children = new ReadOnlyCollection<Actor>(_actors);
        }

        public void DoUpdate(Double delta)
        {
            Update(delta);
            foreach (Actor a in _actors)
            {
                a.DoUpdate(delta);
            }
        }
        public abstract void Update(Double delta);

        public void DoDraw(Double delta, SpriteBatch batch, Vector2 offset)
        {
            // TODO: if this turns into perf problems, try individual batches + matrices
            Draw(delta, batch, offset);
            Vector2 o = new Vector2(offset.X + _bounds.X,
                offset.Y + _bounds.Y);
            foreach (Actor a in _actors)
                a.DoDraw(delta, batch, o);
        }
        public abstract void Draw(Double delta, SpriteBatch batch, Vector2 offset);

        public void AddChild(Actor child)
        {
            if (child.Parent != null)
                throw new InvalidOperationException("Child must not have a parent to be added to an actor.");

            child.Parent = this;
            child.Stage = this.Stage;

            _actors.Add(child);
            if (child.Name != null) Stage.Register(child);
        }

        public void RemoveChild(Actor child)
        {
            if (child.Parent != this)
                throw new InvalidOperationException("Child must be parented to this actor to be removed.");

            child.Parent = null;
            child.Stage = null;

            _actors.Remove(child);
            if (child.Name != null) Stage.Deregister(child);
        }
        
        /// <summary>
        /// Recursively (through the children of this actor, if any) sets the
        /// active stage for it and its children. The method will also deregister
        /// named actors from the stage.
        /// </summary>
        public Stage Stage
        {
            get { return _stage; }
            set
            {
                if (value != _stage)
                {
                    if (_stage != null)
                    {
                        if (this.Name != null) _stage.Deregister(this);
                    }

                    if (value != null)
                    {
                        if (this.Name != null) value.Register(this);
                    }

                    _stage = value;
                    foreach (Actor a in _actors) a.Stage = value;
                }
            }
        }
    }
}
