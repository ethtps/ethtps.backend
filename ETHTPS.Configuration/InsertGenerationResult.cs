using System.ComponentModel.DataAnnotations.Schema;

namespace ETHTPS.Configuration;

[NotMapped]
public class InsertGenerationResult
{
    [Column("id")]
    public required int ID { get; set; }

    [Column("tableName")]
    public required string Table { get; set; }
    [Column("script")]
    public required string Script { get; set; }
}