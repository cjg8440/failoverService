using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace failoverService
{
    public partial class failoverService : ServiceBase
    {

        Process proc_1;

        public failoverService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FileStream FS = new FileStream(@"C:\FailoverService\logs\Servicelog.log", FileMode.Append, FileAccess.Write);
            FileStream FS_ER = new FileStream(@"C:\FailoverService\logs\Errorlog.log", FileMode.Append, FileAccess.Write);

            StreamWriter SW = new StreamWriter(FS);

            SW.WriteLine(DateTime.Now.ToString()+ " - " + "서비스가 실행되었습니다.");

            try
            {

                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = "cmd.exe";
                procInfo.Arguments = " /C " + "D:\\DBAQ\\WAS.bat";
                proc_1 = new Process();
                proc_1.StartInfo = procInfo;
                proc_1.Start();
            }
            catch (Exception ex)
            {
                SW = new StreamWriter(FS_ER);

                SW.WriteLine(DateTime.Now.ToString() + " - " + ex.ToString());
            }

            SW.Close();
            FS.Close();

         }

        protected override void OnStop()
        {
            FileStream FS = new FileStream(@"C:\FailoverService\logs\Servicelog.log", FileMode.Append, FileAccess.Write);

            StreamWriter SW = new StreamWriter(FS);

            SW.WriteLine(DateTime.Now.ToString() +" - " + "서비스가 중지되었습니다.");

            Process[] processList = Process.GetProcessesByName("dbaq");

            if(processList.Length > 0)
            {
                processList[0].Kill();
            }


            SW.Close();
            FS.Close();


        }
    }
}
