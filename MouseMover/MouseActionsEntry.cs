using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MouseMover {
    class MouseActionsEntry {
        public Trigger Trigger {
            get;
            set;
        }

        public IList<IMouseAction> MouseActions {
            get;
            set;
        }

        public void DoActions() {
            foreach (var action in MouseActions) {
                action.DoAction();
            }
        }
     
    }
}
