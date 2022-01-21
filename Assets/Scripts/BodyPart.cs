using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{

    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodyPart : MonoBehaviour
    {
        private CellsSettings m_CellsSettings;
        private BoxCollider2D m_BoxCollider2D;
        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_Renderer;
        private RagdollModel m_RagdollModel;
        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;


        [SerializeField] public BodyCellNode[,] BodyCells;
        [SerializeField] private List<HingeJoint2D> m_SliceJoints;
        [SerializeField] private bool m_CheckSliceHirozontal;
        [SerializeField] private bool m_KillSlicedCellsToDown;
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



        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.TryGetComponent<Bullet>(out _))
            {
                m_RagdollModel.Activate();
                var vector = (transform.position - collision.transform.position).normalized;
                Rigidbody2D.AddForce(vector* m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);

                
            }
 
        }


        public void BodySliced(BodyCellNode node)
        {
            if (IsBodySliced(node))
            {
                print($"{name} part Sliced");
                for (int i = 0; i < m_SliceJoints.Count; i++)
                {
                    m_SliceJoints[i].connectedBody = null;
                    m_SliceJoints[i].enabled = false;
                }

                KillSlicedCells(node);
            }
          
        }

        public void KillSlicedCells(BodyCellNode node)
        {
            if(m_KillSlicedCellsToDown)
                KillDown(node);
            else
                KillUp(node);

        }

        private void KillDown(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node,  100, node.transform.position);

            KillLeft(node.m_LeftCell);
            KillRight(node.m_RightCell);

            KillDown(node.m_BottomCell);
        }

        private void KillUp(BodyCellNode node)
        {
            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);

            KillLeft(node.m_LeftCell);
            KillRight(node.m_RightCell);

            KillUp(node.m_TopCell);
        }
        private void KillLeft(BodyCellNode node)
        {
           

            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);

            KillLeft(node.m_LeftCell);
        }
        private void KillRight(BodyCellNode node)
        {
           

            if (node == null)
                return;

            node.ApplayDamageToNode(node, 100, node.transform.position);

            KillRight(node.m_RightCell);
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
