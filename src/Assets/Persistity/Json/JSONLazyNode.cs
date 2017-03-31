namespace Persistity.Json
{
    public class JSONLazyNode : JSONNode
    {
        private JSONNode m_Node = null;
        private string m_Key = null;

        public override JSONNodeType Tag { get { return JSONNodeType.None; } }

        public JSONLazyNode()
        {
        }

        public void SetKey(string key)
        { m_Key = key; }

        public void SetValue(JSONNode node)
        { m_Node = node; }

        private void Set(JSONNode aVal)
        {
            if (m_Key == null)
            {
                m_Node.Add(aVal);
            }
            else
            {
                m_Node.Add(m_Key, aVal);
            }
            m_Node = null; // Be GC friendly.
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                return new JSONLazyCreator(this);
            }
            set
            {
                var tmp = new JSONArray();
                tmp.Add(value);
                Set(tmp);
            }
        }

        public override JSONNode this[string aKey]
        {
            get
            {
                return new JSONLazyCreator(this, aKey);
            }
            set
            {
                var tmp = new JSONObject();
                tmp.Add(aKey, value);
                Set(tmp);
            }
        }

        public override void Add(JSONNode aItem)
        {
            if (m_Node == null)
            { m_Node = new JSONArray(); }

            var tmp = new JSONArray();
            tmp.Add(aItem);
            Set(tmp);
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            if(m_Node == null)
            { m_Node = new JSONObject(); }

            var tmp = new JSONObject();
            tmp.Add(aKey, aItem);
            Set(tmp);
        }

        public static bool operator ==(JSONLazyNode a, object b)
        {
            if (b == null)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONLazyNode a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "";
        }

        internal override string ToString(string aIndent, string aPrefix)
        {
            return "";
        }

        public override int AsInt
        {
            get
            {
                JSONNumber tmp = new JSONNumber(0);
                Set(tmp);
                return 0;
            }
            set
            {
                JSONNumber tmp = new JSONNumber(value);
                Set(tmp);
            }
        }

        public override float AsFloat
        {
            get
            {
                JSONNumber tmp = new JSONNumber(0.0f);
                Set(tmp);
                return 0.0f;
            }
            set
            {
                JSONNumber tmp = new JSONNumber(value);
                Set(tmp);
            }
        }

        public override double AsDouble
        {
            get
            {
                JSONNumber tmp = new JSONNumber(0.0);
                Set(tmp);
                return 0.0;
            }
            set
            {
                JSONNumber tmp = new JSONNumber(value);
                Set(tmp);
            }
        }

        public override bool AsBool
        {
            get
            {
                JSONBool tmp = new JSONBool(false);
                Set(tmp);
                return false;
            }
            set
            {
                JSONBool tmp = new JSONBool(value);
                Set(tmp);
            }
        }

        public override JSONArray AsArray
        {
            get
            {
                JSONArray tmp = new JSONArray();
                Set(tmp);
                return tmp;
            }
        }

        public override JSONObject AsObject
        {
            get
            {
                JSONObject tmp = new JSONObject();
                Set(tmp);
                return tmp;
            }
        }
    }
}