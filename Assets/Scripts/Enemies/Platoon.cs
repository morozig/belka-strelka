using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Place {
    public int i;
    public int j;
    public GameObject unitObj;

    public Place(int i, int j, GameObject unitObj) {
        this.i = i;
        this.j = j;
        this.unitObj = unitObj;
    }
}

public class Platoon : MonoBehaviour
{
    public int columnsCount;
    public int rowsCount;
    public Vector2 direction;
    public GameObject childPrefab;
    public bool isFull;
    public int emptyCount = 0;

    private float childWidth = 1;
    private float childHeight = 1;
    private float gap = 1;
    private Roaming roaming;
    private List<Place> places = new List<Place>();

    private float width;
    private float height;
    private int maxCount;

    // Start is called before the first frame update
    void Start()
    {
        width = columnsCount * childWidth + (columnsCount - 1) * gap;
        height = rowsCount * childHeight + (rowsCount - 1) * gap;
        maxCount = columnsCount * rowsCount;

        roaming = GetComponent<Roaming>();
        roaming.width = width;
        roaming.height = height;

        for (int i = 0; i < rowsCount; i++) {
            for (int j = 0; j < columnsCount; j++) {
                if (isFull) {
                    var position = (
                        transform.position +
                        new Vector3(
                            - width / 2 + childWidth / 2,
                            height / 2 - childHeight / 2
                        ) +
                        new Vector3(
                            j * (childHeight + gap),
                            - i * (childWidth + gap)
                        )
                    );
                    var unitObj = GameObject.Instantiate(
                        childPrefab,
                        position,
                        transform.rotation,
                        transform
                    );
                    var enemy = unitObj.GetComponent<Enemy>();
                    enemy.direction = direction;
                    enemy.platoonObj = gameObject;

                    var place = new Place(
                        i,
                        j,
                        unitObj
                    );
                    places.Add(place);
                } else {
                    var place = new Place(
                        i,
                        j,
                        null
                    );
                    places.Add(place);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        emptyCount = places.Count(place => place.unitObj == null);
        
        isFull = (emptyCount <= 0);
    }

    public Vector3 Join(GameObject unitObj){
        var empty = places
            .Where(place => place.unitObj == null)
            .ToList();
        var index = Random.Range(0, empty.Count);
        var place = empty[index];
        place.unitObj = unitObj;

        var position = (
            new Vector3(
                - width / 2 + childWidth / 2,
                height / 2 - childHeight / 2
            ) +
            new Vector3(
                place.j * (childHeight + gap),
                - place.i * (childWidth + gap)
            )
        );
        return position;
    }

    public void Leave(GameObject unitObj){
        var place = places.First(place => place.unitObj == unitObj);
        place.unitObj = null;
    }
}
