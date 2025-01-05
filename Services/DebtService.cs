using coursework.Models;
using System.Text.Json;

public class DebtService
{
    private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static readonly string FolderPath = Path.Combine(DesktopPath, "Data");
    private static readonly string FilePath = Path.Combine(FolderPath, "SavedData.json");

    // Load all data (including debts) from the JSON file
    public AppData LoadAppData()
    {
        if (!File.Exists(FilePath))
        {
            // Return an empty AppData if the file doesn't exist
            return new AppData();
        }

        try
        {
            var json = File.ReadAllText(FilePath);

            // Handle if the JSON file is empty or the structure is incorrect
            return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        }
        catch (JsonException ex)
        {
            // Log the exception or handle it as needed (e.g., return a new AppData or retry logic)
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return new AppData(); // Return a new AppData if deserialization fails
        }
    }

    // Save all data (including debts) to the JSON file
    public void SaveAppData(AppData appData)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);  // Ensure the directory exists
        }

        var json = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });

        // Ensure the file is created
        File.WriteAllText(FilePath, json);
    }


    // Save the list of debts
    public void SaveDebts(List<DebtModel> debts)
    {
        var appData = LoadAppData(); // Load existing data
        appData.Debts = debts; // Update the Debts list
        SaveAppData(appData); // Save updated AppData to file
    }

    // Get all debts (no user filter)
    public List<DebtModel> GetAllDebts()
    {
        return LoadAppData().Debts; // Return all debts in the data
    }

    // Add a new debt
    public void AddDebt(DebtModel newDebt)
    {
        var appData = LoadAppData(); // Load existing data

        // Generate a unique ID for the new debt
        newDebt.Id = appData.Debts.Any() ? appData.Debts.Max(d => d.Id) + 1 : 1;
        appData.Debts.Add(newDebt); // Add the new debt

        SaveAppData(appData); // Save updated data to the file
    }

    // Update an existing debt
    public void UpdateDebt(DebtModel updatedDebt)
    {
        var appData = LoadAppData(); // Load existing data
        var existingDebt = appData.Debts.FirstOrDefault(d => d.Id == updatedDebt.Id);

        if (existingDebt != null)
        {
            // Update the debt's properties
            existingDebt.Amount = updatedDebt.Amount;
            existingDebt.PaidAmount = updatedDebt.PaidAmount;
            existingDebt.Source = updatedDebt.Source;
            existingDebt.DueDate = updatedDebt.DueDate;
            existingDebt.IsCleared = updatedDebt.IsCleared;
            existingDebt.ClearedDate = updatedDebt.ClearedDate;
            existingDebt.Notes = updatedDebt.Notes;
            existingDebt.Date = updatedDebt.Date;
        }

        SaveAppData(appData); // Save updated data to the file
    }

    // Delete a debt by ID
    public void DeleteDebt(int debtId)
    {
        var appData = LoadAppData(); // Load existing data
        var debtToRemove = appData.Debts.FirstOrDefault(d => d.Id == debtId);

        if (debtToRemove != null)
        {
            appData.Debts.Remove(debtToRemove); // Remove the debt
        }

        SaveAppData(appData); // Save updated data to the file
    }
}
