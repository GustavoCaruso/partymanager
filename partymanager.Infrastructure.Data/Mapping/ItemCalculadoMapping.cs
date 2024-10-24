using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using partymanager.Domain.Entidades;

namespace partymanager.Infrastructure.Data.Mapping
{
    public class ItemCalculadoMapping : IEntityTypeConfiguration<ItemCalculado>
    {
        public void Configure(EntityTypeBuilder<ItemCalculado> builder)
        {
            builder.HasKey(ic => ic.id);  // Chave primária

            builder.Property(ic => ic.quantidade)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");  // Quantidade calculada

            

          

            // Relacionamento com Evento (Muitos para Um)
            builder.HasOne(ic => ic.evento)
                   .WithMany(e => e.itemcalculado)
                   .HasForeignKey(ic => ic.idEvento)
                   .OnDelete(DeleteBehavior.Cascade);  // Exclui ItemCalculado se Evento for excluído

            // Relacionamento com Item (Muitos para Um)
            builder.HasOne(ic => ic.item)
                   .WithMany(i => i.itemcalculado)
                   .HasForeignKey(ic => ic.idItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir Item se houver ItemCalculado associado
        }
    }
}
