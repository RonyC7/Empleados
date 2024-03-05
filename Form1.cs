using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            public Dictionary<string, decimal> HorasTrabajadasPorMes;
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

            nuevoEmpleado.HorasTrabajadasPorMes = new Dictionary<string, decimal>();

            listaEmpleados.Add(nuevoEmpleado);

            ActualizarComboBoxNoEmpleados();
            ActualizarComboBoxNombres();

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

        private void ActualizarComboBoxNombres()
        {
            comboBox1.Items.Clear();
            foreach (Empleado empleado in listaEmpleados)
            {
                comboBox1.Items.Add(empleado.NombreCompleto);
            }
        }

        private void comboBoxNoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxNoEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un empleado.");
                return;
            }

            int numeroEmpleadoSeleccionado = (int)comboBoxNoEmpleado.SelectedItem;

            Empleado empleadoSeleccionado = listaEmpleados.FirstOrDefault(emp => emp.NumeroEmpleado == numeroEmpleadoSeleccionado);

            if (!string.IsNullOrWhiteSpace(textBoxMes.Text) && decimal.TryParse(textBoxHorasMes.Text, out decimal horasTrabajadas))
            {
                if (empleadoSeleccionado.HorasTrabajadasPorMes.ContainsKey(textBoxMes.Text))
                {
                    empleadoSeleccionado.HorasTrabajadasPorMes[textBoxMes.Text] += horasTrabajadas;
                }
                else
                {
                    empleadoSeleccionado.HorasTrabajadasPorMes.Add(textBoxMes.Text, horasTrabajadas);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un mes y un valor válido para las horas trabajadas.");
                return;
            }

            MessageBox.Show("Datos del empleado guardados", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            foreach (Empleado empleado in listaEmpleados)
            {
                foreach (var par in empleado.HorasTrabajadasPorMes)
                {
                    decimal salarioMes = empleado.SueldoPorHora * par.Value;
                    dataGridView1.Rows.Add(empleado.NumeroEmpleado, empleado.NombreCompleto, empleado.SueldoPorHora, par.Key, par.Value, salarioMes);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un empleado.");
                return;
            }

            string nombreEmpleadoSeleccionado = comboBox1.SelectedItem.ToString();

            Empleado empleadoSeleccionado = listaEmpleados.FirstOrDefault(emp => emp.NombreCompleto == nombreEmpleadoSeleccionado);

            if (empleadoSeleccionado.Equals(default(Empleado)))
            {
                MessageBox.Show("Empleado no encontrado.");
                return;
            }

            foreach (var par in empleadoSeleccionado.HorasTrabajadasPorMes)
            {
                decimal salarioMes = empleadoSeleccionado.SueldoPorHora * par.Value;
                dataGridView1.Rows.Add(empleadoSeleccionado.NumeroEmpleado, empleadoSeleccionado.NombreCompleto, empleadoSeleccionado.SueldoPorHora, par.Key, par.Value, salarioMes);
            }
        }
    }
}
