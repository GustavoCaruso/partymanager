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
    public class TipoEventoItemMapping : IEntityTypeConfiguration<TipoEventoItem>
    {
        public void Configure(EntityTypeBuilder<TipoEventoItem> builder)
        {
            builder.HasKey(tei => tei.id);  // Chave primária

            builder.Property(tei => tei.quantidadePorPessoa)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");  // Quantidade por pessoa

           

            // Relacionamento com TipoEvento (Muitos para Um)
            builder.HasOne(tei => tei.tipoevento)
                   .WithMany(te => te.tipoeventoitem)
                   .HasForeignKey(tei => tei.idTipoEvento)
                   .OnDelete(DeleteBehavior.Cascade);  // Exclui TipoEventoItem se TipoEvento for excluído

            // Relacionamento com Item (Muitos para Um)
            builder.HasOne(tei => tei.item)
                   .WithMany(i => i.tipoeventoitem)
                   .HasForeignKey(tei => tei.idItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir Item se houver TipoEventoItem associado
        }
    }
}
