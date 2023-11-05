namespace co2_sensor
{
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
      components = new System.ComponentModel.Container();
      label1 = new Label();
      timer1 = new System.Windows.Forms.Timer(components);
      AvgLabel = new Label();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(25, 32);
      label1.TabIndex = 0;
      label1.Text = "?";
      // 
      // timer1
      // 
      timer1.Enabled = true;
      timer1.Interval = 5000;
      timer1.Tick += timer1_Tick;
      // 
      // AvgLabel
      // 
      AvgLabel.AutoSize = true;
      AvgLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
      AvgLabel.Location = new Point(12, 41);
      AvgLabel.Name = "AvgLabel";
      AvgLabel.Size = new Size(58, 32);
      AvgLabel.TabIndex = 1;
      AvgLabel.Text = "Avg";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(305, 83);
      Controls.Add(AvgLabel);
      Controls.Add(label1);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      Name = "Form1";
      Text = "CO2";
      TopMost = true;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private System.Windows.Forms.Timer timer1;
    private Label AvgLabel;
  }
}