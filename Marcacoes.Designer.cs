
namespace Impar
{
    partial class Marcacoes
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
            this.components = new System.ComponentModel.Container();
            this.tbhorario = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.tbhorafim = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.tbhorainicio = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.btatualizar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonCheckBox1 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.tbdescricao = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbtitulogoogle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbcodcliente = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbnomepaciente = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbtlmpaciente = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbIdMarcacao = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbidgoogle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbtipotratamento = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.tbobs = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.button3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.tbidtipotratamento = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // tbhorario
            // 
            this.tbhorario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbhorario.Location = new System.Drawing.Point(95, 141);
            this.tbhorario.Name = "tbhorario";
            this.tbhorario.Size = new System.Drawing.Size(118, 21);
            this.tbhorario.TabIndex = 31;
            this.tbhorario.ValueNullable = new System.DateTime(2023, 5, 6, 0, 0, 0, 0);
            // 
            // tbhorafim
            // 
            this.tbhorafim.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbhorafim.Location = new System.Drawing.Point(96, 196);
            this.tbhorafim.Name = "tbhorafim";
            this.tbhorafim.ShowUpDown = true;
            this.tbhorafim.Size = new System.Drawing.Size(117, 21);
            this.tbhorafim.TabIndex = 32;
            this.tbhorafim.ValueNullable = new System.DateTime(2023, 5, 6, 12, 30, 0, 0);
            // 
            // tbhorainicio
            // 
            this.tbhorainicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbhorainicio.Location = new System.Drawing.Point(95, 170);
            this.tbhorainicio.Name = "tbhorainicio";
            this.tbhorainicio.ShowUpDown = true;
            this.tbhorainicio.Size = new System.Drawing.Size(118, 21);
            this.tbhorainicio.TabIndex = 33;
            this.tbhorainicio.ValueNullable = new System.DateTime(2023, 5, 6, 8, 0, 0, 0);
            // 
            // btatualizar
            // 
            this.btatualizar.Enabled = false;
            this.btatualizar.Location = new System.Drawing.Point(399, 486);
            this.btatualizar.Name = "btatualizar";
            this.btatualizar.Size = new System.Drawing.Size(132, 38);
            this.btatualizar.TabIndex = 30;
            this.btatualizar.Text = "atualizar";
            this.btatualizar.UseVisualStyleBackColor = true;
            this.btatualizar.Click += new System.EventHandler(this.btatualizar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(245, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "TituloGoogle";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 196);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Hora Fim";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Hora Inicio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Horario";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 326);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "obs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "IdGoogle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 271);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "TipoTratamento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "CodCliente";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Descrição Google";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "ID";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(49, 565);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(359, 51);
            this.button2.TabIndex = 9;
            this.button2.Text = "gravar no google e base de dados";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(34, 486);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 38);
            this.button1.TabIndex = 8;
            this.button1.Text = "gravar no google";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(165, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Nome";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(443, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Telemovel";
            // 
            // kryptonCheckBox1
            // 
            this.kryptonCheckBox1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            this.kryptonCheckBox1.Location = new System.Drawing.Point(58, 421);
            this.kryptonCheckBox1.Name = "kryptonCheckBox1";
            this.kryptonCheckBox1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonCheckBox1.Size = new System.Drawing.Size(88, 22);
            this.kryptonCheckBox1.TabIndex = 36;
            this.kryptonCheckBox1.Values.Text = "Enviar SMS";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(178, 490);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(145, 34);
            this.button4.TabIndex = 44;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tbdescricao
            // 
            this.tbdescricao.Location = new System.Drawing.Point(322, 186);
            this.tbdescricao.Name = "tbdescricao";
            this.tbdescricao.Size = new System.Drawing.Size(241, 23);
            this.tbdescricao.TabIndex = 45;
            // 
            // tbtitulogoogle
            // 
            this.tbtitulogoogle.Location = new System.Drawing.Point(322, 152);
            this.tbtitulogoogle.Name = "tbtitulogoogle";
            this.tbtitulogoogle.Size = new System.Drawing.Size(241, 23);
            this.tbtitulogoogle.TabIndex = 46;
            // 
            // tbcodcliente
            // 
            this.tbcodcliente.Location = new System.Drawing.Point(25, 88);
            this.tbcodcliente.Name = "tbcodcliente";
            this.tbcodcliente.Size = new System.Drawing.Size(90, 23);
            this.tbcodcliente.TabIndex = 47;
            // 
            // tbnomepaciente
            // 
            this.tbnomepaciente.Location = new System.Drawing.Point(158, 88);
            this.tbnomepaciente.Name = "tbnomepaciente";
            this.tbnomepaciente.Size = new System.Drawing.Size(270, 23);
            this.tbnomepaciente.TabIndex = 48;
            // 
            // tbtlmpaciente
            // 
            this.tbtlmpaciente.Location = new System.Drawing.Point(434, 90);
            this.tbtlmpaciente.Name = "tbtlmpaciente";
            this.tbtlmpaciente.Size = new System.Drawing.Size(279, 23);
            this.tbtlmpaciente.TabIndex = 49;
            // 
            // tbIdMarcacao
            // 
            this.tbIdMarcacao.Location = new System.Drawing.Point(58, 26);
            this.tbIdMarcacao.Name = "tbIdMarcacao";
            this.tbIdMarcacao.Size = new System.Drawing.Size(101, 23);
            this.tbIdMarcacao.TabIndex = 50;
            // 
            // tbidgoogle
            // 
            this.tbidgoogle.Location = new System.Drawing.Point(261, 26);
            this.tbidgoogle.Name = "tbidgoogle";
            this.tbidgoogle.Size = new System.Drawing.Size(147, 23);
            this.tbidgoogle.TabIndex = 51;
            // 
            // tbtipotratamento
            // 
            this.tbtipotratamento.Location = new System.Drawing.Point(230, 271);
            this.tbtipotratamento.Name = "tbtipotratamento";
            this.tbtipotratamento.Size = new System.Drawing.Size(215, 23);
            this.tbtipotratamento.TabIndex = 52;
            // 
            // tbobs
            // 
            this.tbobs.Location = new System.Drawing.Point(108, 316);
            this.tbobs.Name = "tbobs";
            this.tbobs.Size = new System.Drawing.Size(191, 23);
            this.tbobs.TabIndex = 53;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(119, 87);
            this.button3.Name = "button3";
            this.button3.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.button3.Size = new System.Drawing.Size(31, 25);
            this.button3.TabIndex = 54;
            this.button3.Values.Text = "---";
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // tbidtipotratamento
            // 
            this.tbidtipotratamento.Location = new System.Drawing.Point(103, 271);
            this.tbidtipotratamento.Name = "tbidtipotratamento";
            this.tbidtipotratamento.Size = new System.Drawing.Size(84, 23);
            this.tbidtipotratamento.TabIndex = 52;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(193, 269);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Orientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.kryptonButton1.Size = new System.Drawing.Size(31, 25);
            this.kryptonButton1.TabIndex = 54;
            this.kryptonButton1.Values.Text = "---";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // Marcacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 658);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tbobs);
            this.Controls.Add(this.tbidtipotratamento);
            this.Controls.Add(this.tbtipotratamento);
            this.Controls.Add(this.tbidgoogle);
            this.Controls.Add(this.tbIdMarcacao);
            this.Controls.Add(this.tbtlmpaciente);
            this.Controls.Add(this.tbnomepaciente);
            this.Controls.Add(this.tbcodcliente);
            this.Controls.Add(this.tbtitulogoogle);
            this.Controls.Add(this.tbdescricao);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.kryptonCheckBox1);
            this.Controls.Add(this.tbhorario);
            this.Controls.Add(this.tbhorafim);
            this.Controls.Add(this.tbhorainicio);
            this.Controls.Add(this.btatualizar);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Marcacoes";
            this.Text = "Marcacoes";
            this.Load += new System.EventHandler(this.Marcacoes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btatualizar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button4;
        internal ComponentFactory.Krypton.Toolkit.KryptonCheckBox kryptonCheckBox1;
        public ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorario;
        public ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorafim;
        public ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorainicio;
        private ComponentFactory.Krypton.Toolkit.KryptonButton button3;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbdescricao;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbtitulogoogle;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbcodcliente;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbnomepaciente;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbtlmpaciente;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbIdMarcacao;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbidgoogle;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbtipotratamento;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbobs;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox tbidtipotratamento;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
    }
}