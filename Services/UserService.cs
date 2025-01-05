using coursework.Models;
using System.Text.Json;

namespace coursework.Services
{
    public class UserService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "users.json");

        public UserModel? CurrentUser { get; private set; } // Property for current logged-in user

        // Load all users from the JSON file
        public List<UserModel> LoadUsers()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FilePath, "[]");
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<UserModel>>(json) ?? new List<UserModel>();
        }

        // Save the list of users to the JSON file
        public void SaveUsers(List<UserModel> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        // Method to validate password
        public bool ValidatePassword(string inputPassword, string storedHashedPassword)
        {
            // Use the method of the UserModel to compare the input password
            return HashPassword(inputPassword) == storedHashedPassword;
        }

        // Method to hash the password (SHA256 or another algorithm can be used here)
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes); // Return base64 string as hashed password
            }
        }

        // Login method to authenticate the user
        public bool Login(string username, string password)
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && ValidatePassword(password, user.PasswordHash))
            {
                CurrentUser = user; // Set the current logged-in user
                return true;
            }

            return false; // Invalid login
        }

        // Logout method to clear the current logged-in user
        public void Logout()
        {
            CurrentUser = null; // Clear the current user to log out
        }

        // Register new user by setting password securely
        public void Register(string username, string password, string email)
        {
            var users = LoadUsers();
            var newUser = new UserModel
            {
                Id = users.Any() ? users.Max(u => u.Id) + 1 : 1, // Generate new ID
                Username = username,
                Email = email
            };
            newUser.SetPassword(password); // Set the hashed password

            users.Add(newUser); // Add the new user to the list
            SaveUsers(users); // Save updated list to the file
        }
    }
}
