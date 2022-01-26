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
    public class BodyPart : MonoBehaviour, IDamageable
    {

        [SerializeField] public BodyCellLine[] BodyCellLines;
        [SerializeField] private bool m_CheckSliceHirozontal;
        [SerializeField] private JointPoints[] m_JointPoints;
        [SerializeField] private BodySubPart m_BodySubPart1;
        [SerializeField] private CutSettings m_CutSettings;

        [SerializeField] private SubBodyPosition m_SubBodyPosition;

        private CellsSettings m_CellsSettings;
        private BoxCollider2D m_BoxCollider2D;
        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_Renderer;
        private HingeJoint2D m_Joint;
        private RagdollModel m_RagdollModel;
        private SlowmotionManager m_SlowmotionManager;
        private Transform m_Transform;
        private bool m_Cutted = false;
        private Dictionary<int, BodyCellNode> m_HorizontalDeadNodes = new Dictionary<int, BodyCellNode>();
        private Dictionary<int, BodyCellNode> m_VerticalDeadNodes = new Dictionary<int, BodyCellNode>();

        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;
        public HingeJoint2D Joint => m_Joint;

        private Vector2 m_PushBodyForce;

        [Inject]
        void Construct(
            CellsSettings settings,
            BoxCollider2D boxCollider,
            SpriteRenderer renderer,
            Rigidbody2D rigidbody2D,
            RagdollModel ragdollModel,
            SlowmotionManager slowmotionManager)
        {
            m_CellsSettings = settings;
            m_BoxCollider2D = boxCollider;
            m_Renderer = renderer;
            m_Rigidbody2D = rigidbody2D;
            m_RagdollModel = ragdollModel;
            m_SlowmotionManager = slowmotionManager;
            InitBodyCells();
        }

        bool m_CellsActivated = false;
        private void Awake()
        {
            m_Joint = GetComponent<HingeJoint2D>();
            m_Transform = transform;
            
        }

        private void InitBodyCells()
        {
            for (int i = 0; i < BodyCellLines.Length; i++)
            {
                for (int j = 0; j < BodyCellLines[i].Cells.Length; j++)
                {
                    BodyCellLines[i].Cells[j].Construct(m_CellsSettings);
                }
            }
        }

    
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.TryGetComponent<Projectile2D>(out _))
            {              
                m_PushBodyForce = (m_Transform.position - collision.transform.position).normalized;      
                ActivateAllBodyCells();
            }

        }

       
        public void ActivateAllBodyCells()
        {
            if (!m_CellsActivated)
            {
                m_CellsActivated = true;
                m_Renderer.enabled = false;
                for (int i = 0; i < BodyCellLines.Length; i++)
                {
                    var cells = BodyCellLines[i].Cells;

                    for (int j = 0; j < cells.Length; j++)
                    {
                        cells[j].gameObject.SetActive(true);
                    }
                }
            }
        }

        void DeactivateAllBodyCells()
        {
            if (m_CellsActivated)
            {
                m_CellsActivated = false;
                m_Renderer.enabled = true;
                for (int i = 0; i < BodyCellLines.Length; i++)
                {
                    var cells = BodyCellLines[i].Cells;

                    for (int j = 0; j < cells.Length; j++)
                    {
                        cells[j].gameObject.SetActive(false);
                    }
                }
            }
        }


        void CollectCellsFromHorizontalDeadNodes()
        {

            switch (m_SubBodyPosition)
            {
                case SubBodyPosition.Up:
                    CollectSubBodyCellsUp();

                    break;
                case SubBodyPosition.Down:
                    CollectSubBodyCellsDown();
                    break;
                default:
                    break;
            }


            m_BoxCollider2D.size = m_CutSettings.colliderSize;
            m_BoxCollider2D.offset = m_CutSettings.colliderOffset;
        }

        private void CollectSubBodyCellsUp()
        {
            var deadNodes = m_HorizontalDeadNodes.Values;

            foreach (var item in deadNodes)
            {
                var IndexY = item.IndexY;
                var IndexX = item.IndexX;

                for (int i = 0; i < BodyCellLines.Length; i++)
                {
                    var nodecell = BodyCellLines[i].Cells[IndexX];

                    if (!nodecell.IsDead && IndexY < i)
                    {
                        nodecell.transform.parent = m_BodySubPart1.transform;                      
                    }
                }

            }
        }

        private void CollectSubBodyCellsDown()
        {
            var deadNodes = m_HorizontalDeadNodes.Values;

            foreach (var item in deadNodes)
            {
                var IndexY = item.IndexY;
                var IndexX = item.IndexX;

                for (int i = 0; i < BodyCellLines.Length; i++)
                {
                    var nodecell = BodyCellLines[i].Cells[IndexX];

                    if (!nodecell.IsDead && IndexY > i)
                    {
                         nodecell.transform.parent = m_BodySubPart1.transform;
                        
                    }
                }

            }
        }

        private void CollectSubBodyCellLeft()
        {

            var deadNodes = m_VerticalDeadNodes.Values;

            foreach (var item in deadNodes)
            {
                var indexX = item.IndexX;
                var indexY = item.IndexY;

                var lineOfCells = BodyCellLines[indexY].Cells;

               

                for (int i = 0; i < lineOfCells.Length; i++)
                {
                    var nodecell = lineOfCells[i];

                    if (!nodecell.IsDead && indexX > i)
                    {
                         nodecell.transform.parent = m_BodySubPart1.transform;
                        
                    }
                }

            }
        }

        private void CollectSubBodyCellRight()
        {
            var deadNodes = m_VerticalDeadNodes.Values;

            foreach (var item in deadNodes)
            {
                var indexX = item.IndexX;
                var indexY = item.IndexY;

                var lineOfCells = BodyCellLines[indexY].Cells;
              
                for (int i = 0; i < lineOfCells.Length; i++)
                {
                    var nodecell = lineOfCells[i];

                    if (!nodecell.IsDead && indexX < i)
                    {
                        nodecell.transform.parent = m_BodySubPart1.transform;
                    }
                }

            }
        }

        void CollectCellsFromVerticalDeadNodes()
        {
            switch (m_SubBodyPosition)
            {
                case SubBodyPosition.Left:
                    CollectSubBodyCellLeft();
                    break;
                case SubBodyPosition.Right:
                    CollectSubBodyCellRight();
                    break;
                default:
                    break;
            }

            m_BoxCollider2D.size = m_CutSettings.colliderSize;
            m_BoxCollider2D.offset = m_CutSettings.colliderOffset;
        }

        private bool IsCuttedHorizontal() => m_HorizontalDeadNodes.Count == BodyCellLines[0].Cells.Length;
        private bool IsCuttedVertical() => m_VerticalDeadNodes.Count == BodyCellLines.Length;

        bool m_NeedCheckJoints = true;

        private void LateUpdate()
        {
            if (m_NeedCheckJoints)
            {
                m_NeedCheckJoints = false;

                CheckJointsConnection();

                if (!m_Cutted)
                {

                    if (m_CheckSliceHirozontal && IsCuttedHorizontal())
                    {
                        
                        CollectCellsFromHorizontalDeadNodes();
                      
                        m_Cutted = true;
                        m_BodySubPart1?.ActivateAndConnectJoints();
                        m_BodySubPart1.Rigidbody2D.velocity = Rigidbody2D.velocity;
                        m_RagdollModel.Activate();
                        Rigidbody2D.AddForce(m_PushBodyForce * m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);
                        m_SlowmotionManager.DoSlowmotion();

                    }
                    else if (!m_CheckSliceHirozontal && IsCuttedVertical())
                    {
                        CollectCellsFromVerticalDeadNodes();
                       
                        m_Cutted = true;
                        m_BodySubPart1?.ActivateAndConnectJoints();
                        m_BodySubPart1.Rigidbody2D.velocity = Rigidbody2D.velocity;
                        m_RagdollModel.Activate();
                        Rigidbody2D.AddForce(m_PushBodyForce * m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);
                        m_SlowmotionManager.DoSlowmotion();
                    }

                }
            }
        }
        public void BodySliceCheck(BodyCellNode node)
        {

          
            m_HorizontalDeadNodes[node.IndexX] = node;
            m_VerticalDeadNodes[node.IndexY] = node;
            m_NeedCheckJoints = true;

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
            switch (m_SubBodyPosition)
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

        public void ApplyDamage(int damage, Vector3 pos)
        {
            ActivateAllBodyCells();
        }
    }


}
