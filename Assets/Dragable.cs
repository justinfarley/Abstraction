using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEditor;

public class Dragable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public event Action OnMouseDown;
    public event Action OnMouseUp;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private TMP_Text _priceText, _escToCancelText;
    private Tower _newTower;
    private bool _mouseDown = false;
    private int _basePrice;
    //TODO: implement a way to cancel once already placing
    private void Awake()
    {
        OnMouseDown += DragPointerDown;
        OnMouseUp += DragPointerUp;
    }
    private void Start()
    {
        _basePrice = _towerPrefab.GetComponent<Tower>().GetBasePrice();
        GameManager.OnCashChanged += UpdatePriceText;
        StartCoroutine(Init_cr());
    }
    private void Update()
    {
        if (_newTower == null) return;
        if(_newTower.PlacingState == Tower.PlaceState.Placing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResetDragable();
            }
            if (!MouseScreenCheck())
            {
                ResetDragable();
            }
        }
    }
    private IEnumerator Init_cr()
    {
        yield return new WaitUntil(() => GameManager.Instance.CurrentLevel != null);
        UpdatePriceText();
    }
    private void UpdatePriceText()
    {
        _priceText.text = $"${_basePrice}";
        if (GameManager.Instance.CurrentLevel.Properties.Cash >= _basePrice)
        {
            _priceText.color = Color.black;
        }
        else
        {
            _priceText.color = Color.red;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Change to be when you drag OFF the towers panel then instantiate it
        if (_mouseDown)
        {
            InstantiateTower();
        }
    }
    private void DragPointerDown()
    {
        if (GameUtils.IsPlacing) return;
        if(GameManager.Instance.CurrentLevel.Properties.Cash >= _basePrice)
            _mouseDown = true;

    }
    private void DragPointerUp()
    {
        _mouseDown = false;
        if (_newTower == null)
        {
            ResetDragable();
            return;
        }
        if(_newTower.PlacingState != Tower.PlaceState.Placing)
        {
            ResetDragable();
            return;
        }
        if (!_newTower.GetComponent<Tower>().CanBePlaced)
        {
            ResetDragable();
            return;
        }
        _escToCancelText.gameObject.SetActive(false);
        PlaceTower();
    }
    private void PlaceTower()
    {
        print("placed");
        _escToCancelText.gameObject.SetActive(false);
        _newTower.GetComponent<Tower>().TowerDeselected();
        _newTower.PlacingState = Tower.PlaceState.Placed;
        GameManager.Instance.AddMoney(-_basePrice);
        GameUtils.IsPlacing = false;
    }
    private void ResetDragable()
    {
        print("reset");
        Destroy(_newTower.gameObject);
        GameUtils.IsPlacing = false;
        _escToCancelText.gameObject.SetActive(false);
        _mouseDown = false;
    }
    private void InstantiateTower()
    {
        _escToCancelText.gameObject.SetActive(true);
        _mouseDown = false;
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //instantiate tower in cursor position, disable the Tower component for now
        _newTower = Instantiate(_towerPrefab, worldPos, Quaternion.identity).GetComponent<Tower>();
        _newTower.GetComponent<Tower>().TowerSelected();
        _newTower.PlacingState = Tower.PlaceState.Placing;
        GameUtils.IsPlacing = true;
    }
    public bool MouseScreenCheck()
    {
    #if UNITY_EDITOR
        if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
        {
            return false;
        }
    #else
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }
    #endif
        else
        {
            return true;
        }
    }

    }
