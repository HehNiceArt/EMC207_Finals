using UnityEngine;
using TMPro;

namespace Platformer
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel playerHealthChannel;
        [SerializeField] TextMeshProUGUI playerHealthText;

        [SerializeField] int currentHealth;

        public bool IsDead => currentHealth <= 0;

        void Awake()
        {
            currentHealth = maxHealth;
            playerHealthText.text = currentHealth.ToString();
        }

        void Start()
        {
            PublishHealthPercentage();
        }
        void Update()
        {
            playerHealthText.text = currentHealth.ToString();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            PublishHealthPercentage();
        }

        void PublishHealthPercentage()
        {
            if (playerHealthChannel != null)
            {
                playerHealthChannel.Invoke(currentHealth / (float)maxHealth);
            }
        }
    }
}