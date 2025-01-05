namespace coursework.Models
{
    public class TransactionModel
    {
        // Enum defined inside the Transaction model
        public enum TransactionType
        {
            Inflow,  // Credit
            Outflow,   // Debit
        }

        // Enum for transaction method
        public enum TransactionMethod
        {
            Cash,
            Online,
            Loan,
            Gift,
            Bonus,
            Other
        }

        public decimal Amount { get; set; } // Amount of the transaction
        public TransactionType Type { get; set; } // Enum: Credit, Debit
        public List<string> Tags { get; set; } = new List<string>(); // List of custom tags (e.g., "Rent", "Monthly")
        public List<string> Source { get; set; } = new List<string>(); // List of sources like "Cash", "Online", etc.
        public string Notes { get; set; } // Optional notes
        public DateTime Date { get; set; } // Transaction date
        public decimal Balance { get; set; } // Current balance
        public TransactionMethod Method { get; set; } // Method used for the transaction

        // Method to check if there is sufficient balance for an outflow
        public bool IsSufficientBalanceForOutflow()
        {
            if (Type == TransactionType.Outflow && Amount > Balance)
            {
                return false; // Not enough balance
            }
            return true; // Sufficient balance or not an outflow transaction
        }

        // Method to update balance after a transaction
        public void UpdateBalance()
        {
            if (Type == TransactionType.Inflow)
            {
                Balance += Amount; // Add amount for inflow
            }
            else if (Type == TransactionType.Outflow)
            {
                if (IsSufficientBalanceForOutflow())
                {
                    Balance -= Amount; // Subtract amount for outflow
                }
                else
                {
                    throw new InvalidOperationException("Insufficient balance for this transaction.");
                }
            }
        }

        // Method to add a custom or existing tag
        public void AddTag(string tag)
        {
            if (!string.IsNullOrEmpty(tag) && !Tags.Contains(tag))
            {
                Tags.Add(tag); // Add tag if it is not empty and not already in the list
            }
        }

        // Method to remove a tag
        public void RemoveTag(string tag)
        {
            if (Tags.Contains(tag))
            {
                Tags.Remove(tag); // Remove tag if it exists
            }
        }

        // Method to check if a tag exists in the transaction
        public bool HasTag(string tag)
        {
            return Tags.Contains(tag); // Return true if the tag exists
        }

        // Method to get all tags (just for convenience)
        public List<string> GetTags()
        {
            return Tags; // Return list of tags
        }

        // Method to add a transaction method
        public void SetMethod(TransactionMethod method)
        {
            Method = method; // Set the transaction method (e.g., Cash, Online)
        }

        // Method to get the string representation of the transaction method
        public string GetMethodAsString()
        {
            return Method.ToString(); // Get method as string (e.g., "Cash", "Online")
        }

        // Method to filter transactions based on type, tags, and date range
        public static List<TransactionModel> FilterTransactions(
            List<TransactionModel> transactions,
            TransactionType? type = null,
            List<string> tags = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            return transactions
                .Where(t => !type.HasValue || t.Type == type)
                .Where(t => tags == null || tags.All(tag => t.Tags.Contains(tag)))
                .Where(t => (!startDate.HasValue || t.Date >= startDate) && (!endDate.HasValue || t.Date <= endDate))
                .ToList();
        }
    }
}
