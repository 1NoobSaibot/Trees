using System.Numerics;

namespace Trees.BTree
{
	internal class BTreeNode<TKey, TValue> where TKey : INumber<TKey>
	{
		private readonly TKey _key;
		private readonly TValue _value;
		private BTreeNode<TKey, TValue>? _left;
		private BTreeNode<TKey, TValue>? _right;


		public BTreeNode(TKey key, TValue value)
		{
			_key = key;
			_value = value;
		}


		public BTreeNode(List<(TKey, TValue)> pairs)
		{
			int centerIndex = pairs.Count / 2;
			_key = pairs[centerIndex].Item1;
			_value = pairs[centerIndex].Item2;

			if (centerIndex > 0)
			{
				_left = new(pairs, 0, centerIndex - 1);
			}

			int lastIndex = pairs.Count - 1;
			if (centerIndex < lastIndex)
			{
				_right = new(pairs, centerIndex + 1, lastIndex);
			}
		}


		private BTreeNode(List<(TKey, TValue)> pairs, int firstIndex, int lastIndex)
		{
			int localCount = lastIndex - firstIndex + 1;
			int localCenterIndex = localCount / 2 + firstIndex;
			_key = pairs[localCenterIndex].Item1;
			_value = pairs[localCenterIndex].Item2;

			if (localCenterIndex > firstIndex)
			{
				_left = new(pairs, firstIndex, localCenterIndex - 1);
			}
			if (localCenterIndex < lastIndex)
			{
				_right = new(pairs, localCenterIndex + 1, lastIndex);
			}
		}


		public void Add(TKey key, TValue value)
		{
			if (key == _key)
			{
				throw new Exception($"Key {key} is already exist. Cannot Add");
			}

			if (key < _key)
			{
				if (_left is null)
				{
					_left = new(key, value);
				}
				else
				{
					_left.Add(key, value);
				}
			}
			else
			{
				if (_right is null)
				{
					_right = new(key, value);
				}
				else
				{
					_right.Add(key, value);
				}
			}
		}


		public bool ContainsKey(TKey key)
		{
			if (_key == key)
			{
				return true;
			}
			if (key < _key)
			{
				return _left?.ContainsKey(key) ?? false;
			}
			return _right?.ContainsKey(key) ?? false;
		}


		public bool ContainsKey(TKey key, out TValue? value)
		{
			if (_key == key)
			{
				value = _value;
				return true;
			}
			value = default;
			if (key < _key)
			{
				return _left?.ContainsKey(key, out value) ?? false;
			}
			return _right?.ContainsKey(key, out value) ?? false;
		}


		public void GetValuesInRange(TKey minKey, TKey maxKey, List<TValue> valueAccumulator)
		{
			if (minKey < _key)
			{
				_left?.GetValuesInRange(minKey, maxKey, valueAccumulator);
			}
			if (minKey <= _key && _key < maxKey)
			{
				valueAccumulator.Add(_value);
			}
			if (maxKey > _key)
			{
				_right?.GetValuesInRange(minKey, maxKey, valueAccumulator);
			}
		}


		public TKey GetMinKey()
		{
			if (_left is not null)
			{
				return _left.GetMinKey();
			}
			return _key;
		}


		public TKey GetMaxKey()
		{
			if (_right is not null)
			{
				return _right.GetMaxKey();
			}
			return _key;
		}


		public void ForEach(Action<TKey, TValue> actionForAll)
		{
			_left?.ForEach(actionForAll);
			actionForAll(_key, _value);
			_right?.ForEach(actionForAll);
		}

		internal int Count()
		{
			int res = 1;
			if (_left != null)
			{
				res += _left.Count();
			}
			if (_right != null)
			{
				res += _right.Count();
			}
			return res;
		}

		internal void CopyAllPairsToTheList(List<(TKey key, TValue value)> pairs)
		{
			_left?.CopyAllPairsToTheList(pairs);
			pairs.Add((_key, _value));
			_right?.CopyAllPairsToTheList(pairs);
		}
	}
}
