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
                            string mes = datos[2];
                            decimal horasTrabajadas = decimal.Parse(datos[1]);
                            if (empleadoSeleccionado.HorasTrabajadasPorMes.ContainsKey(mes))
                            {
                                empleadoSeleccionado.HorasTrabajadasPorMes[mes] += horasTrabajadas;
                            }
                            else
                            {
                                empleadoSeleccionado.HorasTrabajadasPorMes.Add(mes, horasTrabajadas);
                            }
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

            if (empleadoSeleccionado.Equals(default(Empleado)))
            {
                MessageBox.Show("Empleado no encontrado.");
                return;
            }

            if (!decimal.TryParse(textBoxHorasMes.Text, out decimal horasTrabajadas))
            {
                MessageBox.Show("Por favor, ingresa un valor numérico válido para las horas trabajadas.");
                return;
            }

            string mesTrabajo = textBoxMes.Text;
            if (string.IsNullOrWhiteSpace(mesTrabajo))
            {
                MessageBox.Show("Por favor, ingresa un valor válido para el mes trabajado.");
                return;
            }

            if (empleadoSeleccionado.HorasTrabajadasPorMes.ContainsKey(mesTrabajo))
            {
                empleadoSeleccionado.HorasTrabajadasPorMes[mesTrabajo] += horasTrabajadas;
            }
            else
            {
                empleadoSeleccionado.HorasTrabajadasPorMes.Add(mesTrabajo, horasTrabajadas);
            }

            MessageBox.Show("Datos del empleado guardados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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



        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
