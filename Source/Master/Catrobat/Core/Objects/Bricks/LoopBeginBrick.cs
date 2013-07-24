﻿using System.ComponentModel;
using System.Xml.Linq;
using Catrobat.Core.Misc.Helpers;

namespace Catrobat.Core.Objects.Bricks
{
    public abstract class LoopBeginBrick : Brick
    {
        protected LoopEndBrickReference _loopEndBrickReference;
        public LoopEndBrickReference LoopEndBrickReference
        {
            get { return _loopEndBrickReference; }
            set
            {
                if (_loopEndBrickReference == value)
                {
                    return;
                }

                _loopEndBrickReference = value;
                RaisePropertyChanged();
            }
        }

        public LoopEndBrick LoopEndBrick
        {
            get { return _loopEndBrickReference.LoopEndBrick; }
            set
            {
                if (_loopEndBrickReference == null)
                    _loopEndBrickReference = new LoopEndBrickReference();

                if (_loopEndBrickReference.LoopEndBrick == value)
                    return;

                _loopEndBrickReference.LoopEndBrick = value;

                if (value == null)
                    _loopEndBrickReference = null;

                RaisePropertyChanged();
            }
        }


        protected LoopBeginBrick() {}

        protected LoopBeginBrick(XElement xElement) : base(xElement) {}

        internal abstract override void LoadFromXML(XElement xRoot);

        internal abstract override XElement CreateXML();

        protected override void LoadFromCommonXML(XElement xRoot)
        {
            if (xRoot.Element("loopEndBrick") != null)
            {
                _loopEndBrickReference = new LoopEndBrickReference(xRoot.Element("loopEndBrick"));
            }
        }

        protected override void CreateCommonXML(XElement xRoot)
        {
            xRoot.Add(_loopEndBrickReference.CreateXML());
        }

        internal override void LoadReference()
        {
            if (_loopEndBrickReference.LoopEndBrick == null)
                _loopEndBrickReference.LoadReference();
        }

        public abstract override DataObject Copy();
    }
}