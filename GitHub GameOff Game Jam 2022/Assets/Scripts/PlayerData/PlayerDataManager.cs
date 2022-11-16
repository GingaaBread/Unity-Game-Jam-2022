namespace PlayerData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// This class stores all player data, including the player's money balance,
    /// and all items that will be added to their inventory (animal products, crops, etc.)
    /// The money balance is stored separately as an int, and the dictionary stores the items
    /// </summary>
    /// <author>Gino</author>
    public class PlayerDataManager : MonoBehaviour
    {
        // The singleton
        private static PlayerDataManager _instance = null;
        public static PlayerDataManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("PlayerDataManager singleton was called without PlayerDataManager being set up (check that PlayerDataManager is in the scene)");
                return _instance;
            }
            private set { _instance = value; }
        }

        /// <summary>
        /// Returns the player's current money balance
        /// </summary>
        public int AmountOfMoney { get; private set; }

        // A hashmap is used to have a dynamic range of items that can be added
        private Dictionary<ResourceSO, int> inventoryDictionary;

        private void Awake()
        {
            inventoryDictionary = new Dictionary<ResourceSO, int>();

            Assert.IsNull(_instance, "PlayerDataManager singleton is already set. (check there is only one PlayerDataManager in the scene)");
            Instance = this;
        }

        private void Start() 
        {
            Assert.IsNotNull(QuestManager.Instance, "PlayerDataManager expects QuestManager to exist in scene");
        }

        // Money Management

        /// <summary>
        /// Adds the specified amount of money to the player's current balance
        /// </summary>
        /// <param name="amount">How much money should be added?</param>
        public void IncreaseMoneyAmount(int amount)
        {
            Assert.IsTrue(amount > 0);
            AmountOfMoney += amount;
            // TODO: UI Notification
        }

        /// <summary>
        /// Removes  the specified amount of money from the player's current balance
        /// </summary>
        /// <param name="amount">How much money should be removed?</param>
        /// <exception cref="ArithmeticException">
        /// Thrown when this operation results in a negative money balance.
        /// Use HasMoreOrEqualMoneyThan(n) to check if neccessary
        /// </exception>
        public void DecreaseMoneyAmount(int amount)
        {
            Assert.IsTrue(amount > 0);
            AmountOfMoney -= amount;
            // TODO: notification?

            if (AmountOfMoney < 0) throw new ArithmeticException("The last transaction " +
                "left the player with a negative money balance. This should never be the case.");
        }

        // The following methods are all syntactic sugar for ==, <, >, and >=

        public bool HasExactAmountOfMoney(int exactAmount) => AmountOfMoney == exactAmount;
        public bool HasMoreMoneyThan(int amount) => AmountOfMoney > amount;
        public bool HasLessMoneyThan(int amount) => AmountOfMoney < amount;
        public bool HasMoreOrEqualMoneyThan(int amount) => AmountOfMoney >= amount;

        // Inventory Management

        /// <summary>
        /// Used to add an item to the hashmap for the first time
        /// The item name is the key, and the initial value is zero
        /// </summary>
        /// <param name="resourceToInitialise">Which scriptable object should be added to the hashmap?</param>
        public void InitialiseInventoryItem(ResourceSO resourceToInitialise)
        {
            inventoryDictionary.Add(resourceToInitialise, 0);
        }

        /// <summary>
        /// Used to increase the item amount of the specified item
        /// Example usecases would be harvesting a crop or an animal product
        /// </summary>
        /// <param name="resourceToIncrease">The item amount of which item should be increased? (case sensitive)</param>
        /// <param name="amountToIncrease"></param>
        /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public void IncreaseInventoryItemAmount(ResourceSO resourceToIncrease, int amountToIncrease)
        {
            Assert.IsTrue(amountToIncrease > 0);

            if (!HasItemInInventory(resourceToIncrease))
            {
                InitialiseInventoryItem(resourceToIncrease);
            }

            inventoryDictionary[resourceToIncrease] += amountToIncrease;
            QuestManager.Instance.NotifyOfResourceCollected(resourceToIncrease, amountToIncrease);
        }

        /// <summary>
        /// Used to decreasse the item amount of the specified item
        /// Example usecases would be selling a crop or an animal product
        /// </summary>
        /// <param name="resourceToDecrease">The item amount of which item should be decreased? (case sensitive)</param>
        /// <param name="amountToDecrease"></param>
        /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public void DecreaseInventoryItemAmount(ResourceSO resourceToDecrease, int amountToDecrease)
        {
            Assert.IsTrue(amountToDecrease > 0);

            if (!HasItemInInventory(resourceToDecrease))
            {
                InitialiseInventoryItem(resourceToDecrease);
            }

            Assert.IsTrue(inventoryDictionary[resourceToDecrease] >= amountToDecrease);

            inventoryDictionary[resourceToDecrease] -= amountToDecrease;
            // Todo: Notification?
        }

        /// <summary>
        /// Returns if the specified item has already been initialised and added to the inventory
        /// </summary>
        /// <param name="resourceToCheck">The resource scriptable object to check</param>
        /// <returns>true if it exists in the hashmap, else false</returns>
        public bool HasItemInInventory(ResourceSO resourceToCheck) => inventoryDictionary.ContainsKey(resourceToCheck);

        /// <summary>
        /// Returns the item amount of the specified item as an int
        /// </summary>
        /// <param name="ofResource">The amount of which resource?</param>
        /// <returns>The amount of the specified item</returns>
        /// /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public int GetInventoryItemAmount(ResourceSO ofResource)
        {
            if (HasItemInInventory(ofResource))
                return inventoryDictionary[ofResource];

            return 0;
        }
    }
}
