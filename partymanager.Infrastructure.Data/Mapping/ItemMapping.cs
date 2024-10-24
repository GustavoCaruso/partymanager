using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using partymanager.Domain.Entidades;

namespace partymanager.Infrastructure.Data.Mapping
{
    public class ItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.id);  // Chave primária

            builder.Property(i => i.nome)
                   .IsRequired()
                   .HasMaxLength(100);  // Nome do item

            

            // Relacionamento com TipoItem (Muitos para Um)
            builder.HasOne(i => i.tipoitem)
                   .WithMany(ti => ti.item)
                   .HasForeignKey(i => i.idTipoItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir TipoItem se houver Itens associados

            // Relacionamento com TipoEventoItem (Um para Muitos)
            builder.HasMany(i => i.tipoeventoitem)
                   .WithOne(tei => tei.item)
                   .HasForeignKey(tei => tei.idItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir Item se houver TipoEventoItem associado

            // Relacionamento com ItemCalculado (Um para Muitos)
            builder.HasMany(i => i.itemcalculado)
                   .WithOne(ic => ic.item)
                   .HasForeignKey(ic => ic.idItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir Item se houver ItemCalculado associado
        }
    }
}
