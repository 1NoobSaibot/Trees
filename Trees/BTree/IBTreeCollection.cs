using System.Numerics;

namespace Trees.BTree
{
	public interface IBTreeCollection<TKey, TValue> where TKey : INumber<TKey>
	{
		bool ContainsKey(TKey key);
		bool ContainsKey(TKey key, out TValue? value);
		void Add(TKey key, TValue value);
		List<TValue> GetValuesInRange(TKey key1, TKey key2);
		TKey GetMinKey();
		TKey GetMaxKey();
		void ForEach(Action<TKey, TValue> actionForAll);
		void Rebalance();
	}
}
