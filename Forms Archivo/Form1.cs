using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_Archivo
{
    struct Persona
    {
        public string name { get; set; }
        public int year_old { get; set; }
        public int grade {  get; set; }
        public char gender { get; set; }

    }
    public partial class Form1 : Form
    {
        List<Persona> lista;
        public Form1()
        {
            InitializeComponent();
            lista = new List<Persona>();

            dgvPersonas.Columns.Add("name", "Nombre");
            dgvPersonas.Columns.Add("year_old", "Edad");
            dgvPersonas.Columns.Add("grade", "Nota");
            dgvPersonas.Columns.Add("gender", "Género");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FileStream mArchivoEscritor = new FileStream("datos.dat", FileMode.Append, FileAccess.Write))
            using (BinaryWriter escritor = new BinaryWriter(mArchivoEscritor))
            {
                try
                {
                    string nombre = tbName.Text;
                    if (string.IsNullOrEmpty(nombre))
                    {
                        MessageBox.Show("El nombre no puede estar vacío.");
                        return;
                    }

                    if (!int.TryParse(tbYears.Text, out int edad))
                    {
                        MessageBox.Show("La edad debe ser un número entero.");
                        return;
                    }

                    if (!int.TryParse(tbGrade.Text, out int nota))
                    {
                        MessageBox.Show("La nota debe ser un número entero.");
                        return;
                    }

                    if (tbGender.Text.Length != 1)
                    {
                        MessageBox.Show("El género debe ser un solo carácter.");
                        return;
                    }
                    char genero = char.Parse(tbGender.Text);


                    escritor.Write(nombre.Length); // Longitud del nombre
                    escritor.Write(nombre.ToCharArray()); // Nombre
                    escritor.Write(edad);
                    escritor.Write(nota);
                    escritor.Write(genero);

                    MessageBox.Show("Datos guardados correctamente.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar los datos: " + ex.Message);
                }
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            dgvPersonas.Rows.Clear();
            lista.Clear(); // Limpia la lista antes de cargar nuevos datos

            if (!File.Exists("datos.dat"))
            {
                MessageBox.Show("El archivo no existe.");
                return;
            }

            using (FileStream mArchivoLector = new FileStream("datos.dat", FileMode.Open, FileAccess.Read))
            using (BinaryReader Lector = new BinaryReader(mArchivoLector))
            {
                try
                {
                    while (mArchivoLector.Position != mArchivoLector.Length)
                    {
                        Persona persona = new Persona();
                        int length = Lector.ReadInt32(); // Longitud del nombre
                        char[] nombreArray = Lector.ReadChars(length); // Nombre
                       persona.name = new string(nombreArray);
                        persona.name = new string(nombreArray);
                        persona.year_old = Lector.ReadInt32();
                        persona.grade = Lector.ReadInt32();
                        persona.gender = Lector.ReadChar();

                        lista.Add(persona);
                        dgvPersonas.Rows.Add(persona.name, persona.year_old, persona.grade, persona.gender);
                    
                
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
            }


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvPersonas.Rows.Clear();

            // Limpiar el archivo
            if (File.Exists("datos.dat"))
            {
                File.Delete("datos.dat"); // Eliminar el archivo
                MessageBox.Show("Datos borrados correctamente.");
            }
            else
            {
                MessageBox.Show("No hay datos para borrar.");
            }
        }
    }
}
