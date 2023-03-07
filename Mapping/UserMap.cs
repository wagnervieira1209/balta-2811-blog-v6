using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluentBlog.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.Bio)
            .IsRequired(false);

            builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

            builder.Property(x => x.Image)
            .IsRequired(false);

            builder.Property(x => x.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255); 
            
            builder.Property(x => x.GitHub);

            // Índices e Unique
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();

            // Relacionamento Many to Many N-N
            // Aqui cria uma nova tabela no banco de datos
            // Tabela chamada PostTags onde faz o relacionamento
            builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                    role => role
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRole_RoleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    user => user
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRole_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
            );
        }
    }
}