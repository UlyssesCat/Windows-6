using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using Microsoft.Win32;

namespace Windows_6
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable dt = null;
        static string path = "";
        public MainWindow()
        {
            InitializeComponent();
           
        }

        public static DataTable ReadData()    
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=C:\\Users\\91950\\source\\repos\\Windows-6\\Windows-6\\bin\\Debug\\1.xlsx;" + "Extended Properties=Excel 12.0;";   
            string strExcel = "select * from [Sheet1$]";                                
            OleDbConnection ole = new OleDbConnection(strConn);
            ole.Open();                                                                                      
            DataTable schemaTable = new DataTable();
            OleDbDataAdapter odp = new OleDbDataAdapter(strExcel, strConn);
            odp.Fill(schemaTable);
            ole.Close();
            return schemaTable;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();

            file.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";

            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            file.Multiselect = false;

            file.InitialDirectory = @"C:\Users\91950\source\repos\Windows-6\Windows-6\bin\Debug";


            if (file.ShowDialog() == false)

                return;

            //判断文件后缀

            path = file.FileName;

            string fileSuffix = System.IO.Path.GetExtension(path);

            if (string.IsNullOrEmpty(fileSuffix))

                return;


            dt = ExcelUtility.ExcelToDataTable(path, false);


            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ExcelUtility.DataTableToExcel(MainWindow.path);
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //dt = (dataGrid.ItemsSource as DataView).ToTable();
            //string preValue = (e.Column.GetCellContent(e.Row) as TextBlock).Text;
            string newValue = (e.EditingElement as TextBox).Text;
            int column = e.Column.DisplayIndex;
            int row = e.Row.GetIndex();
            ExcelUtility.ChangeWorkbook(row, column, newValue);
        }
    }
    

    public class ExcelUtility
    {
        static IWorkbook workbook = null;
       
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))//File.OpenRead......
                {
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);


                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数  
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行  
                                int cellCount = firstRow.LastCellNum;//列数  

                                //构建datatable的列  
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行  
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                fs.Close();
                return dataTable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        public static void ChangeWorkbook(int row, int column, string newValue)
        {
            try {
                workbook.GetSheetAt(0).GetRow(row + 1).GetCell(column).SetCellValue(newValue);
            }
            catch(Exception ex)
            {
                MessageBox.Show("不能修改最后一行");
            }
        }
        public static bool DataTableToExcel(DataTable dt)
        {
            bool result = false;
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;
            ISheet sheet = null;
            ICell cell = null;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet("Sheet0");//创建一个名称为Sheet0的表  
                    int rowCount = dt.Rows.Count;//行数  
                    int columnCount = dt.Columns.Count;//列数  

                    //设置列头  
                    row = sheet.CreateRow(0);//excel第一行设为列头  
                    for (int c = 0; c < columnCount; c++)
                    {
                        cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].ColumnName);
                    }

                    //设置每行每列的单元格,  
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);//excel第二行开始写入数据  
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    using (fs = File.OpenWrite(@"C:\\Users\\91950\\source\\repos\\Windows-6\\Windows-6\\bin\\Debug\\4.xlsx"))
                    {
                        workbook.Write(fs);//向打开的这个xls文件中写入数据  
                        result = true;
                    }
                }
                fs.Close();
                return result;
            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return false;
            }
        }

        public static void DataTableToExcel(string path)
        {
            System.IO.File.Delete(path);
            FileStream fs = File.OpenWrite(path);
            workbook.Write(fs);
            fs.Close();
        }
    }


}
