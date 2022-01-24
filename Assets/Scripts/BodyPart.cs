using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public enum SubBodyPosition
    {
        Left,
        Right,
        Up,
        Down
    }

    [System.Serializable]
    public class BodyCellLine
    {
        public BodyCellNode[] Cells;
    }

    [System.Serializable]
    public class CutSettings
    {
        public Vector2 colliderSize;
        public Vector2 colliderOffset;
    }

    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodyPart : MonoBehaviour
    {
        private CellsSettings m_CellsSettings;
        private BoxCollider2D m_BoxCollider2D;
        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_Renderer;
        private HingeJoint2D m_Joint;
        private RagdollModel m_RagdollModel;
        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;
        public HingeJoint2D Joint => m_Joint;

        [SerializeField] public BodyCellLine[] BodyCellLines;
        [SerializeField] private bool m_CheckSliceHirozontal;
        [SerializeField] private JointPoints[] m_JointPoints;
        [SerializeField] private BodySubPart m_BodySubPart1;
        [SerializeField] private CutSettings m_CutSettings;

        [SerializeField] private SubBodyPosition m_CollectCellsDirection;

        [Inject]
        void Construct(
            CellsSettings settings,
            BoxCollider2D boxCollider,
            SpriteRenderer renderer,
            Rigidbody2D rigidbody2D,
            RagdollModel ragdollModel)
        {
            m_CellsSettings = settings;
            m_BoxCollider2D = boxCollider;
            m_Renderer = renderer;
            m_Rigidbody2D = rigidbody2D;
            m_RagdollModel = ragdollModel;

        }

        private void Awake()
        {
            m_Joint = GetComponent<HingeJoint2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.TryGetComponent<Bullet>(out _))
            {
                m_RagdollModel.Activate();
                var vector = (transform.position - collision.transform.position).normalized;
                Rigidbody2D.AddForce(vector * m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);


            }

        }

        bool m_Cutted = false;


        Dictionary<int, BodyCellNode> m_HorizontalDeadNodes = new Dictionary<int, BodyCellNode>();
        Dictionary<int, BodyCellNode> m_VerticalDeadNodes = new Dictionary<int, BodyCellNode>();

        void CollectCellsFromHorizontalDeadNodes()
        {


            foreach (var item in m_HorizontalDeadNodes.Values)
            {
                for (int i = 0; i < BodyCellLines.Length; i++)
                {
                    var nodecell = BodyCellLines[i].Cells[item.IndexX];

                    if (!nodecell.IsDead)
                    {

                        if (item.IndexY < i && m_BodySubPart1)
                        {
                            nodecell.transform.parent = m_BodySubPart1.transform;
                        }
                    }
                }


            }

            m_BoxCollider2D.size = m_CutSettings.colliderSize;
            m_BoxCollider2D.offset = m_CutSettings.colliderOffset;
        }

        void CollectCellsFromVerticalDeadNodes()
        {
            m_BoxCollider2D.size = m_CutSettings.colliderSize;
            m_BoxCollider2D.offset = m_CutSettings.colliderOffset;
        }

            private bool IsCuttedHorizontal() => m_HorizontalDeadNodes.Count == BodyCellLines[0].Cells.Length;
        private bool IsCuttedVertical() => m_VerticalDeadNodes.Count == BodyCellLines.Length;
        public void BodySliceCheck(BodyCellNode node)
        {


            m_HorizontalDeadNodes[node.IndexX] = node;
            m_VerticalDeadNodes[node.IndexY] = node;

            if(!m_Cutted)
            {

                if (m_CheckSliceHirozontal && IsCuttedHorizontal())
                {
                    CollectCellsFromHorizontalDeadNodes();

                    Debug.Log("BodySliced");

                    m_Cutted = true;

                    m_BodySubPart1?.ReconnectAndActivate();

                }
                else if (!m_CheckSliceHirozontal && IsCuttedVertical())
                {
                    
                }

            }
        }

        void CheckJointsConnection()
        {
            for (int i = 0; i < m_JointPoints.Length; i++)
            {
                if (m_JointPoints[i].CheckJointConnection())
                {
                    m_JointPoints[i].DisconnectJoint();
                }
            }
        }

        public void KillSlicedCells(BodyCellNode node)
        {
            switch (m_CollectCellsDirection)
            {
                case SubBodyPosition.Left:
                    KillLeftEntry(node);
                    break;
                case SubBodyPosition.Right:
                    KillRightEntry(node);
                    break;
                case SubBodyPosition.Up:
                    KillUpEntry(node);
                    break;
                case SubBodyPosition.Down:
                    KillDownEntry(node);
                    break;
                default:
                    break;
            }


        }
        private void KillLeftEntry(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillDown(node.m_BottomCell);
            KillUp(node.m_TopCell);

            KillLeftEntry(node.m_LeftCell);
        }
        private void KillRightEntry(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillDown(node.m_BottomCell);
            KillUp(node.m_TopCell);

            KillRightEntry(node.m_RightCell);
        }
        private void KillDownEntry(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillLeft(node.m_LeftCell);
            KillRight(node.m_RightCell);

            KillDownEntry(node.m_BottomCell);
        }

        private void KillUpEntry(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillLeft(node.m_LeftCell);
            KillRight(node.m_RightCell);

            KillUpEntry(node.m_TopCell);
        }
        private void KillLeft(BodyCellNode node)
        {


            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillLeft(node.m_LeftCell);
        }
        private void KillRight(BodyCellNode node)
        {


            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillRight(node.m_RightCell);
        }

        private void KillUp(BodyCellNode node)
        {


            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillRight(node.m_TopCell);
        }

        private void KillDown(BodyCellNode node)
        {


            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);
            node.Rigidbody2D.velocity = Rigidbody2D.velocity;
            KillRight(node.m_BottomCell);
        }
        bool IsBodySliced(BodyCellNode node)
        {
            var result = true;

            if (m_CheckSliceHirozontal)
            {
                CheckSliceLeft(ref result, node);
                CheckSliceRight(ref result, node);
            }
            else
            {
                CheckSliceUp(ref result, node);
                CheckSliceDown(ref result, node);
            }

            return result;
        }

        void CheckSliceLeft(ref bool sliced, BodyCellNode node)
        {
            if (node)
            {
                if (node.Health.IsDead == false)
                    sliced = false;

                CheckSliceLeft(ref sliced, node.m_LeftCell);
            }
        }
        void CheckSliceRight(ref bool sliced, BodyCellNode node)
        {
            if (node)
            {
                if (node.Health.IsDead == false)
                    sliced = false;

                CheckSliceRight(ref sliced, node.m_RightCell);
            }
        }

        void CheckSliceUp(ref bool sliced, BodyCellNode node)
        {
            if (node)
            {
                if (node.Health.IsDead == false)
                    sliced = false;

                CheckSliceUp(ref sliced, node.m_TopCell);
            }
        }
        void CheckSliceDown(ref bool sliced, BodyCellNode node)
        {
            if (node)
            {
                if (node.Health.IsDead == false)
                    sliced = false;

                CheckSliceDown(ref sliced, node.m_BottomCell);
            }
        }

    }


}
