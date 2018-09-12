using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Persistity.Tests.Extensions
{
    public class AssertExtensions : Assert
    {
        public static void AreEqual(object a, object b)
        {
            var obj1Str = JsonConvert.SerializeObject(a);
            var obj2Str = JsonConvert.SerializeObject(b);
            Equal(obj1Str, obj2Str);
        }
        
        public static void AreEqual(JObject a, JObject b)
        {
            var obj1Str = a.ToString();
            var obj2Str = b.ToString();
            Equal(obj1Str, obj2Str);
        }
    }
}