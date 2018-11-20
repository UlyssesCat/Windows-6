using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfMySQL
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public static DataSet ds;
        MySqlConnection con;
        public Window1()
        {
            InitializeComponent();

            string str = "Server=localhost;User ID=root;Password=****;Database=t;CharSet=gbk";
            con = new MySqlConnection(str);
            con.Open();
            ds = new DataSet("Book");
            string strcmd = "select * from book";
            MySqlCommand cmd = new MySqlCommand(strcmd, con);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            ada.Fill(ds);//查询结果填充数据集
       
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            ModifyWindow mW = new ModifyWindow();
            mW.Show();
            //this.Hide();

        }

        private void dataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            
          
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                var item = dataGrid.SelectedItem;
                var id = item as DataRowView;
                bookID = id.Row[0].ToString();

                string strcmd = "delete from book where BookID=" + bookID + ";";
                MySqlCommand cmd = new MySqlCommand(strcmd, con);
                cmd.ExecuteNonQuery();
                //ds.Tables[0].Rows.Add(Convert.ToInt32(textBox1.Text), textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, float.Parse(textBox6.Text));
                ds.Clear();
                string strcmd2 = "select * from book";
                MySqlCommand cmd2 = new MySqlCommand(strcmd2, con);
                MySqlDataAdapter ada2 = new MySqlDataAdapter(cmd2);
                ada2.Fill(ds);//查询结果填充数据集
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        string bookID="";
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           
          
        }

        static string change;
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var item = dataGrid.SelectedItem;
                var id = item as DataRowView;
                string[] s = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    s[i] = id.Row[i].ToString();
                }
                Window2 window2 = new Window2(this, s);
                window2.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           // btn4_Click(sender, e);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s1 = comboBox.Text;
                string s2 = textBox.Text;
                string strcmd2 = "";

                if (comboBox.Text == "BookID" || comboBox.Text == "Price")
                {
                    strcmd2 = "select * from book where " + s1 + " = " + s2 + " ;";
                    if (checkBox.IsChecked == true) strcmd2 = "select * from book where " + s1 + " like '%" + s2 + "%' ;";
                    
                }
                else
                {
                    strcmd2 = "select * from book where " + s1 + " = \"" + s2 + "\" ;";
                    if (checkBox.IsChecked == true) strcmd2 = "select * from book where " + s1 + " like '%" + s2 + "%' ;";
                    
                }
                if (s2 == "") strcmd2 = "select * from book";
                MySqlCommand cmd2 = new MySqlCommand(strcmd2, con);
                cmd2.ExecuteNonQuery();
                ds.Clear();
                MySqlDataAdapter ada2 = new MySqlDataAdapter(cmd2);
                ada2.Fill(ds);//查询结果填充数据集
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
