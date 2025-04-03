using System;
using System.Collections.Generic;
using Runtime.Enums;

namespace Runtime.Data.ValueObject.Modifier
{
    [Serializable]
    public class PlayerModifiers
    {
        public List<ItemModifiers> itemModifiers;
    }
}