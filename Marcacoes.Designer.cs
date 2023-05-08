
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
            this.tbtitulogoogle = new System.Windows.Forms.TextBox();
            this.tbobs = new System.Windows.Forms.TextBox();
            this.tbhorafim2 = new System.Windows.Forms.TextBox();
            this.tbhorainicio2 = new System.Windows.Forms.TextBox();
            this.tbhorario2 = new System.Windows.Forms.TextBox();
            this.tbtipotratamento = new System.Windows.Forms.TextBox();
            this.tbidgoogle = new System.Windows.Forms.TextBox();
            this.tbdescricao = new System.Windows.Forms.TextBox();
            this.tbcodcliente = new System.Windows.Forms.TextBox();
            this.tbIdMarcacao = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbnomepaciente = new System.Windows.Forms.TextBox();
            this.tbtlmpaciente = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonCheckBox1 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // tbhorario
            // 
            this.tbhorario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbhorario.Location = new System.Drawing.Point(191, 147);
            this.tbhorario.Name = "tbhorario";
            this.tbhorario.Size = new System.Drawing.Size(168, 21);
            this.tbhorario.TabIndex = 31;
            this.tbhorario.ValueNullable = new System.DateTime(2023, 5, 6, 0, 0, 0, 0);
            // 
            // tbhorafim
            // 
            this.tbhorafim.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbhorafim.Location = new System.Drawing.Point(192, 202);
            this.tbhorafim.Name = "tbhorafim";
            this.tbhorafim.ShowUpDown = true;
            this.tbhorafim.Size = new System.Drawing.Size(86, 21);
            this.tbhorafim.TabIndex = 32;
            this.tbhorafim.ValueNullable = new System.DateTime(2023, 5, 6, 12, 30, 0, 0);
            // 
            // tbhorainicio
            // 
            this.tbhorainicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbhorainicio.Location = new System.Drawing.Point(191, 176);
            this.tbhorainicio.Name = "tbhorainicio";
            this.tbhorainicio.ShowUpDown = true;
            this.tbhorainicio.Size = new System.Drawing.Size(87, 21);
            this.tbhorainicio.TabIndex = 33;
            this.tbhorainicio.ValueNullable = new System.DateTime(2023, 5, 6, 8, 0, 0, 0);
            // 
            // btatualizar
            // 
            this.btatualizar.Enabled = false;
            this.btatualizar.Location = new System.Drawing.Point(469, 333);
            this.btatualizar.Name = "btatualizar";
            this.btatualizar.Size = new System.Drawing.Size(132, 68);
            this.btatualizar.TabIndex = 30;
            this.btatualizar.Text = "atualizar";
            this.btatualizar.UseVisualStyleBackColor = true;
            this.btatualizar.Click += new System.EventHandler(this.btatualizar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 260);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "TituloGoogle";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 202);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Hora Fim";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Hora Inicio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Horario";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 388);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "obs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(799, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "IdGoogle";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 338);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "TipoTratamento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "CodCliente";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "descricao";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(810, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "ID";
            // 
            // tbtitulogoogle
            // 
            this.tbtitulogoogle.Location = new System.Drawing.Point(144, 260);
            this.tbtitulogoogle.Name = "tbtitulogoogle";
            this.tbtitulogoogle.Size = new System.Drawing.Size(134, 20);
            this.tbtitulogoogle.TabIndex = 19;
            // 
            // tbobs
            // 
            this.tbobs.Location = new System.Drawing.Point(124, 390);
            this.tbobs.Name = "tbobs";
            this.tbobs.Size = new System.Drawing.Size(134, 20);
            this.tbobs.TabIndex = 18;
            // 
            // tbhorafim2
            // 
            this.tbhorafim2.Location = new System.Drawing.Point(123, 202);
            this.tbhorafim2.Name = "tbhorafim2";
            this.tbhorafim2.Size = new System.Drawing.Size(62, 20);
            this.tbhorafim2.TabIndex = 17;
            // 
            // tbhorainicio2
            // 
            this.tbhorainicio2.Location = new System.Drawing.Point(123, 176);
            this.tbhorainicio2.Name = "tbhorainicio2";
            this.tbhorainicio2.Size = new System.Drawing.Size(62, 20);
            this.tbhorainicio2.TabIndex = 16;
            // 
            // tbhorario2
            // 
            this.tbhorario2.Location = new System.Drawing.Point(123, 147);
            this.tbhorario2.Name = "tbhorario2";
            this.tbhorario2.Size = new System.Drawing.Size(62, 20);
            this.tbhorario2.TabIndex = 15;
            // 
            // tbtipotratamento
            // 
            this.tbtipotratamento.Location = new System.Drawing.Point(124, 340);
            this.tbtipotratamento.Name = "tbtipotratamento";
            this.tbtipotratamento.Size = new System.Drawing.Size(134, 20);
            this.tbtipotratamento.TabIndex = 14;
            // 
            // tbidgoogle
            // 
            this.tbidgoogle.Location = new System.Drawing.Point(757, 93);
            this.tbidgoogle.Name = "tbidgoogle";
            this.tbidgoogle.Size = new System.Drawing.Size(122, 20);
            this.tbidgoogle.TabIndex = 13;
            // 
            // tbdescricao
            // 
            this.tbdescricao.Location = new System.Drawing.Point(144, 286);
            this.tbdescricao.Name = "tbdescricao";
            this.tbdescricao.Size = new System.Drawing.Size(134, 20);
            this.tbdescricao.TabIndex = 12;
            // 
            // tbcodcliente
            // 
            this.tbcodcliente.Location = new System.Drawing.Point(12, 56);
            this.tbcodcliente.Name = "tbcodcliente";
            this.tbcodcliente.Size = new System.Drawing.Size(75, 20);
            this.tbcodcliente.TabIndex = 11;
            // 
            // tbIdMarcacao
            // 
            this.tbIdMarcacao.Location = new System.Drawing.Point(782, 54);
            this.tbIdMarcacao.Name = "tbIdMarcacao";
            this.tbIdMarcacao.Size = new System.Drawing.Size(67, 20);
            this.tbIdMarcacao.TabIndex = 10;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(440, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(359, 51);
            this.button2.TabIndex = 9;
            this.button2.Text = "gravar no google e base de dados";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(469, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 49);
            this.button1.TabIndex = 8;
            this.button1.Text = "gravar no google";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbnomepaciente
            // 
            this.tbnomepaciente.Location = new System.Drawing.Point(124, 56);
            this.tbnomepaciente.Name = "tbnomepaciente";
            this.tbnomepaciente.Size = new System.Drawing.Size(357, 20);
            this.tbnomepaciente.TabIndex = 34;
            // 
            // tbtlmpaciente
            // 
            this.tbtlmpaciente.Location = new System.Drawing.Point(498, 58);
            this.tbtlmpaciente.Name = "tbtlmpaciente";
            this.tbtlmpaciente.Size = new System.Drawing.Size(255, 20);
            this.tbtlmpaciente.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(130, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Nome";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(503, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Telemovel";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(26, 22);
            this.button3.TabIndex = 35;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // kryptonCheckBox1
            // 
            this.kryptonCheckBox1.Location = new System.Drawing.Point(34, 464);
            this.kryptonCheckBox1.Name = "kryptonCheckBox1";
            this.kryptonCheckBox1.Size = new System.Drawing.Size(84, 20);
            this.kryptonCheckBox1.TabIndex = 36;
            this.kryptonCheckBox1.Values.Text = "Enviar SMS";
            // 
            // Marcacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 658);
            this.Controls.Add(this.kryptonCheckBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tbtlmpaciente);
            this.Controls.Add(this.tbnomepaciente);
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
            this.Controls.Add(this.tbtitulogoogle);
            this.Controls.Add(this.tbobs);
            this.Controls.Add(this.tbhorafim2);
            this.Controls.Add(this.tbhorainicio2);
            this.Controls.Add(this.tbhorario2);
            this.Controls.Add(this.tbtipotratamento);
            this.Controls.Add(this.tbidgoogle);
            this.Controls.Add(this.tbdescricao);
            this.Controls.Add(this.tbcodcliente);
            this.Controls.Add(this.tbIdMarcacao);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Marcacoes";
            this.Text = "Marcacoes";
            this.Load += new System.EventHandler(this.Marcacoes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorario;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorafim;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker tbhorainicio;
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
        private System.Windows.Forms.TextBox tbtitulogoogle;
        private System.Windows.Forms.TextBox tbobs;
        private System.Windows.Forms.TextBox tbhorafim2;
        private System.Windows.Forms.TextBox tbhorainicio2;
        private System.Windows.Forms.TextBox tbhorario2;
        private System.Windows.Forms.TextBox tbtipotratamento;
        private System.Windows.Forms.TextBox tbidgoogle;
        private System.Windows.Forms.TextBox tbdescricao;
        private System.Windows.Forms.TextBox tbIdMarcacao;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button3;
        internal System.Windows.Forms.TextBox tbcodcliente;
        internal System.Windows.Forms.TextBox tbnomepaciente;
        internal System.Windows.Forms.TextBox tbtlmpaciente;
        private System.Windows.Forms.Timer timer1;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox kryptonCheckBox1;
    }
}