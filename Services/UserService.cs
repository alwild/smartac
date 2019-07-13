using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using smartacfe.Data;
using smartacfe.Models;

namespace smartacfe.Services
{
    public class UserService
    {
        private DBContext _db;

        public UserService(DBContext context)
        {
            _db = context;
        }
        
        public async Task<User> Authenticate(string username, string password)
        {
            var passwordHash = CalculateMD5Hash(password);
            return await _db.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == passwordHash);
        }

        public async Task<User> CreateUser(string username, string password, string firstName, string lastName)
        {
            var passwordHash = CalculateMD5Hash(password);
            var created = await _db.Users.AddAsync(new User()
            {
                Username = username,
                Password = passwordHash,
                FirstName = firstName,
                LastName = lastName
            });

            return created.Entity;
        }
        
        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
 
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}