using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace GnollMods.FasterEntityProcessing
{
    public class DictionaryList : IList<Game.GameEntity>
    {
        private Dictionary<Game.GameEntity, Game.GameEntity> _entries;

        public int Count
        {
            get
            {
                lock (_entries)
                {
                    return _entries.Values.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public Game.GameEntity this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DictionaryList()
        {
            _entries = new Dictionary<Game.GameEntity, Game.GameEntity>();
        }

        public void Add(Game.GameEntity item)
        {
            _entries.Add(item, item);
        }

        public bool Contains(Game.GameEntity item)
        {
            return _entries.ContainsKey(item);
        }

        public bool Remove(Game.GameEntity item)
        {
            _entries.Remove(item);
            return true;
        }

        public int IndexOf(Game.GameEntity item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Game.GameEntity item)
        {
            _entries.Add(item, item);
        }

        public void RemoveAt(int index)
        {
            _entries.Remove(_entries.ElementAt(index).Key);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public void CopyTo(Game.GameEntity[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<Game.GameEntity> IEnumerable<Game.GameEntity>.GetEnumerator()
        {
            return _entries.Values.Cast<Game.GameEntity>().ToList().GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _entries.Values.GetEnumerator();
        }
    }

    class MainMod : IGnollMod
    {
        public string Name { get { return "FasterEntityProcessing"; } }
        public string Description { get { return "Replaces some of the internal structures for the game update to run faster"; } }

        public string BuiltWithLoaderVersion { get { return "G1.4"; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
        }

        private void HookManager_InGameHUDInit(InGameHUD inGameHUD, Manager manager)
        {
            List<Game.GameEntity> copy = new List<Game.GameEntity>(Game.GnomanEmpire.Instance.EntityManager.list_1);

            // Replace the main entity collection with out custom implmenetation
            Game.GnomanEmpire.Instance.EntityManager.list_1 = new DictionaryList();

            foreach (var entity in copy)
            {
                // Copy over all the entitites
                Game.GnomanEmpire.Instance.EntityManager.list_1.Add(entity);
            }

            System.Console.WriteLine(" -- Entity size {0}, list type {1}", Game.GnomanEmpire.Instance.EntityManager.list_1.Count, Game.GnomanEmpire.Instance.EntityManager.list_1.GetType());
            // Don't forget to cleanup after yourself
            copy.Clear();
        }
    }
}
