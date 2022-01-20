using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class BodyPart : MonoBehaviour
    {

        BodyCellNode.Factory m_BodyCellFactory;
        CellsSettings m_CellsSettings;
        BoxCollider2D m_BoxCollider2D;
        SpriteRenderer m_Renderer;
        [Inject]
        void Construct(BodyCellNode.Factory bodyCellFactory, CellsSettings settings, BoxCollider2D boxCollider, SpriteRenderer renderer)
        {
            m_BodyCellFactory = bodyCellFactory;
            m_CellsSettings = settings;
            m_BoxCollider2D = boxCollider;
            m_Renderer = renderer;

        }
        private void Start()
        {
            Setup();
            m_BoxCollider2D.enabled = false;
        }


        BodyCellNode[,] Nodes;

        public void Setup()
        {
            CreateNodes();
            ConnectNodes();

        }

        private void CreateNodes()
        {
            m_Renderer.color = Color.gray;
            var bounds = m_BoxCollider2D.bounds;


            float size = m_CellsSettings.cellSize;
            var cellSize = new Vector3(size, size, 1);
            int horizonalCellsNumber = Mathf.CeilToInt(bounds.size.x / size);
            int verticalCellsNumber = Mathf.CeilToInt(bounds.size.y / size);

            float startOffset = size / 2;
            var minPosWithOffset = new Vector3(bounds.min.x + startOffset, bounds.min.y + startOffset, 0);
            float moveOffset = size;

            Nodes = new BodyCellNode[horizonalCellsNumber, verticalCellsNumber];
            for (int i = 0; i < horizonalCellsNumber; i++)
            {
                for (int j = 0; j < verticalCellsNumber; j++)
                {
                    var cell = m_BodyCellFactory.Create();
                    cell.transform.position = new Vector3(minPosWithOffset.x + moveOffset * i, minPosWithOffset.y + moveOffset * j, 0);
                    cell.transform.localScale = cellSize;

                    Nodes[i, j] = cell;

                }
            }
        }

        private void ConnectNodes()
        {
            var height = Nodes.GetLength(0);
            var width = Nodes.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    BodyCellNode right = null;
                    BodyCellNode top = null;
                    BodyCellNode bottom = null;
                    BodyCellNode left = null;


                    var topIndex = i + 1;
                    var bottomIndex = i - 1;
                    var rightIndex = j + 1;
                    var leftIndex = j - 1;

                    if (rightIndex < width)
                        top = Nodes[i, rightIndex];
                    if (leftIndex >= 0)
                        bottom = Nodes[i, leftIndex];

                    if (topIndex < height)
                        right = Nodes[topIndex, j];
                    if (bottomIndex >= 0)
                        left = Nodes[bottomIndex, j];

                    Nodes[i, j].ConnectCells(right, left, bottom, top);

                }
            }
        }

    }


}