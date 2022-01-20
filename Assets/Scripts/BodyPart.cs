using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class BodyPart : MonoBehaviour
    {
        private BodyCellNode.Factory m_BodyCellFactory;
        private CellsSettings m_CellsSettings;
        private BoxCollider2D m_BoxCollider2D;
        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_Renderer;
        private RagdollModel m_RagdollModel;
        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;

        private BodyCellNode[,] Nodes;
        [Inject]
        void Construct(
            BodyCellNode.Factory bodyCellFactory,
            CellsSettings settings,
            BoxCollider2D boxCollider,
            SpriteRenderer renderer,
            Rigidbody2D rigidbody2D,
            RagdollModel ragdollModel)
        {
            m_BodyCellFactory = bodyCellFactory;
            m_CellsSettings = settings;
            m_BoxCollider2D = boxCollider;
            m_Renderer = renderer;
            m_Rigidbody2D = rigidbody2D;
            m_RagdollModel = ragdollModel;

        }
        private void Start()
        {
            Setup();
            
        }




        public void Setup()
        {
            CreateNodes();
            ConnectNodes();

        }

        private void CreateNodes()
        {
            
            var savedRot = transform.rotation;
            transform.rotation = Quaternion.identity;
            m_Renderer.ResetBounds();
            var bounds = m_Renderer.bounds;

            float size = m_CellsSettings.cellSize;
            var cellSize = new Vector3(size, size, 1);
            int horizonalCellsNumber = Mathf.CeilToInt(bounds.size.x / size);
            int verticalCellsNumber = Mathf.CeilToInt(bounds.size.y / size);

            float startOffset = size/2;
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
                    cell.transform.rotation = transform.rotation;
                    cell.transform.parent = transform;
                    Nodes[i, j] = cell;

                }
            }

            transform.rotation = savedRot;
            m_Renderer.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.TryGetComponent<Bullet>(out _))
            {
                m_RagdollModel.Activate();
                var vector = (transform.position - collision.transform.position).normalized;
                Debug.Log($"{vector} push body part");
                Rigidbody2D.AddForce(vector* m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);

                
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
