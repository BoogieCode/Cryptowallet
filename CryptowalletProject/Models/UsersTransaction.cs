//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptowalletProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersTransaction
    {
        public int TransactionsId { get; set; }
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrencyRate { get; set; }
        public System.DateTime TransactionData { get; set; }
    
        public virtual UsersBankAccount UsersBankAccount { get; set; }
        public virtual UsersBankAccount UsersBankAccount1 { get; set; }
    }
}
