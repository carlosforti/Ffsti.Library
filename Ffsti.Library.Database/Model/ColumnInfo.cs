namespace Ffsti.Library.Database.Model
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsAutoIncrement { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var comparisionObj = (ColumnInfo)obj;

            return comparisionObj.Name == Name &&
                comparisionObj.IsNullable == IsNullable &&
                comparisionObj.IsPrimaryKey == IsPrimaryKey &&
                comparisionObj.IsAutoIncrement == IsAutoIncrement;
        }
    }
}
