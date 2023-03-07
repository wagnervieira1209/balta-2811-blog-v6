using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogV6.Data
{
    public class BlogDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<PostWithTagsCount> PostWithTagsCount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Exemplo FluentMap
            //options.UseSqlServer(@"server=localhost,1433;Database=FluentBlog;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True");
            //options.LogTo(Console.WriteLine);

            // Exemplo Migrations
            options.UseSqlServer(@"server=localhost,1433;Database=BlogV6;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True");
            options.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());

            modelBuilder.Entity<PostWithTagsCount>(x =>
            {
                x.ToSqlQuery(@"
                    SELECT
                        [Title] AS [Name],
                        SELECT COUNT() FROM Tags WHERE [PostId] = [Id] AS Count
                    FROM 
                        [Posts]");
            });*/
        }

    }
}