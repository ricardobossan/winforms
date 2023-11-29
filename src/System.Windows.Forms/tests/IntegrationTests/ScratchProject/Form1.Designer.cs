//Added for https://github.com/dotnet/winforms/issues/6514
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScratchProject;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        // 
        // Form1
        // 
        //AutoScaleDimensions = new SizeF(7F, 15F);
        //AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 130);
        //Name = "Form1";
        //Text = "Form1";
        //Load += Form1_Load;

        /*CheckBox checkBoxUncheckedFocused = new()
        {
            FlatStyle = FlatStyle.Standard,
            Text = "CheckBox (Button appearance) Standard Unchecked Focused",
            Appearance = Appearance.Button,
            CheckState = CheckState.Unchecked,

            AutoSize = true,
        };*/

        CheckBox checkBoxUnchecked = new()
        {
            FlatStyle = FlatStyle.Standard,
            Text = "CheckBox (Button appearance) Standard Unchecked",
            Appearance = Appearance.Button,
            CheckState = CheckState.Unchecked,

            AutoSize = true,
            Location = new Point(1, 0)
        };

        CheckBox checkBoxChecked = new()
        {
            FlatStyle = FlatStyle.Standard,
            Text = "CheckBox (Button appearance) Standard Checked",
            Appearance = Appearance.Button,
            CheckState = CheckState.Checked,

            AutoSize = true,
            Location = new Point(1, 30)
        };

        Button buttonStandard = new()
        {
            FlatStyle = FlatStyle.Standard,
            Text = "Real Button Standard",

            AutoSize = true,
            Location = new Point(1, 60)
        };

        Button buttonStandardPressed = new()
        {
            FlatStyle = FlatStyle.Standard,
            Text = "Real Button Standard Pressed",

            AutoSize = true,
            Location = new Point(1, 90)
        };

        //if (Application.RenderWithVisualStyles
            //&& buttonStandard.FlatStyle == FlatStyle.Standard)
        //{
            //DrawRoundBorder();
        //}
        buttonStandard.Paint += (sender, e) => DrawRoundBorder((Control)sender, e.Graphics);
        buttonStandardPressed.Paint += (sender, e) => DrawRoundBorder((Control)sender, e.Graphics);
        checkBoxUnchecked.Paint += (sender, e) => DrawRoundBorder((Control)sender, e.Graphics);


        void DrawRoundBorder(Control sender, Graphics g)
        {
                GraphicsPath path = new GraphicsPath();
                Pen pen = new Pen(SystemColors.ControlDarkDark, 1);

                int x = sender.ClientRectangle.X + 1;
                int y = sender.ClientRectangle.Y + 1;
                int width = sender.ClientRectangle.Width - 10;
                int height = sender.ClientRectangle.Height - 10;
                const int radius = 8;

                // Top-left corner
                path.AddArc(new Rectangle(x, y, radius, radius), 180, 90);

                // Top-right corner
                path.AddArc(new Rectangle(width, y, radius, radius), 270, 90);

                // Bottom-right corner
                path.AddArc(new Rectangle(width, height, radius, radius), 0, 90);

                // Bottom-left corner
                path.AddArc(new Rectangle(x, height, radius, radius), 90, 90);

                // Back to Top-left corner
                path.AddArc(new Rectangle(x, y, radius, radius), 180, 90);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawPath(pen, path);
        }



        Controls.Add(checkBoxUnchecked);
        Controls.Add(checkBoxChecked);
        Controls.Add(buttonStandard);
        Controls.Add(buttonStandardPressed);
    }

    #endregion

    private DataGridView dataGridView2;
    private DataGridViewTextBoxColumn Column1;
    private DataGridViewTextBoxColumn Column2;
    private DataGridViewTextBoxColumn Column3;
    private DataGridViewTextBoxColumn Column4;
}
