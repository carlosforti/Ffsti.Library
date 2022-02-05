namespace Ffsti.Library.Database.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsNullable { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var comparisionObj = (ColumnInfo)obj;

            return comparisionObj.Name == Name
                   && comparisionObj.IsNullable == IsNullable
                   && comparisionObj.IsPrimaryKey == IsPrimaryKey
                   && comparisionObj.IsAutoIncrement == IsAutoIncrement;
        }
    }
}
