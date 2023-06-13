using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class InfiniteScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public LeaderboardElement listItemPrefab; // G�r�nt�lenecek veri ��elerini temsil eden prefab
    public RectTransform contentTransform; // Scroll View'in i�indeki Content objesi

    [SerializeField] LeaderBoardReader lbJsonReader;
    private List<Datum> dataList; // Veri listesi
    int currentIndex = 0; // Ge�erli veri indeksi
    private float contentHeight; // Content objesi y�ksekli�i
    private Vector2 contentOffset; // Content objesi pozisyonu

    [Header("Settings")]
    public int visibleItemCount = 5; // G�r�nt�lenecek veri ��esi say�s�
    public float itemHeight = 100f; // Veri ��esi y�ksekli�i
    public float spacing = 10f; // Veri ��eleri aras�ndaki bo�luk
    public float boundary;

    [Header("Debug")]
    [SerializeField] Datum data;
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
        // G�r�nt�lenecek ilk veri ��elerini olu�tur
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
            // A�a�� do�ru scroll yap�l�yorsa ve �ekme i�lemi yap�l�yorsa
            isPulling = true;
        }
        else if (scrollDelta.y > 0f && isPulling)
        {
            // Yukar� do�ru scroll yap�l�yorsa ve �ekme i�lemi tamamland�ysa
            isPulling = false;
        }

        if (isPulling)
        {
            pullDistance = (previousScrollPosition.y - currentScrollPosition.y);

            if (-pullDistance >= boundary)
            {
                // Yeni i�erik eklemek i�in gerekli �ekme mesafesi a��ld�
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
            // Veri kayna��ndan veri al ve dataList'e ekle
            Datum newData = FetchDataFromSource(index);
            dataList.Add(newData);
            contentHeight = (dataList.Count * itemHeight) + ((dataList.Count - 1) * spacing);
            contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, contentHeight);
            return newData;
        }
    }

    // Farkl� bir veri kayna��ndan veri almak i�in bu metod �zelle�tirilebilir
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
    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
