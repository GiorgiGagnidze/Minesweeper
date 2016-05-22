using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace MineSweeper.Game
{
	public class CellController : MonoBehaviour, IPointerClickHandler   {
		public GameObject One;
		public GameObject Two;
		public GameObject Three;
		public GameObject Four;
		public GameObject Five;
		public GameObject Six;
		public GameObject Seven;
		public GameObject Eight;
		public GameObject Mine;
		public GameObject Flag;
		public GameObject CrossedMine;
		public GameObject BlastedMine;
		public GameObject Tile;
		public GameObject View;
		private Action<CellController> OnRightClick;
		private Action<CellController> OnLeftClick;
		public int I;
		public int J;
		
		public void Init(int i, int j,Action<CellController> onRightClick,Action<CellController> onLeftClick) 
		{
			OnLeftClick = onLeftClick;
			OnRightClick = onRightClick;
			I = i;
			J = j;
			Flag.SetActive(false);
			One.SetActive(false);
			Two.SetActive(false);
			Three.SetActive(false);
			Four.SetActive(false);
			Five.SetActive(false);
			Six.SetActive(false);
			Seven.SetActive(false);
			Eight.SetActive(false);
			Mine.SetActive(false);
			CrossedMine.SetActive(false);
			BlastedMine.SetActive(false);
			Tile.SetActive(false);
			View = Tile;
			View.SetActive(true);
		}
		
		public void Redraw(CellView view)
		{
			View.SetActive(false);
			switch (view)
			{
				case CellView.Mine:
					View = Mine;
					break;
				case CellView.Blasted:
					View = BlastedMine;
					break;
				case CellView.Crossed:
					View = CrossedMine;
					break;
				case CellView.One:
					View = One;
					break;
				case CellView.Two:
					View = Two;
					break;
				case CellView.Three:
					View = Three;
					break;
				case CellView.Four:
					View = Four;
					break;
				case CellView.Five:
					View = Five;
					break;
				case CellView.Six:
					View = Six;
					break;
				case CellView.Seven:
					View = Seven;
					break;
				case CellView.Eight:
					View = Eight;
					break;
				case CellView.Flag:
					View = Flag;
					break;
				default:
					View = Tile;
					break;
			}
			View.SetActive(true);
			if (view == CellView.Open)
				View.SetActive(false);
		}

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            	OnLeftClick(this);
			else if (eventData.button == PointerEventData.InputButton.Right)
				OnRightClick(this);

        }
    }
}
