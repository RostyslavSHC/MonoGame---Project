using Editor;
using Editor.Editor;
using Editor.Engine;
using Editor.GUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




// Groups this code inside the Editor project
namespace Editor
{
    public partial class FormEditor : Form // Creates editor window
    {
        // [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // public GameEditor Game { get; set; } // Stores the MonoGame editor
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameEditor Game { get => m_game; set { m_game = value; HookEvents(); } }
        private GameEditor m_game = null;
        private Process m_MGCBProcess = null;


        public FormEditor() // Runs when the editor window is created
        {
            InitializeComponent(); // Creates and loads the form controls
            KeyPreview = true; // Prevents the form from intercepting key presses
            toolStripStatusLabel1.Text = Directory.GetCurrentDirectory(); // Shows the current working directory
            listBoxAssets.MouseDown += ListBoxAssets_MouseDown;
        }

        private void ListBoxAssets_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxAssets.Items.Count == 0) return;

            int index = listBoxAssets.IndexFromPoint(e.X, e.Y);
            if (index < 0) return;
            var lia = listBoxAssets.Items[index] as ListItemAsset;
            if (lia.Type == AssetTypes.MODEL)
            {
                DoDragDrop(lia, DragDropEffects.Copy);
            }
        }




        private void HookEvents()
        {
            Form gameForm = Control.FromHandle(m_game.Window.Handle) as Form;
            gameForm.MouseDown += GameForm_MouseDown;
            gameForm.MouseUp += GameForm_MouseUp;
            gameForm.MouseWheel += GameForm_MouseWheel;
            gameForm.MouseMove += GameForm_MouseMove;
            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;

            gameForm.DragDrop += GameForm_DragDrop;
            gameForm.DragOver += GameForm_DragOver;
            gameForm.AllowDrop = true;
        }

        private void GameForm_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void GameForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListItemAsset)))
            {
                var lia = e.Data.GetData(typeof(ListItemAsset)) as ListItemAsset;
                Models model = new(m_game, lia.Name, "DefaultTexture",
                                   "DefaultEffect", Vector3.Zero, 1.0f);
                m_game.Project.CurrentLevel.AddModel(model);
            }
        }










        private void exitToolStripMenuItem_Click(object sender, EventArgs e) // Runs when Exit is clicked
        {
            Game.Exit(); // Closes the MonoGame application
        }

        private void splitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {
            if (Game == null) return; // If the game is not loaded, do nothing
            Game.AdjustAspectRatio(); // Adjusts the aspect ratio
        }
        private void FormEditor_SizeChanged(object sender, EventArgs e)
        {
            if (Game == null) return; // If the game is not loaded, do nothing
            Game.AdjustAspectRatio(); // Adjusts the aspect ratio

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Game.Project = new(Game.GraphicsDevice, Game.Content, sfd.FileName);
                Game.Project.OnAssetsUpdated += Project_OnAssetsUpdated;
                Game.Project.AssetMonitor.UpdateAssetDB();
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
            saveToolStripMenuItem_Click(sender, e);
        }

        private void Project_OnAssetsUpdated()
        {
            this.Invoke(delegate
            {
                listBoxAssets.Items.Clear();
                var assets = Game.Project.AssetMonitor.Assets;
                if (!assets.ContainsKey(AssetTypes.MODEL)) return;
                foreach (AssetTypes assetType in Enum.GetValues(typeof(AssetTypes)))
                {
                    if (assets.ContainsKey(assetType))
                    {
                        listBoxAssets.Items.Add(new ListItemAsset()
                        {
                            Name = assetType.ToString().ToUpper() + "S:",
                            Type = AssetTypes.NONE
                        });
                        foreach (string asset in assets[assetType])
                        {
                            ListItemAsset lia = new()
                            {
                                Name = asset,
                                Type = assetType
                            };
                            listBoxAssets.Items.Add(lia);
                        }
                        listBoxAssets.Items.Add(new ListItemAsset()
                        {
                            Name = " ",
                            Type = AssetTypes.NONE
                        });
                    }
                }
            });
        }





        // Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname = Path.Combine(Game.Project.Folder, Game.Project.Name);
            using var stream = File.Open(fname, FileMode.Create);
            using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
            Game.Project.Serialize(writer);
        }

        // Load
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "OCE Files|*.oce";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using var stream = File.Open(ofd.FileName, FileMode.Open);
                using var reader = new BinaryReader(stream, Encoding.UTF8, false);
                Game.Project = new();
                Game.Project.Deserialize(reader, Game);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
        }






        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            var p = new Vector2(e.Location.X, e.Location.Y); // Get mouse position
            InputController.Instance.MousePosition = p; // Save current mouse position
        }

        private void GameForm_MouseWheel(object sender, MouseEventArgs e)
        {
            // Save how much the mouse wheel was scrolled
            InputController.Instance.SetWheel(e.Delta / SystemInformation.MouseWheelScrollDelta);
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            InputController.Instance.SetButtonUp(e.Button); // Mouse button released

            var p = new Vector2(e.Location.X, e.Location.Y); // Get mouse position
            InputController.Instance.DragEnd = p; // Save where dragging ended
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            InputController.Instance.SetButtonDown(e.Button); // Mouse button pressed

            var p = new Vector2(e.Location.X, e.Location.Y); // Get mouse position
            InputController.Instance.DragStart = p; // Save where dragging started
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            InputController.Instance.SetKeyUp(e.KeyCode); // Keyboard key released
            e.Handled = true; // Mark the event as handled
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            InputController.Instance.SetKeyDown(e.KeyCode); // Keyboard key pressed
            e.Handled = true; // Mark the event as handled
        }


        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mgcbEditorPath = ConfigurationManager.AppSettings["MGCB_EditorPath"];
            ProcessStartInfo startInfo = new()
            {


                FileName = "\"" + Path.Combine(mgcbEditorPath, "mgcb-editor-windows.exe") + "\"",
                Arguments = "\"" + Path.Combine(Game.Project.ContentFolder, "Content.mgcb") + "\""

            };

            m_MGCBProcess = Process.Start(startInfo);
        }

        private void FormEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_MGCBProcess == null) return;
            m_MGCBProcess.Kill();
        }











        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
