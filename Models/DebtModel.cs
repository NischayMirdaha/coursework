namespace coursework.Models
{
    public class DebtModel
    {
        public int Id { get; set; } // Unique debt identifier
        public decimal Amount { get; set; } // Total debt amount
        public decimal PaidAmount { get; set; } // Amount paid towards debt
        public string Source { get; set; } // Source of the debt (e.g., loan, mortgage)
        public DateTime DueDate { get; set; } // Due date for repayment
        public bool IsCleared { get; set; } // Whether the debt is cleared
        public DateTime? ClearedDate { get; set; } // Date when the debt was cleared
        public string Notes { get; set; } // Optional notes
        public DateTime Date { get; set; }
    }
}
