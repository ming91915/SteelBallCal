using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace dirPro
{
    class reader
    {
        //读节点
        public Dictionary<int, node> nodes = new Dictionary<int, node>();
        //读球截面
        public Dictionary<int, CrSectionProp> crSections = new Dictionary<int, CrSectionProp>();
        //读杆件截面
        public Dictionary<int, ElemSectionProp> elementSections = new Dictionary<int, ElemSectionProp>();
        //读杆件
        public Dictionary<int, elem> elements = new Dictionary<int, elem>();


        //public delegate void TaskDelegate();
        public reader() { }
        public reader(string obj)
        {
            StreamReader sr = new StreamReader(obj+@"\单元.log", Encoding.Default);
            if (sr != null)
            {
                string line = "";
                string saveType = "无";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "/NODECORD2/"){
                        saveType = "节点";
                        continue;
                    }
                    else if (line == "/MAST_ELEMS_6/"){
                        saveType = "杆件";
                        continue;
                    }
                    else if (line == "/SECTION PROPERTIES/")
                    {
                        saveType = "杆件截面";
                        continue;
                    }
                    else if (line == "##")
                    {
                        saveType = "无";
                        continue;
                    }
                    string[] lineSplit = Regex.Replace(line, @"\s+", " ").Split(new char[1] { ' ' });

                    if (saveType == "节点")
                    {
                        
                        int no = Convert.ToInt32(lineSplit[1].ToString());
                        double x = Convert.ToDouble(lineSplit[2].ToString());
                        double y = Convert.ToDouble(lineSplit[3].ToString());
                        double z = Convert.ToDouble(lineSplit[4].ToString());
                        if (!nodes.ContainsKey(no)) nodes.Add(no, new node());
                        nodes[no].x = x;
                        nodes[no].y = y;
                        nodes[no].z = z;

                        //Form1.SafeInvoke(, new Form1.TaskDelegate(delegate()
                        //{ }));

                    }
                    else if (saveType == "杆件")
                    {
                        int no = Convert.ToInt32(lineSplit[2].ToString());
                        int n0 = Convert.ToInt32(lineSplit[3].ToString());
                        int n1 = Convert.ToInt32(lineSplit[4].ToString());
                        int sec = Convert.ToInt32(lineSplit[6].ToString());
                        if (!elements.ContainsKey(no)) elements.Add(no, new elem());
                        elements[no].node0ID = n0;
                        elements[no].node1ID = n1;
                        elements[no].sectionID = sec;
                    }
                    else if (saveType == "杆件截面")
                    {
                        int no = Convert.ToInt32(lineSplit[2].ToString());
                        double d = Convert.ToDouble(lineSplit[3].ToString());
                        double t = Convert.ToDouble(lineSplit[4].ToString());
                        if (!elementSections.ContainsKey(no)) elementSections.Add(no, new ElemSectionProp());
                        elementSections[no].r = d;
                        elementSections[no].t = t;
                    }
                }
            }
            sr = new StreamReader(obj + @"\球节点.log", Encoding.Default);
            if (sr != null)
            {
                string line = "";
                string saveType = "无";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "/INFORMSPHERES/")
                    {
                        saveType = "球节点";
                        continue;
                    }
                    else if (line == "##")
                    {
                        saveType = "无";
                        continue;
                    }
                    string[] lineSplit = Regex.Replace(line, @"\s+", " ").Split(new char[1] { ' ' });
                    if (saveType == "球节点")
                    {
                        int no = Convert.ToInt32(lineSplit[2].ToString());
                        int nodeID = Convert.ToInt32(lineSplit[1].ToString());
                        string sec = (lineSplit[3].ToString());
                        var Pattern = new Regex(@"(\d+)x(\d+.\d+)");
                        var result = Pattern.Match(sec).Groups;
                        if (!crSections.ContainsKey(no)) crSections.Add(no, new CrSectionProp( Convert.ToDouble(result[1].ToString()),Convert.ToDouble(result[2].ToString())));
                        //
                        if (nodes.ContainsKey(nodeID)) nodes[nodeID].sectionID = no;

                    }
                }
            }
            sr = new StreamReader(obj + @"\内力", Encoding.Default);
            if (sr != null)
            {
                string line = "";
                string saveType = "无";
                while ((line = sr.ReadLine()) != null)
                {
                    if (Regex.Replace(line, " ", "") == "") continue;
                    if (line.IndexOf("AXIAL FORCE")>0)
                    {
                        saveType = "内力";
                        continue;
                    }
                    else if (line == "##")
                    {
                        saveType = "无";
                        continue;
                    }
                    string[] lineSplit = Regex.Replace(line, @"\s+", " ").Split(new char[1] { ' ' });
                    if (saveType == "内力")
                    {
                        if (lineSplit[0].ToString() == "") continue;
                        int no = Convert.ToInt32( lineSplit[0].ToString());
                        double nz = Convert.ToDouble(lineSplit[1].ToString());
                        if (elements.ContainsKey(no)) elements[no].nz = nz;
                    }
                }
            }
            startCal();
        }


        public void startCal()
        {

            //计算逻辑 按杆件编号
            for (int i = 1; i <= this.elements.Count ; i++)
            {
                var ele = this.elements[i];
                var eleSection = this.elementSections[ele.sectionID];
                var node0Section = this.crSections[this.nodes[ele.node0ID].sectionID];
                var node1Section = this.crSections[this.nodes[ele.node1ID].sectionID];

                ele.maxNz0 = node0Section.n0 * node0Section.nm * (0.29 + 0.54 * eleSection.r / node0Section.d) * 3.14 * eleSection.r * node0Section.t * node0Section.fy / 1000;
                ele.maxNz1 = node1Section.n0 * node1Section.nm * (0.29 + 0.54 * eleSection.r / node1Section.d) * 3.14 * eleSection.r * node1Section.t * node1Section.fy / 1000;
            }


            //输出逻辑 按求节点顺序
            for (int i = 1; i <= this.nodes.Count ; i++)
            {
                if (nodes[i].sectionID == 0) continue;

            }
        }

    }
}
