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

namespace FreeCam
{
    public partial class Main : Form
    {
        private Point mousePoint;

        #region
        string ScreenPath;


        private Form m_InstanceRef = null;
        public Form InstanceRef
        {

            get
            {

                return m_InstanceRef;

            }
            set
            {

                m_InstanceRef = value;

            }

        }
        #endregion

        #region
        //이동1
        //선언부1

        Point fPt;
        bool isMove;
        #endregion


        public Main()
        {
            InitializeComponent();


        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {


        }

        #region
        //이동1
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장

            // 버튼을 눌렀을 움직이게 하기 위함.
            isMove = true;
            // 버튼을 눌렀을 당시의 위치를 저장
            fPt = new Point(e.X, e.Y);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // 버튼이 눌러졌고, 왼쪽버튼일 경우에만
            if (isMove && (e.Button & MouseButtons.Left) == MouseButtons.Left)
                // 폼의 Location을 재지정
                Location = new Point(this.Left - (fPt.X - e.X), this.Top - (fPt.Y - e.Y));
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }



        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;

        }
        #endregion

        #region
        //메뉴바
        private void 최소화ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            this.WindowState = FormWindowState.Normal;
            this.Visible = false;
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
            Hide();
        }

        private void 파일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("준비중입니다.", "준비중");

        }
        #endregion



        #region
        //전체화면 캡쳐


        private void Button1_Click(object sender, EventArgs e)
        {

            ScreenCapture(false);

        }
        public void ScreenCapture(bool showCursor)
        {

            Point curPos = new Point(Cursor.Position.X, Cursor.Position.Y);
            Size curSize = new Size
            {
                Height = Cursor.Current.Size.Height,
                Width = Cursor.Current.Size.Width
            };

            ScreenPath = "";

            if (!ScreenShot.saveToClipboard)
            {

                saveFileDialog1.DefaultExt = "bmp";
                saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg|gif files (*.gif)|*.gif|tiff files (*.tiff)|*.tiff|png files (*.png)|*.png";
                saveFileDialog1.Title = "FreeCam";
                saveFileDialog1.ShowDialog();
                ScreenPath = saveFileDialog1.FileName;

            }



            if (ScreenPath != "" || ScreenShot.saveToClipboard)
            {

                //Conceal this form while the screen capture takes place
                this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                this.TopMost = false;

                //Allow 250 milliseconds for the screen to repaint itself (we don't want to include this form in the capture)
                System.Threading.Thread.Sleep(250);

                Rectangle bounds = Screen.GetBounds(Screen.GetBounds(Point.Empty));
                string fi = "";

                if (ScreenPath != "")
                {

                    fi = new FileInfo(ScreenPath).Extension;

                }

                ScreenShot.CaptureImage(showCursor, curSize, curPos, Point.Empty, Point.Empty, bounds, ScreenPath, fi);

                //The screen has been captured and saved to a file so bring this form back into the foreground
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.TopMost = true;


                if (ScreenShot.saveToClipboard)
                {


                    MessageBox.Show("화면 캡쳐 완료", "FreeCam", MessageBoxButtons.OK);

                }
                else
                {
                    Main main = new Main();
                    main.Show();
                    MessageBox.Show("파일 저장 완료", "FreeCam", MessageBoxButtons.OK);

                }


            }


        }

        #endregion

        private void Button2_Click(object sender, EventArgs e)
        {
            CP drag = new CP();
            drag.Show();
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            {   //시스템 트레이에서의 아이콘이 더블클릭되었을때의 이벤트
                this.Visible = true; //윈도우의 모습을 보인다.
                this.ShowIcon = true; //아이콘의 모습을 보인다.
                notifyIcon1.Visible = false; //시스템트레이의 모습을 감춘다.
            }

        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;
            }
        }
        private void NenuStrip1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //winform의 최소화버튼을 눌렀을때의 이벤트
                this.Visible = false;  //윈도우의 모습을 감춘다.
                this.ShowIcon = false;  //아이콘의 모습을 감춘다.
                notifyIcon1.Visible = true; //시스템트레이의 모습을 보인다.
            }
        }

        private void Button1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장

        }

        private void Button1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void Button2_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장

        }

        private void Button2_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void Button3_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장

        }

        private void Button3_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장

        }

        private void MenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }

        }
    }
}