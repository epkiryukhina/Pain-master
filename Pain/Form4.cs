using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pain
{
    public partial class Form4 : Form
    {
        public Form1 f1;
        Model1Container db = new Model1Container();
        byte typeOfEntity = 1, typeOfPerson = 0;
        delegate void loadFunction(object sender, EventArgs e);
        loadFunction lf;
        public Employee currentEmployee;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            lf = должностиToolStripMenuItem_Click_1;
            должностиToolStripMenuItem_Click_1(sender, e);
        }

        private void ComboBoxMode(string str, object[] list)
        {
            label3.Text = str;
            comboBox1.Items.AddRange(list);
        }

        private void ClearItems(int n, string[] a)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(a);
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            if (n == 1)
                comboBox1.Items.Clear();
        }

        private void AccessModes(bool a, bool b, bool c, bool d)
        {
            textBox1.Enabled = a;
            textBox2.Enabled = b;
            textBox3.Enabled = c;
            comboBox1.Enabled = d;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void должностиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            typeOfEntity = 1;
            lf = должностиToolStripMenuItem_Click_1;
            ClearItems(1, (from j in db.JobSet select j.JobName).ToArray());
            ComboBoxMode("Вместимость", new string[] { "1", "2", "3", "4", "5", "6", "7" });
            AccessModes(true, false, false, false);
        }

        private void услугиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            typeOfEntity = 2;
            lf = услугиToolStripMenuItem_Click_1;
            ClearItems(1, (from s in db.TypeOfServiceSet select s.Name).ToArray());
            ComboBoxMode("Тип оплаты", (from p in db.TypeOfPriceSet select p.Type).ToArray());
            AccessModes(true, true, false, true);
        }

        private void типыОплатыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            typeOfEntity = 3;
            lf = типыОплатыToolStripMenuItem_Click_1;
            ClearItems(1, (from r in db.TypeOfPriceSet select r.Type).ToArray());
            ComboBoxMode("Вместимость", new string[] { "1", "2", "3", "4", "5", "6", "7" });
            AccessModes(true, false, false, false);
        }

        private void номераToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            typeOfEntity = 4;
            lf = номераToolStripMenuItem_Click_1;
            ClearItems(1, (from r in db.RoomSet select r.Number).ToArray());
            ComboBoxMode("Тип номера", (from t in db.TypeOfRoomSet select t.Type).ToArray());
            AccessModes(false, false, true, true);
        }

        private void типыНомеровToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            typeOfEntity = 5;
            lf = типыНомеровToolStripMenuItem_Click_1;
            ClearItems(1, (from r in db.TypeOfRoomSet select r.Type).ToArray());
            ComboBoxMode("Вместимость", new string[] { "1", "2", "3", "4", "5", "6", "7" });
            AccessModes(true, true, false, true);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if ((textBox1.Text != null) && (typeOfEntity == 1) && !(db.JobSet.Any(b => b.JobName == textBox1.Text)))
            {
                db.JobSet.Add(new Job { JobName = textBox1.Text });
            }

            else if ((textBox1.Text != null) && (comboBox1.SelectedIndex >= 0) && (typeOfEntity == 2) && !(db.TypeOfServiceSet.Any(b => b.Name == textBox1.Text)))
            {
                string str = comboBox1.SelectedItem.ToString();
                TypeOfPrice p = (from q in db.TypeOfPriceSet where q.Type == str select q).ToList()[0];
                db.TypeOfServiceSet.Add(new TypeOfService { Name = textBox1.Text, Price = Int32.Parse(textBox2.Text), TypeOfPrice = p });
            }

            else if ((textBox1.Text != null) && (typeOfEntity == 3) && !(db.TypeOfPriceSet.Any(b => b.Type == textBox1.Text)))
            {
                db.TypeOfPriceSet.Add(new TypeOfPrice { Type = textBox1.Text });
            }

            else if ((comboBox1.SelectedIndex >= 0) && (typeOfEntity == 4) && !(db.RoomSet.Any(b => b.Number == textBox3.Text)))
            {
                string str = comboBox1.SelectedItem.ToString();
                TypeOfRoom t = (from d in db.TypeOfRoomSet where d.Type == str select d).ToList()[0];
                db.RoomSet.Add(new Room { Number = textBox3.Text, TypeOfRoom = t });
            }

            else if ((textBox1.Text != null) && (comboBox1.SelectedIndex >= 0) && (typeOfEntity == 5) && !(db.TypeOfRoomSet.Any(b => b.Type == textBox1.Text)))
            {
                db.TypeOfRoomSet.Add(new TypeOfRoom { Type = textBox1.Text, Price = Int32.Parse(textBox2.Text), Capacity = Int32.Parse(comboBox1.SelectedItem.ToString()) });
            }

            else
                MessageBox.Show("Не все поля заполнены, или элемент с таким названием уже существует!");

            db.SaveChanges();
            lf.Invoke(sender, e);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Будут удалены все связанные данные!");

            foreach (string x in checkedListBox1.CheckedItems)
            {
                if (typeOfEntity == 1)
                {
                    Job j = (from a in db.JobSet where a.JobName == x select a).ToList()[0];
                    List<Employee> p = (from a in db.PersonSet where (a is Employee) && ((a as Employee).Job.Id == j.Id) select (Employee)a).ToList();
                    foreach (Employee curP in p)
                        db.PersonSet.Remove(curP);
                    db.JobSet.Remove(j);
                }

                else if (typeOfEntity == 2)
                {
                    TypeOfService ts = (from a in db.TypeOfServiceSet where a.Name == x select a).ToList()[0];
                    List<Person> p;
                    List<TypeOfPrice> tp;
                    List<Service> s;

                    p = (from a in db.PersonSet where (a is Employee) && ((a as Employee).TypeOfService.Any(b => b.Id == ts.Id)) select a).ToList();
                    foreach (Employee curP in p)
                        curP.TypeOfService.Remove(ts);

                    s = (from a in db.ServiceSet where (a.TypeOfService.Id == ts.Id) select a).ToList();
                    foreach (Service curS in s)
                    {
                        List<Person> c = (from a in db.PersonSet where ((a is Client) && ((a as Client).Service.Any(b => b.Id == curS.Id))) select a).ToList();
                        foreach (Client curC in c)
                            curC.Service.Remove(curS);

                        db.ServiceSet.Remove(curS);
                    }

                    tp = (from a in db.TypeOfPriceSet where (ts.TypeOfPrice.Id == a.Id) select a).ToList();
                    foreach (TypeOfPrice curTP in tp)
                        curTP.TypeOfService.Remove(ts);

                    db.TypeOfServiceSet.Remove(ts);
                }

                else if (typeOfEntity == 3)
                {
                    TypeOfPrice tp = (from a in db.TypeOfPriceSet where a.Type == x select a).ToList()[0];
                    List<TypeOfService> ts = (from a in db.TypeOfServiceSet where (a.TypeOfPrice.Id == tp.Id) select a).ToList();
                    List<Person> p;
                    List<Service> s;

                    foreach (TypeOfService curTS in ts)
                    {
                        p = (from a in db.PersonSet where (a is Employee) && ((a as Employee).TypeOfService.Any(b => b.Id == curTS.Id)) select a).ToList();
                        foreach (Employee curP in p)
                            curP.TypeOfService.Remove(curTS);

                        s = (from a in db.ServiceSet where (a.TypeOfService.Id == curTS.Id) select a).ToList();
                        foreach (Service curS in s)
                        {
                            List<Person> c = (from a in db.PersonSet where ((a is Client) && ((a as Client).Service.Any(b => b.Id == curS.Id))) select a).ToList();
                            foreach (Client curC in c)
                                curC.Service.Remove(curS);

                            db.ServiceSet.Remove(curS);
                        }

                        db.TypeOfServiceSet.Remove(curTS);
                    }

                    db.TypeOfPriceSet.Remove(tp);
                }

                else if (typeOfEntity == 4)
                {
                    Room r = (from a in db.RoomSet where a.Number == x select a).ToList()[0];
                    List<Visit> v = (from a in db.VisitSet where (a.Room.Id == r.Id) select a).ToList();
                    List<TypeOfRoom> tr = (from a in db.TypeOfRoomSet where (r.TypeOfRoom.Id == a.Id) select a).ToList();

                    foreach (Visit curV in v)
                    {
                        List<Person> p= (from a in db.PersonSet where ((a is Client) && ((a as Client).Visit.Any(b => b.Room.Id == curV.Id))) select a).ToList();
                        foreach (Client curP in p)
                            curP.Visit.Remove(curV);

                        db.VisitSet.Remove(curV);
                    }

                    foreach (TypeOfRoom curTR in tr)
                        curTR.Room.Remove(r);

                    db.RoomSet.Remove(r);
                }

                else if (typeOfEntity == 5)
                {
                    TypeOfRoom tr = (from a in db.TypeOfRoomSet where a.Type == x select a).ToList()[0];
                    List<Room> r = (from a in db.RoomSet where (a.TypeOfRoom.Id == tr.Id) select a).ToList();                   

                    foreach (Room curR in r)
                    {
                        List<Visit> v = (from a in db.VisitSet where (a.Room.Id == curR.Id) select a).ToList();
                        foreach (Visit curV in v)
                        {
                            List<Person> p = (from a in db.PersonSet where ((a is Client) && ((a as Client).Visit.Any(b => b.Room.Id == curV.Id))) select a).ToList();
                            foreach (Client curP in p)
                                curP.Visit.Remove(curV);

                            db.VisitSet.Remove(curV);
                        }
                        db.RoomSet.Remove(curR);
                    }

                    db.TypeOfRoomSet.Remove(tr);
                }
            }

            db.SaveChanges();
            lf.Invoke(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            typeOfPerson = 1;
            dataGridView1.DataSource = (from a in db.PersonSet where a is Client select new { a.Id, a.Name, a.Passport, a.DateBirth, a.Sex, (a as Client).Debt }).ToArray();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            typeOfPerson = 2;
            dataGridView1.DataSource = (from a in db.PersonSet where a is Employee select new {a.Id, a.Name, a.Passport, a.DateBirth, a.Sex, (a as Employee).Job.JobName }).ToArray();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Hide();
            Form3 f = new Form3();
            f.f1 = this.f1;
            f.currentEmployee = currentEmployee;
            f.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Hide();
            Form1 f = this.f1;
            f.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1.Close();
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar)) || (e.KeyChar == 8)) return;
            else
                e.Handled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (typeOfPerson != 0)
            {
                DataGridView dgv = dataGridView1;
                Excel.Application exApp = new Excel.Application();
                exApp.Visible = true;
                exApp.Workbooks.Add();
                Excel.Worksheet workSheet = (Excel.Worksheet)exApp.ActiveSheet;
                workSheet.Cells[1, 1] = "ID";
                workSheet.Cells[1, 2] = "Name";
                workSheet.Cells[1, 3] = "Passport";
                workSheet.Cells[1, 4] = "DateBirth";
                workSheet.Cells[1, 5] = "Sex";
                if (typeOfPerson == 1)
                    workSheet.Cells[1, 6] = "Debt";
                else
                    workSheet.Cells[1, 6] = "Job";

                int rowExcel = 2;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    workSheet.Cells[rowExcel, "A"] = dgv.Rows[i].Cells["ID"].Value;
                    workSheet.Cells[rowExcel, "B"] = dgv.Rows[i].Cells["Name"].Value;
                    workSheet.Cells[rowExcel, "C"] = dgv.Rows[i].Cells["Passport"].Value;
                    workSheet.Cells[rowExcel, "D"] = dgv.Rows[i].Cells["DateBirth"].Value;
                    workSheet.Cells[rowExcel, "E"] = dgv.Rows[i].Cells["Sex"].Value;
                    if (typeOfPerson == 1)
                        workSheet.Cells[rowExcel, "F"] = dgv.Rows[i].Cells["Debt"].Value;
                    else
                        workSheet.Cells[rowExcel, "F"] = dgv.Rows[i].Cells["JobName"].Value;

                    ++rowExcel;
                }
                workSheet.SaveAs("MyFile.xls");
                exApp.Quit();
            }
        }
    }
}
