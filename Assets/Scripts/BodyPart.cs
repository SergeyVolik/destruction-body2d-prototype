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
                Debug.Log($"{vector} push body part");
                Rigidbody2D.AddForce(vector* m_RagdollModel.Settings.bodyPushForce, ForceMode2D.Impulse);

                
            }
 
        }


    }


}
