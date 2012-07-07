using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace MouseMover {
    public interface IMouseAction {
        string CommandText { get; }
        void DoAction();
    }

    public abstract class MouseMove : IMouseAction {
        public int Diff { get; set; }
        public abstract string CommandText { get; }

        public MouseMove(int diff) {
            Diff = diff;
        }

        public abstract void DoAction();
    }

    public sealed class MouseUp : MouseMove {
        public MouseUp(int diff) : base(diff) {

        }

        public override string CommandText {
            get {
                return "Up(" + Diff + ")";
            }
        }

        public override void DoAction() {
            var pos = System.Windows.Forms.Cursor.Position;
            pos.Y -= Diff;
            System.Windows.Forms.Cursor.Position = pos;
        }
    }

    public sealed class MouseDown : MouseMove {
        public MouseDown(int diff) : base(diff) {
        }

        public override string CommandText {
            get { return "Down(" + Diff + ")"; }
        }

        public override void DoAction() {
            var pos = System.Windows.Forms.Cursor.Position;
            pos.Y += Diff;
            System.Windows.Forms.Cursor.Position = pos;
        }
    }

    public sealed class MouseLeft : MouseMove {
        public MouseLeft(int diff) : base(diff) {
        }

        public override string CommandText {
            get { return "Left(" + Diff + ")"; }
        }

        public override void DoAction() {
            var pos = System.Windows.Forms.Cursor.Position;
            pos.X -= Diff;
            System.Windows.Forms.Cursor.Position = pos;
        }
    }

    public sealed class MouseRight : MouseMove {
        public MouseRight(int diff) : base(diff) {
        }

        public override string CommandText {
            get { return "Right(" + Diff + ")"; }
        }

        public override void DoAction() {
            var pos = System.Windows.Forms.Cursor.Position;
            pos.X += Diff;
            System.Windows.Forms.Cursor.Position = pos;
        }
    }

    public sealed class Sleep : IMouseAction {
        public int SleepTimeInMillisecond { get; set; }

        public Sleep(int sleepTimeInMillisecond) {
            SleepTimeInMillisecond = sleepTimeInMillisecond;
        }

        public string CommandText {
            get { return "Wait(" + SleepTimeInMillisecond + ")"; }
        }

        public void DoAction() {
            Thread.Sleep(SleepTimeInMillisecond);
        }
    }
}
