using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{

	public GameObject objectToPool;
	public int amountToPool;
	public bool shouldExpand = true;

	public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
	{
		objectToPool = obj;
		amountToPool = Mathf.Max(amt, 2);
		shouldExpand = exp;
	}
}

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler Instance;
	public List<ObjectPoolItem> itemsToPool;


	public List<List<GameObject>> pooledObjectsList;
	public List<GameObject> pooledObjects;
	private List<int> positions;

	void Awake()
	{

		Instance = this;

		pooledObjectsList = new List<List<GameObject>>();
		pooledObjects = new List<GameObject>();
		positions = new List<int>();

		for (int i = 0; i < itemsToPool.Count; i++)
		{
			ObjectPoolItemToPooledObject(i);
		}

	}

	//public GameObject GetPooledObject(PooledObjectType type)
 //   {
	//	return GetPooledObject((int)type);
	//}
    public GameObject GetPooledObject(GameObject obj, Transform tf, float disableTime)
    {
        string str = string.Format("{0}(Clone)", obj.name);
        int index = -1;
		int counter = 0;
        foreach(List<GameObject> list in pooledObjectsList)
        {
			//Debug.Log(string.Format("list.Count: {0}, name: {1}", list.Count, list[0].name));

			if (list.Count > 0 && list[0].name.Contains(str)) {
				index = counter;
				//Debug.Log("here: index: " + index);
				break;
            }
			counter++;
        }
        if(index < 0)
        {
			return GetPooledObject(AddObject(obj, 1, true), tf, disableTime);
		}
        else
        {
			return GetPooledObject(index, tf, disableTime);
        }
    }
	public GameObject GetPooledObject(int index, Transform tf, float disableTime)
	{
		GameObject theObject = null;
		int curSize = pooledObjectsList[index].Count;
		//for (int i = positions[index] + 1; i < positions[index] + pooledObjectsList[index].Count; i++)
        for(int i = positions[index]; i < curSize; i++)
		{
			//if (!pooledObjectsList[index][i % curSize].activeSelf)
			if (!pooledObjectsList[index][i].activeSelf)
			{
				//positions[index] = i % curSize;
				positions[index] = i;
				theObject = pooledObjectsList[index][i];
				i++;
				if (i >= curSize)
                {
					positions[index] = 0;
                }
				break;
			}
		}

		//if (theObject == null && itemsToPool[index].shouldExpand)
		if (theObject == null && itemsToPool[index].shouldExpand)
		{
			//Debug.Log("should expand");
			GameObject obj = (GameObject)Instantiate(itemsToPool[index].objectToPool);
			obj.SetActive(false);
			obj.transform.parent = this.transform;
			pooledObjectsList[index].Add(obj);
			theObject = obj;
		}

        if(theObject != null)
		{
			theObject.SetActive(true);
			theObject.transform.position = tf.position;
			theObject.transform.rotation = tf.rotation;
			ParticleSystem ps = theObject.GetComponent<ParticleSystem>();
			if (ps != null)
            {
				
				if (ps.isPlaying) { ps.Stop(); }
				ps.time = 0;
				ps.Play();
				for (int i = 0; i < theObject.transform.childCount; i++)
                {
					ps = theObject.transform.GetChild(i).GetComponent<ParticleSystem>();
                    if(ps != null)
					{
						if (ps.isPlaying) { ps.Stop(); }
						ps.time = 0;
						ps.Play();
                    }
				}
            }
			Sequence seq = DOTween.Sequence();
			seq.AppendInterval(disableTime);
			seq.AppendCallback(delegate
			{
			    ps = theObject.GetComponent<ParticleSystem>();
				if (ps != null)
				{
					ps.Clear();
				}
				theObject.SetActive(false);
			});
			theObject.transform.localScale = theObject.transform.localScale * 0.3f;
		}
		return theObject;
	}

	public List<GameObject> GetAllPooledObjects(int index)
	{
		return pooledObjectsList[index];
	}

	public int AddObject(GameObject GO, int amt = 3, bool exp = true)
	{
		ObjectPoolItem item = new ObjectPoolItem(GO, amt, exp);
		int currLen = itemsToPool.Count;
		itemsToPool.Add(item);
		ObjectPoolItemToPooledObject(currLen);
		return currLen;
	}

	void ObjectPoolItemToPooledObject(int index)
	{
		ObjectPoolItem item = itemsToPool[index];

		pooledObjects = new List<GameObject>();
		for (int i = 0; i < item.amountToPool; i++)
		{
			GameObject obj = (GameObject)Instantiate(item.objectToPool);
			obj.SetActive(false);
			obj.transform.parent = this.transform;
			pooledObjects.Add(obj);
		}
		pooledObjectsList.Add(pooledObjects);
		positions.Add(0);

	}
}