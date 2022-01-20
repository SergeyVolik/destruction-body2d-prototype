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
        }


        bool Damaged = false;



        private void ApplayDamageToNode(BodyCellNode node, int dmg, Vector3 damagePos)
        {
            if (node)
            {
                node.m_Health.ApplyDamage(dmg);
               

                if (node.m_Health.IsDead)
                {
                    var forceVector = node.transform.position - damagePos;
                    node.m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    node.m_Rigidbody2D.AddForce(forceVector.normalized * m_CellSettings.pushCellForce, ForceMode2D.Impulse);
                    node.m_Collider2D.enabled = false;
                    node.transform.parent = null;
                    node.KillWithDelay();
                    Color.Lerp(m_CellSettings.minHealthColor, m_CellSettings.maxHealthColor, Random.Range(0, 1f));
                    return;
                }

                node.m_SpriteRenderer.color = Color.Lerp(m_CellSettings.minHealthColor, m_CellSettings.maxHealthColor, node.m_Health.Health / (float)node.m_Health.MaxHealth);
            }
        }
        public void PopulateDamage(int damage, int horizontalDepth, int verticalDepth, Vector3 damagePos, bool horizontal)
        {


            if (horizontalDepth == 4)
                return;

            int dmg = (int)(damage / horizontalDepth);
            Damaged = true;

            horizontalDepth++;

        
            ApplayDamageToNode(this, dmg, damagePos);

            m_TopCell?.PopulateDamage(25, horizontalDepth, verticalDepth, damagePos, horizontal);
            m_BottomCell?.PopulateDamage(25, horizontalDepth, verticalDepth, damagePos, horizontal);
            m_LeftCell?.PopulateDamage(25, horizontalDepth, verticalDepth, damagePos, horizontal);
            m_RightCell?.PopulateDamage(25, horizontalDepth, verticalDepth, damagePos, horizontal);

        }




        void Update()
        {

        }

        public void ApplyDamage(int damage, Vector3 damagePos)
        {
            PopulateDamage(damage, 1, 1, damagePos, true);
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
