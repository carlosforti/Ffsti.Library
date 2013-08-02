using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti.Library.Database
{
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// Seta o valor de uma propriedade de um objeto usando o nome da propriedade
        /// </summary>
        /// <param name="obj">Objeto a ser manipulado</param>
        /// <param name="propName">Nome da propriedade</param>
        /// <param name="value">Valor a ser atribuido</param>
        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value, null);
        }

        /// <summary>
        /// Pega o valor de uma propriedade usando o nome da mesma
        /// </summary>
        /// <param name="obj">Objeto a ser manipulado</param>
        /// <param name="name">Nome da propriedade</param>
        /// <returns>Valor da propriedade</returns>
        public static object GetPropertyValue(this object obj, string name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                System.Reflection.PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        /// <summary>
        /// Pega o valor de uma propriedade de um objeto usando o nome da propriedade, e retorna tipado
        /// </summary>
        /// <typeparam name="T">Tipo do retorno</typeparam>
        /// <param name="obj">Objeto a ser manipulado</param>
        /// <param name="name">Nome da propriedade</param>
        /// <returns>Valor da propriedade tipado</returns>
        public static T GetPropertyValue<T>(this object obj, string name)
        {
            Object retval = GetPropertyValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }
    }
}
