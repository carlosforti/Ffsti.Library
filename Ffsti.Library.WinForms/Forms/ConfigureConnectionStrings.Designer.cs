namespace Ffsti.Library.WinForms.Forms
{
	partial class ConfigureConnectionStrings
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cboConnectionStrings = new System.Windows.Forms.ComboBox();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cboProviderName = new System.Windows.Forms.ComboBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNome = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(65, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Existing";
			// 
			// cboConnectionStrings
			// 
			this.cboConnectionStrings.FormattingEnabled = true;
			this.cboConnectionStrings.Location = new System.Drawing.Point(114, 12);
			this.cboConnectionStrings.Name = "cboConnectionStrings";
			this.cboConnectionStrings.Size = new System.Drawing.Size(392, 21);
			this.cboConnectionStrings.TabIndex = 1;
			this.cboConnectionStrings.SelectionChangeCommitted += new System.EventHandler(this.cboConnectionStrings_SelectionChangeCommitted);
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Location = new System.Drawing.Point(114, 65);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(392, 20);
			this.txtConnectionString.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Connection String";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(31, 94);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Provider Name";
			// 
			// cboProviderName
			// 
			this.cboProviderName.FormattingEnabled = true;
			this.cboProviderName.Location = new System.Drawing.Point(114, 91);
			this.cboProviderName.Name = "cboProviderName";
			this.cboProviderName.Size = new System.Drawing.Size(392, 21);
			this.cboProviderName.TabIndex = 4;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(431, 118);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(114, 118);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 23);
			this.btnNew.TabIndex = 6;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(73, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Name";
			// 
			// txtNome
			// 
			this.txtNome.Location = new System.Drawing.Point(114, 39);
			this.txtNome.Name = "txtNome";
			this.txtNome.Size = new System.Drawing.Size(392, 20);
			this.txtNome.TabIndex = 7;
			// 
			// ConfigureConnectionStrings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(739, 363);
			this.Controls.Add(this.txtNome);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.cboProviderName);
			this.Controls.Add(this.txtConnectionString);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cboConnectionStrings);
			this.Controls.Add(this.label1);
			this.Name = "ConfigureConnectionStrings";
			this.Text = "ConfigureConnectionStrings";
			this.Load += new System.EventHandler(this.ConfigureConnectionStrings_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboConnectionStrings;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cboProviderName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtNome;
	}
}