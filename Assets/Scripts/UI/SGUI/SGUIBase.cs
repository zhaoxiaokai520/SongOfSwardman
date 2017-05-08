using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class SGUIBase : MonoBehaviour
	{
		[HideInInspector]
		public int m_indexInlist;

		public GameObject[] m_widgets = new GameObject[0];

		protected bool m_isInitialized;

		public virtual void Initialize()
		{
			if (this.m_isInitialized)
			{
				return;
			}

			this.m_isInitialized = true;
		}

		public virtual void Close()
		{
            m_isInitialized = false;
		}

		public virtual void Hide()
        {

		}

		public virtual void Appear()
        {

		}

		public virtual void SetSortingOrder(int sortingOrder)
		{
		}

		public void SetBelongedList()
		{

		}

		public GameObject GetWidget(int index)
		{
			if (index < 0 || index >= this.m_widgets.Length)
			{
				return null;
			}
			return this.m_widgets[index];
		}

		protected T GetComponentInChildren<T>(GameObject go) where T : Component
		{
			T t = go.GetComponent<T>();
			if (t != null)
			{
				return t;
			}
			for (int i = 0; i < go.transform.childCount; i++)
			{
				t = this.GetComponentInChildren<T>(go.transform.GetChild(i).gameObject);
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		protected GameObject Instantiate(GameObject srcGameObject)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(srcGameObject) as GameObject;
			gameObject.transform.SetParent(srcGameObject.transform.parent);
			RectTransform rectTransform = srcGameObject.transform as RectTransform;
			RectTransform rectTransform2 = gameObject.transform as RectTransform;
			if (rectTransform != null && rectTransform2 != null)
			{
				rectTransform2.pivot=(rectTransform.pivot);
				rectTransform2.anchorMin=(rectTransform.anchorMin);
				rectTransform2.anchorMax=(rectTransform.anchorMax);
				rectTransform2.offsetMin=(rectTransform.offsetMin);
				rectTransform2.offsetMax=(rectTransform.offsetMax);
				rectTransform2.localPosition=(rectTransform.localPosition);
				rectTransform2.localRotation=(rectTransform.localRotation);
				rectTransform2.localScale=(rectTransform.localScale);
			}
			return gameObject;
		}

		protected void DispatchUIEvent()
		{

		}

		protected void InitializeComponent(GameObject root)
		{
			for (int j = 0; j < root.transform.childCount; j++)
			{
				this.InitializeComponent(root.transform.GetChild(j).gameObject);
			}
		}
	}
}