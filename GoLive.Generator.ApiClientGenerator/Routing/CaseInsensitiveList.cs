using System;
using System.Collections.Generic;
using System.Linq;

namespace GoLive.Generator.ApiClientGenerator.Routing;

public class CaseInsensitiveList : List<string>
{
    public new bool Contains(string item)
    {
        return this.Any(x => string.Equals(x, item, StringComparison.InvariantCultureIgnoreCase));
    }

    public new int IndexOf(string item)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (string.Equals(this[i], item, StringComparison.InvariantCultureIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    public new bool Remove(string item)
    {
        int index = IndexOf(item);
        if (index >= 0)
        {
            this.RemoveAt(index);
            return true;
        }
        return false;
    }
}