using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MouseMover {
    static class Parser {
        private static Regex COMMAND_REGEX = new Regex(@"(\w+)\((-?\d+)?\)", RegexOptions.Compiled);

        public static MouseActionsEntry Parse(string line) {
            try {
                line = line.Replace(" ", "");
                var spl = line.Split(':');
                if (spl.Length != 2) {
                    throw new Exception();
                }
                var triggerString = spl[0];
                var actionsString = spl[1];

                var key = parseTrigger(triggerString);
                var actions = parseActions(actionsString);

                var entry = new MouseActionsEntry();
                entry.Trigger = key;
                entry.MouseActions = actions;
                return entry;
            }
            catch {
                throw new Exception("パースに失敗しました。: " + line);
            }
        }

        private static Trigger parseTrigger(string triggerString) {
            var trigger = new Trigger();

            var spl = triggerString.Split('-');
            if (spl.Length > 4) {
                throw new Exception();
            }
            foreach (var modifierOrKey in spl) {
                if (modifierOrKey.Length != 1) {
                    throw new Exception();
                }

                char c = modifierOrKey[0];

                switch (c) {
                    case 'S':
                        trigger.Modifier |= ModifierKeys.Shift;
                        break;
                    case 'A':
                        trigger.Modifier |= ModifierKeys.Alt;
                        break;
                    case 'C':
                        trigger.Modifier |= ModifierKeys.Control;
                        break;
                    case 'W':
                        trigger.Modifier |= ModifierKeys.Win;
                        break;
                    default:
                        if (!isValidKeyForTrigger(c)) {
                            throw new Exception();
                        }
                        trigger.Key = (Keys)('A' + (c - 'a'));
                        break;
                }
            }

            return trigger;
        }

        private static bool isValidKeyForTrigger(char c) {
            return ('a' <= c && c >= 'z') || ('0' <= c && c >= '9');
        }

        private static List<IMouseAction> parseActions(string actionsString) {
            var actions = new List<IMouseAction>();
            var spl = actionsString.Split(',');
            foreach (var command in spl) {
                if (!COMMAND_REGEX.IsMatch(command)) {
                    throw new Exception();
                }
                var m = COMMAND_REGEX.Match(command);
                var action = m.Groups[1].Value;
                int param;
                int.TryParse(m.Groups[2].Value, out param);
                
                switch (action) {
                    case "Up":
                        actions.Add(new MouseUp(param));
                        break;
                    case "Down":
                        actions.Add(new MouseDown(param));
                        break;
                    case "Left":
                        actions.Add(new MouseLeft(param));
                        break;
                    case "Right":
                        actions.Add(new MouseRight(param));
                        break;
                    case "Wait":
                        actions.Add(new Sleep(param));
                        break;
                    case "Wheel":
                        actions.Add(new MouseWheel(param));
                        break;
                    case "LeftClick":
                        actions.Add(new LeftClick());
                        break;
                    case "RightClick":
                        actions.Add(new RightClick());
                        break;
                    default:
                        throw new Exception();
                }
            }
            return actions;
        }
    }
}
