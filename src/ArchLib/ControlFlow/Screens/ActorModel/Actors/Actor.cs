﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArchLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.ControlFlow.Screens.ActorModel.Actors
{
    public abstract class Actor
    {
        private readonly List<Actor> _actors;
        public readonly ReadOnlyCollection<Actor> Children;
        public readonly String Name;

        public Actor Parent { get; private set; }

        private Stage _stage;

        private RectangleF _bounds;
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