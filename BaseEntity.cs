namespace Veichi.SRM.Model.Base
{
    public class BaseEntityGuid
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string id { get; set; } = Guid.NewGuid().ToString();
    }
}