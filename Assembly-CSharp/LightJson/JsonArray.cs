using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LightJson.Serialization;

namespace LightJson
{
	// Token: 0x02000019 RID: 25
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonArray.JsonArrayDebugView))]
	public sealed class JsonArray : IEnumerable<JsonValue>, IEnumerable
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000A73A File Offset: 0x0000893A
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000023 RID: 35
		public JsonValue this[int index]
		{
			get
			{
				if (index >= 0 && index < this.items.Count)
				{
					return this.items[index];
				}
				return JsonValue.Null;
			}
			set
			{
				this.items[index] = value;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A77C File Offset: 0x0000897C
		public JsonArray()
		{
			this.items = new List<JsonValue>();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A790 File Offset: 0x00008990
		public JsonArray(params JsonValue[] values) : this()
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			foreach (JsonValue item in values)
			{
				this.items.Add(item);
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A7D5 File Offset: 0x000089D5
		public JsonArray Add(JsonValue value)
		{
			this.items.Add(value);
			return this;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A7E4 File Offset: 0x000089E4
		public JsonArray AddIfNotNull(JsonValue value)
		{
			if (!value.IsNull)
			{
				this.Add(value);
			}
			return this;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public JsonArray Insert(int index, JsonValue value)
		{
			this.items.Insert(index, value);
			return this;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A808 File Offset: 0x00008A08
		public JsonArray InsertIfNotNull(int index, JsonValue value)
		{
			if (!value.IsNull)
			{
				this.Insert(index, value);
			}
			return this;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A81D File Offset: 0x00008A1D
		public JsonArray Remove(int index)
		{
			this.items.RemoveAt(index);
			return this;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A82C File Offset: 0x00008A2C
		public JsonArray Clear()
		{
			this.items.Clear();
			return this;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A83A File Offset: 0x00008A3A
		public bool Contains(JsonValue item)
		{
			return this.items.Contains(item);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A848 File Offset: 0x00008A48
		public int IndexOf(JsonValue item)
		{
			return this.items.IndexOf(item);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A856 File Offset: 0x00008A56
		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A863 File Offset: 0x00008A63
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A86B File Offset: 0x00008A6B
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A874 File Offset: 0x00008A74
		public string ToString(bool pretty)
		{
			string result;
			using (JsonWriter jsonWriter = new JsonWriter(pretty))
			{
				result = jsonWriter.Serialize(this);
			}
			return result;
		}

		// Token: 0x040000D3 RID: 211
		private IList<JsonValue> items;

		// Token: 0x02000401 RID: 1025
		private class JsonArrayDebugView
		{
			// Token: 0x170005DE RID: 1502
			// (get) Token: 0x0600266C RID: 9836 RVA: 0x000D4C58 File Offset: 0x000D2E58
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonValue[] Items
			{
				get
				{
					JsonValue[] array = new JsonValue[this.jsonArray.Count];
					for (int i = 0; i < this.jsonArray.Count; i++)
					{
						array[i] = this.jsonArray[i];
					}
					return array;
				}
			}

			// Token: 0x0600266D RID: 9837 RVA: 0x000D4CA0 File Offset: 0x000D2EA0
			public JsonArrayDebugView(JsonArray jsonArray)
			{
				this.jsonArray = jsonArray;
			}

			// Token: 0x04001775 RID: 6005
			private JsonArray jsonArray;
		}
	}
}
