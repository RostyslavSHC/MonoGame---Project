using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Editor;
using Editor.Editor;
using Editor.Engine;




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


        public FormEditor() // Runs when the editor window is created
        {
            InitializeComponent(); // Creates and loads the form controls
            KeyPreview = true; // Prevents the form from intercepting key presses
        }

        private void HookEvents()
        {
            Form gameForm = Control.FromHandle(m_game.Window.Handle) as Form;
            gameForm.MouseDown += GameForm_MouseDown;
            gameForm.MouseUp += GameForm_MouseUp;
            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;
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
                Game.Project = new(Game.Content, sfd.FileName);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
            saveToolStripMenuItem_Click(sender, e);
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
                Game.Project.Deserialize(reader, Game.Content);
                Text = "Our Cool Editor - " + Game.Project.Name;
                Game.AdjustAspectRatio();
            }
        }







        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            InputController.Instance.SetButtonUp(e.Button); // Mouse button was released
        } 

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            InputController.Instance.SetButtonDown(e.Button); // Mouse button was pressed
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            InputController.Instance.SetKeyUp(e.KeyCode); // Keyboard key was released
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            InputController.Instance.SetKeyDown(e.KeyCode); // Keyboard key was pressed
        }




    }
}
