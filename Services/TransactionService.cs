using coursework.Models;
using System.Text.Json;

public class TransactionService
{
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "Data");
    private static readonly string FilePath = Path.Combine(FolderPath, "SavedData.json"); // Updated file to store complete AppData

    // Load all data (users, debts, transactions) from the JSON file
    public AppData LoadAppData()
    {
        if (!File.Exists(FilePath))
            return new AppData();  // Return an empty AppData if the file doesn't exist

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<AppData>(json) ?? new AppData(); // Deserialize into AppData
    }

    // Save all data (users, debts, transactions) to the JSON file
    public void SaveAppData(AppData appData)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        var json = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);  // Save AppData to the file
    }

    // Process a transaction and update balance accordingly
    public bool ProcessTransaction(TransactionModel transaction, ref decimal balance)
    {
        // Sufficient balance check for outflow transactions
        if (transaction.Type == TransactionModel.TransactionType.Outflow)
        {
            if (transaction.Amount > balance)
            {
                Console.WriteLine("Error: Insufficient balance for this transaction.");
                return false;  // Transaction failed due to insufficient balance
            }
        }

        // Update the balance after the transaction is processed
        if (transaction.Type == TransactionModel.TransactionType.Outflow)
        {
            balance -= transaction.Amount;  // Deduct the amount for outflow
        }
        else if (transaction.Type == TransactionModel.TransactionType.Inflow)
        {
            balance += transaction.Amount;  // Add the amount for inflow
        }

        // Save the updated data
        var appData = LoadAppData();
        appData.Transactions.Add(transaction); // Assuming AppData has a Transactions list to hold transactions
        SaveAppData(appData);

        return true;  // Transaction successfully processed
    }

    // Method to add a custom or existing tag to a transaction
    public void AddTagToTransaction(TransactionModel transaction, string tag)
    {
        if (transaction != null && !string.IsNullOrEmpty(tag))
        {
            transaction.AddTag(tag);  // Use the AddTag method from TransactionModel
            var appData = LoadAppData();
            appData.Transactions.Add(transaction);  // Add updated transaction back to AppData
            SaveAppData(appData);  // Save the updated data
        }
    }

    // Method to remove a tag from a transaction
    public void RemoveTagFromTransaction(TransactionModel transaction, string tag)
    {
        if (transaction != null && !string.IsNullOrEmpty(tag))
        {
            transaction.RemoveTag(tag);  // Use the RemoveTag method from TransactionModel
            var appData = LoadAppData();
            appData.Transactions.Add(transaction);  // Add updated transaction back to AppData
            SaveAppData(appData);  // Save the updated data
        }
    }
  


    // Method to filter transactions by type, tags, and date range
    public List<TransactionModel> FilterTransactions(
        TransactionModel.TransactionType? type = null,
        List<string> tags = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var appData = LoadAppData();  // Load the latest AppData
        return appData.Transactions
            .Where(t => !type.HasValue || t.Type == type)
            .Where(t => tags == null || tags.All(tag => t.Tags.Contains(tag)))
            .Where(t => (!startDate.HasValue || t.Date >= startDate) && (!endDate.HasValue || t.Date <= endDate))
            .ToList();
    }
}
