using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Empleados
{
    public partial class Form1 : Form
    {
        struct Empleado
        {
            public int NumeroEmpleado;
            public string NombreCompleto;
            public decimal SueldoPorHora;
        }

        List<Empleado> listaEmpleados = new List<Empleado>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNoEmpleado.Text) || string.IsNullOrWhiteSpace(textBoxNomEmpleado.Text) || string.IsNullOrWhiteSpace(textBoxSueldo.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            if (!decimal.TryParse(textBoxSueldo.Text, out decimal sueldo))
            {
                MessageBox.Show("Por favor, ingresa un sueldo válido.");
                return;
            }
            Empleado nuevoEmpleado = new Empleado();
            nuevoEmpleado.NumeroEmpleado = int.Parse(textBoxNoEmpleado.Text);
            nuevoEmpleado.NombreCompleto = textBoxNomEmpleado.Text;
            nuevoEmpleado.SueldoPorHora = sueldo;

            listaEmpleados.Add(nuevoEmpleado);

            ActualizarComboBoxNoEmpleados();

            textBoxNoEmpleado.Clear();
            textBoxNomEmpleado.Clear();
            textBoxSueldo.Clear();

            MessageBox.Show("Empleado guardado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ActualizarComboBoxNoEmpleados()
        {
            comboBoxNoEmpleado.Items.Clear();

            foreach (Empleado empleado in listaEmpleados)
            {
                comboBoxNoEmpleado.Items.Add(empleado.NumeroEmpleado);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
