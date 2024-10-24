using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using partymanager.Domain.Entidades;

namespace partymanager.Infrastructure.Data.Mapping
{
    public class TipoItemMapping : IEntityTypeConfiguration<TipoItem>
    {
        public void Configure(EntityTypeBuilder<TipoItem> builder)
        {
            builder.HasKey(ti => ti.id);  // Chave primária

            builder.Property(ti => ti.nome)
                   .IsRequired()
                   .HasMaxLength(100);  // Nome do TipoItem

            // Relacionamento com Item (Um para Muitos)
            builder.HasMany(ti => ti.item)
                   .WithOne(i => i.tipoitem)
                   .HasForeignKey(i => i.idTipoItem)
                   .OnDelete(DeleteBehavior.Restrict);  // Não permite excluir TipoItem se houver Itens associados
        }
    }
}
