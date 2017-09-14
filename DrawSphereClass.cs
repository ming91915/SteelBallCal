using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Windows.Forms;


namespace dirPro
{
    class DrawSphereClass
    {
        public Device device;//定义设备 
        public Vector3 shpereCenterVectors;//定义球心坐标 
        public float radius;//定义球体半径 
        public int mNumber;//定义球体在水平方向的分块数目 
        public int nNumber;//定义球体在竖直方向的分块数目 
        private CustomVertex.PositionColored[] vertices;//定义球体网格顶点 
        private int[] indexData;//定义球体网格中三角形索引 
        public Color setcolor;//颜色
        public DrawSphereClass(Device _device, Vector3 _shpereCenterVectors, float
_radius, int _mNumber, int _nNumber, Color _color)
        {
            device = _device;
            shpereCenterVectors = _shpereCenterVectors;
            radius = _radius;
            mNumber = _mNumber;
            nNumber = _nNumber;
            setcolor = _color;
            VertexDeclaration();//定义顶点 
            IndicesDeclaration();//定义索引 
            
            
        }
        private void VertexDeclaration()//定义顶点 
        {
            vertices = new CustomVertex.PositionColored[(mNumber + 1) * (nNumber + 1)];
            float theta = (float)Math.PI / nNumber;
            float alpha = 2 * (float)Math.PI / mNumber;
            for (int i = 0; i < nNumber + 1; i++)
            {
                for (int j = 0; j < mNumber + 1; j++)
                {
                    vertices[j + i * (mNumber + 1)].X = shpereCenterVectors.X + radius * (float)Math.Sin(i * theta) * (float)Math.Cos(j * alpha);
                    vertices[j + i * (mNumber + 1)].Y = shpereCenterVectors.Y + radius * (float)Math.Cos(i * theta);
                    vertices[j + i * (mNumber + 1)].Z = shpereCenterVectors.Z + radius * (float)Math.Sin(i * theta) * (float)Math.Sin(j * alpha);
                    vertices[j + i * (mNumber + 1)].Color = setcolor.ToArgb();
                }
            }
        }
        private void IndicesDeclaration()//定义索引 
        {
            indexData = new int[6 * mNumber * nNumber];
            for (int i = 0; i < nNumber; i++)
            {
                for (int j = 0; j < mNumber; j++)
                {
                    indexData[6 * (j + i * mNumber)] = j + i * (mNumber + 1);
                    indexData[6 * (j + i * mNumber) + 1] = j + i * (mNumber + 1) + 1;
                    indexData[6 * (j + i * mNumber) + 2] = j + (i + 1) * (mNumber + 1);
                    indexData[6 * (j + i * mNumber) + 3] = j + i * (mNumber + 1) + 1;
                    indexData[6 * (j + i * mNumber) + 4] = j + (i + 1) * (mNumber + 1) + 1;
                    indexData[6 * (j + i * mNumber) + 5] = j + (i + 1) * (mNumber + 1);
                }
            }
        }
        public void DrawSphere()
        {
            //device.VertexFormat = CustomVertex.PositionTextured.Format; 
            device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, (mNumber + 1) *
            (nNumber + 1), 2 * mNumber * nNumber, indexData, false, vertices);
        } 
    }
}
