using DBragas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public static class UserSeeder
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(
            new Users
            {
                Id = Guid.NewGuid(),
                Name = "dBragas",
                Email = "administração@dbragas.com.br",
                Password = "$2a$11$lniElpvb7zV4yqVO3pMrJ.qRztzLCtJPkzvjnyeGicomsEkSDFdCW",
                Username = "DBragas",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );
    }
}
