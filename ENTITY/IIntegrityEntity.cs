namespace ENTITY
{
    public interface IIntegrityEntity
    {
        string DVH { get; set; }
        string GetConcatDataForDVH();
    }
}
