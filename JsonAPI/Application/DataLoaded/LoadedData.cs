using Domain.User;
using BCrypt.Net;
public class LoadedData
{
    public static readonly List<User> UsersList = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "sajad",
                Email = "sajad@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password1"),
                Role = "user"
            },
            new User
            {
                Id = 2,
                Name = "Fadi Khail",
                Email = "fadi.khail@student.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password2"),
                Role = "user"
            },
            new User
            {
                Id = 3,
                Name = "Ola Jaber",
                Email = "ola.jaber@student.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password3"),
                Role = "user"
            },
            new User
            {
                Id = 4,
                Name = "Alia Maher",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password1"),
                Role = "Admin"
            }
        };
}