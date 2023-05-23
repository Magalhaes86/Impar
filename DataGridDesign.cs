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
        }

    }
}
