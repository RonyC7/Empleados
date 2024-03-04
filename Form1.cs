using System;
using System.Collections.Generic;
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
            public decimal HorasTrabajadas;
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

        private void comboBoxNoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxNoEmpleado.SelectedIndex != -1)
            {
                int numeroEmpleadoSeleccionado = (int)comboBoxNoEmpleado.SelectedItem;
                Empleado empleadoSeleccionado = listaEmpleados.FirstOrDefault(emp => emp.NumeroEmpleado == numeroEmpleadoSeleccionado);
                if (empleadoSeleccionado.NumeroEmpleado != 0)
                {
                    string[] lineas = File.ReadAllLines("horas_trabajadas.txt");
                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split(',');
                        int noEmpleado = int.Parse(datos[0]);
                        if (noEmpleado == empleadoSeleccionado.NumeroEmpleado)
                        {
                            empleadoSeleccionado.HorasTrabajadas += decimal.Parse(datos[1]);
                        }
                    }
                }
            }
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

            decimal salarioTotal = empleadoSeleccionado.SueldoPorHora * empleadoSeleccionado.HorasTrabajadas;

            MessageBox.Show($"El salario total del empleado seleccionado es: {salarioTotal.ToString("C")}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (comboBoxNoEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona un empleado.");
                return;
            }

            int numeroEmpleadoSeleccionado = (int)comboBoxNoEmpleado.SelectedItem;

            Empleado empleadoSeleccionado = listaEmpleados.FirstOrDefault(emp => emp.NumeroEmpleado == numeroEmpleadoSeleccionado);

            decimal salarioTotal = empleadoSeleccionado.SueldoPorHora * empleadoSeleccionado.HorasTrabajadas;

            dataGridView1.Rows.Add(empleadoSeleccionado.NumeroEmpleado, empleadoSeleccionado.NombreCompleto, empleadoSeleccionado.SueldoPorHora, empleadoSeleccionado.HorasTrabajadas, salarioTotal);
        }
    }
}
