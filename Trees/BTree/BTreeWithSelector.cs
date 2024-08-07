using System.Numerics;

namespace Trees.BTree
{
	internal class BTreeWithSelector<TKey, TValue>
		: IBTreeCollection<TKey, TValue>
		where TKey : INumber<TKey>
	{
		private BTreeNode<TKey, TValue>? _root;
		private readonly List<TValue> _valuesSelector = new();
		private const int REBALANCE_EVERY_ADDS = 5000;
		private int _adds = 0;

		public bool ContainsKey(TKey key)
		{
			return _root?.ContainsKey(key) ?? false;
		}


		public bool ContainsKey(TKey key, out TValue? value)
		{
			value = default;
			return _root?.ContainsKey(key, out value) ?? false;
		}


		public void Add(TKey key, TValue value)
		{
			if (_root is null)
			{
				_root = new(key, value);
			}
			else
			{
				_root.Add(key, value);
			}

			_adds++;
			if (_adds >= REBALANCE_EVERY_ADDS)
			{
				Rebalance();
				_adds = 0;
			}
		}


		public void ForEach(Action<TKey, TValue> actionForAll)
		{
			_root?.ForEach(actionForAll);
		}


		public List<TValue> GetValuesInRange(TKey key1, TKey key2)
		{
			_valuesSelector.Clear();
			TKey minKey = key1 < key2 ? key1 : key2;
			TKey maxKey = key1 > key2 ? key1 : key2;

			_root?.GetValuesInRange(
				minKey: minKey,
				maxKey: maxKey,
				_valuesSelector
			);

			return _valuesSelector;
		}


		public TKey GetMinKey()
		{
			if (_root is null)
			{
				throw new Exception("This collection is empty");
			}

			return _root.GetMinKey();
		}


		public TKey GetMaxKey()
		{
			if (_root is null)
			{
				throw new Exception("This collection is empty");
			}

			return _root.GetMaxKey();
		}


		public void Rebalance()
		{
			if (_root is null)
			{
				return;
			}

			int count = _root.Count();
			List<BTreeNode<TKey, TValue>> pairs = new(count);
			_root.CopyAllPairsToTheList(pairs);

			int firstIndex = 0;
			int lastIndex = pairs.Count - 1;
			int middleIndex = lastIndex >> 1;

			_root = pairs[middleIndex];
			_root.SetLeftChildren(pairs, firstIndex, middleIndex - 1);
			_root.SetRightChildren(pairs, middleIndex + 1, lastIndex);
		}
	}
}
