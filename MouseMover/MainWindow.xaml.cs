using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace MouseMover {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        private IList<MouseActionsEntry> _entries;
        private KeyboardHook _hook;
        private NotifyIcon _notifyIcon;
        private const string TRAY_ICON = "Icon1.ico";
        private const string CONFIG_FILE_PATH = "conf.txt";

        public MainWindow() {
            InitializeComponent();
            buildNotifyIcon();
            _entries = new List<MouseActionsEntry>();
            loadFile(CONFIG_FILE_PATH);
            registerHotKeys();
        }

        private void buildNotifyIcon() {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon(this.GetType(), TRAY_ICON);
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("再読み込み(&R)", null, (_, __) => {
                _hook.Dispose();
                _entries.Clear();

                loadFile(CONFIG_FILE_PATH);
                registerHotKeys();
            }));
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("終了(&X)", null, (_, __) => {
                App.Current.Shutdown();
            }));

            _notifyIcon.Visible = true;
        }

        private void registerHotKeys() {
            _hook = new KeyboardHook();
            _hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            
            foreach (var entry in _entries) {
                _hook.RegisterHotKey(entry.Trigger.Modifier, entry.Trigger.Key);
            }
        }

        private void loadFile(string path) {
            string line;
            using (var reader = File.OpenText(path))
            {
                while ((line = reader.ReadLine()) != null) {
                    if (line != "") {
                        _entries.Add(Parser.Parse(line));
                    }
                }
            }
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e) {
            foreach(var entry in _entries) {
                if (entry.Trigger.Modifier == e.Modifier && entry.Trigger.Key == e.Key) {
                    entry.DoActions();
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            _entries[0].DoActions();
        }
    }
}
