using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGraphic
{
    [AddComponentMenu("UI/Effects/Gradient Color")]
    public class GradientColor : BaseMeshEffect
    {
        public Color32 startColor = Color.white;
        public Color32 endColor = Color.black;
        public bool isVertical = true;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            var count = vh.currentVertCount;
            if (count == 0)
                return;

            var vertexs = new List<UIVertex>();
            for (var i = 0; i < count; i++)
            {
                var vertex = new UIVertex();
                vh.PopulateUIVertex(ref vertex, i);
                vertexs.Add(vertex);
            }

            if (isVertical)
            {
                VerticalMesh(vh, vertexs, count);
            }
            else
            {
                HorizontalMesh(vh, vertexs, count);
            }
        }

        private void HorizontalMesh(VertexHelper vh, List<UIVertex> vertexs, int count)
        {
            var minX = vertexs[0].position.x;
            var maxX = vertexs[0].position.x;


            for (var i = 1; i < count; i++)
            {
                var x = vertexs[i].position.x;
                minX = Mathf.Min(minX, x);
                maxX = Mathf.Max(maxX, x);
            }

            var width = maxX - minX;
            if (width > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var vertex = vertexs[i];

                    var color = Color32.Lerp(startColor, endColor, (vertex.position.x - minX) / width);

                    vertex.color = color;

                    vh.SetUIVertex(vertex, i);
                }
            }
        }

        private void VerticalMesh(VertexHelper vh, List<UIVertex> vertexs, int count)
        {
            var maxY = vertexs[0].position.y;
            var minY = vertexs[0].position.y;


            for (var i = 1; i < count; i++)
            {
                var y = vertexs[i].position.y;
                minY = Mathf.Min(minY, y);
                maxY = Mathf.Max(maxY, y);
            }

            var height = maxY - minY;
            if (height > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var vertex = vertexs[i];

                    var color = Color32.Lerp(endColor, startColor, (vertex.position.y - minY) / height);

                    vertex.color = color;

                    vh.SetUIVertex(vertex, i);
                }
            }

        }
    }
}