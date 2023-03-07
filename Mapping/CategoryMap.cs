using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluentBlog.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Tabela
            // [Table("Category")] -> DataAnnotation
            builder.ToTable("Category");

            // Chave Primária
            // [Key] -> DataAnnotation
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); // PK Identity (1,1)

            // Propriedades
            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            // Propriedades
            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            // Índices e Unique
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();
        }
    }
}