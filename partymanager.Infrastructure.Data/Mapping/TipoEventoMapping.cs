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
    public class TipoEventoMapping : IEntityTypeConfiguration<TipoEvento>
    {
        public void Configure(EntityTypeBuilder<TipoEvento> builder)
        {
            builder.HasKey(te => te.id);  // Chave primária

            builder.Property(te => te.nome)
                   .IsRequired()
                   .HasMaxLength(100);  // Nome do TipoEvento

            // Relacionamento com TipoEventoItem (Um para Muitos)
            builder.HasMany(te => te.tipoeventoitem)
                   .WithOne(tei => tei.tipoevento)
                   .HasForeignKey(tei => tei.idTipoEvento)
                   .OnDelete(DeleteBehavior.Cascade);  // Exclui TipoEventoItem se TipoEvento for excluído

            // Relacionamento com Evento (Um para Muitos)
            builder.HasMany(te => te.evento)
                   .WithOne(e => e.tipoevento)
                   .HasForeignKey(e => e.idTipoEvento)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir TipoEvento se houver Eventos associados
        }
    }
}
