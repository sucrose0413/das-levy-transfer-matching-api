﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;

namespace SFA.DAS.LevyTransferMatching.Services.Audit
{
    public class StateService : IStateService
    {
        public Dictionary<string, object> GetState(object item)
        {
            var result = new Dictionary<string, object>();
            var targetType = item.GetType();

            foreach (var property in targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var propertyType = property.PropertyType;

                var isClass = propertyType.IsClass;
                var isString = propertyType == typeof(string);
                var isEnumerable = propertyType.GetInterface(nameof(IEnumerable)) != null;
                var isAbstract = propertyType.IsAbstract;

                if (isString || (!isClass && !isEnumerable && !isAbstract))
                {
                    result.Add(property.Name, property.GetValue(item));
                }
            }

            return result;
        }
    }
}
