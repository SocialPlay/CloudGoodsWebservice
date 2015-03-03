using UnityEngine;
using System.Collections;

public class TestContainers : MonoBehaviour {

    public ItemContainer itemContainer;

	// Use this for initialization
	void Start () {
        ItemData itemData = new ItemData()
        {
            Id = 1,
            CollectionId = 2,
            Amount = 5,
            ClassId = 10,
            Energy = 100,
            Location = 0,
            Name = "Test Item",
            StackLocationId = "123456",
            Detail = "Some test detail here"
        };

        for (int i = 0; i < 3; i++)
        {
            ItemData.Tag tag = new ItemData.Tag() { name = i.ToString(), Id = i };
            //itemData.tags.Add(tag);
        }

        for (int i = 0; i < 3; i++)
        {
            ItemData.Behaviours behaviour = new ItemData.Behaviours() { name = i.ToString(), Id = i };
            itemData.behaviours.Add(behaviour);
        }

        itemContainer.Add(itemData, -1, false);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
