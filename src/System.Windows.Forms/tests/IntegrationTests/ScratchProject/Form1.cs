using System.ComponentModel;

namespace ScratchProject
{
    // As we can't currently design in VS in the runtime solution, mark as "Default" so this opens in code view by default.^M
    [DesignerCategory("Default")]
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click event if needed
        }
    }
}

