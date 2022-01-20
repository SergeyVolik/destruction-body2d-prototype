using System.Collections;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class BodyCellNode : MonoBehaviour, IDamageable
    {
        private BodyCellNode m_TopCell;
        private BodyCellNode m_BottomCell;
        private BodyCellNode m_LeftCell;
        private BodyCellNode m_RightCell;

        private HealthHandler m_Health;
        private SpriteRenderer m_SpriteRenderer;
        private CellsSettings m_CellSettings;
        private BoxCollider2D m_Collider2D;
        private Rigidbody2D m_Rigidbody2D;

        public void ConnectCells(BodyCellNode right, BodyCellNode left, BodyCellNode bottom, BodyCellNode top)
        {
            m_TopCell = top;
            m_BottomCell = bottom;
            m_LeftCell = left;
            m_RightCell = right;
        }

        [Inject]
        void Construct(
            HealthHandler health,
            SpriteRenderer spriteRenderer,
            CellsSettings settings,
            BoxCollider2D collider,
            Rigidbody2D rb

            )
        {
            m_Health = health;
            m_SpriteRenderer = spriteRenderer;
            m_CellSettings = settings;
            m_Collider2D = collider;
            m_Rigidbody2D = rb;

            health.Init(settings.maxHealth);
            //spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }


        bool Damaged = false;



        private void ApplayDamageToNode(BodyCellNode node, int dmg, Vector3 damagePos)
        {
            if (node)
            {
                node.m_Health.ApplyDamage(dmg);
                node.m_SpriteRenderer.color = Color.Lerp(m_CellSettings.minHealthColor, m_CellSettings.maxHealthColor, node.m_Health.Health / (float)node.m_Health.MaxHealth);

                if (node.m_Health.IsDead)
                {
                    var forceVector = node.transform.position - damagePos;
                    forceVector = new Vector3(forceVector.x + Random.Range(-0.1f, 0.1f), forceVector.y + Random.Range(-0.1f, 0.1f), forceVector.z);
                    node.m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    node.m_Rigidbody2D.AddForce(forceVector.normalized * 200);
                    node.m_Collider2D.enabled = false;
                    node.KillWithDelay();
                }
            }
        }
        public void PopulateDamage(int damage, int horizontalDepth, int verticalDepth, Vector3 damagePos, bool horizontal)
        {
         

           

            int dmg = (int)(damage / horizontalDepth);
            Damaged = true;

            horizontalDepth++;

         

            //m_LeftCell?.PopulateDamage(dmg, horizontalDepth, verticalDepth, damagePos, horizontal);
            m_RightCell?.PopulateDamage(dmg, horizontalDepth, verticalDepth, damagePos, horizontal);


            PopulateTopDamage(m_TopCell, dmg, 1, damagePos);
            PopulateBottomDamage(m_BottomCell, dmg, 1, damagePos);
            ApplayDamageToNode(this, dmg, damagePos);

        }

        public void PopulateTopDamage(BodyCellNode topNode, int damage, int verticalDepth, Vector3 damagePos)
        {


            if (verticalDepth > 3 || topNode == null)
            {
                if (damage > 50)
                    topNode?.ApplayDamageToNode(this, 50, damagePos);
                return;
            }

            topNode.ApplayDamageToNode(this, damage, damagePos);
            verticalDepth++;

            topNode.PopulateTopDamage(topNode.m_TopCell, damage / verticalDepth, verticalDepth, damagePos);
        }

        public void PopulateBottomDamage(BodyCellNode topNode, int damage, int verticalDepth, Vector3 damagePos)
        {

            if (verticalDepth > 3 || topNode == null)
            {
                if(damage > 50)
                    topNode?.ApplayDamageToNode(this, 50, damagePos);

                return;
            }

            topNode.ApplayDamageToNode(this, damage, damagePos);
            verticalDepth++;

            topNode.PopulateBottomDamage(topNode.m_BottomCell, damage / verticalDepth, verticalDepth, damagePos);
        }


        void Update()
        {
            Damaged = false;
        }

        public void ApplyDamage(int damage)
        {
            PopulateDamage(damage, 1, 1, transform.position, true);
        }

        public class Factory : PlaceholderFactory<BodyCellNode> { }

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