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
    /// ModifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyWindow : Window
    {
        
        MySqlConnection con;
        public ModifyWindow()
        {
            InitializeComponent();
            string str = "Server=localhost;User ID=root;Password=****;Database=t;CharSet=gbk";
            con = new MySqlConnection(str);
            con.Open();
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
                string strcmd = "insert into book values (" + s1 + "," + s2 + "," + s3+ "," + s4+ "," + s5+ "," + s6+ ");";
                MySqlCommand cmd = new MySqlCommand(strcmd, con);
                cmd.ExecuteNonQuery();
                Window1.ds.Tables[0].Rows.Add(Convert.ToInt32(textBox1.Text),textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, float.Parse(textBox6.Text));
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            con.Close();
        }
    }
}
