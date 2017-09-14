using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.ComponentModel; 
namespace dirPro
{
    static class Program
    {
        //public double mouseZoom=0;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainform = new Form1();
            mainform.Show();//Application.Run(mainform);
            while (mainform.Created) //设置一个循环用于实时更新渲染状态
            {
                if (mainform.finishReading)
                {
                    /*
                    if (mainform.InitializeGraphics() == false) //检查Direct3D是否启动
                    {
                        MessageBox.Show("无法启动Direct3D！", "错误！");
                        return;
                    }*/
                    mainform.Render(); //保持device渲染，直到程序结束
                }
                Application.DoEvents(); //处理键盘鼠标等输入事件
            }
        }
        public static Form1 mainform;
        
    }
}
