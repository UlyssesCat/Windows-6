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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfMySQL
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string accountId = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string str = "Server=localhost;User ID=root;Password=****;Database=t;CharSet=gbk";

            if (accountId == "" || password == "")
            {
                MessageBox.Show("ID or password can't be empty");
                textBox1.Text = "";
                textBox2.Text = "";
            }

            MySqlConnection con = new MySqlConnection(str);
            con.Open();
            DataSet ds = new DataSet("user");
            string strcmd = "select * from user";
            MySqlCommand cmd = new MySqlCommand(strcmd, con);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            ada.Fill(ds);//查询结果填充数据集
            int flag = 0;
            int fla = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                if (accountId == dr["accountID"].ToString() && password == ds.Tables[0].Rows[i]["password"].ToString())
                {
                    fla = 1;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    Window1 w = new Window1();
                    w.Show();
                    this.Hide();
                    con.Close();
                    break;


                }
            }
            if (flag == 0&&fla==0)
            {
                MessageBox.Show("password is wrong");
                textBox2.Text = "";
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}