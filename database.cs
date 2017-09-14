using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dirPro
{
    class node
    {
        public double x, y, z;//坐标
        //public CrSectionProp section;//球截面
        public int sectionID=0;//球截面ID
    }
    class elem
    {
        //public int no;//编号
        public int node0ID, node1ID;//0节点、1节点
        public int sectionID;//截面信息
        public double nz;//轴力
        public double maxNz0, maxNz1;
        public elem() { }
    }
    class ElemSectionProp
    {
        //public int no;
        public double r, t;
        public ElemSectionProp() { }
    }
    class CrSectionProp
    {
        //public int no;
        public double d, t;
        public double n0, nm, fy;
        public CrSectionProp() { }
        public CrSectionProp(double d, double t) { 
            this.d = d;
            this.t = t;
            n0 = d - 500 <= 0 ? 1 : 0.9;
            nm = d - 300 <= 0 ? 1 : 1.1;
            fy = t - 16 <= 0 ? 310 : t - 35 <= 0 ? 295 : t - 50 <= 0 ? 265 : 250;
        }

    }

    class database
    {
        
    }
}
