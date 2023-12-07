using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace CW_Havadzyn_V5
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(MainPage_Paint);
            closeBtn.ImageLocation = Path.Combine(Application.StartupPath, "../..", "pictures/closeBtn.png");
            logoBtn.ImageLocation = Path.Combine(Application.StartupPath, "../..", "pictures/logoBtn.png");
            infoPanel2.BackColor = Color.FromArgb(30, Color.White);
            infoPanel2_Img.BackColor = Color.FromArgb(0, Color.White);
            infoPanel2_Header.BackColor = Color.FromArgb(0, Color.White);
            infoPanel2_Label.BackColor = Color.FromArgb(0, Color.White);
            management_Panel.ForeColor = Color.White;
            management_View.ForeColor = Color.Black;

            newsPanel1.Height -= 2;

            worker_ComboBox.SelectedIndexChanged += new EventHandler(worker_ComboBox_SelectedIndexChanged);
            ShowAllData();
        }

        private static readonly string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Path.GetFullPath(@"..\..\Database.mdf")};Integrated Security=True";

        private void FillWorker_ComboBox()
        {
            try
            {
                worker_ComboBox.Items.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT DISTINCT CONCAT(First_Name, ' ', Last_Name) AS FullName, Job, JobFormat, Address FROM Workers;";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string fullName = reader["FullName"].ToString();
                        string job = reader["Job"].ToString();
                        string jobFormat = reader["JobFormat"].ToString();
                        string address = reader["Address"].ToString();
                        worker_ComboBox.Items.Add(fullName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void worker_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWorker = "";
            if (worker_ComboBox.SelectedIndex >= 0)
            {
                selectedWorker = worker_ComboBox.SelectedItem.ToString();
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sqlQuery = "SELECT Job, JobFormat, Address FROM Workers WHERE CONCAT(First_Name, ' ', Last_Name) = @WorkerName;";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@WorkerName", selectedWorker);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string job = reader["Job"].ToString();
                            string jobFormat = reader["JobFormat"].ToString();
                            string address = reader["Address"].ToString();

                            workerJob.Text = job;
                            workModel.Text = jobFormat;
                            workerAddress.Text = address;
                        }

                        reader.Close();

                        sqlQuery = "SELECT Name, Start_Date, End_Date, Done AS Duration FROM [dbo].[CalendarPlanView] WHERE CONCAT(First_Name, ' ', Last_Name) = @WorkerName;";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@WorkerName", selectedWorker);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        management_View.DataSource = dataTable;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка завантаження даних: " + ex.Message);
                }
            }
        }

        private void ShowAllData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT CONCAT(W.First_Name, ' ', W.Last_Name) AS FullName, CONCAT(W.Job, ', ', W.JobFormat) AS JobInfo, " +
                                      "C.Name, C.Start_Date, C.End_Date, CONCAT(CAST(C.Done AS NVARCHAR), '%') AS Complete " +
                                      "FROM Workers W " +
                                      "INNER JOIN CalendarPlanView C ON CONCAT(W.First_Name, ' ', W.Last_Name) = CONCAT(C.First_Name, ' ', C.Last_Name);";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    management_View.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних: " + ex.Message);
            }
        }


        private Point lastPoint = new Point();

        private void MainPage_Paint(object sender, PaintEventArgs e)
        {
            Color color1 = ColorTranslator.FromHtml("#B973FF");
            Color color2 = ColorTranslator.FromHtml("#6422D0");

            LinearGradientBrush gradientBrush = new LinearGradientBrush(this.ClientRectangle, color1, color2, LinearGradientMode.BackwardDiagonal);
            e.Graphics.FillRectangle(gradientBrush, this.ClientRectangle);
        }

        private void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - lastPoint.X;
                Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPage_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        // Bottom buttons click
        private void twitterImg_Click(object sender, EventArgs e)
        {
            string url = "https://twitter.com/RiseAnd65979743";
            System.Diagnostics.Process.Start(url);
        }

        private void facebookImg_Click(object sender, EventArgs e)
        {
            string url = "https://www.facebook.com/arseniyp/";
            System.Diagnostics.Process.Start(url);
        }

        private void linkedinImg_Click(object sender, EventArgs e)
        {
            string url = "https://www.linkedin.com/in/arsen-gavadzyn-1546041bb/";
            System.Diagnostics.Process.Start(url);
        }

        private void githubImg_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/ArsenHavadzyn";
            System.Diagnostics.Process.Start(url);
        }

        private void youtubeBtn_Click(object sender, EventArgs e)
        {
            string url = "https://www.youtube.com/channel/UCUdhVzkJMnjbsE2DRL2OgJQ";
            System.Diagnostics.Process.Start(url);
        }

        // Top buttons
        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void closeBtn_MouseEnter(object sender, EventArgs e)
        {
            closeBtn.ImageLocation = Path.Combine(Application.StartupPath, "../..", "pictures/closeBtn_Action.png");
        }

        private void closeBtn_MouseLeave(object sender, EventArgs e)
        {
            closeBtn.ImageLocation = Path.Combine(Application.StartupPath, "../..", "pictures/closeBtn.png");
        }

        public void SwitchPage(string pageName)
        {
            switch (pageName)
            {
                case "logoBtn":
                    if (!(infoPanel1.Visible && infoPanel2.Visible && infoPanel3.Visible))
                    {
                        infoPanel1.Visible = infoPanel2.Visible = infoPanel3.Visible = true;
                    }
                    news_Label.Font = new Font(news_Label.Font, news_Label.Font.Style & ~FontStyle.Underline);
                    aboutUs_Label.Font = new Font(aboutUs_Label.Font, aboutUs_Label.Font.Style & ~FontStyle.Underline);
                    management_Label.Font = new Font(management_Label.Font, management_Label.Font.Style & ~FontStyle.Underline);
                    management_Panel.Visible = news_Panel.Visible = aboutPanel.Visible = false;
                    break;
                case "news_Label":
                    news_Label.Font = new Font(news_Label.Font, FontStyle.Underline);

                    news_Panel.Visible = true;

                    aboutUs_Label.Font = new Font(aboutUs_Label.Font, aboutUs_Label.Font.Style & ~FontStyle.Underline);
                    management_Label.Font = new Font(management_Label.Font, management_Label.Font.Style & ~FontStyle.Underline);

                    management_Panel.Visible = aboutPanel.Visible = infoPanel1.Visible = infoPanel2.Visible = infoPanel3.Visible = false;
                    break;
                case "management_Label":
                    management_Label.Font = new Font(management_Label.Font, FontStyle.Underline);

                    management_Panel.Visible = true;

                    aboutUs_Label.Font = new Font(aboutUs_Label.Font, aboutUs_Label.Font.Style & ~FontStyle.Underline);
                    news_Label.Font = new Font(news_Label.Font, news_Label.Font.Style & ~FontStyle.Underline);

                    news_Panel.Visible = aboutPanel.Visible = infoPanel1.Visible = infoPanel2.Visible = infoPanel3.Visible = false;
                    break;
                case "aboutUs_Label":
                    aboutUs_Label.Font = new Font(aboutUs_Label.Font, FontStyle.Underline);

                    aboutPanel.Visible = true;

                    news_Label.Font = new Font(news_Label.Font, news_Label.Font.Style & ~FontStyle.Underline);
                    management_Label.Font = new Font(management_Label.Font, management_Label.Font.Style & ~FontStyle.Underline);

                    management_Panel.Visible = news_Panel.Visible = infoPanel1.Visible = infoPanel2.Visible = infoPanel3.Visible = false;
                    break;
                default:

                    aboutUs_Label.Font = new Font(aboutUs_Label.Font, aboutUs_Label.Font.Style & ~FontStyle.Underline);
                    management_Label.Font = new Font(management_Label.Font, management_Label.Font.Style & ~FontStyle.Underline);
                    news_Label.Font = new Font(news_Label.Font, news_Label.Font.Style & ~FontStyle.Underline);

                    management_Panel.Visible = aboutPanel.Visible = news_Panel.Visible = false;
                    infoPanel1.Visible = infoPanel2.Visible = infoPanel3.Visible = true;
                    break;
            }
        }

        // About Us

        private void aboutUs_Label_MouseEnter(object sender, EventArgs e)
        {
            aboutUs_Label.ForeColor = Color.White;
        }

        private void aboutUs_Label_MouseDown(object sender, MouseEventArgs e)
        {
            aboutUs_Label.ForeColor = Color.LightGray;
        }

        private void aboutUs_Label_Click(object sender, EventArgs e)
        {
            SwitchPage(aboutUs_Label.Name);
        }

        private void aboutUs_Label_MouseLeave(object sender, EventArgs e)
        {
            aboutUs_Label.ForeColor = Color.WhiteSmoke;
        }

        // News
        private void news_Label_MouseEnter(object sender, EventArgs e)
        {
            news_Label.ForeColor = Color.White;
        }

        private void news_Label_MouseDown(object sender, MouseEventArgs e)
        {
            news_Label.ForeColor = Color.LightGray;
        }

        private void news_Label_Click(object sender, EventArgs e)
        {
            SwitchPage(news_Label.Name);
        }

        private void news_Label_MouseLeave(object sender, EventArgs e)
        {
            news_Label.ForeColor = Color.WhiteSmoke;
        }

        // Management
        private void management_Label_MouseEnter(object sender, EventArgs e)
        {
            management_Label.ForeColor = Color.White;
        }

        private void management_Label_MouseDown(object sender, MouseEventArgs e)
        {
            management_Label.ForeColor = Color.LightGray;
        }

        private void management_Label_Click(object sender, EventArgs e)
        {
            SwitchPage(management_Label.Name);
        }

        private void management_Label_MouseLeave(object sender, EventArgs e)
        {
            management_Label.ForeColor = Color.WhiteSmoke;
        }

        private void logoBtn_Click(object sender, EventArgs e)
        {
            SwitchPage(logoBtn.Name);
        }

        private void goToNews_Btn_Click(object sender, EventArgs e)
        {
            SwitchPage(news_Label.Name);
        }

        private void viewTypeRadio1_CheckedChanged(object sender, EventArgs e)
        {
            management_View.Width = viewTypeRadio1.Checked ? 723 : management_View.Width;
            worker_ComboBox.SelectedIndex = -1;
            management_View.DataSource = null;
            management_View.Rows.Clear();
            ShowAllData();
        }

        private void viewTypeRadio2_CheckedChanged(object sender, EventArgs e)
        {
            management_View.Width = viewTypeRadio2.Checked ? 508 : management_View.Width;
            management_View.DataSource = null;
            management_View.Rows.Clear();
            FillWorker_ComboBox();
        }
    }
}
