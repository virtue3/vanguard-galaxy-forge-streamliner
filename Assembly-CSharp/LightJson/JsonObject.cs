using System;
using System.Collections;
using System.Collections.Generic;
using LightJson.Serialization;

namespace LightJson
{
	// Token: 0x0200001A RID: 26
	public sealed class JsonObject : IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable, IEnumerable<JsonValue>
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		public int Count
		{
			get
			{
				return this.properties.Count;
			}
		}

		// Token: 0x17000025 RID: 37
		public JsonValue this[string key]
		{
			get
			{
				JsonValue result;
				if (this.properties.TryGetValue(key, out result))
				{
					return result;
				}
				return JsonValue.Null;
			}
			set
			{
				this.properties[key] = value;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A8F7 File Offset: 0x00008AF7
		public JsonObject()
		{
			this.properties = new Dictionary<string, JsonValue>();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A90A File Offset: 0x00008B0A
		public JsonObject Add(string key)
		{
			return this.Add(key, JsonValue.Null);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A918 File Offset: 0x00008B18
		public JsonObject Add(string key, JsonValue value)
		{
			this.properties.Add(key, value);
			return this;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A928 File Offset: 0x00008B28
		public JsonObject AddIfNotNull(string key, JsonValue value)
		{
			if (!value.IsNull)
			{
				this.Add(key, value);
			}
			return this;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A93D File Offset: 0x00008B3D
		public bool Remove(string key)
		{
			return this.properties.Remove(key);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000A94B File Offset: 0x00008B4B
		public JsonObject Clear()
		{
			this.properties.Clear();
			return this;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A95C File Offset: 0x00008B5C
		public JsonObject Rename(string oldKey, string newKey)
		{
			JsonValue value;
			if (this.properties.TryGetValue(oldKey, out value))
			{
				this.Remove(oldKey);
				this[newKey] = value;
			}
			return this;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A98A File Offset: 0x00008B8A
		public bool ContainsKey(string key)
		{
			return this.properties.ContainsKey(key);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A998 File Offset: 0x00008B98
		public bool Contains(JsonValue value)
		{
			return this.properties.Values.Contains(value);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A9AB File Offset: 0x00008BAB
		public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
		{
			return this.properties.GetEnumerator();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
		{
			return this.properties.Values.GetEnumerator();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A9CA File Offset: 0x00008BCA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A9D2 File Offset: 0x00008BD2
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000A9DC File Offset: 0x00008BDC
		public string ToString(bool pretty)
		{
			string result;
			using (JsonWriter jsonWriter = new JsonWriter(pretty))
			{
				result = jsonWriter.Serialize(this);
			}
			return result;
		}

		// Token: 0x040000D4 RID: 212
		private IDictionary<string, JsonValue> properties;
	}
}
