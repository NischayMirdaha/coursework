﻿namespace coursework.Models
{

    public class AppData
    {
        public List<Usermodel> Users { get; set; } = new();
        public List<DebtModel> Debts { get; set; } = new();
        public List<TransactionModel> Transactions { get; set; } = new();


    }
}
