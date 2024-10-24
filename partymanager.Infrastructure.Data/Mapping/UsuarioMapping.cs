using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using partymanager.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Infrastructure.Data.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.id);  // Chave primária

            builder.Property(u => u.nome)
                   .IsRequired()
                   .HasMaxLength(100);  // Nome do usuário

            builder.Property(u => u.email)
                   .IsRequired()
                   .HasMaxLength(100);  // Email do usuário

            builder.Property(u => u.senha)
                   .IsRequired()
                   .HasMaxLength(255);  // Senha do usuário (armazenada como hash)

            // Relacionamento com Evento (Um para Muitos)
            builder.HasMany(u => u.evento)
                   .WithOne(e => e.usuario)
                   .HasForeignKey(e => e.idUsuario)
                   .OnDelete(DeleteBehavior.SetNull);  // Deixa o idUsuario nulo se o usuário for excluído
        }
    }
}
