using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pain
{
    public partial class Form3 : Form
    {
        public Form1 f1;
        Model1Container db = new Model1Container();
        public Employee currentEmployee;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = currentEmployee.Name;
            comboBox2.Items.AddRange((from a in db.JobSet select a.JobName).ToArray());
            checkedListBox1.Items.AddRange((from a in db.TypeOfServiceSet select a.Name).ToArray());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if ((currentEmployee.Job.JobName == "Администратор") && (currentEmployee.TypeOfService.Any(b => b.Name == "Администрация")))
            {
                Hide();
                Form4 f = new Form4();
                f.currentEmployee = currentEmployee;
                f.f1 = this.f1;
                f.Show();
            }
            else
            {
                Hide();
                Form1 f = this.f1;
                f.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (monthCalendar1.SelectionRange != null)
                dataGridView1.DataSource = (from a in db.ServiceSet where ((a.Date >= monthCalendar1.SelectionStart) && (a.Date <= monthCalendar1.SelectionEnd)) select new { a.TypeOfService.Name, a.Date, a.TypeOfService.TypeOfPrice, a.TypeOfService.Price, a.NumberOfHours, a.NumberOfPeople, }).ToArray();
            else
                MessageBox.Show("Выберите даты!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex >= 0)
            {
                string str = comboBox2.SelectedItem.ToString();
                ((from a in db.PersonSet where (a.Id == currentEmployee.Id) select a).ToList()[0] as Employee).Job = (from a in db.JobSet where (a.JobName == str) select a).ToList()[0];
                comboBox2.Text = "";
            }

            if (checkedListBox1.CheckedItems != null)
            {
                List<TypeOfService> ts = new List<TypeOfService>();

                foreach (string curS in checkedListBox1.SelectedItems)
                    ts.Add((from a in db.TypeOfServiceSet where (a.Name == curS) select a).ToList()[0]);

                ((from a in db.PersonSet where (a.Id == currentEmployee.Id) select a).ToList()[0] as Employee).TypeOfService = ts;
            }

            if (textBox3.Text != "")
            {
                (from a in db.PersonSet where a.Id == currentEmployee.Id select a).ToList()[0].Password = textBox3.Text;
                textBox3.Clear();
            }

            db.SaveChanges();
            MessageBox.Show("Изменения сохранены.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Person p = (from a in db.PersonSet where (a.Id == currentEmployee.Id) select a).ToList()[0];

            Job j = (from a in db.JobSet where (a.Id == currentEmployee.Job.Id) select a).ToList()[0];
            j.Employee.Remove(currentEmployee);

            List<TypeOfService> ts = (from a in db.TypeOfServiceSet select a).ToList();
            foreach (TypeOfService curTS in ts)
                if (currentEmployee.TypeOfService.Any(b => b.Id == curTS.Id))
                curTS.Employee.Remove(currentEmployee);

            db.PersonSet.Remove(p);

            db.SaveChanges();

            Hide();
            Form1 f = this.f1;
            f.Show();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Close();
        }
    }
}
