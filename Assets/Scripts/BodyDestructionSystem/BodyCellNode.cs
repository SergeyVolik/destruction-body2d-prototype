using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace Prototype
{
    public class BodyCellNode : MonoBehaviour, IDamageable, IProjectile2DVisitor
    {
        [SerializeField] public BodyCellNode m_TopCell;
        [SerializeField] public BodyCellNode m_BottomCell;
        [SerializeField] public BodyCellNode m_LeftCell;
        [SerializeField] public BodyCellNode m_RightCell;

        [SerializeField] private HealthHandler m_Health;
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
      
        [SerializeField] private BoxCollider2D m_Collider2D;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;

        [SerializeField]  public int IndexX;
        [SerializeField]  public int IndexY;
        public HealthHandler Health => m_Health;
        public bool IsDead => Health.IsDead;
        public Rigidbody2D Rigidbody2D => m_Rigidbody2D;

        public BodyPart BodyPart;
        private Transform m_Transform;
        private CellsSettings.Settings m_CellSettings;

        private TransformSavedData m_TransformSavedData;

        void Awake()
        {
           
            m_Transform = transform;

            m_TransformSavedData = new TransformSavedData()
            {
                parent = m_Transform.parent,
                localPosition = m_Transform.localPosition,
                localRotation = m_Transform.localRotation,
                localScale = m_Transform.localScale
                
            };


            gameObject.SetActive(false);
        }

        public void Construct(CellsSettings cellSettings)
        {
            m_CellSettings = cellSettings.settings;
            m_Health.ResetValues(m_CellSettings.maxHealth);
        }

        public void ConnectCells(BodyCellNode topCell, BodyCellNode bottomCell, BodyCellNode leftCell, BodyCellNode rightCell)
        {
            m_TopCell = topCell;
            m_BottomCell = bottomCell;
            m_LeftCell = leftCell;
            m_RightCell = rightCell;
        }


        public void ApplayDamageToNode(BodyCellNode node, int dmg, Vector3 damagePos)
        {
            if (node)
            {
                node.m_Health.ApplyDamage(dmg);
               

                if (node.m_Health.IsDead)
                {
                    var rb = node.m_Rigidbody2D;
                    var trans = node.m_Transform;

                    var forceVector = trans.position - damagePos;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.AddForce(forceVector.normalized * m_CellSettings.pushCellForce, ForceMode2D.Impulse);
                    trans.parent = null;
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

            //m_TopCell?.PopulateDamage(25, horizontalDepth, damagePos);
            //m_BottomCell?.PopulateDamage(25, horizontalDepth, damagePos);
            //m_LeftCell?.PopulateDamage(25, horizontalDepth, damagePos);
            //m_RightCell?.PopulateDamage(25, horizontalDepth, damagePos);

        }

        public void ApplyDamage(int damage, Vector3 damagePos)
        {
            BodyPart.ActivateAllBodyCells();
            PopulateDamage(damage, 1, damagePos);

            if(Health.IsDead)
                BodyPart.SetDeadCell(this);

        }



        
      

        public void ResetValues()
        {
            m_TransformSavedData.ResetValues(m_Transform);

            m_Health.ResetValues(m_CellSettings.maxHealth);
            m_SpriteRenderer.color = m_CellSettings.maxHealthColor;

            Rigidbody2DUtils.ResetValues(m_Rigidbody2D);

            gameObject.SetActive(false);

        }

        public void Visit(CannonBall ball)
        {
            Profiler.BeginSample("CannonBall Visit");
            ApplyDamage(ball.Settings.damage, ball.transform.position);
            Profiler.EndSample();
        }

        public void Visit(GrenadeProjectile grenadeProjectile)
        {
            Profiler.BeginSample("GrenadeProjectile Visit");
            grenadeProjectile.Explode();
            Profiler.EndSample();
        }

        public void Visit(LaserProjectile laserProjectile)
        {
            Profiler.BeginSample("LaserProjectile Visit");
            ApplyDamage(laserProjectile.Settings.damage, laserProjectile.transform.position);
            Profiler.EndSample();

        }

        public void Visit(PistolBullet pistolBullet)
        {
            Profiler.BeginSample("PistolBullet Visit");
            pistolBullet.SlowBullet();
            ApplyDamage(pistolBullet.Settings.damage, pistolBullet.transform.position);
            Profiler.EndSample();
        }
    }

}
