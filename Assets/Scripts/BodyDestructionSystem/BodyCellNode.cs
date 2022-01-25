using System.Collections;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class BodyCellNode : MonoBehaviour, IDamageable
    {
        [SerializeField] public BodyCellNode m_TopCell;
        [SerializeField] public BodyCellNode m_BottomCell;
        [SerializeField] public BodyCellNode m_LeftCell;
        [SerializeField] public BodyCellNode m_RightCell;


        [SerializeField] private HealthHandler m_Health;
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private CellsSettings m_CellSettings;
        [SerializeField] private BoxCollider2D m_Collider2D;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;

        [SerializeField]  public int IndexX;
        [SerializeField]  public int IndexY;
        public HealthHandler Health => m_Health;
        public bool IsDead => Health.IsDead;
        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;

        public BodyPart BodyPart;
        private Transform m_Transform;
        void Start()
        {
            m_Health.Init(100);
            m_Transform = transform;
        }

        public void ConnectCells(BodyCellNode topCell, BodyCellNode bottomCell, BodyCellNode leftCell, BodyCellNode rightCell)
        {
            m_TopCell = topCell;
            m_BottomCell = bottomCell;
            m_LeftCell = leftCell;
            m_RightCell = rightCell;
        }

        IEnumerator ActivateBoxCollider(BodyCellNode node)
        {
            yield return new WaitForSeconds(0.1f);
            node.m_Collider2D.isTrigger = false;
        }
        public void ApplayDamageToNode(BodyCellNode node, int dmg, Vector3 damagePos)
        {
            if (node)
            {
                node.m_Health.ApplyDamage(dmg);
               

                if (node.m_Health.IsDead)
                {
                    var forceVector = node.m_Transform.position - damagePos;
                    node.m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    node.m_Rigidbody2D.AddForce(forceVector.normalized * m_CellSettings.pushCellForce, ForceMode2D.Impulse);
                    StartCoroutine(ActivateBoxCollider(node));
                    node.m_Transform.parent = null;
                    node.KillWithDelay();
                    Color.Lerp(m_CellSettings.minHealthColor, m_CellSettings.maxHealthColor, Random.Range(0, 1f));
                    return;
                }

                node.m_SpriteRenderer.color = Color.Lerp(m_CellSettings.minHealthColor, m_CellSettings.maxHealthColor, node.m_Health.Health / (float)node.m_Health.MaxHealth);
            }
        }
        public void PopulateDamage(int damage, int horizontalDepth, Vector3 damagePos)
        {


            if (horizontalDepth == 4)
                return;

            int dmg = (int)(damage / horizontalDepth);

            horizontalDepth++;

        
            ApplayDamageToNode(this, dmg, damagePos);

            m_TopCell?.PopulateDamage(25, horizontalDepth, damagePos);
            m_BottomCell?.PopulateDamage(25, horizontalDepth, damagePos);
            m_LeftCell?.PopulateDamage(25, horizontalDepth, damagePos);
            m_RightCell?.PopulateDamage(25, horizontalDepth, damagePos);

        }

        public void ApplyDamage(int damage, Vector3 damagePos)
        {

            PopulateDamage(damage, 1, damagePos);

            if(Health.IsDead)
                BodyPart.BodySliceCheck(this);

        }

        void KillWithDelay()
        {
            StartCoroutine(KillWithDelay(2f));
        }

        IEnumerator KillWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }

}