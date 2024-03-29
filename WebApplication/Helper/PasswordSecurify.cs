using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication.Helper
{
    public class PasswordSecurify
    {
        public string HashPassword(string password, string salt)
        {
            // 将密码和盐值连接起来
            string combinedPassword = password + salt;

            // 创建一个SHA256哈希算法的实例
            using (SHA256 sha256 = SHA256.Create())
            {
                // 将密码和盐值组合后进行哈希处理
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));

                // 将哈希值转换为字符串表示形式
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateSalt()
        {
            // 生成一个随机的盐值
            byte[] saltBytes = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public bool VerifyPassword(string password, string storedHashedPassword, string storedSalt)
        {
            // 将用户输入的密码与盐值组合后进行哈希处理
            string combinedPassword = password + storedSalt;
            string hashedPassword;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                hashedPassword = builder.ToString();
            }

            // 比较计算得到的哈希值与数据库中存储的哈希值
            return hashedPassword == storedHashedPassword;
        }
    }
}