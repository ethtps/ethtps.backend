using System.Reflection;

namespace ETHTPS.Tests.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether this class has a public method that throws a <see cref="NotImplementedException"/>. Please note that this code is a simplification and assumes that the NotImplementedException is thrown directly in the method's body. If the exception is thrown in a method called by the inspected method, or in a different branch of execution (e.g. in an async method), this approach would not detect it. Thanks ChatGPT <3
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if the method throws <see cref="NotImplementedException"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNotImplementedExceptionMethod<T>()
        {
            var type = typeof(T);

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
                {
                    var body = method.GetMethodBody();
                    if (body != null)
                    {
                        var il = body.GetILAsByteArray();
                        if (il != null)
                            foreach (var instruction in il)
                            {
                                // 0x73 is the IL opcode for 'newobj', which is typically used to create an instance of an exception class.
                                if (instruction == 0x73)
                                {
                                    var ilb = body.GetILAsByteArray();
                                    if (ilb != null)
                                    {
                                        // Get constructor info
                                        ConstructorInfo? ctorInfo = method.Module.ResolveMethod(BitConverter.ToInt32(ilb, 1)) as ConstructorInfo;
                                        if (ctorInfo != null && ctorInfo.DeclaringType == typeof(NotImplementedException))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                    }
                }
            }

            return false;
        }

    }
}
