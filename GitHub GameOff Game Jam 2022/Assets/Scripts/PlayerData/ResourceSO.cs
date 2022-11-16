namespace PlayerData
{

    using UnityEngine;

    [CreateAssetMenu(fileName = "NewResource", menuName = "Resources/Resource")]
    public class ResourceSO : ScriptableObject 
    {

        public int basePrice;

        public Sprite iconSprite;
    
    }    
}
