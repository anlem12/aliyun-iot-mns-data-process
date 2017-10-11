using Iot.Mns.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Iot.Mns.Utils;
using System.Configuration;

namespace Iot.Mns
{
    public partial class FrmMain : Form
    {
        Thread th = null;
        Thread thMonitor = null;
        MNSService mns = null;
        bool isMonitor = false;
        static object lockTextBox = new object();
        delegate void SetMsgDelegate(string msg);
        delegate void SetLabelTime(string msg);

        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                if (!isMonitor)
                {
                    ThreadMns();
                    ThreadMonitor();
                    isMonitor = true;
                    btnStart.Text = "停止";
                }
                else
                {
                    isMonitor = false;
                    btnStart.Text = "开始";

                    if (thMonitor != null)
                    {
                        thMonitor.Abort();
                        thMonitor = null;
                    }

                    if (mns != null)
                    {
                        mns.isRunning = false;
                    }
                    if (th != null)
                    {
                        th.Abort();
                        th = null;
                        mns = null;
                    }

                }
            }
            catch (Exception err)
            {
                LogHelper.Write("开始按钮异常：" + err.Message + err.StackTrace);
                MessageBox.Show(err.Message);
            }
        }

        private void ThreadMonitor()
        {
            try
            {
                thMonitor = new Thread(delegate ()
                {
                    while (isMonitor)
                    {
                        if (mns != null)
                        {
                            SetDelegateTime("监控时间：" + DateTime.Now.ToString() + "  ----  调度时间：" + mns.runCurrentDateTime.ToString());
                            TimeSpan ts = DateTime.Now - mns.runCurrentDateTime;
                            if (ts.TotalSeconds > 40)
                            {
                                try
                                {
                                    if (th != null)
                                    {
                                        th.Abort();
                                        th.Abort();
                                        th = null;
                                        mns = null;
                                    }
                                    ThreadMns();
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Write("111.ThreadMonitor监控线程异常：" + ex.Message + ex.StackTrace);
                                }
                            }
                            Thread.Sleep(1000);
                        }
                    }
                });
                thMonitor.IsBackground = true;
                thMonitor.Name = "监控线程";
                thMonitor.Start();
            }
            catch (Exception err)
            {
                LogHelper.Write("ThreadMonitor监控线程异常：" + err.Message + err.StackTrace);
            }
        }

        private void ThreadMns()
        {
            mns = new MNSService();
            try
            {
                mns.RunningState += mns_RunningState;
                th = new Thread(delegate ()
                {
                    try
                    {
                        mns.isRunning = true;
                        mns.StartMns(ConfigurationManager.AppSettings["topicName"]);
                    }
                    catch (Exception err)
                    {
                        LogHelper.Write("开始线程内部异常：" + err.Message + err.StackTrace);
                    }
                });
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception err)
            {
                //mns.isRunning = false;
                //MessageBox.Show(err.Message);
                //th.Abort();
                LogHelper.Write("ThreadMns开始线程异常：" + err.Message + err.StackTrace);

            }
        }

        void mns_RunningState(string status)
        {
            try
            {
                if (this.richTextBox1.InvokeRequired == false)
                {
                    richTextBox1.Text = status;
                }
                else
                {
                    SetMsgDelegate msg = new SetMsgDelegate(SetMsg);
                    this.richTextBox1.Invoke(msg, new object[] { status });

                }
                //lock (lockTextBox)
                //{
                //    richTextBox1.Text = status;
                //}
            }
            catch (Exception err)
            {
                MessageBox.Show("写数据到文本框（mns_RunningState）：" + err.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + "-" + ConfigurationManager.AppSettings["topicName"];
            notifyIcon1.Text = this.Text;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SetMsg(string msg)
        {
            richTextBox1.Text = msg;
        }


        private void SetDelegateTime(string time)
        {
            if (this.label1.InvokeRequired == false)
            {
                label1.Text = time;
            }
            else
            {
                SetLabelTime msg = new SetLabelTime(SetLabelRunTime);
                this.label1.Invoke(msg, new object[] { time });
            }
        }

        private void SetLabelRunTime(string msg)
        {
            label1.Text = msg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (th == null)
            {
                MessageBox.Show("线程 IS　NULL");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("线程状态：" + th.ThreadState.ToString() + "， th.IsAlive=" + th.IsAlive.ToString() + "， mns.isRunning=" + mns.isRunning.ToString() + "\n");

            if (thMonitor != null)
            {
                sb.Append("监控线程状态：" + thMonitor.ThreadState.ToString() + "， th.IsAlive=" + thMonitor.IsAlive.ToString() + "， isMonitor=" + isMonitor.ToString());
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
