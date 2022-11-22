namespace PlayerData
{

    using UnityEngine;

    [CreateAssetMenu(fileName = "NewResource", menuName = "Resources/Resource")]
    public class ResourceSO : ScriptableObject
    {

        public enum SeasonBonus {SUMMER, WINTER, FALL, SPRING, NONE };

        public int basePrice;
        public SeasonBonus seasonBonus = SeasonBonus.NONE;
        public Sprite iconSprite;

    }
}
