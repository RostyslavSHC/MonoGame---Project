using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


// Groups this code inside the Editor project
namespace Editor
{
    public partial class FormEditor : Form // Creates editor window
    {
        public GameEditor Game { get; set; } // Stores the MonoGame editor


        public FormEditor() // Runs when the editor window is created
        {
            InitializeComponent(); // Creates and loads the form controls
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
        }
    }
}
