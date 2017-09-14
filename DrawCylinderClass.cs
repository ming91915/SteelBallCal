using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace dirPro
{
    class DrawCylinderClass
    {
        /// <summary> 
        /// <param name="device">device:设备对象</param> 
        /// </summary> 
        public Device device;//定义设备 
        /// <summary> 
        /// <param name="topCircleCenterVector">topCircleCenterVector:圆柱顶部圆心坐标</param> 
        /// </summary> 
        public Vector3 topCircleCenterVector;//定义圆柱顶部圆心坐标 
        /// <summary> 
        /// <param name="underCircleCenterVector">underCircleCenterVector:圆柱底部圆心坐标</param> 
        /// </summary> 
        public Vector3 underCircleCenterVector;//定义圆柱底部圆心坐标 
        ///<summary> 
        ///<param name="radius">radius:圆柱半径</param> 
        ///</summary>         
        public float radius;//定义圆柱半径 
        /// <summary> 
        /// <param name="mNumber">mNumber:沿切向的分块数目</param> 
        /// </summary> 
        public int mNumber;//定义沿切向的分块数目 
        /// <summary> 
        /// <param name="nNumber">nNumber:沿径向的分块数目</param> 
        /// </summary> 
        public int nNumber;//定义沿径向的分块数目 
        /// <summary> 
        /// <param name="hNumber">hNumber:沿高度方向的分块数目</param> 
        /// </summary> 
        public int hNumber;//定义沿高度方向的分块数目 
        private CustomVertex.PositionColored[] vertices;//定义圆柱网格顶点 
        private int[] indexData;//定义圆柱网格中三角形索引 
        /// <summary> 
        /// 定义一个绘制圆柱对象 
        /// </summary> 
        /// <param name="_device">设备对象</param> 
        /// <param name="_topCircleCenterVector">圆柱顶部圆心坐标</param> 
        /// <param name="_underCircleCenterVector">圆柱底部圆心坐标</param> 
        /// <param name="_radius">圆半径</param> 
        /// <param name="_mNumber">沿切向的分块数目</param> 
        /// <param name="_nNumber">沿径向的分块数目</param> 
        /// <param name="_hNumber">沿高度方向的分块数目</param> 
        public DrawCylinderClass(Device _device, Vector3 _topCircleCenterVector,
Vector3 _underCircleCenterVector, float _radius, int _mNumber, int _nNumber, int
_hNumber)
        {
            device = _device;
            topCircleCenterVector = _topCircleCenterVector;
            underCircleCenterVector = _underCircleCenterVector;
            radius = _radius;
            mNumber = _mNumber;
            nNumber = _nNumber;
            hNumber = _hNumber;
            VertexDeclaration();//定义顶点 
            IndicesDeclaration();//定义索引 
        }
        private void VertexDeclaration()//定义顶点 
        {
            Vector3 circleNormalVectors = Vector3.Subtract(topCircleCenterVector, underCircleCenterVector);
            vertices = new CustomVertex.PositionColored[2 * (mNumber + 1) * (nNumber + 1) + (mNumber + 1) * (hNumber + 1)];
            float cosAlpha = circleNormalVectors.Y / circleNormalVectors.Length();
            float sinAlpha = (float)Math.Sqrt(1 - cosAlpha * cosAlpha);
            float sinBeta = circleNormalVectors.Z / (float)Math.Sqrt(circleNormalVectors.X * circleNormalVectors.X + circleNormalVectors.Z * circleNormalVectors.Z);
            float cosBeta = (float)Math.Sqrt(1 - sinBeta * sinBeta);
            Vector3 R0Vector = new Vector3();
            R0Vector.X = radius * cosAlpha * cosBeta;
            R0Vector.Y = -radius * sinAlpha;
            R0Vector.Z = radius * cosAlpha * sinBeta;
            float theta = (float)Math.PI * 2 / mNumber;
            //定义圆柱顶部和底部网格节点坐标   
            int baseIndex = (mNumber + 1) * (nNumber + 1);
            for (int i = 0; i < mNumber + 1; i++)
            {
                Vector4 tempRVector = Vector3.Transform(R0Vector, Matrix.RotationAxis(circleNormalVectors, theta * i));
                for (int j = 0; j < nNumber + 1; j++)
                {
                    float scaleFactor = (float)j / nNumber;
                    //圆柱体顶部圆 
                    vertices[j + i * (nNumber + 1)].X = topCircleCenterVector.X + tempRVector.X * scaleFactor;
                    vertices[j + i * (nNumber + 1)].Y = topCircleCenterVector.Y + tempRVector.Y * scaleFactor;
                    vertices[j + i * (nNumber + 1)].Z = topCircleCenterVector.Z + tempRVector.Z * scaleFactor;
                    vertices[j + i * (nNumber + 1)].Color = Color.Yellow.ToArgb();
                    //圆柱体底部圆 
                    vertices[j + i * (nNumber + 1) + baseIndex].X = underCircleCenterVector.X + tempRVector.X * scaleFactor;
                    vertices[j + i * (nNumber + 1) + baseIndex].Y = underCircleCenterVector.Y + tempRVector.Y * scaleFactor;
                    vertices[j + i * (nNumber + 1) + baseIndex].Z = underCircleCenterVector.Z + tempRVector.Z * scaleFactor;
                    vertices[j + i * (nNumber + 1) + baseIndex].Color = Color.Yellow.ToArgb();
                }
            }
            //定义圆柱四周网格节点坐标 
            baseIndex = 2 * (mNumber + 1) * (nNumber + 1);
            for (int i = 0; i < mNumber + 1; i++)
            {
                Vector4 tempRVector = Vector3.Transform(R0Vector,
Matrix.RotationAxis(circleNormalVectors, theta * i));
                for (int j = 0; j < hNumber + 1; j++)
                {
                    Vector3 tempOPVector = Vector3.Scale(circleNormalVectors,
(float)j / hNumber);
                    vertices[j + i * (hNumber + 1) + baseIndex].X = underCircleCenterVector.X + tempRVector.X + tempOPVector.X;
                    vertices[j + i * (hNumber + 1) + baseIndex].Y = underCircleCenterVector.Y + tempRVector.Y + tempOPVector.Y;
                    vertices[j + i * (hNumber + 1) + baseIndex].Z = underCircleCenterVector.Z + tempRVector.Z + tempOPVector.Z;
                    vertices[j + i * (hNumber + 1) + baseIndex].Color = Color.Yellow.ToArgb();
                }
            }
        }
        private void IndicesDeclaration()//定义索引 
        {
            indexData = new int[12 * mNumber * nNumber + 6 * mNumber * hNumber];
            //定义圆柱顶部和底部网格节点索引   
            int baseIndex01 = 6 * mNumber * nNumber;
            int baseIndex02 = (mNumber + 1) * (nNumber + 1);

            for (int i = 0; i < mNumber; i++)
            {
                for (int j = 0; j < nNumber; j++)
                {
                    //顶部圆 
                    indexData[6 * (j + i * nNumber)] = j + i * (nNumber + 1);
                    indexData[6 * (j + i * nNumber) + 1] = j + i * (nNumber + 1) + 1;
                    indexData[6 * (j + i * nNumber) + 2] = j + (i + 1) * (nNumber + 1);
                    indexData[6 * (j + i * nNumber) + 3] = j + i * (nNumber + 1) + 1;
                    indexData[6 * (j + i * nNumber) + 4] = j + (i + 1) * (nNumber + 1) + 1;
                    indexData[6 * (j + i * nNumber) + 5] = j + (i + 1) * (nNumber + 1);
                    //底部圆 
                    indexData[6 * (j + i * nNumber) + baseIndex01] = j + i * (nNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * nNumber) + 1 + baseIndex01] = j + (i + 1) * (nNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * nNumber) + 2 + baseIndex01] = j + i * (nNumber + 1) + 1 + baseIndex02;
                    indexData[6 * (j + i * nNumber) + 3 + baseIndex01] = j + i * (nNumber + 1) + 1 + baseIndex02;
                    indexData[6 * (j + i * nNumber) + 4 + baseIndex01] = j + (i + 1) * (nNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * nNumber) + 5 + baseIndex01] = j + (i + 1) * (nNumber + 1) + 1 + baseIndex02;
                }
            }
            //定义圆柱侧面索引 
            baseIndex01 = 12 * mNumber * nNumber;
            baseIndex02 = 2 * (mNumber + 1) * (nNumber + 1);

            for (int i = 0; i < mNumber; i++)
            {
                for (int j = 0; j < hNumber; j++)
                {
                    indexData[6 * (j + i * hNumber) + baseIndex01] = j + i * (hNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * hNumber) + 1 + baseIndex01] = j + (i + 1) * (hNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * hNumber) + 2 + baseIndex01] = j + i * (hNumber + 1) + 1 + baseIndex02;
                    indexData[6 * (j + i * hNumber) + 3 + baseIndex01] = j + i * (hNumber + 1) + 1 + baseIndex02;
                    indexData[6 * (j + i * hNumber) + 4 + baseIndex01] = j + (i + 1) * (hNumber + 1) + baseIndex02;
                    indexData[6 * (j + i * hNumber) + 5 + baseIndex01] = j + (i + 1) * (hNumber + 1) + 1 + baseIndex02;
                }
            }
        }
        /// <summary> 
        /// 绘制圆柱 
        /// </summary> 
        public void DrawCylinder()
        {
            device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0, 2 * (mNumber + 1)
            * (nNumber + 1) + (mNumber + 1) * (hNumber + 1), 4 * mNumber * nNumber + 2 * mNumber
            * hNumber, indexData, false, vertices);
        }
    }
}
