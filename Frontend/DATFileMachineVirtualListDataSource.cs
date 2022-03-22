using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Frontend
{
    public class DATFileMachineVirtualListDataSource : FastObjectListDataSource
    {
        public DATFileMachineVirtualListDataSource(FastObjectListView listView) : base(listView)
        {
        }
    }

    /*
    public class DATFileMachineVirtualListDataSource : IVirtualListDataSource
    {
        //private List<DATFileMachine> fullObjectList = new List<DATFileMachine>();
        //private List<DATFileMachine> filteredObjectList  = new List<DATFileMachine>();
        private ArrayList fullObjectList = new ArrayList();
        private ArrayList filteredObjectList  = new ArrayList();
        readonly Dictionary<Object, int> objectsToIndexMap = new Dictionary<Object, int>();
        private IModelFilter modelFilter;
        private IListFilter listFilter;

        private FastObjectListView listView;
        // public DATFileMachineVirtualListDataSource(FastObjectListView lv, List<DATFileMachine> list)
        public DATFileMachineVirtualListDataSource(FastObjectListView lv)
        {
            // fullObjectList = list;
            // fullObjectList = new ArrayList(list);
            listView = lv;
        }

        public object GetNthObject(int n)
        {
            if (n >= 0 && n < filteredObjectList.Count)
                return filteredObjectList[n];

            return null;
        }

        public int GetObjectCount()
        {
            return filteredObjectList.Count;
        }

        public int GetObjectIndex(object model)
        {
            int index;

            if (model != null && this.objectsToIndexMap.TryGetValue(model, out index))
                return index;
            
            return -1;
        }

        public void PrepareCache(int first, int last)
        {
        }

        public int SearchText(string text, int first, int last, OLVColumn column)
        {
            if (first <= last) {
                for (int i = first; i <= last; i++) {
                    string data = column.GetStringValue(this.listView.GetNthItemInDisplayOrder(i).RowObject);
                    if (data.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                        return i;
                }
            } else {
                for (int i = first; i >= last; i--) {
                    string data = column.GetStringValue(this.listView.GetNthItemInDisplayOrder(i).RowObject);
                    if (data.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                        return i;
                }
            }

            return -1;
        }

        public void Sort(OLVColumn column, SortOrder sortOrder)
        {
            if (sortOrder != SortOrder.None)
            {
                ModelObjectComparer comparer = new ModelObjectComparer(column, sortOrder, this.listView.SecondarySortColumn, this.listView.SecondarySortOrder);
                this.fullObjectList.Sort(comparer);
                this.filteredObjectList.Sort(comparer);
            }

            this.RebuildIndexMap();
        }

        public void AddObjects(ICollection modelObjects)
        {
            foreach (object modelObject in modelObjects)
            {
                if (modelObject != null)
                    this.fullObjectList.Add(modelObject);
            }

            this.FilterObjects();
            this.RebuildIndexMap();
        }

        public void RemoveObjects(ICollection modelObjects)
        {
            List<int> indicesToRemove = new List<int>();
            foreach (object modelObject in modelObjects)
            {
                int i = this.GetObjectIndex(modelObject);
                if (i >= 0)
                    indicesToRemove.Add(i);

                // Remove the objects from the unfiltered list
                this.fullObjectList.Remove(modelObject);
            }

            // Sort the indices from highest to lowest so that we
            // remove latter ones before earlier ones. In this way, the
            // indices of the rows doesn't change after the deletes.
            indicesToRemove.Sort();
            indicesToRemove.Reverse();

            foreach (int i in indicesToRemove)
                this.listView.SelectedIndices.Remove(i);

            this.FilterObjects();
            this.RebuildIndexMap();
        }

        public void SetObjects(IEnumerable collection)
        {
            ArrayList newObjects = ObjectListView.EnumerableToArray(collection, true);

            this.fullObjectList = newObjects;
            this.FilterObjects();
            this.RebuildIndexMap();
        }

        public void UpdateObject(int index, object modelObject)
        {
            if (index < 0 || index >= this.filteredObjectList.Count)
                return;

            int i = this.fullObjectList.IndexOf(this.filteredObjectList[index]);
            if (i < 0)
                return;

            this.fullObjectList[i] = modelObject;
            this.filteredObjectList[index] = modelObject;
            this.objectsToIndexMap[modelObject] = index;
        }

        private void RebuildIndexMap()
        {
            this.objectsToIndexMap.Clear();
            for (int i = 0; i < this.filteredObjectList.Count; i++)
            {
                this.objectsToIndexMap[this.filteredObjectList[i]] = i;
            }
        }

        /// <summary>
        /// Build our filtered list from our full list.
        /// </summary>
        protected void FilterObjects()
        {
            if (!this.listView.UseFiltering || (this.modelFilter == null && this.listFilter == null))
            {
                this.filteredObjectList = new ArrayList(this.fullObjectList);
                // this.filteredObjectList = new List<DATFileMachine>(this.fullObjectList);
                return;
            }

            IEnumerable objects = (this.listFilter == null) ?
                this.fullObjectList : this.listFilter.Filter(this.fullObjectList);

            // Apply the object filter if there is one
            if (this.modelFilter == null)
            {
                this.filteredObjectList = ObjectListView.EnumerableToArray(objects, false);
            }
            else
            {
                this.filteredObjectList = new ArrayList();
                foreach (object model in objects)
                {
                    if (this.modelFilter.Filter(model))
                        this.filteredObjectList.Add(model);
                }
            }
        }
    }
    */
}