﻿
namespace Impar
{
    partial class listagemPacientesMarcacoes
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
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.tbpesquisaId = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbpesquisanome = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbpesquisatlm = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbpesquisatlf = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.kryptonDataGridView1.Location = new System.Drawing.Point(12, 135);
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.Size = new System.Drawing.Size(1268, 431);
            this.kryptonDataGridView1.TabIndex = 0;
            this.kryptonDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellClick);
            this.kryptonDataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView1_CellContentClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1134, 589);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 44);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel4);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel3);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.tbpesquisatlf);
            this.kryptonGroupBox1.Panel.Controls.Add(this.tbpesquisatlm);
            this.kryptonGroupBox1.Panel.Controls.Add(this.tbpesquisanome);
            this.kryptonGroupBox1.Panel.Controls.Add(this.tbpesquisaId);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(1268, 104);
            this.kryptonGroupBox1.TabIndex = 2;
            // 
            // tbpesquisaId
            // 
            this.tbpesquisaId.Location = new System.Drawing.Point(19, 37);
            this.tbpesquisaId.Name = "tbpesquisaId";
            this.tbpesquisaId.Size = new System.Drawing.Size(206, 23);
            this.tbpesquisaId.TabIndex = 0;
            this.tbpesquisaId.Tag = "";
            this.tbpesquisaId.TextChanged += new System.EventHandler(this.tbpesquisaId_TextChanged);
            // 
            // tbpesquisanome
            // 
            this.tbpesquisanome.Location = new System.Drawing.Point(256, 37);
            this.tbpesquisanome.Name = "tbpesquisanome";
            this.tbpesquisanome.Size = new System.Drawing.Size(206, 23);
            this.tbpesquisanome.TabIndex = 0;
            this.tbpesquisanome.Tag = "Procura por Nome";
            this.tbpesquisanome.TextChanged += new System.EventHandler(this.tbpesquisanome_TextChanged);
            // 
            // tbpesquisatlm
            // 
            this.tbpesquisatlm.Location = new System.Drawing.Point(489, 37);
            this.tbpesquisatlm.Name = "tbpesquisatlm";
            this.tbpesquisatlm.Size = new System.Drawing.Size(206, 23);
            this.tbpesquisatlm.TabIndex = 0;
            this.tbpesquisatlm.TextChanged += new System.EventHandler(this.tbpesquisatlm_TextChanged);
            // 
            // tbpesquisatlf
            // 
            this.tbpesquisatlf.Location = new System.Drawing.Point(717, 37);
            this.tbpesquisatlf.Name = "tbpesquisatlf";
            this.tbpesquisatlf.Size = new System.Drawing.Size(206, 23);
            this.tbpesquisatlf.TabIndex = 0;
            this.tbpesquisatlf.TextChanged += new System.EventHandler(this.tbpesquisatlf_TextChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(19, 11);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(35, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "Cod.";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(256, 11);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(44, 20);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "Nome";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(489, 11);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel3.TabIndex = 1;
            this.kryptonLabel3.Values.Text = "Telm.";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(717, 11);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(27, 20);
            this.kryptonLabel4.TabIndex = 1;
            this.kryptonLabel4.Values.Text = "Tlf.";
            // 
            // listagemPacientesMarcacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1433, 727);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.kryptonDataGridView1);
            this.Name = "listagemPacientesMarcacoes";
            this.Text = "listagemPacientesMarcacoes";
            this.Load += new System.EventHandler(this.listagemPacientesMarcacoes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView1;
        private System.Windows.Forms.Button button1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbpesquisatlf;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbpesquisatlm;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbpesquisanome;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbpesquisaId;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
    }
}