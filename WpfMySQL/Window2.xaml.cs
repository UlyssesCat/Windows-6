using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        string[] ss=new string[6];
        MySqlConnection con;
        public Window2(Window1 window1,string[] s)
        {
            InitializeComponent();
            string str = "Server=localhost;User ID=root;Password=****;Database=t;CharSet=gbk";
            con = new MySqlConnection(str);
            con.Open();
            for (int i = 0; i < 6; i++)
            {
                ss[i] = s[i];
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label.Content = ss[0];
            label_Copy.Content = ss[1];
            label_Copy1.Content = ss[2];
            label_Copy2.Content = ss[3];
            label_Copy3.Content = ss[4];
            label_Copy4.Content = ss[5];





            textBox1.Text = ss[0];
            textBox2.Text = ss[1];
            textBox3.Text = ss[2];
            textBox4.Text = ss[3];
            textBox5.Text = ss[4];
            textBox6.Text = ss[5];

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try { 
                string s1 = textBox1.Text;
                string s2 = "\"" + textBox2.Text + "\"";
                string s3 = "\"" + textBox3.Text + "\"";
                string s4 = "\"" + textBox4.Text + "\"";
                string s5 = "\"" + textBox5.Text + "\"";
                string s6 = textBox6.Text;

                string strcmd = "update book set BookID = " + s1 + ",BookName = " + s2 + ",AuthorXing = " + s3 + ",AuthorMing = " + s4 + ",Publish = " + s5 + ",Price = " + s6 + " where BookID ="+ s1+";";
                MySqlCommand cmd = new MySqlCommand(strcmd, con);
                cmd.ExecuteNonQuery();
                Window1.ds.Clear();
                string strcmd2 = "select * from book";
                MySqlCommand cmd2 = new MySqlCommand(strcmd2, con);
                MySqlDataAdapter ada2 = new MySqlDataAdapter(cmd2);
                ada2.Fill(Window1.ds);//查询结果填充数据集


                MessageBoxResult result = MessageBox.Show("修改成功，退出界面", "", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            con.Close();
        }
    }
}
