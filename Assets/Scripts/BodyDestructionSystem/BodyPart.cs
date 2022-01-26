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

        private BodyCellNode[] m_HorizontalDeadNodes;
        private BodyCellNode[] m_VerticalDeadNodes;

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
            m_HorizontalDeadNodes = new BodyCellNode[BodyCellLines[0].Cells.Length];
            m_VerticalDeadNodes = new BodyCellNode[BodyCellLines.Length];

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

            for (int j = 0; j < m_HorizontalDeadNodes.Length; j++)     
            {
                var item = m_HorizontalDeadNodes[j];

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
            for (int j = 0; j < m_HorizontalDeadNodes.Length; j++)
            {
                var item = m_HorizontalDeadNodes[j];

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

            for (int j = 0; j < m_VerticalDeadNodes.Length; j++)
            {
                var item = m_VerticalDeadNodes[j];

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
            for (int j = 0; j < m_VerticalDeadNodes.Length; j++)
            {

                var item = m_VerticalDeadNodes[j];

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

        private bool IsCuttedHorizontal() => IsAnyNullInArray(m_HorizontalDeadNodes);
        private bool IsCuttedVertical() => IsAnyNullInArray(m_VerticalDeadNodes);

        private bool IsAnyNullInArray(BodyCellNode[] array)
        {
            bool cutted = true;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                    cutted = false;
            }

            return cutted;
        }
        
    

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
                        m_BodySubPart1.ActivateAndConnectJoints();
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
        public void SetDeadCell(BodyCellNode node)
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

        

        public void ApplyDamage(int damage, Vector3 pos)
        {
            ActivateAllBodyCells();
        }
    }


}
