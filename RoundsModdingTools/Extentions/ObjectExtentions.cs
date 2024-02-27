using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ModdingTools.Extentions {
    public static class ObjectExtentions {

        private static readonly Dictionary<Type, ConditionalWeakTable<object, Dictionary<string, object>>> Bindings
            = new Dictionary<Type, ConditionalWeakTable<object, Dictionary<string, object>>>();

        public static void StoreExtraData<ObjectType, DataType>(this ObjectType thing, string identifier, DataType data) {
            Type type = thing.GetType();
            if(!Bindings.ContainsKey(type)) Bindings.Add(type, new ConditionalWeakTable<object, Dictionary<string, object>>());
            Bindings[type].GetOrCreateValue(thing)[identifier] = data;
        }

        public static DataType RetrieveExtraData<ObjectType, DataType>(this ObjectType thing, string identifier) {
            Type type = thing.GetType();
            if(!Bindings.ContainsKey(type)) return default;
            Dictionary<string, object> dic = Bindings[type].GetOrCreateValue(type);
            if(!dic.ContainsKey(identifier)) return default;
            return (DataType)dic[identifier];
        }

    }
}
