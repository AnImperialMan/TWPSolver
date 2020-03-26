using System.Collections.Generic;

namespace TWPPract.DataStructures
{
    public class Group: List<string>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}