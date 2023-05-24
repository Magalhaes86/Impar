using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Impar
{
    internal class DataGridDesign
    {

        // !!!!!!!!!!!!! EXEMPLO DE COMO CHAMAR ESTA CLASS EM OUTROS FORMULARIOS !!!!!!!!!!!!

        // DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5); // Aplica as personalizações ao KryptonDataGridView5
        //DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2); // Aplica as personalizações ao KryptonDataGridView2

        // !!!!!!!!!!!!! EXEMPLO DE COMO CHAMAR ESTA CLASS EM OUTROS FORMULARIOS !!!!!!!!!!!!


        public static void CustomizeKryptonDataGridView5(KryptonDataGridView dataGridView)
        {
            dataGridView.RowTemplate.Height = 40; // Define a altura das linhas para 40 pixels
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centraliza o conteúdo das células
         
        }

        public static void CustomizeKryptonDataGridView2(KryptonDataGridView dataGridView)
        {
            dataGridView.RowTemplate.Height = 40; // Define a altura das linhas para 40 pixels
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centraliza o conteúdo das células

            if (dataGridView.Columns.Count >= 8)
            {
                dataGridView.Columns[0].Width = 80;
                dataGridView.Columns[1].Width = 80;
                dataGridView.Columns[2].Width = 80;
                dataGridView.Columns[3].Width = 80;
                dataGridView.Columns[4].Width = 80;
                dataGridView.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView.Columns[7].Width = 80;

                dataGridView.ColumnWidthChanged += DataGridView_ColumnWidthChanged;
            }
            else
            {
                Console.WriteLine("O número de colunas no KryptonDataGridView é menor do que o esperado.");
            }
        }

        private static void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            const int minColumnWidth = 95; // Define o tamanho mínimo da largura da coluna
            const int maxColumnWidth = 160; // Define o tamanho máximo da largura da coluna

            if (e.Column.Index == 5 || e.Column.Index == 6)
            {
                if (e.Column.Width < minColumnWidth)
                {
                    e.Column.Width = minColumnWidth;
                }
                else if (e.Column.Width > maxColumnWidth)
                {
                    e.Column.Width = maxColumnWidth;
                }
            }
        }
    }
}




