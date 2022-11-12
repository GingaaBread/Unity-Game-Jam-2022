namespace PlayerManagement
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
        private Dictionary<string, int> inventoryDictionary;

        private void Awake()
        {
            inventoryDictionary = new Dictionary<string, int>();

            Assert.IsNull(_instance, "TimeManager singleton is already set. (check there is only one TimeManager in the scene)");
            Instance = this;            
        }

        // Money Management

        /// <summary>
        /// Adds the specified amount of money to the player's current balance
        /// </summary>
        /// <param name="amount">How much money should be added?</param>
        public void IncreaseMoneyAmount(int amount)
        {
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
        /// <param name="itemName">How should the item be called? (case sensitive)</param>
        public void InitialiseInventoryItem(string itemName)
        {
            inventoryDictionary.Add(itemName, 0);
        }

        /// <summary>
        /// Used to increase the item amount of the specified item
        /// Example usecases would be harvesting a crop or an animal product
        /// </summary>
        /// <param name="itemName">The item amount of which item should be increased? (case sensitive)</param>
        /// <param name="amountToIncrease"></param>
        /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public void IncreaseInventoryItemAmount(string itemName, int amountToIncrease)
        {
            if (HasItemInInventory(itemName))
            {
                inventoryDictionary[itemName] += amountToIncrease;
                // Todo: Notification?
            }
            else throw new ApplicationException("Trying to increase an item amount of " +
                $"an item from the player's inventory that doesn't exist: {itemName}.");
        }

        /// <summary>
        /// Used to decreasse the item amount of the specified item
        /// Example usecases would be selling a crop or an animal product
        /// </summary>
        /// <param name="itemName">The item amount of which item should be decreased? (case sensitive)</param>
        /// <param name="amountToIncrease"></param>
        /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public void DecreaseInventoryItemAmount(string itemName, int amountToDecrease)
        {
            if (HasItemInInventory(itemName))
            {
                inventoryDictionary[itemName] -= amountToDecrease;
                // Todo: Notification?
            }
            else throw new ApplicationException("Trying to decrease an item amount of " +
               $"an item from the player's inventory that doesn't exist: {itemName}.");
        }

        /// <summary>
        /// Returns if the specified item has already been initialised and added to the inventory
        /// </summary>
        /// <param name="itemName">The item name to check</param>
        /// <returns>true if it exists in the hashmap, else false</returns>
        public bool HasItemInInventory(string itemName) => inventoryDictionary.ContainsKey(itemName);

        /// <summary>
        /// Returns the item amount of the specified item as an int
        /// </summary>
        /// <param name="ofItemName">The amount of which item name?</param>
        /// <returns>The amount of the specified item</returns>
        /// /// <exception cref="ApplicationException">
        /// Thrown when the item is not in the hashmap.
        /// Use InitialiseInventoryItem() first or check case sensitivity
        /// </exception>
        public int GetInventoryItemAmount(string ofItemName)
        {
            if (HasItemInInventory(ofItemName))
                return inventoryDictionary[ofItemName];

            throw new ApplicationException("Trying to get an item amount of " +
               $"an item from the player's inventory that doesn't exist: {ofItemName}.");
        }
    }
}
