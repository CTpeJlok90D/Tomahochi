using System.Collections.Generic;
using UnityEngine;

namespace AICore
{
    public class Brain : MonoBehaviour
    {
        [SerializeField] private UnityDictionarity<Factor, Memory> _factorMemotyDictionary = new();
        [SerializeField] private List<Memory> _activeMemorys = new();

		public void AddFactor(FactorInfo info)
        {
			if (_factorMemotyDictionary.Keys.Contains(info.Factor) == false)
			{
				return;
			}

            Memory memory = _factorMemotyDictionary[info.Factor];
            AddMemory(memory, info);
		}

		private void AddMemory(Memory memory, FactorInfo info)
		{
			if (_activeMemorys.Contains(memory) == false)
			{
				_activeMemorys.Add(memory);
				memory.OnMemoryAdd(info);
			}
			else
			{
				memory.UpdateDituration();
				memory.UpdateInfo(info);
			}
		}

		private void Update()
		{
            foreach (Memory memory in _activeMemorys.ToArray()) 
            {
                memory.OnUpdate();
                if (memory.CurrentDituration <= 0)
                {
                    RemoveMemory(memory);
				}
            }
		}

        private void RemoveMemory(Memory memory)
        {
			_activeMemorys.Remove(memory);
			memory.OnMemoryRemove();
		}

		public void ClearMemorys()
		{
			foreach (Memory memory in _activeMemorys.ToArray())
			{
				RemoveMemory(memory);
			}
		}
	}
}