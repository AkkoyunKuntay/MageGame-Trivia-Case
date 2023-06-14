using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class InfiniteScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("References")]
    public LeaderboardElement listItemPrefab; // prefab that represents the data items to display
    public RectTransform contentTransform; // SContent object inside Scroll View

    [SerializeField] LeaderBoardReader lbJsonReader;
    private List<Datum> dataList;
    int currentIndex = 0; // Current data index
    private float contentHeight; 

    [Header("Settings")]
    public int visibleItemCount = 5; // Number of data items to display
    public float itemHeight = 100f; 
    public float spacing = 10f;
    public float boundary;

    [Header("Debug")]
    [SerializeField] Vector2 scrollDelta;
    ScrollRect scrollRect;
    [SerializeField] Vector2 currentScrollPosition;
    [SerializeField] Vector2 previousScrollPosition;
    [SerializeField] bool isPulling = false;
    [SerializeField] float pullDistance;
    
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        
    }
    private void Start()
    {
        dataList = new List<Datum>();
        lbJsonReader = GetComponent<LeaderBoardReader>();
        // Create the first data items to display
        for (int i = 0; i < visibleItemCount; i++)
        {
            CreateListItem(i);
        }
     
        contentHeight = (dataList.Count * itemHeight) + ((dataList.Count - 1) * spacing);
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, contentHeight);
    }

    private void CreateListItem(int index)
    {
        LeaderboardElement listItem = Instantiate(listItemPrefab);
        listItem.transform.SetParent(contentTransform, false);
        listItem.transform.localPosition = new Vector3(0f, -index * (itemHeight + spacing), 0f);

        listItem.SetFetchedData(GetDataAtIndex(index));       
    }

    private void Update()
    {
        if (scrollDelta.y < 0f && !isPulling)
        {
            // If scrolling down and pulling
            isPulling = true;
        }
        else if (scrollDelta.y > 0f && isPulling)
        {
            // If scrolling upwards and pulling is complete
            isPulling = false;
        }

        if (isPulling)
        {
            pullDistance = (previousScrollPosition.y - currentScrollPosition.y);

            if (-pullDistance >= boundary)
            {
                // Required pull distance exceeded to add new content
                if (currentIndex >= dataList.Count) return;
                currentIndex += visibleItemCount;
                for (int i = 0; i < visibleItemCount; i++)
                {
                    CreateListItem(currentIndex+i);
                }
                previousScrollPosition = currentScrollPosition;
            }
        }
        
    }
    private Datum GetDataAtIndex(int index)
    {
        int dataCount = dataList.Count;

        if (index >= 0 && index < dataCount)
        {
            return dataList[index];
        }
        else
        {
            // Veri kaynaðýndan veri al ve dataList'e ekle
            Datum newData = FetchDataFromSource(index);
            dataList.Add(newData);
            contentHeight = (dataList.Count * itemHeight) + ((dataList.Count - 1) * spacing);
            contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, contentHeight);
            return newData;
        }
    }

    // This method can be customized to get data from a different data source
    private Datum FetchDataFromSource(int index)
    {
        Datum data = lbJsonReader.RequestElementData(index);        
        return data;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousScrollPosition = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentScrollPosition = eventData.position;
        scrollDelta = previousScrollPosition - currentScrollPosition;
    }
}
