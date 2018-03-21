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
    public partial class Form1 : Form
    {
        Model1Container db = new Model1Container();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if ((from a in db.JobSet where (a.JobName == "Администратор") select a).ToList().Capacity == 0)
                db.JobSet.Add(new Job { JobName = "Администратор", Id = 1 });

            if ((from a in db.TypeOfPriceSet where (a.Type == "Бесплатно") select a).ToList().Capacity == 0)
                db.TypeOfPriceSet.Add(new TypeOfPrice { Type = "Бесплатно", Id = 1 });

            if ((from a in db.TypeOfServiceSet where (a.Name == "Администрация") select a).ToList().Capacity == 0)
                db.TypeOfServiceSet.Add(new TypeOfService { Name = "Администрация", Id = 1 , TypeOfPrice = db.TypeOfPriceSet.Find(1), Price = 0 });

            db.SaveChanges();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<Person> list = (from d in db.PersonSet where d.Passport == textBox1.Text && d.Password == textBox2.Text select d).ToList();
            if (list.Capacity != 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";

                Person p = list[0];
                if ((p is Employee) && (p as Employee).Job == db.JobSet.Find(1))
                {
                    Hide();
                    Form4 f = new Form4();
                    f.f1 = this;
                    f.currentEmployee = (Employee)p;
                    f.Show();
                }
                else if (p is Employee)
                {
                    Hide();
                    Form3 f = new Form3();
                    f.f1 = this;
                    f.currentEmployee = (Employee)p;
                    f.Show();
                }
                else
                {
                    Hide();
                    Form5 f = new Form5();
                    f.f1 = this;
                    f.currentClient = (Client)p;
                    f.Show();
                }
            }
            else
                MessageBox.Show("Неверный логин или пароль");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Hide();
            Form2 f = new Form2();
            f.f1 = this;
            f.Show();
        }
    }
}
