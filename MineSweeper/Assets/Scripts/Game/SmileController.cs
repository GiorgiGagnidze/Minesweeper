using UnityEngine;
using System;

namespace MineSweeper.Game
{
	public class SmileController : MonoBehaviour {
		public GameObject Lose;
		public GameObject Win;
		
		public GameObject Normal;
		public GameObject View;
		private Action<SmileController> OnClick;
	
		public void Init(Action<SmileController> onClick)
		{
			OnClick = onClick;
			Lose.SetActive(false);
			Normal.SetActive(false);
			Win.SetActive(false);
			View = Normal;
			View.SetActive(true);
		}
		
		public void Click()
		{
			OnClick(this);
		}
		
		public void ChangeImage(FaceMode mode){
			View.SetActive(false);
			switch (mode)
			{
				case FaceMode.Normal:
					View =  Normal;
					break;
				case FaceMode.Win:
					View =  Win;
					break;
				default:
					View = Lose;  
					break;
			}
			View.SetActive(true);
		}
	}
}
