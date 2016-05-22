using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper.Game
{
	public class GameController : MonoBehaviour{
		public SmileController Smile;
		private Board Board;
		private const int MAX_TIME = 99999;
		public Transform prefab;
		public GameObject GameBoard;
		public GameObject Flags;
		public GameObject Timer;
		private const string MethodToInvoke = "IncrementTime";
		
		public CellController[,] Cells;
		private int Time;

		// Use this for initialization
		void Start () {
			InitBoard();
		}
		
		private void InitBoard()
		{
			Board = new Board();
			Cells = new CellController[Board.GRID_WIDTH,Board.GRID_HEIGHT];
			Smile.Init(Restart);
			Time = 0;
			CancelInvoke(MethodToInvoke);
			
			Text countFlag = Flags.GetComponent<Text>();
			countFlag.text = ""+Board.NumFlags;
			Text timeElapsed = Timer.GetComponent<Text>();
			timeElapsed.text = ""+Time;
			InvokeRepeating(MethodToInvoke, 1,1);
			
			for (int i = 0; i < Board.GRID_WIDTH; i++)
			{
				for (int j = 0; j < Board.GRID_HEIGHT; j++)
				{
					GameObject obj = GameObject.Find(Board.CEL+(i+j*Board.GRID_WIDTH)) as GameObject;
					Cells[i,j] = obj.GetComponent(typeof(CellController)) as CellController;
					Cells[i,j].Init(i,j,OnRightClick,OnLeftClick);
				}
			}
		}
		
		private void IncrementTime() 
		{
			Time++;
			if (Time == MAX_TIME)
				CancelInvoke(MethodToInvoke);
			Text timeElapsed = Timer.GetComponent<Text>();
			timeElapsed.text = ""+Time;
		}
		
		
		
		private void Restart(SmileController controller)
		{
			InitBoard();
		}
		
		private void OnRightClick(CellController cell)
		{
			if (Board.Mode == GameMode.Playing) 
			{
				Board.RightClick(cell.I,cell.J);
				Cell cel = Board.Cells[cell.I,cell.J];
				if (cel.Mode == CellMode.Flaged)
				{
					Cells[cell.I,cell.J].Redraw(CellView.Flag);
				}
				if (cel.Mode == CellMode.Closed) 
				{
					Cells[cell.I,cell.J].Redraw(CellView.Tile);
				}
				Text countFlag = Flags.GetComponent<Text>();
				countFlag.text = ""+Board.NumFlags;
			}
		}
		
		private void OnLeftClick(CellController cell) 
		{
			if (Board.Mode == GameMode.Playing) 
			{
				Board.LeftClick(cell.I,cell.J);
				UpdateCells(cell.I,cell.J);
				Text countFlag = Flags.GetComponent<Text>();
				countFlag.text = ""+Board.NumFlags;
			}
		}
		
		private CellView SelectView(int neighborCount) 
		{
			CellView view = CellView.Open;
			switch (neighborCount)
			{
				case 1:
					view = CellView.One;
					break;
				case 2:
					view = CellView.Two;
					break;
				case 3:
					view = CellView.Three;
					break;
				case 4:
					view = CellView.Four;
					break;
				case 5:
					view = CellView.Five;
					break;
				case 6:
					view = CellView.Six;
					break;
				case 7:
					view = CellView.Seven;
					break;
				case 8:
					view = CellView.Eight;
					break;
			}
			return view;
		}
		
		private void UpdateCells(int i0, int j0)
		{
			if (Board.Mode == GameMode.Playing) 
			{
				for (int i = 0; i < Board.GRID_WIDTH; i++)
				{
					for (int j = 0; j < Board.GRID_HEIGHT; j++)
					{
						Cell cel = Board.Cells[i,j];
						if (cel.Mode == CellMode.Open)
						{
							CellView view = SelectView(cel.NeighborCount);
							Cells[i,j].Redraw(view);	
						}
					}
				}
			} else if (Board.Mode == GameMode.Lose) 
			{				
				for (int i = 0; i < Board.GRID_WIDTH; i++)
				{
					for (int j = 0; j < Board.GRID_HEIGHT; j++)
					{
						if (i == i0 && j == j0)
							Cells[i0,j0].Redraw(CellView.Blasted);
						else 
						{
							Cell cel = Board.Cells[i,j];
							if (cel.IsBomb && cel.Mode != CellMode.Flaged)
								Cells[i,j].Redraw(CellView.Mine);
							if (!cel.IsBomb && cel.Mode == CellMode.Flaged)
								Cells[i,j].Redraw(CellView.Crossed);
						}
					}
				}
				Smile.ChangeImage(FaceMode.Lose);
				CancelInvoke(MethodToInvoke);
			} else 
			{
				Cells[i0,j0].Redraw(SelectView(Board.Cells[i0,j0].NeighborCount));
				for (int i = 0; i < Board.GRID_WIDTH; i++)
				{
					for (int j = 0; j < Board.GRID_HEIGHT; j++)
					{
						Cell cel = Board.Cells[i,j];
						if (cel.IsBomb)
							Cells[i,j].Redraw(CellView.Mine);	
					}
				}
				Smile.ChangeImage(FaceMode.Win);
				CancelInvoke(MethodToInvoke);
			}
		}
		
		private void printRealBoard(){
			string all = "";
			for (int i = 0; i < 10; i++)
			{
				string s = "";
				for (int j = 0; j < 10; j++)
				{
					if (!Board.Cells[i,j].IsBomb)
					{
						s+=Board.Cells[i,j].NeighborCount + " ";
					} else 
					{
						s+="b ";
					}
					
				}
				all += s+System.Environment.NewLine;
			}
			print(all);
		}
		
		private void printBoard(){
			string all = "";
			for (int i = 0; i < 10; i++)
			{
				string s = "";
				for (int j = 0; j < 10; j++)
				{
					if (Board.Cells[i,j].Mode == CellMode.Closed)
					{
						s+="x ";
					} else if (Board.Cells[i,j].Mode == CellMode.Flaged)
					{
						s+="F ";
					} else 
					{
						if (!Board.Cells[i,j].IsBomb)
						{
							s+=Board.Cells[i,j].NeighborCount+ " ";
						} else 
						{
							s+="b ";
						}
					}
					
				}
				all += s+System.Environment.NewLine;
			}
			print(all);
		}
	}
}
