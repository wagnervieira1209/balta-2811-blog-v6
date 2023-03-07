using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogV6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluentBlog.Mapping
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

            builder.Property(x => x.LastUpdateDate)
            .IsRequired()
            .HasColumnName("LastUpdateDate")
            .HasColumnType("SMALLDATETIME")
            .HasMaxLength(60)
            .HasDefaultValueSql("GETDATE()"); // Usa Valor default do SQL
            // .HasDefaultValue(DateTime.Now.ToUniversalTime()); // Usa Valor default do .NET

            builder.HasIndex(x => x.Slug, "IX_Post_Slug");

            // Relacionamento UM para MUITOS
            builder.HasOne(x => x.Author)
            .WithMany(x => x.Posts) // Relacionamento
            .HasConstraintName("FK_Post_Author") // CONSTRAINT
            .OnDelete(DeleteBehavior.Cascade); // COMPORTAMENTO DO ONDELETE

            builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Many to Many N-N
            // Aqui cria uma nova tabela no banco de datos
            // Tabela chamada PostTags onde faz o relacionamento
            builder.HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                post => post.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade),
                        tag => tag.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade)
            );
        }
    }
}