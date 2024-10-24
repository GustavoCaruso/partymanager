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
    public class EventoMapping : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.HasKey(e => e.id);  

            builder.Property(e => e.nome)
                   .IsRequired()
                   .HasMaxLength(100);  

            builder.Property(e => e.data)
                   .IsRequired();  

            builder.Property(e => e.localidade)
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(e => e.numeroAdultos)
                   .IsRequired(); 

            builder.Property(e => e.numeroCriancas)
                   .IsRequired(); 



            // Relacionamento com TipoEvento (Muitos para Um)
            builder.HasOne(e => e.tipoevento)
                   .WithMany(te => te.evento)
                   .HasForeignKey(e => e.idTipoEvento)
                   .OnDelete(DeleteBehavior.Restrict);  // Não exclui TipoEvento se houver eventos associados

            // Relacionamento com ItemCalculado (Um para Muitos)
            builder.HasMany(e => e.itemcalculado)
                   .WithOne(ic => ic.evento)
                   .HasForeignKey(ic => ic.idEvento)
                   .OnDelete(DeleteBehavior.Cascade);  // Exclui ItemCalculado se o Evento for excluído

            // Relacionamento com Usuario (Opcional)
            builder.HasOne(e => e.usuario)
                   .WithMany(u => u.evento)
                   .HasForeignKey(e => e.idUsuario)
                   .OnDelete(DeleteBehavior.SetNull);  // Deixa o idUsuario nulo se o usuário for excluído
        }
    }
}
