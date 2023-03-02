namespace dotnetConf2023.Shared.Abstraction.Entities;

public abstract class BaseEntity
{
    public string? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? CreatedDt { get; set; }
    public DateTime? CreatedDtServer { get; set; }

    public string? LastUpdatedBy { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTime? LastUpdatedDt { get; set; }
    public DateTime? LastUpdatedDtServer { get; set; }

    public string? DeletedBy { get; set; }
    public string? DeletedByName { get; set; }
    public DateTime? DeletedByDt { get; set; }
    public DateTime? DeletedByDtServer { get; set; }
}