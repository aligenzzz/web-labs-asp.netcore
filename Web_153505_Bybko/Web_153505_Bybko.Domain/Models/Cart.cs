using System;
using System.Collections.Generic;
using System.Linq;
using Web_153505_Bybko.Domain.Entities;

namespace Web_153505_Bybko.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Objects' list in the cart
        /// key - object's id
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Add object to cart
        /// </summary>
        /// <param name="book">Adding object</param>
        public virtual void AddToCart(Book book)
        {
            if (CartItems.ContainsKey(book.Id))
                CartItems[book.Id].Count++;
            else
                CartItems.Add(book.Id, new CartItem() 
                { Item = book, Count = 1 });
        }

        /// <summary>
        /// Delete object from the cart
        /// </summary>
        /// <param name="id"> id of the deleting object</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }

        /// <summary>
        /// Clear cart
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Count of objects in the cart
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Count); }

        /// <summary>
        /// Total price of the cart
        /// </summary>
        public decimal TotalPrice
        {
            get => (decimal)CartItems.Sum(item => item.Value.Item.Price * item.Value.Count)!;
        }
    }
}
