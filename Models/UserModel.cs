using System;
using System.Security.Cryptography;
using System.Text;

namespace coursework.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        // Properties for User details
        public string Username { get; set; } // The username for login
        public string PasswordHash { get; set; } // The hashed password for login
        public string Email { get; set; }    // The user's email address

        // You can add more fields if needed, such as:
        // public string FullName { get; set; }
        // public DateTime DateOfBirth { get; set; }

        // Method to hash the password before storing it
        public void SetPassword(string password)
        {
            PasswordHash = HashPassword(password);
        }

        // Method to verify the entered password by hashing it and comparing with stored hash
        public bool VerifyPassword(string enteredPassword)
        {
            return PasswordHash == HashPassword(enteredPassword);
        }

        // Method to hash a password using SHA256 or any other algorithm of your choice
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes); // Return the hashed password as a base64 string
            }
        }
    }
}
