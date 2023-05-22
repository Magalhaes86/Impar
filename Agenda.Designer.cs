
namespace Impar
{
    partial class Agenda
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBoxNomeEvento = new System.Windows.Forms.TextBox();
            this.textBoxDescricaoEvento = new System.Windows.Forms.TextBox();
            this.textBoxLocalEvento = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.kryptonMonthCalendar1 = new ComponentFactory.Krypton.Toolkit.KryptonMonthCalendar();
            this.kryptonWrapLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.kryptonWrapLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.DtHoraFimTarde = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.DtHoraInicioTarde = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.DtHoraFimManha = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.DtHoraInicioManha = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // textBoxNomeEvento
            // 
            this.textBoxNomeEvento.Location = new System.Drawing.Point(1328, 664);
            this.textBoxNomeEvento.Name = "textBoxNomeEvento";
            this.textBoxNomeEvento.Size = new System.Drawing.Size(61, 20);
            this.textBoxNomeEvento.TabIndex = 6;
            // 
            // textBoxDescricaoEvento
            // 
            this.textBoxDescricaoEvento.Location = new System.Drawing.Point(1261, 654);
            this.textBoxDescricaoEvento.Name = "textBoxDescricaoEvento";
            this.textBoxDescricaoEvento.Size = new System.Drawing.Size(61, 20);
            this.textBoxDescricaoEvento.TabIndex = 6;
            // 
            // textBoxLocalEvento
            // 
            this.textBoxLocalEvento.Location = new System.Drawing.Point(1261, 680);
            this.textBoxLocalEvento.Name = "textBoxLocalEvento";
            this.textBoxLocalEvento.Size = new System.Drawing.Size(61, 20);
            this.textBoxLocalEvento.TabIndex = 6;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToOrderColumns = true;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(395, 138);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(696, 377);
            this.dataGridView2.TabIndex = 77;
            // 
            // kryptonMonthCalendar1
            // 
            this.kryptonMonthCalendar1.Location = new System.Drawing.Point(98, 200);
            this.kryptonMonthCalendar1.Name = "kryptonMonthCalendar1";
            this.kryptonMonthCalendar1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.kryptonMonthCalendar1.Size = new System.Drawing.Size(230, 184);
            this.kryptonMonthCalendar1.TabIndex = 76;
            this.kryptonMonthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.kryptonMonthCalendar1_DateChanged);
            // 
            // kryptonWrapLabel5
            // 
            this.kryptonWrapLabel5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kryptonWrapLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.kryptonWrapLabel5.Location = new System.Drawing.Point(47, 91);
            this.kryptonWrapLabel5.Name = "kryptonWrapLabel5";
            this.kryptonWrapLabel5.Size = new System.Drawing.Size(77, 15);
            this.kryptonWrapLabel5.Text = "Horario tarde";
            // 
            // kryptonWrapLabel6
            // 
            this.kryptonWrapLabel6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kryptonWrapLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.kryptonWrapLabel6.Location = new System.Drawing.Point(37, 55);
            this.kryptonWrapLabel6.Name = "kryptonWrapLabel6";
            this.kryptonWrapLabel6.Size = new System.Drawing.Size(87, 15);
            this.kryptonWrapLabel6.Text = "Horario Manhã";
            // 
            // DtHoraFimTarde
            // 
            this.DtHoraFimTarde.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtHoraFimTarde.Location = new System.Drawing.Point(231, 85);
            this.DtHoraFimTarde.Name = "DtHoraFimTarde";
            this.DtHoraFimTarde.ShowUpDown = true;
            this.DtHoraFimTarde.Size = new System.Drawing.Size(87, 21);
            this.DtHoraFimTarde.TabIndex = 80;
            this.DtHoraFimTarde.ValueNullable = new System.DateTime(2023, 5, 6, 19, 0, 0, 0);
            // 
            // DtHoraInicioTarde
            // 
            this.DtHoraInicioTarde.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtHoraInicioTarde.Location = new System.Drawing.Point(130, 86);
            this.DtHoraInicioTarde.Name = "DtHoraInicioTarde";
            this.DtHoraInicioTarde.ShowUpDown = true;
            this.DtHoraInicioTarde.Size = new System.Drawing.Size(87, 21);
            this.DtHoraInicioTarde.TabIndex = 81;
            this.DtHoraInicioTarde.ValueNullable = new System.DateTime(2023, 5, 22, 14, 0, 0, 0);
            // 
            // DtHoraFimManha
            // 
            this.DtHoraFimManha.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtHoraFimManha.Location = new System.Drawing.Point(232, 53);
            this.DtHoraFimManha.Name = "DtHoraFimManha";
            this.DtHoraFimManha.ShowUpDown = true;
            this.DtHoraFimManha.Size = new System.Drawing.Size(86, 21);
            this.DtHoraFimManha.TabIndex = 82;
            this.DtHoraFimManha.ValueNullable = new System.DateTime(2023, 5, 6, 12, 30, 0, 0);
            // 
            // DtHoraInicioManha
            // 
            this.DtHoraInicioManha.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtHoraInicioManha.Location = new System.Drawing.Point(130, 53);
            this.DtHoraInicioManha.Name = "DtHoraInicioManha";
            this.DtHoraInicioManha.ShowUpDown = true;
            this.DtHoraInicioManha.Size = new System.Drawing.Size(87, 21);
            this.DtHoraInicioManha.TabIndex = 83;
            this.DtHoraInicioManha.ValueNullable = new System.DateTime(2023, 5, 6, 8, 0, 0, 0);
            // 
            // Agenda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1573, 724);
            this.Controls.Add(this.kryptonWrapLabel5);
            this.Controls.Add(this.kryptonWrapLabel6);
            this.Controls.Add(this.DtHoraFimTarde);
            this.Controls.Add(this.DtHoraInicioTarde);
            this.Controls.Add(this.DtHoraFimManha);
            this.Controls.Add(this.DtHoraInicioManha);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.kryptonMonthCalendar1);
            this.Controls.Add(this.textBoxLocalEvento);
            this.Controls.Add(this.textBoxDescricaoEvento);
            this.Controls.Add(this.textBoxNomeEvento);
            this.Name = "Agenda";
            this.Text = "Agenda";
            this.Load += new System.EventHandler(this.Agenda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox textBoxNomeEvento;
        private System.Windows.Forms.TextBox textBoxDescricaoEvento;
        private System.Windows.Forms.TextBox textBoxLocalEvento;
        private System.Windows.Forms.DataGridView dataGridView2;
        private ComponentFactory.Krypton.Toolkit.KryptonMonthCalendar kryptonMonthCalendar1;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel kryptonWrapLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel kryptonWrapLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker DtHoraFimTarde;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker DtHoraInicioTarde;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker DtHoraFimManha;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker DtHoraInicioManha;
    }
}